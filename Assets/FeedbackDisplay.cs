using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackDisplay : MonoBehaviour
{
    public int animationCount;
    // Start is called before the first frame update
    void Start()
    {
        int selectedAnimation = Random.Range(0, animationCount);
        Debug.Log(selectedAnimation);
        gameObject.GetComponent<Animator>().SetInteger("AnimationSelect", selectedAnimation);
        Destroy(this.gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
