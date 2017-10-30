using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamStart : MonoBehaviour {

	[SerializeField] private Canvas menu;
	[SerializeField] private Canvas inGameUI;

	private Animator anim;
	private CameraFollow cameraFollow;

	// Use this for initialization
	void Start () {
		menu.enabled = true;
		cameraFollow = GetComponent<CameraFollow> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnAnimationEnd() {
		inGameUI.enabled = true;
		anim.enabled = false;
		cameraFollow.enabled = true;
	}
}
