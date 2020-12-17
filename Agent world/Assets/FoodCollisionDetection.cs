using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public MetabolismSim metabolismSim;
    public string food_name = "Food_physical";
    public float food_energy = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(!col.gameObject.name.Contains("FoodChem"))
            Debug.Log("Food collider: coll with " + col.gameObject.name);
        if(col.gameObject.name.Contains(food_name))
        {
            Debug.Log("collided with food");
            metabolismSim.Eat(food_energy);
            col.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
