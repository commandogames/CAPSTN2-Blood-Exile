using UnityEngine;
using System.Collections;

public class ClampRotation : MonoBehaviour {

    Vector3 originalRot;
	// Use this for initialization
	void Start () {
        originalRot = transform.eulerAngles;

	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 rotAngle = originalRot;
        rotAngle.x = Mathf.Clamp(rotAngle.x, rotAngle.x, rotAngle.x);
        rotAngle.y = Mathf.Clamp(rotAngle.y, rotAngle.y, rotAngle.y);
        rotAngle.z = Mathf.Clamp(rotAngle.z, rotAngle.z, rotAngle.z);
        transform.eulerAngles = rotAngle;
	}
}
