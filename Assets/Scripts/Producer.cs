using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Inventory))]
public class Producer : MonoBehaviour
{
    public Inventory outputInventory;
    List<Inventory> inputs;
    List<Good> processingGoods;
    public Recipe productionRecipe;

    float progress;
    bool producing;
    float cycleStartTime;

    public UnityEvent OnOutputFull;
    public UnityEvent OnStartProduce;
    public UnityEvent OnEndProduce;
    public UnityEvent<bool> OnInputStatusCheck;
    public UnityEvent<Good> OnOutput;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new List<Inventory>();
        if(productionRecipe != null)
        {
            processingGoods = new List<Good>(productionRecipe.inputs.Count);
            
            
            foreach (Good input in productionRecipe.inputs)
            {
                processingGoods.Add(null);
                bool alreadyCounted = false;
                foreach (Inventory inputInv in inputs)
                {
                    if (inputInv.allowedGoods.Contains(input))
                    {
                        inputInv.maxCapacity += Mathf.RoundToInt(productionRecipe.time);
                        alreadyCounted = true;
                    }
                }
                if (alreadyCounted) continue;

                Inventory newInput = gameObject.AddComponent<Inventory>();
                newInput.Init();
                newInput.enabled = true;
                newInput.allowedGoods.Add(input);
                newInput.requestedGoods.Add(input);
                newInput.maxCapacity += Mathf.RoundToInt(productionRecipe.time);
                inputs.Add(newInput);
            }
            foreach (Good output in productionRecipe.outputs)
            {
                outputInventory.emptyingGoods.Add(output);
            }

            outputInventory.maxCapacity = productionRecipe.outputs.Count * 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(producing)
        {
            progress = Mathf.Min((Time.time - cycleStartTime) / productionRecipe.time, 1f);
        }
        else
        {
            if(CheckInputs())
            {
                OnStartProduce.Invoke();
                cycleStartTime = Time.time;
                producing = true;
            }
        }

        if(progress >= 1f)
        {
            if(producing)
            {
                OnEndProduce.Invoke();
            }
            producing = false;
            
            if(CheckOutputCapacity())
            {
                foreach(Good output in productionRecipe.outputs)
                {
                    //Debug.Log(name + " produced " + output);
                    outputInventory.Deposit(output);
                    OnOutput.Invoke(output);
                }

                progress = 0f;
            }
        }
    }

    public bool CheckInputs()
    {
        if (productionRecipe == null) return false;

        if(productionRecipe.inputs.Count > 0)
        {
            if (inputs.Count == 0) return false;

            foreach(Inventory input in inputs)
            {
                //go through the production recipe
                for (int i = 0; i < productionRecipe.inputs.Count; i++)
                {
                    //if the processing goods doesn't already contain this input, then try to get it
                    if(processingGoods[i] != productionRecipe.inputs[i])
                    {
                        //if the input can be retrieved from the current inventory, add it to the processing goods list at the same position
                        if(input.GetGood(productionRecipe.inputs[i]))
                        {
                            processingGoods[i] = productionRecipe.inputs[i];
                        }
                    }
                }
            }

            //validate the entire processing list against the recipe list
            bool canProduce = true;
            for (int i = 0; i < productionRecipe.inputs.Count; i++)
            {
                if(processingGoods[i] != productionRecipe.inputs[i])
                {
                    canProduce = false;
                    break;
                }
            }

            
            OnInputStatusCheck.Invoke(!canProduce);
            return canProduce;
        }

        OnInputStatusCheck.Invoke(false);
        return true;
    }

    public bool CheckOutputCapacity()
    {
        if (outputInventory == null) return false;
        if (outputInventory.remainingCapacity <= 0) 
        {
            OnOutputFull.Invoke();
            return false;
        } 

        if(outputInventory.remainingCapacity >= productionRecipe.outputs.Count)
        {
            return true;
        }

        OnOutputFull.Invoke();
        return false;
    }
}
