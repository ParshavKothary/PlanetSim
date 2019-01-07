using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour {

    [SerializeField]
    private PhysicsManager physicsManager;
    [SerializeField]
    private PositionPredictor positionPredictor;
    [SerializeField]
    private RectTransform gizmoTransform;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button pauseButton;

    private static EditableObject editObject;

    public static float InverseScaleFactor;
    private static bool paused;
    private static int axisDrag;
    private static Vector3 prevMousePosition;


    public static UIMain instance;

    void Awake()
    {
        gizmoTransform.gameObject.SetActive(false);
        InverseScaleFactor = 1 / gameObject.GetComponent<Canvas>().scaleFactor;
        instance = this;
    }

    void Start()
    {
        prevMousePosition = Vector3.zero;
        SetEditObject(null);
        DoPause();
        RefreshPlanetList();
    }

    public void SetEditObject(EditableObject obj)
    {
        if (!paused) return;
        editObject = obj;
        if (obj != null)
        {
            float zoomScale = (Camera.main.orthographicSize * 2) / Screen.height;
            gizmoTransform.gameObject.SetActive(true);
            Vector2 gizmoPosition = Camera.main.WorldToScreenPoint(editObject.gameObject.transform.position);
            gizmoPosition *= InverseScaleFactor;
            gizmoTransform.anchoredPosition = gizmoPosition;

            positionPredictor.DoPrediction(obj.gameObject.GetInstanceID());
        }
        else gizmoTransform.gameObject.SetActive(false);
    }

    public void MoveUI(Vector2 delta)
    {
        delta *= InverseScaleFactor;
        gizmoTransform.anchoredPosition += delta;
        positionPredictor.GetComponent<RectTransform>().anchoredPosition += delta;
    }

    public void DoPlay()
    {
        playButton.gameObject.SetActive(false);
        physicsManager.running = true;
        gizmoTransform.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        paused = false;
    }

    public void DoPause()
    {
        pauseButton.gameObject.SetActive(false);
        physicsManager.running = false;
        playButton.gameObject.SetActive(true);
        if (editObject != null) gizmoTransform.gameObject.SetActive(true);
        paused = true;
    }

    public void StopAxisDrag() { axisDrag = 0;}
    public void StartXAxisDrag() { axisDrag = 1; prevMousePosition = Input.mousePosition; }
    public void StartYAxisDrag() { axisDrag = 2; prevMousePosition = Input.mousePosition; }
    
    void RefreshPlanetList()
    {
        physicsManager.DoStart();
        positionPredictor.DoStart();
    }

    void Update()
    {
        if (editObject != null && paused)
        {
            if (axisDrag > 0)
            {
                float zoomScale = (Camera.main.orthographicSize * 2) / Screen.height;
                Vector3 delta = Input.mousePosition - prevMousePosition;
                delta.z = (axisDrag == 2) ? delta.y : 0;
                delta.y = 0;
                delta.x = (axisDrag == 1) ? delta.x : 0;
                if (delta.magnitude > 0.1)
                {
                    editObject.gameObject.transform.position += delta * zoomScale;

                    positionPredictor.DoPrediction(editObject.gameObject.GetInstanceID());

                    Vector2 gizmoPosition = Camera.main.WorldToScreenPoint(editObject.gameObject.transform.position);
                    gizmoPosition *= InverseScaleFactor;
                    gizmoTransform.anchoredPosition = gizmoPosition;
                }
                prevMousePosition = Input.mousePosition;
            }
        }
    }
}
