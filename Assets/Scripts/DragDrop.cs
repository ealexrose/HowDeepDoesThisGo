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
    private ShadowEffect shadowEffect;
    private bool dragging;
    public Vector3 shadowOffset = new Vector3(-1, -1, 0);
    public string pickUp = "PaperUp";
    public string putDown = "PaperDown";
    void Start()
    {
        shadowEffect = GetComponent<ShadowEffect>();
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
        dragging = true;

        target.position += (Vector3)Vector2.one * .25f;
        
        shadowEffect.offset += shadowOffset;
        PaperController paperController = FindObjectOfType<PaperController>();
        int myIndex = paperController.GetPaperIndex(transform.parent.gameObject);
        paperController.ReorderPapers(myIndex);
        target.position -= Vector3.forward * 2f;



        dragPlane = new Plane(myMainCamera.transform.forward, target.position);
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        offset = target.position - camRay.GetPoint(planeDist);

        AudioManager.instance.Play(pickUp);
    }

    void OnMouseDrag()
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        target.position = camRay.GetPoint(planeDist) + offset;

    }

    private void OnMouseUp()
    {
        if (dragging)
        {
            target.position -= (Vector3)Vector2.one * .25f;
            target.position += Vector3.forward * 2f;
            shadowEffect.offset -= shadowOffset;
            dragging = false;
            AudioManager.instance.Play(putDown);
        }
    }
}