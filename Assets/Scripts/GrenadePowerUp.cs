using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePowerUp : MonoBehaviour {

	[SerializeField] private AudioClip audio;

	private GameObject player;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player;
		audioSource = player.GetComponent<AudioSource> ();
		GameManager.instance.RegisterPowerUp ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject == player) {
			audioSource.PlayOneShot (audio);
			Destroy (gameObject);
			GameManager.instance.UnRegisterPowerUp ();
			GameManager.instance.AddGrenade ();
		}
	}
}
