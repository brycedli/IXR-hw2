using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePointController : MonoBehaviour
{
    public enum PointType
    {
        Center,
        X,
        Y,
        Z,
        Confirm,
    }
    public PointType pointType;

    public GameObject centerPoint;
    public GameObject xPoint;
    public GameObject yPoint;
    public GameObject zPoint;

    public GameObject centerPointTarget;
    public GameObject xPointTarget;
    public GameObject yPointTarget;
    public GameObject zPointTarget;

    public GameObject pointMarkerPrefab;
    public GameObject targetPointMarkerPrefab;
    //public GameObject cubeMarkerPrefab;
    [SerializeField] private OVRInput.Button selectButton = OVRInput.Button.PrimaryIndexTrigger;
    [SerializeField] private OVRInput.Button backButton = OVRInput.Button.PrimaryHandTrigger;
    [SerializeField] private OVRInput.Button buttonA = OVRInput.Button.One;

    public GameObject taskManager;

    void Start()
    {
        Debug.Assert(pointMarkerPrefab != null);
        Debug.Assert(targetPointMarkerPrefab != null);
        StartTrial();
        centerPointTarget = Instantiate(targetPointMarkerPrefab);
        xPointTarget = Instantiate(targetPointMarkerPrefab);
        yPointTarget = Instantiate(targetPointMarkerPrefab);
        zPointTarget = Instantiate(targetPointMarkerPrefab);

        centerPointTarget.GetComponent<Renderer>().material.color = Color.white;
        xPointTarget.GetComponent<Renderer>().material.color = Color.red;
        yPointTarget.GetComponent<Renderer>().material.color = Color.green;
        zPointTarget.GetComponent<Renderer>().material.color = Color.blue;
    }

    void StartTrial ()
    {
        if (centerPoint)
        {
            Destroy(centerPoint);
        }
        if (xPoint)
        {
            Destroy(xPoint);
        }
        if (yPoint)
        {
            Destroy(yPoint);
        }
        if (zPoint)
        {
            Destroy(zPoint);
        }
        pointType = PointType.Center;
        
    }

    void Update()
    {
        // preview targetPoints
        GameObject targetGameObject = taskManager.GetComponent<TargetInteraction>().targetGameObject;
        Vector3 centerPointPreview = targetGameObject.transform.position;
        Vector3 xPointPreview = centerPointPreview + targetGameObject.transform.right * targetGameObject.transform.localScale.x / 2;
        Vector3 yPointPreview = centerPointPreview + targetGameObject.transform.up * targetGameObject.transform.localScale.y / 2; ;
        Vector3 zPointPreview = centerPointPreview + targetGameObject.transform.forward * targetGameObject.transform.localScale.z / 2; ;
        centerPointTarget.transform.position = centerPointPreview;
        xPointTarget.transform.position = xPointPreview;
        yPointTarget.transform.position = yPointPreview;
        zPointTarget.transform.position = zPointPreview;

        if (OVRInput.GetUp(buttonA, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.R))
        {
            // press A button, reset scene
            StartTrial();
            taskManager.GetComponent<TargetInteraction>().Reset();
        }

        if (OVRInput.GetUp(selectButton, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space))
        {
            // press Index button, increment and select sequence
            if (pointType == PointType.Confirm)
            {
                // confirm if on last step (point type z)
                taskManager.GetComponent<TargetInteraction>().ConfirmSelection();
                StartTrial();
            }
            else
            {
                if (pointType == PointType.Center)
                {
                    centerPoint = GameObject.Instantiate(pointMarkerPrefab);
                    centerPoint.GetComponent<Renderer>().material.color = Color.white;
                    centerPoint.transform.position = transform.position;
                }
                if (pointType == PointType.X)
                {
                    xPoint = GameObject.Instantiate(pointMarkerPrefab);
                    xPoint.GetComponent<Renderer>().material.color = Color.red;
                    xPoint.transform.position = transform.position;
                }
                if (pointType == PointType.Y)
                {
                    yPoint = GameObject.Instantiate(pointMarkerPrefab);
                    yPoint.GetComponent<Renderer>().material.color = Color.green;
                    yPoint.transform.position = transform.position;
                }
                if (pointType == PointType.Z)
                {
                    zPoint = GameObject.Instantiate(pointMarkerPrefab);
                    zPoint.GetComponent<Renderer>().material.color = Color.blue;
                    zPoint.transform.position = transform.position;

                    // last point: visualize cube
                    GameObject source = taskManager.GetComponent<TargetInteraction>().sourceGameObject;

                    Vector3 cVector = centerPoint.transform.position;
                    Vector3 xVector = xPoint.transform.position;
                    Vector3 yVector = yPoint.transform.position;
                    Vector3 zVector = zPoint.transform.position;

                    source.transform.position = cVector;

                    float sX = (xVector - cVector).magnitude;
                    float sY = (yVector - cVector).magnitude;
                    float sZ = (zVector - cVector).magnitude;
                    source.transform.localScale = new Vector3(sX * 2, sY * 2, sZ * 2);

                    source.transform.rotation = Quaternion.LookRotation(zVector - cVector, yVector - cVector);
                }
                pointType++; // otherwise, increment the enum
            }
            
        }
    }

}
