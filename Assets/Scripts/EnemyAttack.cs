using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	[SerializeField] private float range = 3f;
	[SerializeField] private float timeBetweenAttacks = 1f;

	private GameObject player;
	private Animator anim;
	private bool playerInRange;
	private BoxCollider weaponCollider;
	private EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {

		weaponCollider = GetComponentInChildren<BoxCollider> ();
		player = GameManager.instance.Player;
		anim = GetComponent<Animator> ();
		enemyHealth = GetComponent<EnemyHealth> ();

		StartCoroutine (Attack ());
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance (transform.position, player.transform.position) < range && enemyHealth.IsAlive) {
			playerInRange = true;
		} else {
			playerInRange = false;
		}
	}

	IEnumerator Attack(){

		if (!GameManager.instance.GameOver && !GameManager.instance.GamePaused && playerInRange) {
			anim.Play ("Attack");
			yield return new WaitForSeconds (timeBetweenAttacks);
		}

		yield return null;
		StartCoroutine (Attack ());
	}

	public void EnemyAttackBegin()
	{
		weaponCollider.enabled = true;
	}

	public void EnemyAttackEnd()
	{
		weaponCollider.enabled = false;
	}
}
