using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform objectToLookAt;
    public Vector3 myTransform;
    Vector3 myVel;
    public float distance;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        objectToLookAt = GameObject.Find("Character Manager").GetComponent<CharacterManager>().selectedLeader.transform;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, objectToLookAt.transform.position + new Vector3(0, distance+1.5f, -distance),
            ref myVel, 0.5f);

        if (Input.GetKey(KeyCode.Keypad8))
        {
            distance -= 0.05f; ;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distance -= 0.3f; 
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            distance+=0.05f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distance += 0.3f;
        }

        if (Input.GetAxis("ZoomIn") >= 1)
        {
            distance += 0.1f;
        }
        
        else if(Input.GetAxis("ZoomIn") <= -1)
        {
            distance -= 0.1f;
        }

        distance = Mathf.Clamp(distance, 2, 8);

    }
}
