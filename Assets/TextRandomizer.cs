using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextRandomizer : MonoBehaviour
{
    public TextAsset textList;
    private string[] textLines;
    TextMeshPro displayText;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText() 
    {
        textLines = textList.text.Split('\n');
        displayText = GetComponent<TextMeshPro>();
        displayText.text = textLines[UnityEngine.Random.Range(0, textLines.Length)];
    }
}
