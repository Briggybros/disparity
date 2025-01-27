﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformBehaviourScript : Receiver
{
  public bool Lock, Rotating, TwoLocation;
  public int Increment;
  public float Duration, TargetAngle, Facing;

  private float CurrentTime;
  private Vector3 TargetAxis;
  private bool pulse;

  protected override void Start()
  {
    CurrentTime = 0;
    TargetAngle = Increment;
    Quaternion sourceOrientation = this.transform.rotation;
    Facing = sourceOrientation.eulerAngles.y;
    TargetAxis = transform.up;
  }

  protected override void Update()
  {
    if (Rotating)
    {
      Rotation();
    }
    else
    {
      if (!Lock)
      {
        Rotating = true;
        if (pulse)
        {
          ToggleLock();
        }
      }
    }
  }

  protected void Rotation()
  {
    if (CurrentTime < Duration)
    {
      CurrentTime += Time.deltaTime;
      float progress = CurrentTime / Duration;

      if (TargetAngle > -0.5 && TargetAngle < 0.5)
      {
        TargetAngle = 360;
      }
      float currentAngle = Mathf.Lerp(Facing, TargetAngle, progress);
      this.transform.rotation = Quaternion.AngleAxis(currentAngle, TargetAxis);
    }
    else
    {
      Rotating = false;
      Quaternion sourceOrientation = this.transform.rotation;
      Facing = sourceOrientation.eulerAngles.y;
      TargetAxis = transform.up;
      CurrentTime = 0;
      if (TwoLocation)
      {
        Increment = -Increment;
      }
      TargetAngle = Facing + Increment;
    }
  }

  protected void ToggleLock()
  {
    Lock = !Lock;
  }

  protected override void ColliderEnter()
  {
    ToggleLock();
  }

  protected override void ColliderExit()
  {
    ToggleLock();
  }

  protected override void PulseReceived()
  {
    ToggleLock();
    pulse = true;
  }

  protected override void SwitchReceived()
  {
    ToggleLock();
  }
}
