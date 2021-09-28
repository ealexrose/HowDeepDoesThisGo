using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PresetWave 
{
    public string name;
    public List<PaperTemplate> wavePapers;
    public CanvasGroup canvasGroup;
    public int chainLength;
    public int chainCount;
    public int redHerrings;
}
