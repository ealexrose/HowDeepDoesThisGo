using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{

    public GameObject paperBase;
    public GameObject corkBoard;
    public List<GameObject> spawnedPapers;
    public List<GameObject> spawnRegions;
    public float[] regionWeights;
    // Start is called before the first frame update
    void Awake()
    {
        spawnedPapers = new List<GameObject>();
        SetRegionWeights();
    }



    public void SpawnPapers(int paperCount, DataChain[] dataChain, DataNode[] redHerrings)
    {

        CleanBoard();

        spawnedPapers = new List<GameObject>();

        for (int i = 0; i < paperCount; i++)
        {
            spawnedPapers.Add(Instantiate(paperBase, corkBoard.transform));


            spawnedPapers[i].transform.position = GetValidSpawnPosition()+(Vector3.back * (i+1f) *.4f); //new Vector3(UnityEngine.Random.Range(-6f, 6f), UnityEngine.Random.Range(-2.1f, 3.8f), -i * 0.25f);
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



    private Vector3 GetValidSpawnPosition()
    {
        float randomRegionValue = UnityEngine.Random.Range(0f, 1f);
        float countedWeight = 0f;
        int selectedRegion = 0;
        for (int i = 0; i < regionWeights.Length; i++)
        {
            countedWeight += regionWeights[i];
            if (countedWeight >= randomRegionValue)
            {
                selectedRegion = 0;
                break;
            }
        }
        BoxCollider2D region = spawnRegions[selectedRegion].GetComponent<BoxCollider2D>();
        float randX = UnityEngine.Random.Range(-region.size.x/2f, region.size.x/2f) * spawnRegions[selectedRegion].transform.localScale.x;
        float randY = UnityEngine.Random.Range(-region.size.y/2f, region.size.y/2f) * spawnRegions[selectedRegion].transform.localScale.y;

        Vector3 spawnPosition = new Vector3(randX, randY, 0f) + spawnRegions[selectedRegion].transform.position;
        return spawnPosition;
        }

    private void SetRegionWeights()
    {
        float totalArea = 0;
        float[] regionSizes = new float[spawnRegions.Count];

        for (int i = 0; i < regionSizes.Length; i++)
        {
            try
            {
                BoxCollider2D spawnRegion = spawnRegions[i].GetComponent<BoxCollider2D>();
                regionSizes[i] = (spawnRegion.size.x * spawnRegions[i].transform.localScale.x) * (spawnRegion.size.y * spawnRegions[i].transform.localScale.y);
                totalArea += regionSizes[i];
            }
            catch
            {
                Debug.LogError(spawnRegions[i] + " does not have a BoxCollider2D on it or it cannot be found");
            }
        }
        for (int i = 0; i < regionWeights.Length; i++)
        {
            regionWeights[i] = regionSizes[i] / totalArea;
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
