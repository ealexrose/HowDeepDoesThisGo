using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveChecker : MonoBehaviour
{
    public GameController gameController;
    public int quota;
    public List<StringController> debugList;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();

    }

    private void Update()
    {
        //if (Input.GetButtonDown("Jump")) 
        //{
        //    StringController[] foundList = FindObjectsOfType<StringController>();
        //    List<StringController> masterList = new List<StringController>(foundList);
        //    debugList = masterList;
        //    Debug.Log(debugList.Count);
        //}
    }

    int countNodesInLoop(StringController nodeInList, ref List<StringController> masterList) 
    {
        int count = 1;
        StringController nextStringController = nodeInList.outgoingStringTarget.GetComponent<StringController>();

        while (nextStringController != nodeInList && count < 100) 
        {
            count++;
            masterList.Remove(nextStringController);
            nextStringController = nextStringController.outgoingStringTarget.GetComponent<StringController>();

        }
        return count;
    
    }

    //attempt to travel to the top
    public bool CheckForValidSolution() 
    {
        StringController[] foundList = FindObjectsOfType<StringController>();
        List<StringController> masterList = new List<StringController>(foundList);
        int safetyCount = masterList.Count;
        bool validSolution = true;
        quota = 0;
        while (masterList.Count != 0 && safetyCount > 0) 
        {
            if (masterList[0].isHerring)
            {
                masterList.RemoveAt(0);
                Debug.Log("removed a red herring from the list");
            }
            else 
            {
                int chainCount = 0;

                StringController topOfChain = FindTopOfStringChain(masterList[0]);
                if (topOfChain == null)
                {
                    chainCount = countNodesInLoop(masterList[0], ref masterList);
                    Debug.Log("loop was detected");
                }
                else 
                {
                    chainCount = CountNodesInChain(topOfChain, ref masterList);
                    Debug.Log("straight path was detected");
                }
                Debug.Log("Chaincount " + chainCount);
                if (chainCount == gameController.chainLength)
                {
                    quota++;
                }
                else 
                {
                    validSolution = false;
                }
            }
            safetyCount--;
        }
        Debug.Log("safety count is " + safetyCount);
        if (validSolution)
        {
            Debug.Log("Successful solution containing " + quota + " chains of length " + gameController.chainLength);
        }
        else 
        {
            Debug.Log("Bad solution containing " + quota + " chains of length " + gameController.chainLength);
        }
        return validSolution;
    
    }

    private int CountNodesInChain(StringController stringController, ref List<StringController> masterList)
    {
        List<StringController> visitedNodes = new List<StringController>();

        int count = 1;
        masterList.Remove(stringController);
        while (stringController.outgoingStringTarget != null) 
        {
            
            
            count++;

            stringController = stringController.outgoingStringTarget.GetComponent<StringController>();
            masterList.Remove(stringController);
            if (visitedNodes.Contains(stringController))
            {
                return count;
            }
            else 
            {
                visitedNodes.Add(stringController);
            }

        }
        return count;
    }

    private StringController FindTopOfStringChain(StringController stringController)
    {
        List<StringController> visitedNodes = new List<StringController>();

        while (stringController.incomingStringTarget != null) 
        {
            stringController = stringController.incomingStringTarget.GetComponent<StringController>();
            if (visitedNodes.Contains(stringController))
            {
                return null;
            }
            else 
            {
                visitedNodes.Add(stringController);
            }
        }
        return stringController;
    }
    //if a loop is detected(detected by keeping track of which nodes have been visited). Immediatelystart
}
