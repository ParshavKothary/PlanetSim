using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {

    [Range(0.1f, 1f)]
    public float baseMoveSpeed;
    [Range(0.1f, 1)]
    public float rotateSpeed;

    public Vector3 forward;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 left = new Vector3(1, 0, 0);
        Vector3 rotation = Camera.main.transform.rotation.eulerAngles;
        float moveSpeed = (Input.GetKey(KeyCode.LeftShift)) ? baseMoveSpeed * 3 : baseMoveSpeed;
		if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (forward.x * moveSpeed), Camera.main.transform.position.y, Camera.main.transform.position.z + (forward.z * moveSpeed));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (forward.x * -moveSpeed), Camera.main.transform.position.y, Camera.main.transform.position.z + (forward.z * -moveSpeed));
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.Translate(left * -moveSpeed, Space.Self);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.Translate(left * moveSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.rotation = Quaternion.Euler(rotation.x - rotateSpeed, rotation.y, rotation.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.rotation = Quaternion.Euler(rotation.x + rotateSpeed, rotation.y, rotation.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.rotation = Quaternion.Euler(rotation.x, rotation.y - rotateSpeed, rotation.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.rotation = Quaternion.Euler(rotation.x, rotation.y + rotateSpeed, rotation.z);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + moveSpeed, Camera.main.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - moveSpeed, Camera.main.transform.position.z);
        }
    }
}
