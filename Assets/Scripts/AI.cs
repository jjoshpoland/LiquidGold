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
    public float TransportationTime = 3f;

    public float decisionTime = 2f;
    public float marketDecisionTime = 1f;
    public bool randomizeTarget;
    float lastDecision;

    public List<UtilityAction> seasonActions;
    public List<UtilityAction> marketActions;
    Dictionary<int, float> productionTimes;
    Dictionary<Building, int> buildingCounts;
    Dictionary<int, int> harvesterTracker;
    public bool debugAI;
    public int BuildingMax;

    private void Awake()
    {
        harvesterTracker = new Dictionary<int, int>();
        buildingCounts = new Dictionary<Building, int>();
        productionTimes = new Dictionary<int, float>();
        globalInventory = GetComponent<GlobalInventory>();
        mainInventory = GetComponent<Inventory>();
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastDecision = Time.time;
        if(randomizeTarget)
        {
            GoodQuantity randomGQ = Market.singleton.Prices[Random.Range(0, Market.singleton.Prices.Count)];
            targetGood = randomGQ.good;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float requiredTime = SeasonManager.singleton.IsMarketSeason ? marketDecisionTime : decisionTime;
        if(Time.time > lastDecision + requiredTime)
        {
            UtilityAction bestAction = EvaluateActions();
            if(bestAction != null)
            {
                bestAction.Execute(this);
            }
            lastDecision = Time.time;
        }

        if(!SeasonManager.singleton.IsMarketSeason)
        {
            Produce();
        }
        
    }

    void Produce()
    {
        List<int> buildingsToRemove = new List<int>();
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
                    if (Time.time > value + prod.productionRecipe.time + TransportationTime)
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

            if(building.TryGetComponent<Harvester>(out Harvester harvester))
            {
                if (productionTimes.TryGetValue(i, out float value))
                {
                    if (Time.time > value + harvester.harvestTime)
                    {
                        mainInventory.Deposit(harvester.Target);

                        productionTimes[i] = Time.time;
                        harvesterTracker[i] = harvesterTracker[i] + 1;
                        if(harvesterTracker[i] > harvester.AIMaxHarvests)
                        {
                            buildingsToRemove.Add(i);
                            buildingCounts[harvester] = buildingCounts[harvester] - 1;
                        }
                    }
                }
                else
                {
                    productionTimes.Add(i, Time.time);
                    harvesterTracker.Add(i, 1);
                }
            }
        }

        for (int i = 0; i < buildingsToRemove.Count; i++)
        {
            buildings.RemoveAt(buildingsToRemove[i]);
            
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
                    break;
                }
            }

            if(foundGood == false)
            {
                return false;
            }
        }

        for (int i = 0; i < recipe.inputs.Count; i++)
        {
            mainInventory.goods.Remove(recipe.inputs[i]);
        }

        return true;
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
        if(buildingCounts.TryGetValue(building, out int value))
        {
            
            buildingCounts[building] = value + 1;
        }
        else
        {
            buildingCounts.Add(building, 1);
        }
    }

    public void RemoveBuilding(Building building)
    {
        if(buildings.Remove(building))
        {
            buildingCounts[building] = buildingCounts[building] - 1;
        }
    }

    public int GetBuildingCount(Building building)
    {
        if(buildingCounts.TryGetValue(building, out int value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    UtilityAction EvaluateActions()
    {
        float bestScore = 0f;
        UtilityAction bestAction = null;
        List<UtilityAction> actions = SeasonManager.singleton.IsMarketSeason ? marketActions : seasonActions;

        foreach(UtilityAction uAction in actions)
        {
            float score = 0;
            float count = 0;
            foreach(ConsiderationWeight weight in uAction.considerations)
            {
                float thisScore = weight.consideration.Evaluate(this) * weight.weight;
                
                if(weight.invert)
                {
                    thisScore = 1 - thisScore;
                }
                if(debugAI)
                {
                    //Debug.Log(weight.consideration + " has yielded a score of " + thisScore);
                }
                count += weight.weight;
                score += thisScore;
            }

            
            score = score / count;

            if(score > bestScore)
            {
                if(debugAI)
                {
                    //Debug.Log(uAction + " score of " + score + " beats the current best score of " + bestScore + " for " + bestAction);
                }
                
                bestScore = score;
                bestAction = uAction;
            }
            else if(debugAI)
            {
                //Debug.Log(uAction + " score of " + score + " loses to the current best score of " + bestScore + " for " + bestAction);
            }
        }

        if(debugAI)
        {
            Debug.Log(bestAction + " is the best action with a score of " + bestScore);
        }

        return bestAction;
    }
}
