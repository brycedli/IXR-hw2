using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertCubeTest : MonoBehaviour
{
    public Vector3 centerPoint;
    public Vector3 xPoint;
    public Vector3 yPoint;
    public Vector3 zPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        centerPoint = transform.position;
        xPoint = centerPoint + transform.right * transform.localScale.x / 2;
        yPoint = centerPoint + transform.up * transform.localScale.y / 2;
        zPoint = centerPoint + transform.forward * transform.localScale.z / 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centerPoint, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(xPoint, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(yPoint, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(zPoint, 0.1f);
    }
}
