using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetabolismSim : MonoBehaviour
{
    public OSC osc;
    public string osc_adr;
    // Start is called before the first frame update
    private float baseEnergyRate = 0.01f;
    public float transientEnergyRate = 0;
    float energyRate = 0.01f;
    public float energyStore = 10;

    public float EnergyStore { get => energyStore; set => energyStore = value; }
    public float EnergyRate { get => transientEnergyRate; set => transientEnergyRate = value; }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        energyRate = baseEnergyRate + transientEnergyRate;
        energyStore -= energyRate;
        transientEnergyRate = 0;

        OscMessage ermsg = new OscMessage();
        ermsg.address = osc_adr + "/energyrate";
        ermsg.values.Add((float)energyRate);
        osc.Send(ermsg);

        
        OscMessage esmsg = new OscMessage();
        esmsg.address = osc_adr + "/energystore";
        esmsg.values.Add((float)energyStore);
        osc.Send(esmsg);
        
        esmsg = null;
        ermsg = null;
    }

    public void Eat(float aenergy) => EnergyStore += aenergy;
}
