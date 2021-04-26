using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDisplay : MonoBehaviour
{
    public DataNode node;

    public Transform shapePoint;
    GameObject shape;
    public TextMeshPro number;
    public GameObject shapeDisplay;
    // Start is called before the first frame update
    void Start()
    {
        ConfigureDisplay(node);
    }

    private void ConfigureDisplay(DataNode node)
    {



        if (node.signifiers[0] != 0)
        {
            //instantiate Shape
            shape = Instantiate(FindObjectOfType<PaperRandomizer>().shapes[node.signifiers[0]], shapePoint);
            //color Shape
            if (node.signifiers[1] != 0) 
            {
                shape.GetComponent<SpriteRenderer>().color = FindObjectOfType<PaperRandomizer>().colors[node.signifiers[1]];
            }
        }

        if (node.signifiers[2] != 0)
        {
            //Set number
            number.text = node.signifiers[2].ToString();

            // if Shape was zero, set number color
            if (node.signifiers[0] == 0)
            {
                number.color = FindObjectOfType<PaperRandomizer>().colors[node.signifiers[1]];
            }
        }
    }
}
