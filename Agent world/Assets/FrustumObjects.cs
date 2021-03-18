using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrustumObjects : MonoBehaviour
{

    public Camera displayCamera;
 
    public GameObject []Borders;
    public GameObject []Obstacles;
    public GameObject []Goals;
    public OSC osc;

    
 
    // Start is called before the first frame update
    void Start()
    {
        if (displayCamera == null)
        {
            displayCamera = Camera.main;
        }
 
        Borders = GameObject.FindGameObjectsWithTag("Border");
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        Goals = GameObject.FindGameObjectsWithTag("Goal");
    }

    List<Tuple<Vector3, Vector3>>  FindFrustumObjects(GameObject []collection)
    {
        List<Tuple<Vector3, Vector3>> Collected = new List<Tuple<Vector3, Vector3>>();   
        foreach (GameObject target in collection)
        {
 
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(displayCamera);
            if (GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds))
            {
                // print("The object" + target.name + "has appeared");
                // target.GetComponent<MeshRenderer>().enabled = true;
                // calculate relative coords to each object
                var positionDifference = target.transform.InverseTransformPoint(displayCamera.transform.position);

                // print(target.name + " rel pos: " + positionDifference + " box: " + target.GetComponent<Collider>().bounds.extents);
                Collected.Add(Tuple.Create(positionDifference, target.GetComponent<Collider>().bounds.extents));
            }
            // else
            // {
            //     //print("The object" + target.name + "has disappeared");
            //     // target.GetComponent<MeshRenderer>().enabled = false;
            // }
 
        }
        print ("visible obj: " + Collected.Count);
        // TODO send over osc
        foreach (Tuple<Vector3, Vector3> obj in Collected)
        {
            print ("center: " + obj.Item1 + "; box: " + obj.Item2);
        }
        return Collected;
    }
    
 
    // Update is called once per frame
    void Update()
    {
        var CollectedBorders = FindFrustumObjects(Borders);
        var CollectedObstacles = FindFrustumObjects(Obstacles);
        var CollectedGoals = FindFrustumObjects(Goals);

        // send collections over osc
    }
}
