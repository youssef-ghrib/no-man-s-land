using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField] private int startingHealth = 100;
	[SerializeField] private float timeSinceLastHit = 2f;
	[SerializeField] private float shieldUpTime = 20f;
	[SerializeField] private Slider healthBar;
	[SerializeField] private GameObject shieldCounter;

	private float timer = 0f;
	private float timerSinseShieldUp = 0f;
	private CharacterController characterController;
	private Animator anim;
	private int currentHealth;
	private AudioSource audio;
	private ParticleSystem blood;
	private bool shieldUp = false;

	// Use this for initialization
	void Start ()
	{
		characterController = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		currentHealth = startingHealth;
		audio = GetComponent<AudioSource> ();
		blood = GetComponentInChildren<ParticleSystem> ();
		timerSinseShieldUp = shieldUpTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		if (timerSinseShieldUp == 0) {
			StopCoroutine (Countdown ());
			shieldUp = false;
			Behaviour halo = (Behaviour)GetComponent ("Halo");
			halo.enabled = false;
			shieldCounter.SetActive (false);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (timer >= timeSinceLastHit && !GameManager.instance.GameOver && !GameManager.instance.GamePaused && !shieldUp) {
			if (other.tag == "Weapon") {
				TakeHit ();
				timer = 0f;
			}
		}
	}

	void TakeHit ()
	{
		currentHealth -= 10;
		healthBar.value = currentHealth;
		GameManager.instance.PlayerHit (currentHealth);
		audio.PlayOneShot (audio.clip);
		blood.Play ();

		if (currentHealth > 0) {
			anim.Play ("Hit");
		} else {
			KillPlayer ();
		}
	}

	void KillPlayer ()
	{
		anim.SetTrigger ("HeroDie");
		characterController.enabled = false;
	}

	public void PowerUpHealth ()
	{
		if (currentHealth <= 70) {
			currentHealth += 30;
		} else if (currentHealth < startingHealth) {
			currentHealth = startingHealth;
		}
		healthBar.value = currentHealth;
	}

	public void PowerUpShield ()
	{
		Behaviour halo = (Behaviour)GetComponent ("Halo");
		halo.enabled = true;
		shieldUp = true;
		timerSinseShieldUp = shieldUpTime;
		shieldCounter.SetActive (true);
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown ()
	{
		if (shieldUp) {
			for (int i = 0; i < 20; i++) {
				shieldCounter.GetComponentInChildren<Text> ().text = timerSinseShieldUp.ToString ();
				yield return new WaitForSeconds (1f);
				timerSinseShieldUp--;
			}
		}
	}
}
