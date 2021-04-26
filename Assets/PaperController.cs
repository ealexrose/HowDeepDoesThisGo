using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{

    public GameObject paperBase;
    public GameObject corkBoard;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SpawnPapers( int paperCount, DataChain[] dataChain, DataNode[] redHerrings)
    {
        List<GameObject> spawnedPapers = new List<GameObject>();
        for (int i = 0; i <  paperCount; i++) 
        {
            spawnedPapers.Add(Instantiate(paperBase, corkBoard.transform));
            spawnedPapers[i].transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-4f, 4f), -i * 0.25f);
            spawnedPapers[i].transform.localScale = new Vector3(UnityEngine.Random.Range( 0.4f,.8f), UnityEngine.Random.Range(0.4f, .8f), 1);
            spawnedPapers[i].GetComponent<PaperRandomizer>().dataNodeInfo = new List<DataNode>();
            
        }


        int previousPaper = 1;

        for (int i = 0; i < dataChain.Length; i++) 
        {
            for (int j = 0; j < dataChain[i].internalChain.Length; j++) 
            {
                int randomPaper = UnityEngine.Random.Range(0, paperCount);
                if (randomPaper == previousPaper) 
                {
                    randomPaper = (randomPaper + 1) % paperCount;
                }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
