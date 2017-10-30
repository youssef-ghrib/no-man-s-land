using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

	[SerializeField] float timeBetweenAttacks;
	[SerializeField] float attackRadius;
	private GameObject fireBall;
	private EnemyHealth targetEnemy = null;
	private float attackCounter;
	private bool isAttacking = false;

	// Use this for initialization
	void Start ()
	{
		fireBall = GameManager.instance.FireBall;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.instance.GameOver && !GameManager.instance.GamePaused && GameManager.instance.GameStarted) {
			attackCounter -= Time.deltaTime;

			if (targetEnemy == null || !targetEnemy.IsAlive) {
				EnemyHealth nearestsEnemyInRange = GetNearestsEnemyInRange ();
				if (nearestsEnemyInRange != null && Vector3.Distance (transform.localPosition, nearestsEnemyInRange.transform.localPosition) <= attackRadius) {
					targetEnemy = nearestsEnemyInRange;
				}
			} else {
				if (attackCounter <= 0f) {
					isAttacking = true;
					attackCounter = timeBetweenAttacks;
				} else {
					isAttacking = false;
				}
				if (Vector3.Distance (transform.localPosition, targetEnemy.transform.localPosition) > attackRadius) {
					targetEnemy = null;
				}
			}
		}
	}

	void FixedUpdate ()
	{
		if (isAttacking) {
			Attack ();
		}
	}

	public void Attack ()
	{
		isAttacking = false;
		GameObject newFireBall = Instantiate (fireBall, transform.localPosition + Vector3.up * 5f, Quaternion.identity) as GameObject;

		if (targetEnemy == null) {
			Destroy (newFireBall);
		} else {
			StartCoroutine (MoveFireBall (newFireBall));
		}
	}

	IEnumerator MoveFireBall (GameObject fireBall)
	{
		while (GetTargetDistance (targetEnemy) > 0.2f && fireBall != null && targetEnemy != null) {
			Vector3 dir = targetEnemy.transform.localPosition - transform.localPosition;
			fireBall.transform.localPosition = Vector3.MoveTowards (fireBall.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
			yield return null;
		}
		if (fireBall != null || targetEnemy == null) {
			Destroy (fireBall);
		}
	}

	private float GetTargetDistance (EnemyHealth currentEnemy)
	{
		if (currentEnemy == null) {
			currentEnemy = GetNearestsEnemyInRange ();
			if (currentEnemy == null) {
				return 0f;
			}
		}
		return Mathf.Abs (Vector3.Distance (transform.localPosition, currentEnemy.transform.localPosition));
	}

	private List<EnemyHealth> GetEnemiesInRange ()
	{
		List<EnemyHealth> enemiesInRange = new List<EnemyHealth> ();
		foreach (EnemyHealth enemy in GameManager.instance.Enemies) {
			if (enemy.IsAlive) {
				if (Vector3.Distance (transform.localPosition, enemy.transform.localPosition) <= attackRadius) {
					enemiesInRange.Add (enemy);
				}
			}
		}
		return enemiesInRange;
	}

	private EnemyHealth GetNearestsEnemyInRange ()
	{
		EnemyHealth nearestsEnemyInRange = null;
		float smallestDistance = float.PositiveInfinity;
		foreach (EnemyHealth enemyInRange in GetEnemiesInRange()) {
			if (Vector3.Distance (transform.localPosition, enemyInRange.transform.localPosition) < smallestDistance) {
				nearestsEnemyInRange = enemyInRange;
				smallestDistance = Vector3.Distance (transform.localPosition, enemyInRange.transform.localPosition);
			}
		}
		return nearestsEnemyInRange;
	}
}
