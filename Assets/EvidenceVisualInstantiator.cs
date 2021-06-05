using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceVisualInstantiator : MonoBehaviour
{
    public List<Sprite> alternateVersions;
    public bool randomTitle;
    public Transform titlePosition;


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = alternateVersions[Random.Range(0, alternateVersions.Count)];

        if (randomTitle) 
        {
            //set Title
        }
    }
}
