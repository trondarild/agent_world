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
    public GameObject []Agents;
    public OSC osc;

    GameObject Parent;

    
 
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
        Agents = GameObject.FindGameObjectsWithTag("Agent");
        Parent = new GameObject();
        Parent.name = "FrustumObjects_Parent";
        //Parent.transform.rotation = Quaternion.Euler(0f,0f,0f);
        //Agent = GameObject.FindGameObjectsWithTag("Agent")
    }
    // List<Tuple<Vector3, Vector3>>  GetBBox(GameObject []collection)
    // {
    //     List<Tuple<Vector3, Vector3>> Collected = new List<Tuple<Vector3, Vector3>>();   
    //     foreach (GameObject target in collection)
    //     {
    //         Collected.Add(Tuple.Create(positionDifference, target.GetComponent<Collider>().bounds.extents));
    //     }
    // }

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
        //print ("visible obj: " + Collected.Count);
        // TODO send over osc
        foreach (Tuple<Vector3, Vector3> obj in Collected)
        {
            //print ("center: " + obj.Item1 + "; box: " + obj.Item2);
        }
        return Collected;
    }
    
 
    // Update is called once per frame
    void Update()
    {
        // var collectedBorders = FindFrustumObjects(Borders);
        // var collectedObstacles = FindFrustumObjects(Obstacles);
        // var collectedGoals = FindFrustumObjects(Goals);

        var border_bboxes =  CalculateLocalBounds(Borders);
        // send collections over osc
        SendMsg("/borders/", border_bboxes);
        var obst_bboxes = CalculateLocalBounds(Obstacles);
        SendMsg("/obstacles/", obst_bboxes);
        var goal_bboxes = CalculateLocalBounds(Goals);
        SendMsg("/goals/", goal_bboxes);
        var agent_bboxes = CalculateLocalBounds(Agents);
        SendMsg("/agents/", agent_bboxes);
    }

    void SendMsg(String label, List<Tuple<Vector3, Vector3>>  data)
    {
        OscMessage m = new OscMessage();
        m.address = label;
        
        //print("data: " + data.Count);
        String dbg = "";
        foreach (Tuple<Vector3, Vector3> obj in data)
        {
            Vector3 coord = obj.Item1;
            Vector3 bbox = obj.Item2;
            // only add x and z values, y is up
            m.values.Add((float)coord[0]);
            m.values.Add((float)coord[2]);
            dbg += coord[0] + ", " + coord[2] + ", ";
            m.values.Add((float)bbox[0]);
            m.values.Add((float)bbox[2]);
            dbg += bbox[0] + ", " + bbox[2] + "; ";
        }
        //print (dbg);
        osc.Send(m);
        
    }

    /*
    * Return: list of tuple containing position and width, height, depth
    */
    List<Tuple<Vector3, Vector3>>  CalculateLocalBounds(GameObject []objects)
    {
        
        Quaternion currentRotation = Parent.transform.rotation;
        
        
        foreach( GameObject obj in objects)
        {
            obj.transform.parent = Parent.transform;
        }
        Bounds bounds = new Bounds(Parent.transform.position, Vector3.zero);

        foreach(Renderer renderer in Parent.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 localCenter = bounds.center - Parent.transform.position;
        bounds.center = localCenter;
        //Debug.Log("The local bounds of this model is " + bounds);

        //this.transform.rotation = currentRotation;

        List<Tuple<Vector3, Vector3>> retval = new List<Tuple<Vector3, Vector3>>() ;
        
        foreach(GameObject obj in objects)
        {
            Vector3 coord = obj.transform.localPosition - localCenter/ 2.0F;
            Vector3 bbox = obj.GetComponent<Renderer>().bounds.extents; // renderer gives allo pos and size
            retval.Add(Tuple.Create(coord, bbox));
            obj.transform.parent = null;
            
        }
        return retval;
    }
}
