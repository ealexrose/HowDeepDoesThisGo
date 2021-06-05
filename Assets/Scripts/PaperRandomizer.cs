using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperRandomizer : MonoBehaviour
{
    public List<GameObject> paperTypes;
    public GameObject textSpawnPoint;

    public List<GameObject> dataNodeLayouts;
    public GameObject dataNodeRoot;
    public GameObject dataNodeBase;
    bool[] occupiedDataNodes;
    public List<GameObject> shapes;
    public List<Color> colors;

    public List<DataNode> dataNodeInfo;
    // Start is called before the first frame update
    void Start()
    {
        SetEvidenceType();


        
        occupiedDataNodes = new bool[dataNodeRoot.transform.childCount];

        foreach (DataNode newNode in dataNodeInfo) 
        {
            int pickedNode = Random.Range(0, occupiedDataNodes.Length);
            for (int i = 0; i < occupiedDataNodes.Length; i++) 
            {
                if (occupiedDataNodes[pickedNode] == false)
                {
                    break;
                }
                else 
                {
                    pickedNode = (pickedNode + 1) % occupiedDataNodes.Length;
                }
            }

            occupiedDataNodes[pickedNode] = true;

            GameObject instantiatedNode = Instantiate(dataNodeBase, dataNodeRoot.transform.GetChild(pickedNode));
            instantiatedNode.GetComponent<DataDisplay>().node = newNode;

        
        }
    }

    private void SetEvidenceType()
    {
        int paperType = Random.Range(dataNodeInfo.Count, 7);

        GameObject spawnedPaper = Instantiate(paperTypes[paperType], this.transform);
        dataNodeRoot = spawnedPaper;
    }
}
