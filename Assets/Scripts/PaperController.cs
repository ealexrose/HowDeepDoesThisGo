using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{

    public GameObject paperBase;
    public GameObject corkBoard;
    public List<GameObject> spawnedPapers;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedPapers = new List<GameObject>();
    }

    public void SpawnPapers(int paperCount, DataChain[] dataChain, DataNode[] redHerrings)
    {

        CleanBoard();

        spawnedPapers = new List<GameObject>();

        for (int i = 0; i < paperCount; i++)
        {
            spawnedPapers.Add(Instantiate(paperBase, corkBoard.transform));
            spawnedPapers[i].transform.position = new Vector3(UnityEngine.Random.Range(-6f, 6f), UnityEngine.Random.Range(-2.1f, 3.8f), -i * 0.25f);
            //spawnedPapers[i].transform.localScale = new Vector3(UnityEngine.Random.Range(0.4f, .8f), UnityEngine.Random.Range(0.4f, .8f), 1);
            spawnedPapers[i].GetComponent<PaperRandomizer>().dataNodeInfo = new List<DataNode>();

        }


        int previousPaper = 1;

        for (int i = 0; i < dataChain.Length; i++)
        {
            for (int j = 0; j < dataChain[i].internalChain.Length; j++)
            {
                bool validPaper = false;
                int randomPaper = UnityEngine.Random.Range(0, paperCount);
                while (!validPaper)
                {
                    bool paperHasSpace = true;
                    bool wasPreviousPaper = false;

                    if (randomPaper == previousPaper)
                    {

                        wasPreviousPaper = true;
                    }
                    if (spawnedPapers[randomPaper].GetComponent<PaperRandomizer>().dataNodeInfo.Count >= 6) 
                    {
                        paperHasSpace = false;
                    }

                    if (paperHasSpace && !wasPreviousPaper)
                    {
                        validPaper = true;
                    }
                    else 
                    {
                        randomPaper = (randomPaper + 1) % paperCount;
                    }

                }
                previousPaper = randomPaper;
                spawnedPapers[randomPaper].GetComponent<PaperRandomizer>().dataNodeInfo.Add(dataChain[i].internalChain[j]);
            }
        }

        for (int i = 0; i < redHerrings.Length; i++)
        {
            int randomPaper = UnityEngine.Random.Range(0, paperCount);
            if (randomPaper == previousPaper)
            {
                randomPaper = (randomPaper + 1) % paperCount;
            }
            spawnedPapers[randomPaper].GetComponent<PaperRandomizer>().dataNodeInfo.Add(redHerrings[i]);
        }
    }

    private void CleanBoard()
    {
        StringController[] stringControllers = FindObjectsOfType<StringController>();

        foreach (StringController stringController in stringControllers) 
        {
            stringController.DestroyString();
        }
        foreach (GameObject spawnedpaper in spawnedPapers)
        {
            Destroy(spawnedpaper);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
