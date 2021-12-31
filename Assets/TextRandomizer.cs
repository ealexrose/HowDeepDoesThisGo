using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



[RequireComponent(typeof(TextMeshPro))]
public class TextRandomizer : MonoBehaviour
{
    public TextAsset textList;
    public bool alternateSplitter;
    public char alternateSplitChar;
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
        displayText = GetComponent<TextMeshPro>();
        if (!alternateSplitter)
        {
            textLines = textList.text.Split('\n');
            displayText.text = textLines[UnityEngine.Random.Range(0, textLines.Length)];
        }
        else 
        {
            textLines = textList.text.Split(alternateSplitChar);
            displayText.SetText(textLines[UnityEngine.Random.Range(0, textLines.Length)].Trim().Replace("\\n","\n"));
        }
       
    }
}
