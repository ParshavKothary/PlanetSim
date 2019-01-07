using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class GodScript : MonoBehaviour {

    [SerializeField]
    [Range(0.1f, 1f)]
    private float scrollSpeed;
    private float panSpeed;

    private bool dragging;
    private bool mouseDown;
    private bool clicked;

    private Vector3 prevMousePosition;
    private float holdTime;

    private EditableObject currObject;


    private const float MIN_DRAG_SECONDS = 0.135f;

    // Use this for initialization
    void Start () {
        panSpeed = (Camera.main.orthographicSize * 2) / Screen.height;

        dragging = false;
        mouseDown = false;
        clicked = false;

        prevMousePosition = Vector2.zero;
        holdTime = 0;
    }
	
	// Update is called once per frame
	void Update () {

        clicked = false;

        // differentiate between drag and click
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) mouseDown = true;
        else if (Input.GetMouseButtonUp(0) && mouseDown)
        {
            mouseDown = false;
            if (holdTime < MIN_DRAG_SECONDS) clicked = true;
            else dragging = false;
            holdTime = 0;
        }
        else if (mouseDown && !dragging)
        {
            holdTime += Time.deltaTime;
            if (holdTime > MIN_DRAG_SECONDS)
            {
                dragging = true;
                prevMousePosition = Input.mousePosition;
            }
        }


        //
        if (dragging)
        {
            Vector3 delta = Input.mousePosition - prevMousePosition;
            //UIMain.instance.MoveUI(delta);
            delta.z = delta.y;
            delta.y = 0;
            gameObject.transform.position -= delta * panSpeed;
            prevMousePosition = Input.mousePosition;
        }
        else if (clicked)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            EditableObject newObject = (Physics.Raycast(ray, out hit)) ? hit.collider.gameObject.GetComponent<EditableObject>() : null;
            UIMain.instance.SetEditObject(newObject);
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed;
            panSpeed = (Camera.main.orthographicSize * 2) / Screen.height;
        }
        
    }

}
