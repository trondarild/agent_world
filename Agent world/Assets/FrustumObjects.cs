using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumObjects : MonoBehaviour
{

    public Camera displayCamera;
 
    public GameObject []Targets;

    
 
    // Start is called before the first frame update
    void Start()
    {
        if (displayCamera == null)
        {
            displayCamera = Camera.main;
        }
 
        Targets = GameObject.FindGameObjectsWithTag("Target");
    }
 
    // Update is called once per frame
    void Update()
    {
        List<GameObject> Visible = new List<GameObject>();   
        foreach (GameObject target in Targets)
        {
 
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(displayCamera);
            if (GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds))
            {
                // print("The object" + target.name + "has appeared");
                // target.GetComponent<MeshRenderer>().enabled = true;
                // calculate relative coords to each object
                var positionDifference = target.transform.InverseTransformPoint(displayCamera.transform.position);

                print(target.name + " rel pos: " + positionDifference + " box: " + target.GetComponent<Collider>().bounds.extents);
                Visible.Add(target);
            }
            else
            {
                //print("The object" + target.name + "has disappeared");
                // target.GetComponent<MeshRenderer>().enabled = false;
            }
 
        }
        print ("visible obj: " + Visible.Count);
    }
}
