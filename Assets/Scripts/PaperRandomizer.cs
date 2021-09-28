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
    public bool usesTemplate;
    public PaperTemplate paperTemplate;
    // Start is called before the first frame update
    void Start()
    {
        if (!usesTemplate)
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
                // instantiatedNode.transform.parent = dataNodeRoot.transform.GetChild(pickedNode);
                instantiatedNode.transform.localPosition = Vector3.Scale(instantiatedNode.transform.localPosition, new Vector3(0f, 0f, 1f));
                instantiatedNode.GetComponent<DataDisplay>().node = newNode;


            }
        }
        else 
        
        {
            SetFromPassedTemplate(paperTemplate);
        }
       
    }

    private void SetFromPassedTemplate(PaperTemplate paperTemplate)
    {
        SetEvidenceType(paperTemplate.paperType);
        dataNodeInfo = paperTemplate.nodes;
        occupiedDataNodes = new bool[dataNodeRoot.transform.childCount];

        for (int i = 0; i < paperTemplate.nodes.Count;i++)
        {
            int pickedNode = paperTemplate.nodePositions[i];
            occupiedDataNodes[pickedNode] = true;

            GameObject instantiatedNode = Instantiate(dataNodeBase, dataNodeRoot.transform.GetChild(pickedNode));
            instantiatedNode.transform.localPosition = Vector3.Scale(instantiatedNode.transform.localPosition, new Vector3(0f, 0f, 1f));
            instantiatedNode.GetComponent<DataDisplay>().node = paperTemplate.nodes[i];

        }
    }

    private void SetEvidenceType()
    {
        int paperType = Random.Range(dataNodeInfo.Count, 7);

        GameObject spawnedPaper = Instantiate(paperTypes[paperType], this.transform);
        dataNodeRoot = spawnedPaper.transform.GetChild(0).gameObject;
    }

    private void SetEvidenceType(int type)
    {
        int paperType = type;

        GameObject spawnedPaper = Instantiate(paperTypes[paperType], this.transform);
        dataNodeRoot = spawnedPaper.transform.GetChild(0).gameObject;
    }
}
