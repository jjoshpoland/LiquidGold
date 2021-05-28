using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Building> buildings;
    public int Drachmae;
    public GlobalInventory globalInventory;
    public Inventory mainInventory;
    public Player player;
    public Good targetGood;

    public float decisionTime = 2f;
    float lastDecision;

    public List<UtilityAction> actions;
    Dictionary<int, float> productionTimes;

    private void Awake()
    {
        productionTimes = new Dictionary<int, float>();
        globalInventory = GetComponent<GlobalInventory>();
        mainInventory = GetComponent<Inventory>();
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastDecision = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastDecision + decisionTime)
        {
            UtilityAction bestAction = EvaluateActions();
            if(bestAction != null)
            {
                bestAction.Execute(this);
            }
            lastDecision = Time.time;
        }

        Produce();
    }

    void Produce()
    {
        //production
        //check each building for a producer
        for(int i = 0; i < buildings.Count; i++)
        {
            Building building = buildings[i];
            if (building.TryGetComponent<Producer>(out Producer prod))
            {
                //if they are found in the dictionary, check the time and produce accordingly. if not, they are new and will be added to the dictionary with the current time
                if (productionTimes.TryGetValue(i, out float value))
                {
                    if (Time.time > value + prod.productionRecipe.time)
                    {
                        if(CanProduce(prod.productionRecipe))
                        {
                            foreach (Good output in prod.productionRecipe.outputs)
                            {
                                mainInventory.Deposit(output);
                            }
                        }

                        productionTimes[i] = Time.time;
                    }
                }
                else
                {
                    productionTimes.Add(i, Time.time);
                }
            }
        }
    }

    bool CanProduce(Recipe recipe)
    {
        if (recipe.inputs == null) return true;

        if (recipe.inputs.Count == 0) return true;

        if (mainInventory.remainingCapacity < recipe.outputs.Count) return false;

        List<int> targetedIndeces = new List<int>();
        
        foreach(Good input in recipe.inputs)
        {
            bool foundGood = false;
            for (int i = 0; i < mainInventory.goods.Count; i++)
            {
                if(mainInventory.goods[i] == input && !targetedIndeces.Contains(i))
                {
                    foundGood = true;
                    targetedIndeces.Add(i);
                }
            }

            if(foundGood == false)
            {
                return false;
            }
        }

        foreach(int index in targetedIndeces)
        {
            mainInventory.goods.RemoveAt(index);
        }

        return true;
    }

    UtilityAction EvaluateActions()
    {
        float bestScore = float.MinValue;
        UtilityAction bestAction = null;

        foreach(UtilityAction uAction in actions)
        {
            float score = 0;
            foreach(ConsiderationWeight weight in uAction.considerations)
            {
                float thisScore = weight.consideration.Evaluate(this) * weight.weight;
                if(weight.invert)
                {
                    thisScore = 0 - thisScore;
                }

                score += thisScore;
            }

            score = score / (float)uAction.considerations.Count;

            if(score > bestScore)
            {
                bestScore = score;
                bestAction = uAction;
            }
        }

        return bestAction;
    }
}
