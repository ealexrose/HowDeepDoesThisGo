using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerringDisplay : MonoBehaviour
{
    public GameObject prefabHerring;
    public List<GameObject> displayedHerrings;
    public float columnSpacing;
    public float rowSpacing;
    public int maxColumnsBeforeOverflow;
    int currentHerrings;

    private void Start()
    {
        currentHerrings = 0;
        displayedHerrings = new List<GameObject>();
    }


    public void UpdateHerringDisplay()
    {
        //herringcount is the amount of herrings we need to display
        //displayedHerringCount is the amount of herrings we currently display

        int requiredHerrings = FindObjectOfType<GameController>().availableHerrings;

        if (currentHerrings < requiredHerrings)
        {
            for (int i = displayedHerrings.Count; i < requiredHerrings; i++)
            {
                displayedHerrings.Add(null);
            }

            //Animate in herring here
            for (int i = currentHerrings; i < requiredHerrings; i++)
            {
                AddHerring(i);
            }
        }
        else if (currentHerrings > requiredHerrings)
        {
            for (int i = currentHerrings; i > requiredHerrings; --i)
            {
                RemoveHerring(i - 1);

            }
        }
        else
        {
            //we mad chillin here B^)
        }

        currentHerrings = requiredHerrings;

    }

    private void RemoveHerring(int index)
    {

        StartCoroutine(AnimateHerringOut(displayedHerrings[index]));
        displayedHerrings[index] = null;
        AudioManager.instance.Play("Rip1");

    }

    private void AddHerring(int index)
    {
        displayedHerrings[index] = Instantiate(prefabHerring, this.transform);
        Vector3 target = new Vector3(columnSpacing * (index % maxColumnsBeforeOverflow), rowSpacing * (index / maxColumnsBeforeOverflow), 0);
        StartCoroutine(AnimateHerringIn(displayedHerrings[index], target));
        //displayedHerrings[index].GetComponent<RectTransform>().localPosition = new Vector3(columnSpacing * (index % maxColumnsBeforeOverflow), rowSpacing *  (index / maxColumnsBeforeOverflow), 0);
    }

    #region animations
    IEnumerator AnimateHerringIn(GameObject herring, Vector3 endPosition)
    {
        Image image = herring.GetComponent<Image>();

        float time = .3f;

        image.color = new Color(1f, 1f, 1f, 0f);
        Vector3 startPosition = endPosition + (Vector3.up * rowSpacing * .2f);
        Vector3 endScale = herring.transform.localScale;
        herring.transform.localPosition = startPosition;

        float startRotation = UnityEngine.Random.Range(-9, 9) * 5;
        float endRotation = -startRotation;
        herring.transform.localRotation = Quaternion.Euler(0, 0, startRotation);


        yield return new WaitForSeconds(.02f * (float)UnityEngine.Random.Range(1, 30));

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            herring.transform.localPosition = Vector3.Lerp(startPosition, endPosition, i / time);
            herring.transform.localScale = Vector3.Lerp(endScale * 3, endScale, i / time);
            herring.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, i / time));
            image.color = new Color(1f, 1f, 1f, (i / time) * (i / time));
            yield return null;
        }
        string paperHit = "Hit" + UnityEngine.Random.Range(1, 4);
        AudioManager.instance.Play(paperHit);
    }

    IEnumerator AnimateHerringOut(GameObject herring)
    {


        Image image = herring.GetComponent<Image>();

        Vector3 startPosition = herring.transform.localPosition;


        GameObject awful = new GameObject();
        GameObject herringContainer = Instantiate(awful, transform);
        Destroy(awful);

        herringContainer.transform.localPosition = herring.transform.localPosition;
        herring.transform.SetParent(herringContainer.transform);


        Vector3 endPosition = startPosition + (Vector3.down * rowSpacing * .8f);
        Debug.Log(startPosition + " " + endPosition);
        float time = .2f;
        for (float i = 0; i < time; i += Time.deltaTime)
        {

            herringContainer.transform.localScale = new Vector3(Mathf.Lerp(1f, .4f, i / time), 1, 1);
            herringContainer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, i / time);
            image.color = new Color(1f, 1f, 1f, 1 - ((i / time) * (i / time)));
            yield return null;
        }
        string paperHit = "Hit" + UnityEngine.Random.Range(1, 4);
        AudioManager.instance.Play(paperHit);
        Destroy(herringContainer);

    }
    #endregion

}
