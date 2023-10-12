using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private LineRenderer line;
    private Transform lineTarget;

    public GameObject taskManager;
    public GameObject pointer;

    private PointType pointType;

    public GameObject pointMarkerPrefab;
    public GameObject targetPointMarkerPrefab;
    
    private GameObject centerPoint;
    private GameObject xPoint;
    private GameObject yPoint;
    private GameObject zPoint;

    private GameObject centerPointTarget;
    private GameObject xPointTarget;
    private GameObject yPointTarget;
    private GameObject zPointTarget;

    [SerializeField] private OVRInput.Button selectButton = OVRInput.Button.PrimaryIndexTrigger;
    [SerializeField] private OVRInput.Button backButton = OVRInput.Button.Two;
    [SerializeField] private OVRInput.Button buttonA = OVRInput.Button.One;

    private float targetScaleLarge = 0.03f;
    private float targetScaleSmall = 0.01f;

    private float selectorScale = 0.01f;

    private Color halfWhite = new Color(1, 1, 1, 0.1f);
    private Color halfRed = new Color(1, 0, 0, 0.1f);
    private Color halfGreen = new Color(0, 1, 0, 0.1f);
    private Color halfBlue = new Color(0, 0, 1, 0.1f);

    private Color fullWhite = new Color(1, 1, 1, 0.6f);
    private Color fullRed = new Color(1, 0, 0, 0.6f);
    private Color fullGreen = new Color(0, 1, 0, 0.6f);
    private Color fullBlue = new Color(0, 0, 1, 0.6f);

    void Start()
    {
        Debug.Assert(GetComponent<LineRenderer>());
        line = GetComponent<LineRenderer>();
        Debug.Assert(pointMarkerPrefab != null);
        Debug.Assert(targetPointMarkerPrefab != null);
        Debug.Assert(pointer != null);
        centerPointTarget = Instantiate(targetPointMarkerPrefab);
        xPointTarget = Instantiate(targetPointMarkerPrefab);
        yPointTarget = Instantiate(targetPointMarkerPrefab);
        zPointTarget = Instantiate(targetPointMarkerPrefab);
        StartTrial();


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

        lineTarget = centerPointTarget.transform;

        centerPointTarget.GetComponent<Renderer>().material.color = fullWhite;
        xPointTarget.GetComponent<Renderer>().material.color = halfWhite;
        yPointTarget.GetComponent<Renderer>().material.color = halfWhite;
        zPointTarget.GetComponent<Renderer>().material.color = halfWhite;
        centerPointTarget.GetComponentInChildren<TextMeshPro>().text = "Set center point";

        centerPointTarget.transform.localScale = Vector3.one * targetScaleLarge;
        xPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
        yPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
        zPointTarget.transform.localScale = Vector3.one * targetScaleSmall;

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

        if (lineTarget)
        {
            Vector3[] pos =
            {
                pointer.transform.position,
                lineTarget.position,
            };
            line.SetPositions(pos);
            float pointerDist = Vector3.Distance(Camera.main.transform.position, pointer.transform.position);
            float targetDist = Vector3.Distance(Camera.main.transform.position, lineTarget.position);
            line.startColor = new Color (1,1,1,1-Mathf.Clamp(pointerDist*2, 0, 1));
            line.endColor =  new Color(1, 1, 1,1- Mathf.Clamp(targetDist*2, 0, 1));
        }

        centerPointTarget.transform.position = centerPointPreview;
        xPointTarget.transform.position = xPointPreview;
        yPointTarget.transform.position = yPointPreview;
        zPointTarget.transform.position = zPointPreview;

        centerPointTarget.transform.LookAt(Camera.main.transform.position);
        xPointTarget.transform.LookAt(Camera.main.transform.position);
        yPointTarget.transform.LookAt(Camera.main.transform.position);
        zPointTarget.transform.LookAt(Camera.main.transform.position);
        if (OVRInput.GetUp(backButton, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.T))
        {
            // press B button, reset trial
            StartTrial();
        }
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
                    centerPoint.GetComponent<Renderer>().material.color = fullWhite;
                    centerPoint.transform.position = pointer.transform.position;
                    centerPoint.transform.localScale = Vector3.one * selectorScale;

                    centerPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    xPointTarget.GetComponent<Renderer>().material.color = fullWhite;
                    yPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    zPointTarget.GetComponent<Renderer>().material.color = halfWhite;

                    centerPointTarget.GetComponentInChildren<TextMeshPro>().text = "";
                    xPointTarget.GetComponentInChildren<TextMeshPro>().text = "Set X-axis";
                    lineTarget = xPointTarget.transform;

                    centerPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
                    xPointTarget.transform.localScale = Vector3.one * targetScaleLarge;
                }
                if (pointType == PointType.X)
                {
                    xPoint = GameObject.Instantiate(pointMarkerPrefab);
                    xPoint.GetComponent<Renderer>().material.color = fullWhite;
                    xPoint.transform.position = pointer.transform.position;
                    xPoint.transform.localScale = Vector3.one * selectorScale;

                    centerPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    xPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    yPointTarget.GetComponent<Renderer>().material.color = fullWhite;
                    zPointTarget.GetComponent<Renderer>().material.color = halfWhite;

                    xPointTarget.GetComponentInChildren<TextMeshPro>().text = "";
                    yPointTarget.GetComponentInChildren<TextMeshPro>().text = "Set Y-axis";
                    lineTarget = yPointTarget.transform;

                    xPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
                    yPointTarget.transform.localScale = Vector3.one * targetScaleLarge;
                }
                if (pointType == PointType.Y)
                {
                    yPoint = GameObject.Instantiate(pointMarkerPrefab);
                    yPoint.GetComponent<Renderer>().material.color = fullWhite;
                    yPoint.transform.position = pointer.transform.position;
                    yPoint.transform.localScale = Vector3.one * selectorScale;

                    centerPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    xPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    yPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    zPointTarget.GetComponent<Renderer>().material.color = fullWhite;

                    yPointTarget.GetComponentInChildren<TextMeshPro>().text = "";
                    zPointTarget.GetComponentInChildren<TextMeshPro>().text = "Set Z-axis";
                    lineTarget = zPointTarget.transform;

                    yPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
                    zPointTarget.transform.localScale = Vector3.one * targetScaleLarge;
                }
                if (pointType == PointType.Z)
                {
                    zPoint = GameObject.Instantiate(pointMarkerPrefab);
                    zPoint.transform.position = pointer.transform.position;
                    zPoint.transform.localScale *= 0.5f;
                    zPoint.transform.localScale = Vector3.one * selectorScale;

                    centerPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    xPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    yPointTarget.GetComponent<Renderer>().material.color = halfWhite;
                    zPointTarget.GetComponent<Renderer>().material.color = halfWhite;

                    zPointTarget.GetComponentInChildren<TextMeshPro>().text = "";
                    lineTarget = null;



                    centerPointTarget.transform.localScale = Vector3.one * targetScaleLarge;
                    xPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
                    yPointTarget.transform.localScale = Vector3.one * targetScaleSmall;
                    zPointTarget.transform.localScale = Vector3.one * targetScaleSmall;

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

                    if (taskManager.GetComponent<TargetInteraction>().isCorrect())
                    {
                        centerPointTarget.GetComponentInChildren<TextMeshPro>().text = "Correct: Press trigger to confirm selection";
                    }
                    else
                    {
                        centerPointTarget.GetComponentInChildren<TextMeshPro>().text = "Incorrect: Press B to reset selection";
                    }
                }
                pointType++; // otherwise, increment the enum
            }
            
        }
    }

}
