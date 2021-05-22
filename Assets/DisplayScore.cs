using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayScore : MonoBehaviour
{

    public TextMeshProUGUI scoreDisplay;
    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.text = PlayerPrefs.GetInt("score").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
