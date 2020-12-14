using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour, IPooledObject
{
    public float upForce = .01f;
    public float sideForce = .01f;
    public int lifeTimeMax = 200;
    public int lifeTimeMin = 100;
    public int life;
    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);
        GetComponent<Rigidbody>().velocity = force;
        life = Random.Range(lifeTimeMin, lifeTimeMax);
        // Debug.Log("spawened with life: " + life);
    }

    public bool IsDead()
    {
        return life <= 0;
    }

    // Update is called once per frame
    public void Update()
    {   
        life--;
    }

    

}
