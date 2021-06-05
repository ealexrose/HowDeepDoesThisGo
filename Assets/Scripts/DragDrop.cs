using UnityEngine;
using System.Collections;

public class DragDrop : MonoBehaviour
{
    public bool dragParent;
    // The plane the object is currently being dragged on
    private Plane dragPlane;

    // The difference between where the mouse is on the drag plane and 
    // where the origin of the object is on the drag plane
    private Vector3 offset;

    private Camera myMainCamera;
    private Transform target;

    void Start()
    {
        target = transform;
        myMainCamera = Camera.main; // Camera.main is expensive ; cache it here
        if (dragParent)
        {

                target = transform.parent;
            
        }
        else 
        {
            target = transform;
        }
    }

    void OnMouseDown()
    {
        dragPlane = new Plane(myMainCamera.transform.forward, target.position);
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        offset = target.position - camRay.GetPoint(planeDist);
    }

    void OnMouseDrag()
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        target.position = camRay.GetPoint(planeDist) + offset;
    }
}