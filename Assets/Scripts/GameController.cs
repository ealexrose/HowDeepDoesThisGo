using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool allowShapePatterns;
    public bool allowColorPatterns;
    public bool allowNumberPatterns;
    public int chains;
    public int chainLength;
    public int redHerrings;
    public int papers;
    public int variationLimit;

    bool[] allowedPatterns;

    [SerializeField]
    public DataChain[] patternMaster;
    public DataNode[] redHerringNodes;
    // Start is called before the first frame update
    void Start()
    {
        allowedPatterns = new bool[] { allowShapePatterns, allowColorPatterns, allowNumberPatterns };

        GenerateNewConspiricySet(chainLength, redHerrings, chains);
        FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateNewConspiricySet(int chainLength, int redHerrings, int patternCount)
    {
        patternMaster = new DataChain[patternCount];

        for (int i = 0; i < patternCount; i++)
        {
            DataNode[] newPattern = new DataNode[chainLength];
            newPattern = GetValidChain(chainLength);
            patternMaster[i] = new DataChain(newPattern);
        }

        redHerringNodes = new DataNode[redHerrings];
        for (int i = 0; i < redHerrings; i++)
        {
            redHerringNodes[i] = new DataNode(allowedPatterns, variationLimit);
        }

    }


    private DataNode[] GetValidChain(int chainLength)
    {
        //Decide What Pattern To follow
        int chosenPattern = UnityEngine.Random.Range(0, allowedPatterns.Length);

        for (int i = 0; i < allowedPatterns.Length; i++)
        {
            if (allowedPatterns[chosenPattern])
            {
                break;
            }
            else
            {
                chosenPattern++;
                chosenPattern %= 3;
            }
        }

        int patternBase = UnityEngine.Random.Range(1, variationLimit);

        DataNode[] validPattern = new DataNode[chainLength];

        for (int i = 0; i < validPattern.Length; i++)
        {
            validPattern[i] = new DataNode(chosenPattern, patternBase, allowedPatterns, variationLimit);
        }

        return validPattern;
    }
}
