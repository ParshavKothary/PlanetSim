using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour {

    public static float G = 6.67408f;

    public float TimeSpeed = 1;
    public bool running = false;


    private PlanetPhysics[] mPlanets;

    void Start()
    {
        DoStart();
    }

	// Use this for initialization
	public void DoStart () {
        mPlanets = GameObject.FindObjectsOfType<PlanetPhysics>();
    }
	
	// Update is called once per frame
	void Update () {
        if (mPlanets == null || !running) return;
        Time.timeScale = TimeSpeed;
        for (int i = 0; i < mPlanets.Length; i++)
        {
            PlanetPhysics p1 = mPlanets[i];
            if (p1.isSun) continue;
            p1.netForce = Vector3.zero;
            for (int j = 0; j < mPlanets.Length; j++)
            {
                if (i == j) continue;
                PlanetPhysics p2 = mPlanets[j];
                Vector3 force = p2.transform.position - p1.transform.position;
                float inverseDist = 1/force.magnitude;
                force *= (G * p1.mass * p2.mass * inverseDist * inverseDist * inverseDist);
                p1.netForce += force;
            }
            p1.doUpdate();
        }
	}
}
