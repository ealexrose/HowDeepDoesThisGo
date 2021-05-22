using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public bool allowShapePatterns;
    public bool allowColorPatterns;
    public bool allowNumberPatterns;
    public int chains;
    public int chainLength;
    public int redHerrings;
    public int availableHerrings;
    public int papers;
    public int variationLimit;

    bool[] allowedPatterns;

    [SerializeField]
    public DataChain[] patternMaster;
    public DataNode[] redHerringNodes;
    public SolveChecker solveChecker;

    public TextMeshProUGUI chainDisplay;
    [SerializeField]
    public List<Vector4> progressionValues;
    int progression = 0;
    // Start is called before the first frame update
    void Start()
    {
        NextWave();


        //availableHerrings = redHerrings;
        //FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
        //GenerateNewConspiricySet(chainLength, redHerrings, chains);
        //FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Jump")) 
        {

            if (FindObjectOfType<SolveChecker>().CheckForValidSolution())
            {
                availableHerrings = redHerrings;
                FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
                GenerateNewConspiricySet(chainLength, redHerrings, chains);
                FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);
            }

        }

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

    public void TryStartNextRound() 
    {
        if (FindObjectOfType<SolveChecker>().CheckForValidSolution())
        {
            FindObjectOfType<FeedbackController>().DisplayResult(true);
            progression++;
            NextWave();
            //PlayerPrefs.SetInt("score",progression);
            //SetNewProgression();

            //availableHerrings = redHerrings;
            //GenerateNewConspiricySet(chainLength, redHerrings, chains);
            //FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);
        }
        else 
        {
            FindObjectOfType<FeedbackController>().DisplayResult(false);
        }
    }

    void NextWave() 
    {
        PlayerPrefs.SetInt("score", progression);
        SetNewProgression();

        allowedPatterns = new bool[] { allowShapePatterns, allowColorPatterns, allowNumberPatterns };
        availableHerrings = redHerrings;
        chainDisplay.text = chains + " chains of " + chainLength + " nodes required";

        FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
        FindObjectOfType<CountDownController>().progress -= 7.5f;
        GenerateNewConspiricySet(chainLength, redHerrings, chains);
        FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);

    }

    private void SetNewProgression()
    {
        papers = (int)progressionValues[progression].x;
        chainLength = (int)progressionValues[progression].y;
        chains = (int)progressionValues[progression].z;
        redHerrings = (int)progressionValues[progression].w;

        if (progression > 0) 
        {
            allowShapePatterns = true;
        }

        if (progression > 2)
        {
            allowColorPatterns = true;
        }
        else 
        {
            allowColorPatterns = false;
        }
        if (progression > 5)
        {
            allowNumberPatterns = true;
        }
        else 
        {
            allowNumberPatterns = false;
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

    internal void AddHerring()
    {
        availableHerrings++;
        FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
    }

    internal void RemoveHerring()
    {
        availableHerrings--;
        FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
    }
}
