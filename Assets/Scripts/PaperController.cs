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
    public bool debugRender;
    List<Vector2> debugCorners;
    // Start is called before the first frame update
    void Awake()
    {
        debugCorners = new List<Vector2>();
        spawnedPapers = new List<GameObject>();
        SetRegionWeights();

    }


    /// <summary>
    /// creates a set of paper game objects and fills them with semi-randomly distributed nodes that can be solved
    /// </summary>
    /// <param name="paperCount">the amount of papers to create</param>
    /// <param name="dataChain">how many legitimate data nodes there are</param>
    /// <param name="redHerrings">how many false data nodes there are, these are not required to follow puzzle rules</param>
    public void SpawnPapers(int paperCount, DataChain[] dataChain, DataNode[] redHerrings)
    {

        CleanBoard();

        spawnedPapers = new List<GameObject>();

        for (int i = 0; i < paperCount; i++)
        {
            spawnedPapers.Add(Instantiate(paperBase, corkBoard.transform));


            spawnedPapers[i].transform.position = new Vector3(10, -10, 0);
            Vector3 targetPosition = GetValidSpawnPosition() + (Vector3.back * (i + 1f) * .6f);
            StartCoroutine(SlideEvidenceIn(spawnedPapers[i], targetPosition, i));



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

    public void SpawnPapers(List<PaperTemplate> paperTemplates)
    {
        CleanBoard();

        spawnedPapers = new List<GameObject>();

        //Spawn each paper and set the position to the desired preset
        for (int i = 0; i < paperTemplates.Count; i++)
        {
            spawnedPapers.Add(Instantiate(paperBase, corkBoard.transform));


            spawnedPapers[i].transform.position = new Vector3(10, -10, 0);
            Vector3 targetPosition = (Vector3)((Vector2)paperTemplates[i].position) + (Vector3.back * (i + 1f) * .6f);
            StartCoroutine(SlideEvidenceIn(spawnedPapers[i], targetPosition, i));
            spawnedPapers[i].GetComponent<PaperRandomizer>().dataNodeInfo = new List<DataNode>();


            spawnedPapers[i].GetComponent<PaperRandomizer>().usesTemplate = true;
            spawnedPapers[i].GetComponent<PaperRandomizer>().paperTemplate = paperTemplates[i];
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



    #region board utilities
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
                selectedRegion = i;
                break;
            }
        }
        BoxCollider2D region = spawnRegions[selectedRegion].GetComponent<BoxCollider2D>();
        float randX = UnityEngine.Random.Range(-region.size.x / 2f, region.size.x / 2f) * spawnRegions[selectedRegion].transform.localScale.x;
        float randY = UnityEngine.Random.Range(-region.size.y / 2f, region.size.y / 2f) * spawnRegions[selectedRegion].transform.localScale.y;

        Vector3 spawnPosition = new Vector3(randX, randY, 0f) + spawnRegions[selectedRegion].transform.position + new Vector3(region.offset.x, region.offset.y, 0);
        return spawnPosition;
    }

    private void SetRegionWeights()
    {

        float totalArea = 0;
        float[] regionSizes = new float[spawnRegions.Count];
        regionWeights = new float[regionSizes.Length];
        debugCorners.Clear();

        for (int i = 0; i < regionSizes.Length; i++)
        {
            try
            {
                BoxCollider2D spawnRegion = spawnRegions[i].GetComponent<BoxCollider2D>();
                regionSizes[i] = (spawnRegion.size.x * spawnRegions[i].transform.localScale.x) * (spawnRegion.size.y * spawnRegions[i].transform.localScale.y);
                totalArea += regionSizes[i];
                float xLength = (spawnRegion.size.x / 2f);
                float xOffset = spawnRegion.transform.localPosition.x + spawnRegion.offset.x;
                float yLength = (spawnRegion.size.y / 2f);
                float yOffset = spawnRegion.transform.localPosition.y + spawnRegion.offset.y;
                debugCorners.Add(new Vector2(xLength + xOffset, yLength + yOffset));
                debugCorners.Add(new Vector2(-xLength + xOffset, yLength + yOffset));
                debugCorners.Add(new Vector2(xLength + xOffset, -yLength + yOffset));
                debugCorners.Add(new Vector2(-xLength + xOffset, -yLength + yOffset));
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


    public void ReorderPapers(int newTop)
    {
        spawnedPapers.Add(spawnedPapers[newTop]);
        spawnedPapers.RemoveAt(newTop);
        for (int i = 0; i < spawnedPapers.Count; i++)
        {
            spawnedPapers[i].transform.localPosition = new Vector3(spawnedPapers[i].transform.localPosition.x, spawnedPapers[i].transform.localPosition.y, (i + 1f) * -1f);
        }

    }

    public int GetPaperIndex(GameObject targetPaper)
    {
        for (int i = 0; i < spawnedPapers.Count; i++)
        {
            if (spawnedPapers[i] == targetPaper)
            {
                return i;
            }
        }
        return spawnedPapers.Count - 1;
    }

    public float GetTopPaperDepth() 
    {
        return spawnedPapers[spawnedPapers.Count - 1].transform.position.z;
    }
    #endregion


    #region animations

    IEnumerator SlideEvidenceIn(GameObject evidence, Vector3 endPosition, int delayOrder)
    {
        float time = .45f;
        float delay = ((float)delayOrder + 1) * .25f;
        Vector3 startPosition = new Vector3(10, -10, 0);
        yield return new WaitForSeconds(delay);

        string paperWoosh = "Woosh" + UnityEngine.Random.Range(1, 2);
        AudioManager.instance.Play(paperWoosh);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            evidence.transform.position = Vector3.Lerp(startPosition, endPosition, (i / time) * (i / time));
            yield return null;
        }

        string paperHit = "Hit" + UnityEngine.Random.Range(1, 4);
        //AudioManager.instance.Play(paperHit);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (debugRender)
        {
            Gizmos.color = Color.red;
            foreach (Vector2 spawnCorner in debugCorners)
            {
                Gizmos.DrawSphere(spawnCorner, 1f);
            }


        }
    }
}
