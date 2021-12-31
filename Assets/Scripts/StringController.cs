using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringController : MonoBehaviour
{
    static StringController currentDragOrigin;

    public GameObject directionIndicator;
    public DataDisplay dataDisplay;
    public List<GameObject> strings;
    public List<GameObject> instantiatedStrings;

    public float maxStretch;

    public GameObject outgoingStringTarget;
    public GameObject incomingStringTarget;

    bool dragging;
    public GameObject herringCover;
    GameObject instantiatedHerring;
    public bool isHerring;
    public LineRenderer lineRenderer;
    public PaperController paperController;

    private void Awake()
    {
        paperController = FindObjectOfType<PaperController>();
    }
    private void Update()
    {
        if (dragging == true && Input.GetButtonUp("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<StringController>())
                {
                    bool success = hit.collider.gameObject.GetComponent<StringController>().TryAttachString();
                    if (!success)
                    {
                        DestroyString();
                    }

                }
                else
                {
                    DestroyString();
                }
                FindObjectOfType<SolveChecker>().CheckForValidSolution();
            }
            else
            {
                DestroyString();
            }
            dragging = false;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if (isHerring)
                    {

                        UnSetHerring();
                        FindObjectOfType<SolveChecker>().CheckForValidSolution();
                    }
                    else
                    {

                        SetAsHerring();
                        FindObjectOfType<SolveChecker>().CheckForValidSolution();
                    }
                }
            }
        }


        if (outgoingStringTarget != null)
        {
            UpdateString();
        }
    }

    private void UnSetHerring()
    {
        //Destroy(instantiatedHerring);
        float xVelocity = Mathf.Sign(Random.Range(-1f, 1f)) * Random.Range(1f, 5f);
        float yVelocity = Random.Range(6f, 8.5f);
        Vector2 startVelocity = new Vector2(xVelocity, yVelocity);
        float rotation = Mathf.Sign(Random.Range(-1f, 1f)) * (Random.Range(0f, 40f) + 40f);

        StartCoroutine(AnimateHerringOut(instantiatedHerring, startVelocity, rotation));
        isHerring = false;
        FindObjectOfType<GameController>().AddHerring(); ;
    }

    private void SetAsHerring()
    {
        if (FindObjectOfType<GameController>().availableHerrings > 0)
        {
            FindObjectOfType<GameController>().RemoveHerring();
            DestroyString();
            if (incomingStringTarget != null)
            {
                incomingStringTarget.GetComponent<StringController>().DestroyString();
            }

            instantiatedHerring = Instantiate(herringCover);
            instantiatedHerring.transform.localScale = transform.parent.parent.localScale * instantiatedHerring.transform.localScale.magnitude;

            instantiatedHerring.transform.SetParent(this.transform);

            StartCoroutine(AnimateHerringIn(instantiatedHerring, instantiatedHerring.transform.position));

            Vector3 position = instantiatedHerring.transform.localPosition;
            position = new Vector3(0, 0, -.1f);
            instantiatedHerring.transform.localPosition = position;

            isHerring = true;
        }
    }

    IEnumerator AnimateHerringIn(GameObject herring, Vector3 endPosition)
    {



        SpriteRenderer image = herring.GetComponent<SpriteRenderer>();

        float time = .1f;

        image.color = new Color(1f, 1f, 1f, 0f);
        Vector3 startPosition = endPosition + (Vector3.up * 1.2f);
        Vector3 endScale = herring.transform.localScale;
        herring.transform.localPosition = startPosition;

        float startRotation = UnityEngine.Random.Range(-9, 9) * 5;
        float endRotation = -startRotation;
        herring.transform.localRotation = Quaternion.Euler(0, 0, startRotation);


        yield return new WaitForSeconds(.01f * 15);

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            herring.transform.localPosition = Vector3.Lerp(startPosition, endPosition, i / time);
            herring.transform.localScale = Vector3.Lerp(endScale * 3, endScale, i / time);
            herring.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, i / time));
            image.color = new Color(1f, 1f, 1f, (i / time) * (i / time));
            yield return null;
        }
    }

    IEnumerator AnimateHerringOut(GameObject herring, Vector2 startVelocity, float rotationalVelocity)
    {

        string herringRemovalNoise = "HerringRemove" + UnityEngine.Random.Range(1, 2);
        AudioManager.instance.Play(herringRemovalNoise);

        SpriteRenderer image = herring.GetComponent<SpriteRenderer>();
        float time = 1f;

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            herring.transform.position += (Vector3)startVelocity * Time.deltaTime;
            herring.transform.Rotate(Vector3.forward, rotationalVelocity * Time.deltaTime);
            startVelocity -= Vector2.up * 15f * Time.deltaTime;

            image.color = new Color(1f, 1f, 1f, 1 - ((i / time) * (i / time)));
            yield return null;
        }
        Destroy(herring);
    }




    void OnMouseDown()
    {
        if (!isHerring)
        {
            AudioManager.instance.Play("PinUp");
            DestroyString();
            directionIndicator.SetActive(true);
            instantiatedStrings.Add(Instantiate(strings[Random.Range(0, strings.Count)]));
            currentDragOrigin = this.gameObject.GetComponent<StringController>();
            dragging = true;

        }
    }


    
    void OnMouseDrag()
    {

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isHerring)
        {



            worldPosition.z = paperController.GetTopPaperDepth() + -1f;
            Vector3 thisPosition = this.transform.position;
            thisPosition.z = paperController.GetTopPaperDepth() + -1f;

            AlignIndicator(thisPosition, worldPosition);
            BetterString(thisPosition, worldPosition);
        }
    }

    private void AlignIndicator(Vector3 thisPosition, Vector3 worldPosition)
    {
        Vector3 direction = (worldPosition - thisPosition);
        direction = direction.normalized;
        directionIndicator.transform.right = direction;
    }

    void UpdateString()
    {
        Vector3 worldPosition = outgoingStringTarget.transform.position;
        Vector3 thisPosition = this.transform.position;

        worldPosition.z = -2f;
        thisPosition.z = -2f;

        AlignIndicator(thisPosition, worldPosition);

        float zPosition = Mathf.Min(this.transform.position.z + -.5f, outgoingStringTarget.transform.position.z + -.5f);

        thisPosition.z = zPosition;//this.transform.position.z + -.5f;

        worldPosition.z = zPosition;//this.transform.position.z + -.5f;//Mathf.Clamp(outgoingStringTarget.transform.position.z + -.5f, this.transform.position.z + -1f, this.transform.position.z -.5f);



        BetterString(thisPosition, worldPosition);
    }



    public bool TryAttachString()
    {
        if (CheckIfSharedAttribute())
        {
            if (!CheckIfSamePaper())
            {
                if (incomingStringTarget == null)
                {
                    AudioManager.instance.Play("PinDown");
                    currentDragOrigin.outgoingStringTarget = this.gameObject;
                    incomingStringTarget = currentDragOrigin.gameObject;
                    return true;
                }

            }

        }
        return false;
    }

    private bool CheckIfSamePaper()
    {
        if (currentDragOrigin.gameObject.transform.parent.parent.parent.parent == this.transform.parent.parent.parent.parent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckIfSharedAttribute()
    {
        DataNode node1 = currentDragOrigin.dataDisplay.node;
        DataNode node2 = dataDisplay.node;

        for (int i = 0; i < node1.signifiers.Length; i++)
        {
            if (node1.signifiers[i] == node2.signifiers[i] && node1.signifiers[i] != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyString()
    {
        directionIndicator.SetActive(false);

        if (outgoingStringTarget != null)
        {
            outgoingStringTarget.GetComponent<StringController>().incomingStringTarget = null;
        }
        outgoingStringTarget = null;

        lineRenderer.SetPositions(new[] { Vector3.zero, Vector3.zero });
    }


    public void BetterString(Vector3 startPosition, Vector3 endPosition)
    {
        lineRenderer.SetPositions(new[] { startPosition, endPosition });
    }
}
