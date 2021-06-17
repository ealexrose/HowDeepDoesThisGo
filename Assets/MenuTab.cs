using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTab : MonoBehaviour
{
    RectTransform rectTransform;
    public MenuTabMaster menuTabMaster;
    public Vector2 basePosition;
    public Vector2 offsetTarget;
    public Vector2 upOffset;
    public Vector2 activeOffset;
    public Vector2 hiddenOffset;
    public GameObject target;
    public float moveSpeed;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = target.GetComponent<RectTransform>();
        basePosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if ((rectTransform.anchoredPosition - (basePosition + offsetTarget)).magnitude > 0.02f) 
        {
            rectTransform.anchoredPosition -= ((rectTransform.anchoredPosition - (basePosition + offsetTarget))) * moveSpeed * Time.deltaTime;
        }
    }

    public void SetHoverOffset() 
    {
        offsetTarget += upOffset;    
    }

    public void UnSetHoverOffset() 
    {
        offsetTarget -= upOffset;
    }

    public void SetHiddenOffset() 
    {
        offsetTarget += hiddenOffset;
    }

    public void UnSetHiddenOffset() 
    {
        offsetTarget -= hiddenOffset;
    }

    public void ChangeActiveState() 
    {
        if (isActive)
        {
            UnsetActiveOffset();
        }
        else 
        {
            SetActiveOffset();
        }
    }

    void SetActiveOffset()
    {
        basePosition += activeOffset;
        menuTabMaster.SetActiveTab(rectTransform.GetSiblingIndex());
        isActive = true;
    }

    void UnsetActiveOffset() 
    {
        basePosition -= activeOffset;
        menuTabMaster.ResetAllTabs(rectTransform.GetSiblingIndex());
        isActive = false;
    }
}
