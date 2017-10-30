using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUp : MonoBehaviour {

	[SerializeField] private AudioClip audio;

	private GameObject player;
	private PlayerController playerController;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player;
		playerController = player.GetComponent<PlayerController> ();
		audioSource = player.GetComponent<AudioSource> ();
		GameManager.instance.RegisterPowerUp ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject == player) {
			audioSource.PlayOneShot (audio);
			playerController.PowerUpSpeed ();
			Destroy (gameObject);
			GameManager.instance.UnRegisterPowerUp ();
		}
	}
}
