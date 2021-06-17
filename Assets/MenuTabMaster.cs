using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabMaster : MonoBehaviour
{
    public List<MenuTab> menuTabs;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActiveTab(int tabIndex)
    {
        for (int i = 0; i < menuTabs.Count; i++)
        {
            if (i != tabIndex)
            {
                menuTabs[i].SetHiddenOffset();
            }
        }
    }

    public void ResetAllTabs(int tabIndex)
    {
        for (int i = 0; i < menuTabs.Count; i++)
        {
            if (i != tabIndex)
            {
                menuTabs[i].UnSetHiddenOffset();
            }
        }
    }
}
