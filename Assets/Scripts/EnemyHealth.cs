using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	[SerializeField] private int startingHealth = 20;
	[SerializeField] private float timeSinseLastHit = 0.5f;
	[SerializeField] private float dissapearSpeed = 2f;

	private AudioSource audio;
	private float timer = 0f;
	private Animator anim;
	private NavMeshAgent nav;
	private bool isAlive;
	private Rigidbody rigidBody;
	private CapsuleCollider capsuleCollider;
	private bool dissapearEnemy = false;
	private int currentHealth;
	private ParticleSystem blood;


	// Use this for initialization
	void Start ()
	{
		GameManager.instance.RegisterEnemy (this);
		rigidBody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		isAlive = true;
		currentHealth = startingHealth;
		blood = GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (dissapearEnemy == true)
		{
			transform.Translate (Vector3.down * dissapearSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(timer >= timeSinseLastHit && !GameManager.instance.GameOver && !GameManager.instance.GamePaused){
			if(other.tag == "Arrow"){
				TakeHit ();
				timer = 0f;
			}
		}
		if(other.tag == "FireBall"){
			KillEnemy ();
		}

		if(other.tag == "Grenade"){
			Destroy (other.gameObject);
			KillEnemy ();
		}
	}

	void TakeHit(){
		currentHealth -= 10;
		audio.PlayOneShot (audio.clip);
		blood.Play ();

		if (currentHealth > 0) {
			anim.Play ("Hit");
		} else {
			KillEnemy ();
		}
	}

	void KillEnemy()
	{
		isAlive = false;
		GameManager.instance.KillEnemy(this);
		capsuleCollider.enabled = false;
		nav.enabled = false;
		anim.SetTrigger ("EnemyDie");
		rigidBody.isKinematic = true;

		StartCoroutine (RemoveEnemy ());
	}

	IEnumerator RemoveEnemy()
	{
		yield return new WaitForSeconds (4f);
		dissapearEnemy = true;
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}

	public bool IsAlive
	{
		get {return isAlive;}
	}
}
