using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrthoCamControl : MonoBehaviour {

    [SerializeField]
    [Range(0.1f, 1f)]
    private float scrollSpeed;
    
    private float panSpeed;

    private bool DragHeld;
    private Vector3 HoldStartPosition;
    private Camera mCam;
    
	// Use this for initialization
	void Start () {
        mCam = gameObject.GetComponent<Camera>();
        HoldStartPosition = Vector2.zero;
        DragHeld = false;
        panSpeed = (mCam.orthographicSize * 2) / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            DragHeld = true;
            HoldStartPosition = Input.mousePosition;
        }
        else if (DragHeld && Input.GetMouseButtonUp(0)) DragHeld = false;
        if (DragHeld)
        {
            Vector3 delta = Input.mousePosition - HoldStartPosition;
            delta.z = delta.y;
            delta.y = 0;
            gameObject.transform.position -= delta * panSpeed;
            HoldStartPosition = Input.mousePosition;
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            mCam.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed;
            panSpeed = (mCam.orthographicSize * 2) / Screen.height;
        }
	}
}
