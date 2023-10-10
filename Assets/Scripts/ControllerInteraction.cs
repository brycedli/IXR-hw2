using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private OVRInput.Button buttonType = OVRInput.Button.PrimaryIndexTrigger;
    [SerializeField] private OVRInput.Button buttonDimension = OVRInput.Button.PrimaryHandTrigger;
    [SerializeField] private OVRInput.Button buttonA = OVRInput.Button.One;
    [SerializeField] private OVRInput.Button buttonB = OVRInput.Button.Two;
    public GameObject taskManager;
    private bool buttonDown = false;

    private void HandleTypePress()
    {
        taskManager.GetComponent<TargetInteraction>().ChangeType();   
    }

    private void HandleDimensionPress()
    {
        taskManager.GetComponent<TargetInteraction>().ChangeDimension();
    }


    private void HandleButtonA()
    {
        taskManager.GetComponent<TargetInteraction>().Reset();
    }

    private void HandleButtonB()
    {
        taskManager.GetComponent<TargetInteraction>().ConfirmSelection();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(buttonType, OVRInput.Controller.LTouch))
        {
            Debug.Log("Button pressed!");
            buttonDown = true;
        }
        if (OVRInput.GetUp(buttonType, OVRInput.Controller.LTouch) && buttonDown == true)
        {
            Debug.Log("Button released!");
            HandleTypePress();
            buttonDown = false;
        }

        if (OVRInput.GetDown(buttonDimension, OVRInput.Controller.LTouch))
        {
            Debug.Log("Button pressed!");
            buttonDown = true;
        }
        if (OVRInput.GetUp(buttonDimension, OVRInput.Controller.LTouch) && buttonDown == true)
        {
            Debug.Log("Button released!");
            HandleDimensionPress();
            buttonDown = false;
        }


        if (OVRInput.GetDown(buttonA, OVRInput.Controller.LTouch))
        {
            Debug.Log("Button pressed!");
            buttonDown = true;
        }
        if (OVRInput.GetUp(buttonA, OVRInput.Controller.LTouch) && buttonDown == true)
        {
            Debug.Log("Button released!");
            HandleButtonA();
            buttonDown = false;
        }

        if (OVRInput.GetDown(buttonB, OVRInput.Controller.LTouch))
        {
            Debug.Log("Button pressed!");
            buttonDown = true;
        }
        if (OVRInput.GetUp(buttonB, OVRInput.Controller.LTouch) && buttonDown == true)
        {
            Debug.Log("Button released!");
            HandleButtonB();
            buttonDown = false;
        }

        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if(joystickInput.y > 0.5f)
        {
            taskManager.GetComponent<TargetInteraction>().ExecuteManipulation(0.02f);
        }
        else if (joystickInput.y < -0.5f)
        {
            taskManager.GetComponent<TargetInteraction>().ExecuteManipulation(-0.02f);
        }
    }
}
