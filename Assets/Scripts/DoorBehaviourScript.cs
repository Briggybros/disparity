﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DoorBehaviourScript : Receiver {

    [SyncVar]
    public bool open;

    public Vector3 target,home;
	public int timer;

    protected virtual void ToggleOpen(){
        Debug.Log("toggled");
        open = !open;
    }

    protected override void ColliderEnter(){
        Debug.Log("collider enter");
        ToggleOpen();
    }

    protected override void ColliderExit(){
        Debug.Log("Collider Exit");
        ToggleOpen();
    }

	protected override void PulseReceived() { 
		StartCoroutine(TimerWait());
	}

    protected override void SwitchReceived() {
        ToggleOpen();
    }

	IEnumerator TimerWait() {
		ToggleOpen();
		yield return new WaitForSeconds(timer);
		ToggleOpen();
	}

    protected void init()
    {
        open = false;
        target = transform.position;
        home = transform.position;
    }

    protected void Looping()
    {
        StartCoroutine(TimerWaitLoop());
    }

    IEnumerator TimerWaitLoop() {
        while(true)
        {
            ToggleOpen();
            yield return new WaitForSeconds(timer);
            ToggleOpen();
            yield return new WaitForSeconds(timer);
        }
	}
}
