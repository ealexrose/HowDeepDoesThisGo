using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WhiteBoardController : MonoBehaviour
{

    public SpriteRenderer whiteboardMask;
    public TextMeshPro textDisplay;
    public TextAsset conspiracyLineMaster;

    private string[] conspiracyLines;
    private string savedText;


    float framesUntilRandomText;
    bool displayingConspiracy;
    float framesToDisplayRandomText;

    // Start is called before the first frame update
    void Start()
    {
        framesUntilRandomText = 12f;
        framesToDisplayRandomText = 4f;
        conspiracyLines = conspiracyLineMaster.text.Split('\n');
        textDisplay.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTextTimer();
    }

    private void ProcessTextTimer()
    {
        if (!displayingConspiracy)
        {
            if (framesUntilRandomText <= 0)
            {
                savedText = textDisplay.text;
                EraseWrite(conspiracyLines[UnityEngine.Random.Range(0, conspiracyLines.Length)]);
                framesUntilRandomText = UnityEngine.Random.Range(20f, 30f);
                framesToDisplayRandomText = UnityEngine.Random.Range(1.5f, 3f);
                displayingConspiracy = true;
            }
            else
            {
                framesUntilRandomText -= Time.deltaTime;
            }
        }
        else
        {
            if (framesToDisplayRandomText <= 0)
            {
                EraseWrite(savedText);
                displayingConspiracy = false;
            }
            else
            {
                framesToDisplayRandomText -= Time.deltaTime;
            }
        }
    }

    public void SetSavedString(string textToSave)
    {
        savedText = textToSave;
    }
    public void IncreaseDisplayTime(float time)
    {
        framesUntilRandomText += time;
    }
    public void EraseWrite(string displayText)
    {
        StartCoroutine(EraseText(displayText));
    }
    public IEnumerator EraseText(string displayText)
    {
        float runtime = .75f;
        for (float i = 0; i <= runtime; i += Time.deltaTime)
        {
            whiteboardMask.material.SetFloat("_EraseLevel", i / runtime);
            yield return null;
        }
        whiteboardMask.material.SetFloat("_Write", 1);
        whiteboardMask.material.SetFloat("_EraseLevel", 0);
        textDisplay.text = "";
        yield return new WaitForSeconds(.13f);
        StartCoroutine(WriteText(displayText));
    }

    public IEnumerator WriteText(string displayText)
    {
        textDisplay.text = displayText;
        float runtime = .75f;
        for (float i = 0; i <= runtime; i += Time.deltaTime)
        {
            whiteboardMask.material.SetFloat("_Write", 1 - i / runtime);
            yield return null;
        }
    }
}
