using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPhysics : MonoBehaviour {

    public float mass;
    public Vector3 netForce;
    public Vector3 accel;
    public Vector3 vel;
    public bool isSun;

	// Use this for initialization
	void Start () {
        netForce = Vector3.zero;
        //gameObject.transform.localScale *= Mathf.Log(mass, 2);
	}
	
	// Update is called once per frame
	 public void doUpdate () {
        accel = netForce / (mass * 100);
        vel += accel * Time.deltaTime;
        gameObject.transform.position += vel * Time.deltaTime;
	}
}
