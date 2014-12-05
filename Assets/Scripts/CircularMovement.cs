using UnityEngine;
using System.Collections;

public class CircularMovement : MonoBehaviour {


    public float angleOfRotation = 0;
    public float totalCirculationTimeInSeconds = 5.0f;
    public float speed;
    public float radius = 5.0f;

    Vector3 originalPos;

    public float width = 1.0f;
    public float height = 1.0f;

    void Start()
    {
        originalPos = transform.position;
    }
    void Update()
    {
        speed = (2.0f * Mathf.PI) / totalCirculationTimeInSeconds;


        Vector3 myPos = new Vector3(Mathf.Cos(angleOfRotation) * width, Mathf.Sin(angleOfRotation) * height, 0);
        transform.position = originalPos + myPos;

        if (angleOfRotation == Mathf.Infinity)
        {
            angleOfRotation = 0;
        }
        else
        {
            angleOfRotation += speed * Time.deltaTime;
        }
    }
}
