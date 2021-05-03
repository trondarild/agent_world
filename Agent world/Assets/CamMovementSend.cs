using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** Send movement: rotation and translation

*/
public class CamMovementSend : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera sourceCam;
    public OSC osc;
    public bool send_rotation=true;
    public bool send_translation=true;
    Quaternion prev_rot;
    Vector3 prev_pos;
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
            osc.Send(message_rot);
            prev_rot.Set(dir.x, dir.y, dir.z, dir.w);

        }
        if(send_translation){
            Vector3 pos = sourceCam.transform.position;
            Vector3 trans = prev_pos - pos;
            OscMessage message_trans = new OscMessage();
            message_trans.address = "/camera/translation";
            message_trans.values.Add(trans[0]);
            message_trans.values.Add(trans[1]);
            message_trans.values.Add(trans[2]);
            osc.Send(message_trans);
            prev_pos.x = pos.x;
            prev_pos.y = pos.y;
            prev_pos.z = pos.z;

        }
    }
}
