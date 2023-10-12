using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject textResult;
    private TextMeshProUGUI textMeshResult;
    [SerializeField] private GameObject debugLog;
    private TextMeshProUGUI debugMesh;
    private Vector3 minBounds = new Vector3(-7f, -7f, 5f);
    private Vector3 maxBounds = new Vector3(7f, 7f, 12f);
    private float scaleMin = 0.2f;
    private float scaleMax = 0.6f;
    private float rotationMin = 0f;
    private float rotationMax = 360f;
    private Vector3 positionSource = new Vector3(0f, 0f, 5f);
    private Vector3 scaleSource = new Vector3(1f, 1f, 1f);
    private Vector3 rotationSource = new Vector3(0, 0, 0);
    private float thresholdDistance = 0.2f;
    private float thresholdScale = 0.2f;
    private float thresholdRotation = 50f;
    public GameObject targetGameObject;
    public GameObject sourceGameObject;
    public GameObject wall;
    public GameObject player;

    private int trialAmount = 10;
    public int trialCount = 0;
    private double[] resultsTime;
    private int[] resultsAccuracy;
    private double penalty = 3;
    private DateTime timeStart;
    public enum ManipulationType
    {
        Move,
        Rotate,
        Scale
    }

    public enum ManipulationDimension
    {
        X,
        Y,
        Z
    }
    public ManipulationType manipulationType = ManipulationType.Move;
    public ManipulationDimension manipulationDimension = ManipulationDimension.X;


    void Start()
    {
        Debug.Assert(player);
        debugMesh = debugLog.GetComponent<TextMeshProUGUI>();
        textMeshResult = textResult.GetComponent<TextMeshProUGUI>();
    }

    public void ChangeType()
    {
        switch(manipulationType)
        {
            case ManipulationType.Move:
                manipulationType = ManipulationType.Rotate;
                break;
            case ManipulationType.Rotate:
                manipulationType = ManipulationType.Scale;
                break;
            case ManipulationType.Scale:
                manipulationType = ManipulationType.Move;
                break;
            default:
                break;
        }
    }

    public void ChangeDimension()
    {
        switch (manipulationDimension)
        {
            case ManipulationDimension.X:
                manipulationDimension = ManipulationDimension.Y;
                break;
            case ManipulationDimension.Y:
                manipulationDimension = ManipulationDimension.Z;
                break;
            case ManipulationDimension.Z:
                manipulationDimension = ManipulationDimension.X;
                break;
            default:
                break;
        }
    }

    public void ExecuteManipulation(float value)
    {
        switch(manipulationType)
        {
            case ManipulationType.Move:
                switch(manipulationDimension)
               {
                    case ManipulationDimension.X:
                        sourceGameObject.transform.position += new Vector3(value, 0,0);
                        break;
                    case ManipulationDimension.Y:
                        sourceGameObject.transform.position += new Vector3(0, value, 0);
                        break;
                    case ManipulationDimension.Z:
                        sourceGameObject.transform.position += new Vector3(0, 0, value);
                        break;
                }
                break;
            case ManipulationType.Scale:
                switch (manipulationDimension)
                {
                    case ManipulationDimension.X:
                        sourceGameObject.transform.localScale += new Vector3(value, 0, 0);
                        break;
                    case ManipulationDimension.Y:
                        sourceGameObject.transform.localScale += new Vector3(0, value, 0);
                        break;
                    case ManipulationDimension.Z:
                        sourceGameObject.transform.localScale += new Vector3(0, 0, value);
                        break;
                }
                break;
            case ManipulationType.Rotate:
                switch (manipulationDimension)
                {
                    case ManipulationDimension.X:
                        sourceGameObject.transform.Rotate(new Vector3(value*5, 0, 0));
                        break;
                    case ManipulationDimension.Y:
                        sourceGameObject.transform.Rotate(new Vector3(0, value*5, 0));
                        break;
                    case ManipulationDimension.Z:
                        sourceGameObject.transform.Rotate(new Vector3(0, 0, value*5));
                        break;
                }
                break;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(minBounds.x, maxBounds.x),
            UnityEngine.Random.Range(minBounds.y, maxBounds.y),
            UnityEngine.Random.Range(minBounds.z, maxBounds.z)
        );
        return randomPosition;
    }

    private float GetRandomScale()
    {
        return UnityEngine.Random.Range(scaleMin, scaleMax);
    }

    private float GetRandomRotation()
    {
        return UnityEngine.Random.Range(rotationMin, rotationMax);
    }



    private void InitiateTarget()
    {
        targetGameObject.transform.position = GetRandomPosition();
        targetGameObject.transform.localScale = new Vector3(GetRandomScale(), GetRandomScale(), GetRandomScale());
        targetGameObject.transform.rotation = Quaternion.Euler(new Vector3(GetRandomRotation(), GetRandomRotation(), GetRandomRotation()));
        Vector3 cameraPosition = targetGameObject.transform.position - Camera.main.transform.forward * 0.3f;
        cameraPosition.y = targetGameObject.transform.position.y;
        player.transform.position = cameraPosition;
    }

    private void InitiateSource()
    {
        sourceGameObject.transform.position = positionSource;
        sourceGameObject.transform.localScale = scaleSource;
        sourceGameObject.transform.rotation = Quaternion.Euler(rotationSource);
    }

    public bool isCorrect ()
    {
        float distance = (targetGameObject.transform.position - sourceGameObject.transform.position).magnitude;
        float diffRotation = (targetGameObject.transform.rotation.eulerAngles - sourceGameObject.transform.rotation.eulerAngles).magnitude;
        float diffScale = (targetGameObject.transform.localScale - sourceGameObject.transform.localScale).magnitude;
        if (distance < thresholdDistance && diffRotation < thresholdRotation && diffScale < thresholdScale)
        {
            return true;
            //targetGameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            return false;
        }
    }
    private void UpdateTarget()
    {
        float distance = (targetGameObject.transform.position - sourceGameObject.transform.position).magnitude;
        float diffRotation = (targetGameObject.transform.rotation.eulerAngles - sourceGameObject.transform.rotation.eulerAngles).magnitude;
        float diffScale = (targetGameObject.transform.localScale - sourceGameObject.transform.localScale).magnitude;
        if (distance < thresholdDistance && diffRotation < thresholdRotation && diffScale < thresholdScale)
        {
            Color temp = Color.green;
            temp.a = 0.2f;
            targetGameObject.GetComponent<Renderer>().material.color = temp;
        }
        else
        {
            Color temp = Color.red;
            temp.a = 0.0f;
            targetGameObject.GetComponent<Renderer>().material.color = temp;
        }    
    }

    

    private void UpdateDebugLog()
    {
        float distance = (targetGameObject.transform.position - sourceGameObject.transform.position).magnitude;
        float diffRotation = (targetGameObject.transform.rotation.eulerAngles - sourceGameObject.transform.rotation.eulerAngles).magnitude;
        float diffScale = (targetGameObject.transform.localScale - sourceGameObject.transform.localScale).magnitude;
        if (debugLog.activeSelf == true)
        {
            DateTime timeNow = DateTime.Now;
            TimeSpan timeDifference = timeNow - timeStart;
           
            debugMesh.text = "Trial " + trialCount + " out of " + trialAmount + " \n Time: " + timeDifference.TotalSeconds.ToString("F2"); 
            //debugMesh.text = manipulationType.ToString() + "\n" + manipulationDimension.ToString();// + "\n distance: " + distance.ToString() + "\n rotation: " + targetGameObject.transform.rotation.eulerAngles.ToString() + "\n" + sourceGameObject.transform.rotation.eulerAngles.ToString() + "\n scale: " + diffScale.ToString();
        }
    }


    private void StartTrial()
    {
        InitiateSource();
        InitiateTarget();


        timeStart = DateTime.Now;
        wall.SetActive(false);
        textMeshResult.text = "";
        textResult.SetActive(false);
        debugMesh.text = "";
        debugLog.SetActive(true);
    }

    public void Reset()
    {
        trialCount = 0;
        resultsTime = new double[trialAmount];
        resultsAccuracy = new int[trialAmount];
        StartTrial();
    }
    public void ConfirmSelection()
    {
        if (trialCount >= trialAmount)
        {
            Reset();
            return;
        }
        float distance = (targetGameObject.transform.position - sourceGameObject.transform.position).magnitude;
        float diffRotation = (targetGameObject.transform.rotation.eulerAngles - sourceGameObject.transform.rotation.eulerAngles).magnitude;
        float diffScale = (targetGameObject.transform.localScale - sourceGameObject.transform.localScale).magnitude;
        if (distance < thresholdDistance && diffRotation < thresholdRotation && diffScale < thresholdScale)
        {
            resultsAccuracy[trialCount] = 0;
        }
        else
        {            
            resultsAccuracy[trialCount] = 1;
        }
        DateTime timeNow = DateTime.Now;
        TimeSpan timeDifference = timeNow - timeStart;
        resultsTime[trialCount] = timeDifference.TotalSeconds;
        trialCount++;
        if (trialCount == trialAmount)
        {
            EndTest();
        }
        else
        {
            StartTrial();
        }
    }

    private void EndTest()
    {
        String summary = "Average Time: ";
        double sumTime = 0;
        int sumAccuracy = 0;
        foreach (double t in resultsTime)
        {
            sumTime += t;
        }
        summary += (sumTime / trialAmount).ToString() + "\nError: ";
        foreach (int a in resultsAccuracy)
        {
            sumAccuracy += a;
        }
        if (sumAccuracy > 0)
        {
            sumAccuracy--;
        }
        summary += sumAccuracy.ToString()+"\nTime with Penalty: " + (sumTime / trialAmount + sumAccuracy*penalty).ToString();
        textMeshResult.text = summary;
        textResult.SetActive(true);
        debugLog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
        UpdateDebugLog();
    }
}