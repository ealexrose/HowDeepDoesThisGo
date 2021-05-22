using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FeedbackController : MonoBehaviour
{
    public GameObject resultDisplay;
    public Sprite successImage;
    public Sprite failureImage;
    GameObject instantiatedResultDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayResult(bool successState) 
    {
        instantiatedResultDisplay = Instantiate(resultDisplay, this.transform);
        if (successState)
        {
            instantiatedResultDisplay.GetComponent<Image>().sprite = successImage;
        }
        else 
        {
            instantiatedResultDisplay.GetComponent<Image>().sprite = failureImage;
        }
    }
}
