using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPredictor : MonoBehaviour {

    struct dummyPlanet
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 netForce;
        public float mass;
        public bool isSun;
        public bool toPredict;

        public void doUpdate()
        {
            Vector3 accel = netForce / (mass * 100);
            velocity += accel * Time.deltaTime;
            position += velocity * Time.deltaTime;
        }
    }

    [SerializeField]
    private RectTransform[] positionDots;
    private List<dummyPlanet> dummyPlanets;
    private PlanetPhysics[] mPlanets;

    void Awake()
    {
        dummyPlanets = new List<dummyPlanet>();
    }

    public void DoStart()
    {
        mPlanets = GameObject.FindObjectsOfType<PlanetPhysics>();
    }

    public void DoPrediction(int currPlanetID)
    {
        dummyPlanets.Clear();

        foreach (PlanetPhysics planet in mPlanets)
        {
            dummyPlanet dummy = new dummyPlanet();
            dummy.position = planet.gameObject.transform.position;
            dummy.velocity = planet.vel;
            dummy.mass = planet.mass;
            dummy.isSun = planet.isSun;
            dummy.toPredict = (planet.gameObject.GetInstanceID() == currPlanetID);
            dummyPlanets.Add(dummy);
        }

        int currDot = 0;
        for (int timeStep = 0; timeStep < 166; timeStep++)
        {
            for (int i = 0; i < dummyPlanets.Count; i++)
            {
                dummyPlanet p1 = dummyPlanets[i];
                if (p1.isSun) continue;
                p1.netForce = Vector3.zero;
                for (int j = 0; j < dummyPlanets.Count; j++)
                {
                    if (i == j) continue;
                    dummyPlanet p2 = dummyPlanets[j];
                    Vector3 p1top2 = p2.position - p1.position;
                    float dist = p1top2.magnitude;
                    p1top2.Normalize();
                    float forceMag = PhysicsManager.G * p1.mass * p2.mass / (dist * dist);
                    p1.netForce += forceMag * p1top2;
                }
                p1.doUpdate();
                dummyPlanets[i] = p1;
                if (p1.toPredict && timeStep % 33 == 0)
                {
                    Vector2 dotPos = Camera.main.WorldToScreenPoint(p1.position);
                    dotPos *= UIMain.InverseScaleFactor;
                    positionDots[currDot++].anchoredPosition = new Vector2(dotPos.x, dotPos.y);
                }
            }
        }
    }
}
