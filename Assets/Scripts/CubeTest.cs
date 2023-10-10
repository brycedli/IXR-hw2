using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour
{
    public Vector3 centerPoint;
    public Vector3 xPoint;
    public Vector3 yPoint;
    public Vector3 zPoint;

    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject centerGameObject = new GameObject("centerPoint");
        GameObject xGameObject = new GameObject("xPoint");
        GameObject yGameObject = new GameObject("yPoint");
        GameObject zGameObject = new GameObject("zPoint");

        centerGameObject.transform.parent = transform;
        xGameObject.transform.parent = transform;
        yGameObject.transform.parent = transform;
        zGameObject.transform.parent = transform;

        centerGameObject.transform.localPosition = Vector3.zero;
        xGameObject.transform.localPosition = Vector3.zero;
        yGameObject.transform.localPosition = Vector3.zero;
        zGameObject.transform.localPosition = Vector3.zero;

        xGameObject.transform.position += Vector3.left * 0.5f;
        yGameObject.transform.position += Vector3.up;
        zGameObject.transform.position += Vector3.forward * 0.25f;

    }

    // Update is called once per frame
    void Update()
    {
        centerPoint = transform.Find("centerPoint").position;
        xPoint = transform.Find("xPoint").position;
        yPoint = transform.Find("yPoint").position;
        zPoint = transform.Find("zPoint").position;

        position = centerPoint;
        float sX = (xPoint - centerPoint).magnitude;
        float sY = (yPoint - centerPoint).magnitude;
        float sZ = (zPoint - centerPoint).magnitude;
        scale = new Vector3(sX * 2, sY * 2, sZ * 2);
        rotation = Quaternion.LookRotation(zPoint- centerPoint, yPoint - centerPoint);
        
    }

    private void OnDrawGizmos()
    {
        if (rotation == null)
        {
            return;
        }
        Matrix4x4 transformMat = Matrix4x4.TRS(position, rotation, scale);
        Gizmos.color = Color.white;
        Gizmos.matrix = transformMat;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one); //wire cube (1,1,1) in transformed world space
        Gizmos.matrix = Matrix4x4.identity;  
        Gizmos.DrawSphere(centerPoint, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(xPoint, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(yPoint, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(zPoint, 0.1f);
    }
}
