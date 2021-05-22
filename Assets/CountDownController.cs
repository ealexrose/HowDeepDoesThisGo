using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDownController : MonoBehaviour
{
    public GameObject van;
    GameObject spawnedVan;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float progress;
    public float limit;
    public bool timeLimitIsUp;
    // Start is called before the first frame update
    void Start()
    {
        spawnedVan = Instantiate(van, transform, this);
        spawnedVan.transform.SetSiblingIndex(2);
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime;
        spawnedVan.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPoint, endPoint, progress / limit);
        if (progress >= limit) 
        {
            timeLimitIsUp = true;
            SceneManager.LoadScene("LoseScene");
        }
    }
}
