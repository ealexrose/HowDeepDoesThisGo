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
        Destroy(instantiatedHerring);
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
            instantiatedHerring.transform.localScale = Vector3.one * .44f;

            instantiatedHerring.transform.SetParent(this.transform);

            Vector3 position = instantiatedHerring.transform.localPosition;
            position = new Vector3(0, 0, -.1f);
            instantiatedHerring.transform.localPosition = position;

            isHerring = true;
        }
    }

    void OnMouseDown()
    {
        if (!isHerring)
        {
            DestroyString();
            directionIndicator.SetActive(true);
            instantiatedStrings.Add(Instantiate(strings[Random.Range(0, strings.Count)]));
            currentDragOrigin = this.gameObject.GetComponent<StringController>();
            dragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (!isHerring)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = -3;
            Vector3 thisPosition = this.transform.position;
            thisPosition.z = -3;

            int requiredChunks = (int)Mathf.Ceil((worldPosition - thisPosition).magnitude / maxStretch);

            AlignIndicator(thisPosition, worldPosition);
            TileStretch(requiredChunks, thisPosition, worldPosition);
        }
    }

    private void AlignIndicator(Vector3 thisPosition, Vector3 worldPosition)
    {
        Vector3 direction = (worldPosition - thisPosition);
        direction = direction.normalized;
        directionIndicator.transform.right = direction;//Quaternion.Euler(0, 0, Mathf.Atan(direction.y/ direction.y) * Mathf.Rad2Deg);
    }

    void UpdateString()
    {
        Vector3 worldPosition = outgoingStringTarget.transform.position;
        worldPosition.z = -3;
        Vector3 thisPosition = this.transform.position;
        thisPosition.z = -3;

        int requiredChunks = (int)Mathf.Ceil((worldPosition - thisPosition).magnitude / maxStretch);

        AlignIndicator(thisPosition, worldPosition);
        TileStretch(requiredChunks, thisPosition, worldPosition);
    }

    private void TileStretch(int requiredChunks, Vector3 thisPosition, Vector3 worldPosition)
    {
        if (instantiatedStrings.Count < requiredChunks)
        {
            for (int i = instantiatedStrings.Count; i < requiredChunks; i++)
            {
                instantiatedStrings.Add(Instantiate(strings[Random.Range(0, strings.Count)]));
            }

        }
        else if (instantiatedStrings.Count > requiredChunks)
        {
            for (int i = instantiatedStrings.Count - 1; i >= requiredChunks - 1; i--)
            {
                Destroy(instantiatedStrings[i]);
            }
            instantiatedStrings = instantiatedStrings.GetRange(0, requiredChunks - 1);
        }


        Vector3 directionVector = (worldPosition - thisPosition).normalized;

        for (int i = 0; i < instantiatedStrings.Count; i++)
        {
            Vector3 startpoint = thisPosition + (directionVector * i * maxStretch);
            Vector3 endPoint = thisPosition + (directionVector * (i + 1) * maxStretch);
            if (Vector3.Distance(endPoint, thisPosition) > Vector3.Distance(worldPosition, thisPosition))
            {
                endPoint = worldPosition;
            }

            Stretch(instantiatedStrings[i], startpoint, endPoint, false);

        }
    }

    public void Stretch(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1, 2, 2);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition) * 1.32f;
        _sprite.transform.localScale = scale;
    }

    public bool TryAttachString()
    {
        if (CheckIfSharedAttribute())
        {
            if (!CheckIfSamePaper())
            {
                if (incomingStringTarget == null)
                {
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
        foreach (GameObject stringChunk in instantiatedStrings)
        {
            Destroy(stringChunk);
        }
        instantiatedStrings = new List<GameObject>();
    }


}
