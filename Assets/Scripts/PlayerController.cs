using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	[SerializeField] private Transform fireLocation;
	[SerializeField] private float speedUpTime = 20f;
	[SerializeField] private GameObject speedUpCounter;

	private CharacterController characterController;
	private Animator anim;
	private GameObject arrow;
	private float timerSinseSpeedUp = 0f;
	private float moveSpeed = 5f;
	private GameObject fireTrail;
	private ParticleSystem fireTrailParticles;

	// Use this for initialization
	void Start () {
		fireTrail = GameObject.FindWithTag ("Fire") as GameObject;
		fireTrail.SetActive (false);
		characterController = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		arrow = GameManager.instance.Arrow;
		timerSinseSpeedUp = speedUpTime;
	}
	
	// Update is called once per frame
	void Update () {

		if (!GameManager.instance.GameOver && !GameManager.instance.GamePaused) {

			Vector3 aimDirection = new Vector3 (CrossPlatformInputManager.GetAxis ("HorizontalA"), 0, CrossPlatformInputManager.GetAxis ("VerticalA"));

			Vector3 moveDirection = new Vector3 (CrossPlatformInputManager.GetAxis ("HorizontalM"), 0, CrossPlatformInputManager.GetAxis ("VerticalM"));

			if (aimDirection.Equals (Vector3.zero))
			{
				anim.SetBool ("isAiming", false);

				if (moveDirection.Equals (Vector3.zero))
				{
					anim.SetBool ("isWalking", false);
				}
				else
				{
					anim.SetBool ("isWalking", true);

					Quaternion rotation = Quaternion.LookRotation (moveDirection, Vector3.up);
					transform.rotation = rotation;
				}
			}
			else
			{
				Vector3 localMove = transform.InverseTransformDirection (moveDirection);

				anim.SetBool ("isAiming", true);
				anim.SetFloat ("x", localMove.x, 0.1f, Time.deltaTime);
				anim.SetFloat ("y", localMove.z, 0.1f, Time.deltaTime);

				Quaternion rotation = Quaternion.LookRotation (aimDirection, Vector3.up);
				transform.rotation = rotation;
			}

			characterController.SimpleMove (moveDirection * moveSpeed);
		}
	}

	public void FireArrow(){

		Vector3 aimDirection = new Vector3 (CrossPlatformInputManager.GetAxis ("HorizontalA"), 0, CrossPlatformInputManager.GetAxis ("VerticalA"));
		GameObject newArrow = Instantiate (arrow, fireLocation.position, Quaternion.LookRotation (aimDirection)) as GameObject;
		newArrow.GetComponent<Rigidbody> ().velocity = newArrow.transform.forward * 25;
	}

	public void PowerUpSpeed(){
		moveSpeed = 10f;
		fireTrail.SetActive (true);
		timerSinseSpeedUp = speedUpTime;
		speedUpCounter.SetActive (true);
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown ()
	{
			for (int i = 0; i < 20; i++) {
				speedUpCounter.GetComponentInChildren<Text> ().text = timerSinseSpeedUp.ToString ();
				yield return new WaitForSeconds (1f);
				timerSinseSpeedUp--;
			}
		moveSpeed = 5f;
		speedUpCounter.SetActive (false);
		fireTrailParticles = fireTrail.GetComponent<ParticleSystem> ();
		var em = fireTrailParticles.emission;
		em.enabled = false;
		yield return new WaitForSeconds (3);
		em.enabled = true;
		fireTrail.SetActive (false);
	}
}
