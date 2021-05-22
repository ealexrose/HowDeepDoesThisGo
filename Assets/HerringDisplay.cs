using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerringDisplay : MonoBehaviour
{
    public GameObject prefabHerring;
    public List<GameObject> displayedHerrings;

    private void Start()
    {
        displayedHerrings = new List<GameObject>();
    }

    private void Update()
    {
        //UpdateHerringDisplay();
    }

    public void UpdateHerringDisplay() 
    {
        int herringCount = FindObjectOfType<GameController>().availableHerrings;
        
        if (displayedHerrings.Count != herringCount) 
        {
            foreach (GameObject spawnedHerringDisplay in displayedHerrings) 
            {
                Destroy(spawnedHerringDisplay);
            }
            displayedHerrings = new List<GameObject>();
        }

        for (int i = 0; i < herringCount; i++) 
        {

            displayedHerrings.Add(Instantiate(prefabHerring, this.transform));
            displayedHerrings[i].GetComponent<RectTransform>().localPosition = new Vector3(0, -165 * i, 0);
        }
    
    }

}
