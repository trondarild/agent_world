using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDetourConfig : MonoBehaviour
{
    public enum DetourConfig  {eVShape, eDelayed, eFourCompartment};
    public DetourConfig config = DetourConfig.eVShape;

    string[] config_names = {"V_Obstacle", "Delayed_Detour_Obstacle", "Four_Compartment_Obstacle"};
    string[] config_postfix = {"VO", "DDO", "FCO"};
    string[] content_names = {"Goal", "Outer_Walls", "Obstacles"};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(config == DetourConfig.eVShape){
            // find parent, tag border, obstacle, goal

        }
        else if(config == DetourConfig.eDelayed){

        }
        else { // config == DetourConfig.eFourCompartment

        }
    }

    
}
