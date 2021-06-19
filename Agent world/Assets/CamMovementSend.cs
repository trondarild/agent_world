using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/** Send movement: rotation and translation

*/
public class CamMovementSend : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera sourceCam;
    public OSC osc;
    public bool send_rotation=true;
    public bool send_abs_rotation=true;
    public bool send_translation=true;
    public bool send_ready=true;
    Quaternion prev_rot;
    Vector3 prev_pos;
    public Vector3 reset_position;
    public Vector3 reset_rotation;
    

    bool first = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(send_rotation){
            Quaternion dir = sourceCam.transform.rotation;
            Quaternion relative = Quaternion.Inverse(prev_rot) * dir;
            //float rot = Quaternion.Angle(prev_rot,dir);
            float rot = relative.eulerAngles.y;
            OscMessage message_rot = new OscMessage();
            message_rot.address = "/camera/rotation";
            message_rot.values.Add(rot);
            if(!first)
                osc.Send(message_rot);
            prev_rot.Set(dir.x, dir.y, dir.z, dir.w);

        }
        if(send_abs_rotation){
            Quaternion dir = sourceCam.transform.rotation;
            //float rot = Quaternion.Angle(prev_rot,dir);
            float rot = dir.eulerAngles.y;
            OscMessage message_rot = new OscMessage();
            message_rot.address = "/camera/absrotation";
            message_rot.values.Add(rot);
            if(!first)
                osc.Send(message_rot);
            
        }
        if(send_translation){
            Vector3 pos = sourceCam.transform.position;
            Vector3 trans = prev_pos - pos;
            OscMessage message_trans = new OscMessage();
            message_trans.address = "/camera/translation";
            message_trans.values.Add(trans.x);
            message_trans.values.Add(trans.y);
            message_trans.values.Add(trans.z);
            if(!first)
                osc.Send(message_trans);
            prev_pos.x = pos.x;
            prev_pos.y = pos.y;
            prev_pos.z = pos.z;

        }
        if(send_ready){
            bool ready = LessThanEq(
                Abs3(sourceCam.transform.position-reset_position),  
                new Vector3(0.3f, 0.3f, 0.3f));
            //Debug.Log("checking agent: " + Abs3(sourceCam.transform.position-reset_position));
            if(ready){
                OscMessage message = new OscMessage();
                message.address = "/ready";
                message.values.Add(1.0);
                osc.Send(message);
            }

        }
        first = false;
    }

    bool GreaterThan(Vector3 a, Vector3 b){
        return (a[0] > b[0] && a[1] > b[1] && a[2] > b[2]);
    }
    bool LessThanEq(Vector3 a, Vector3 b){
        return (a[0] <= b[0] && a[1] <= b[1] && a[2] <= b[2]);
    }

    Vector3 Abs3(Vector3 a){
        return new Vector3(Math.Abs(a[0]), Math.Abs(a[1]), Math.Abs(a[2]));
    }
}
