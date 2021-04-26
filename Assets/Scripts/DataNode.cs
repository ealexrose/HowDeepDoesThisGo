using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataNode
{
    [SerializeField]
    public int[] signifiers;
    public DataNode(int color, int shape, int number)
    {

        signifiers[0] = shape;
        signifiers[1] = color;
        signifiers[2] = number;
    }
    public DataNode(int baseField, int baseValue, bool[] allowedParameters, int valueLimit)
    {
        signifiers = new int[allowedParameters.Length];
        signifiers[baseField] = baseValue;

        for (int i = 0; i < allowedParameters.Length; i++)
        {
            if (i != baseField)
            {
                if (allowedParameters[i])
                {
                    signifiers[i] = Random.Range(1, valueLimit);
                }
                else
                {

                    signifiers[i] = 0;
                }
            }

        }

    }

    public DataNode(bool[] allowedParameters, int valueLimit)
    {

        signifiers = new int[allowedParameters.Length];
        for (int i = 0; i < allowedParameters.Length; i++)
        {

            if (allowedParameters[i])
            {
                signifiers[i] = Random.Range(1, valueLimit);
            }
            else
            {

                signifiers[i] = 0;
            }

        }
    }
}
