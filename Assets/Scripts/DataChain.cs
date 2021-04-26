using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataChain 
{
    [SerializeField]
    public DataNode[] internalChain;

    public DataChain(DataNode[] passedChain) 
    {
        internalChain = passedChain;
    }
}
