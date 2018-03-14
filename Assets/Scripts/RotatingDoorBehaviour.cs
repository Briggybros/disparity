﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoorBehaviour : DoorBehaviourScript {


    public bool Turning;
    public float MinRot, MaxRot;

    private float Duration;
    protected float TargetAngle;
    private float Facing;
    private float CurrentTime;
    private Vector3 SourceAxis;
    private Vector3 TargetAxis;
	private Quaternion sourceOrientation;
	private Quaternion baseRot;
	public bool yRot;
	public bool xRot;
	protected override void Start()
    {
        init();
        CurrentTime = 0f;
        Duration = 1f;
        sourceOrientation = this.transform.parent.rotation;
        sourceOrientation.ToAngleAxis(out Facing, out SourceAxis);
		Vector3 v = sourceOrientation.eulerAngles;
		if (yRot) {
			TargetAxis = this.transform.parent.up;
			baseRot = Quaternion.Euler(v.x, 0, v.z);
		} else if (xRot) {
			TargetAxis = this.transform.parent.right;
			baseRot = Quaternion.Euler(0, v.y, v.z);
		} else {
			TargetAxis = this.transform.parent.forward;
			baseRot = Quaternion.Euler(v.x, v.y, 0);
		}
	}

    protected override void Update()
    {
        if (open)
        {
			//Open the door
			TargetAngle = MaxRot;
            if (Turning) {
                Rotation();
            }
            
        }
        else
        {
			//Close the door
			TargetAngle = MinRot;
            if (Turning) {
                Rotation();
            }

        }
    }

    protected override void ColliderEnter()
    {
        ToggleOpen();
        Turning = true;
    }

    protected override void ColliderExit()
    {
        ToggleOpen();
        Turning = true;
    }

    protected override void PulseReceived() {
        ToggleOpen();
        Turning = true;
    }

    protected override void SwitchReceived() {
        ToggleOpen();
        Turning = true;
    }

    protected void Rotation()
    {
        if (CurrentTime < Duration)
        {
            CurrentTime += Time.deltaTime;
            float progress = CurrentTime / Duration;

			// Interpolate to get the current angle/axis between the source and target.
			float currentAngle = Mathf.Lerp(Facing, TargetAngle, progress);
            this.transform.parent.rotation = Quaternion.AngleAxis(currentAngle, TargetAxis)*baseRot;
        }
        else
        {
            Turning = false;
            sourceOrientation = this.transform.parent.rotation;
            sourceOrientation.ToAngleAxis(out Facing, out SourceAxis);
			/*if (yRot) {
				TargetAxis = transform.parent.up;
			} else {
				TargetAxis = transform.parent.right;
			}*/
            CurrentTime = 0;
        }
    }
}
