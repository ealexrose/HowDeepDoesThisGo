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

    [HideInInspector]
    public DataChain[] patternMaster;
    [HideInInspector]
    public DataNode[] redHerringNodes;
    public SolveChecker solveChecker;

    public WhiteBoardController chainDisplay;

    [SerializeField]
    [Tooltip("X = amount of papers, Y = Chain Length, Z = Chains, W = Red errings")]
    public List<Vector4> progressionValues;
    int progression = 0;
    public List<PresetWave> tutorialWave;
    public List<PresetWave> presetWaves;
    public List<PaperTemplate> wave1;
    public List<PaperTemplate> wave2;
    public List<PaperTemplate> wave3;
    public List<PaperTemplate> wave4;
    public List<PaperTemplate> wave5;
    CanvasGroup activeCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("tutorialProgress", 0);
        //PlayerPrefs.SetInt("tutorialEnabled", 1);
        progression = -1;
        NextWave();
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
            if (PlayerPrefs.GetInt("tutorialEnabled") == 1 && (PlayerPrefs.GetInt("tutorialProgress") < 5))
            {
                PlayerPrefs.SetInt("tutorialProgress", PlayerPrefs.GetInt("tutorialProgress") + 1);
            }
            NextWave();
        }
        else
        {
            FindObjectOfType<FeedbackController>().DisplayResult(false);
        }
    }

    void NextWave()
    {
        PlayerPrefs.SetInt("score", progression);
        int tutorialEnabled = PlayerPrefs.GetInt("tutorialEnabled");
        progression++;
        Debug.Log("Starting a wave");
        if (tutorialEnabled == 1)
        {


            int nextTutorial = PlayerPrefs.GetInt("tutorialProgress");
            if (progression <= nextTutorial)
            {

                Debug.Log("tutorial value is " + nextTutorial);

                if (nextTutorial < tutorialWave.Count)
                {
                    FindObjectOfType<PaperController>().SpawnPapers(tutorialWave[nextTutorial].wavePapers);

                    int tutorialPaperCount = tutorialWave[nextTutorial].wavePapers.Count;
                    int tutorialChainLength = tutorialWave[nextTutorial].chainLength;
                    int tutorialChainCount = tutorialWave[nextTutorial].chainCount;
                    int tutorialRedherrings = tutorialWave[nextTutorial].redHerrings;

                    Debug.Log(tutorialPaperCount + " " + tutorialChainCount + " " + tutorialChainLength);
                    SetStaticProgression(tutorialPaperCount, tutorialChainLength, tutorialChainCount, tutorialRedherrings);
                    CanvasGroup tutorialCanvasGroup = tutorialWave[nextTutorial].canvasGroup;
                    DeactivateOverlay();
                    ActivateOverlay(tutorialCanvasGroup);
                    Debug.Log("tutorial wave " + nextTutorial);
                    return;
                }
                else
                {
                    PlayerPrefs.SetInt("tutorialEnabled", 0);
                    Debug.Log("exiting tutorial waves at wave: " + nextTutorial);
                    DeactivateOverlay();
                }

            }


        }


        Debug.Log("Regular");
        SetNewProgression();

        allowedPatterns = new bool[] { allowShapePatterns, allowColorPatterns, allowNumberPatterns };
        availableHerrings = redHerrings;
        string display = chains.ToString() + " chains of " + chainLength.ToString() + " nodes required";
        chainDisplay.EraseWrite(display);
        chainDisplay.SetSavedString(display);
        chainDisplay.IncreaseDisplayTime(3f);


        FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
        FindObjectOfType<CountDownController>().progress -= 7.5f;
        GenerateNewConspiricySet(chainLength, redHerrings, chains);
        FindObjectOfType<PaperController>().SpawnPapers(papers, patternMaster, redHerringNodes);




    }

    private void DeactivateOverlay()
    {
        if (activeCanvasGroup)
        {
            StartCoroutine(FadeCanvasGroupOut(.12f, activeCanvasGroup));
            //activeCanvasGroup.alpha = 0;
        }
    }

    private void ActivateOverlay(CanvasGroup targetCanvasGroup)
    {
        activeCanvasGroup = targetCanvasGroup;
        StartCoroutine(FadeCanvasGroupIn(.25f, activeCanvasGroup));
    }

    public IEnumerator FadeCanvasGroupIn(float time, CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(.12f);
        for (float f = 0; f <= time; f += Time.deltaTime)
        {
            canvasGroup.alpha = (f / time);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeCanvasGroupOut(float time, CanvasGroup canvasGroup)
    {
        for (float f = 0; f <= time; f += Time.deltaTime)
        {
            canvasGroup.alpha = (1 - (f / time));
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
    private void SetStaticProgression(int _papers, int _chainlength, int _chainCount, int _redHerrings)
    {
        papers = _papers;
        chainLength = _chainlength;
        chains = _chainCount;
        redHerrings = _redHerrings;

        availableHerrings = redHerrings;
        string display = chains.ToString() + " chains of " + chainLength.ToString() + " nodes required";
        chainDisplay.EraseWrite(display);
        chainDisplay.SetSavedString(display);
        chainDisplay.IncreaseDisplayTime(3f);
        FindObjectOfType<HerringDisplay>().UpdateHerringDisplay();
        FindObjectOfType<CountDownController>().progress -= 7.5f;
    }

    ///<summary>Generates the values for how many papers, chains, the length of those chains, and red herrings</summary>
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
