using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour {

	[SerializeField] private AudioClip audio;

	private GameObject player;
	private PlayerHealth playerHealth;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth> ();
		audioSource = player.GetComponent<AudioSource> ();
		GameManager.instance.RegisterPowerUp ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject == player) {
			audioSource.PlayOneShot (audio);
			playerHealth.PowerUpShield ();
			Destroy (gameObject);
			GameManager.instance.UnRegisterPowerUp ();
		}
	}
}
