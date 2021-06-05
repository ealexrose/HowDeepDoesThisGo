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

    public DataChain(int size)
    {
        internalChain = new DataNode[size];
    }

    public void Add(DataNode dataNode)
    {
        DataNode[] tempChain = new DataNode[internalChain.Length + 1];
        for (int i = 0; i < internalChain.Length; i++)
        {
            tempChain[i] = internalChain[i];
        }

        tempChain[tempChain.Length - 1] = dataNode;
    }
}
