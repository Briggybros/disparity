﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class JoystickMovement : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image backgroundImage;
    public Image joystickImage;
    private Vector3 inputVector;
    public Vector3 forwardVector = new Vector3(0, 0, 1);
    public Vector3 centre = new Vector3(0, 0, 0);
        
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x*2, 0, pos.y*2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log(inputVector);

            //Move joystick image
            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 3), inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
            return Input.GetAxis("Vertical");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
