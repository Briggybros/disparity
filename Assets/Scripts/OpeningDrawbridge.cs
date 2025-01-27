﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDrawbridge : Receiver
{
  public GameObject Drawbridge;

  private Quaternion newRotation;
  private bool opened;

  protected override void Start()
  {
    opened = false;
  }
	
  protected override void SwitchReceived()
  {
    StopAllCoroutines();
    if (opened == false)
    {
      opened = true;
      StartCoroutine(OpenDrawbridge());
    }
    else
    {
      opened = false;
      StartCoroutine(CloseDrawbridge());
    }
  }

  IEnumerator OpenDrawbridge()
  {
    newRotation = Quaternion.Euler(-90, Drawbridge.transform.rotation.y, Drawbridge.transform.rotation.z);
    while (Drawbridge.transform.rotation.x > -90)
    {
      Drawbridge.transform.rotation = Quaternion.Lerp(Drawbridge.transform.rotation, newRotation, 2 * Time.deltaTime);
      yield return 0;
    }
  }

  IEnumerator CloseDrawbridge()
  {
    newRotation = Quaternion.Euler(0, Drawbridge.transform.rotation.y, Drawbridge.transform.rotation.z);
    while (Drawbridge.transform.rotation.x < 0)
    {
      Drawbridge.transform.rotation = Quaternion.Lerp(Drawbridge.transform.rotation, newRotation, 2 * Time.deltaTime);
      yield return 0;
    }
  }
}
