﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
  private int count;

  void Start()
  {
    count = 0;
  }

  void Update()
  {
    if (CompareTag("Bopped"))
    {
      count++;
      this.transform.Translate(Vector3.up * Time.deltaTime);
    }
    if (count > 60)
    {
      this.gameObject.GetComponent<Renderer>().enabled = false;
      foreach (Collider c in gameObject.GetComponents<Collider>())
      {
        c.enabled = false;
      }
    }
  }
}
