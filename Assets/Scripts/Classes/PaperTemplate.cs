using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PaperTemplate
{
    public Vector3 position;
    public int paperType;
    public List<DataNode> nodes;
    public List<int> nodePositions;

    public PaperTemplate(Vector3 _position, int _paperType, List<DataNode> _nodes, List<int> _nodePositions) 
    {

        position = _position;
        paperType = _paperType;
        nodes = _nodes;
        nodePositions = _nodePositions;

    }

}
