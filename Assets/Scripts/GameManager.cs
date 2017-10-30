using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	[SerializeField] private GameObject orc;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject arrow;
	[SerializeField] private GameObject fireBall;
	[SerializeField] private GameObject grenade;
	[SerializeField] private GameObject[] spawnPoints;
	[SerializeField] private GameObject[] powerUpSpawns;
	[SerializeField] private GameObject[] towerSpawns;
	[SerializeField] private GameObject[] powerUpPrefabs;
	[SerializeField] private GameObject tower;
	[SerializeField] int maxPowerUps = 1;
	[SerializeField] private Text killsText;
	[SerializeField] private Text WaveText;
	[SerializeField] private Text grenadeText;
	[SerializeField] private Text myScore;
	[SerializeField] private Text bestScore;
	[SerializeField] private Canvas inGameUI;
	[SerializeField] private Canvas endGameUI;
	[SerializeField] private Canvas menu;
	[SerializeField] private Animator anim;
	[SerializeField] private AudioClip clip;

	private bool gamePaused;
	private bool gameOver;
	private bool gameStarted;
	private int currentLevel;
	private int currentWave;
	private float generatedSpawnTime = 1f;
	private float currentSpawnTime = 0f;
	private float powerUpSpawnTime = 60f;
	private float currentPowerUpSpawnTime = 0;
	private float waveEnabledTime = 3f;
	private float currentWaveEnabledTime = 0;
	private GameObject newEnemy;
	private int powerups = 0;
	private GameObject newPowerup;
	private AudioSource audio;
	private int kills = 0;
	private int towerCount = 0;
	private int grenadeCount = 0;

	private List<EnemyHealth> enemies = new List<EnemyHealth> ();
	private List<EnemyHealth> killedEnemies = new List<EnemyHealth> ();

	void Awake ()
	{

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		if (!FB.IsInitialized) {
			FB.Init ();
		} else {
			FB.ActivateApp ();
		}
	}

	// Use this for initialization
	void Start ()
	{
		endGameUI.enabled = false;
		gameOver = true;
		gameStarted = false;
		gamePaused = false;
		currentLevel = 5;
		killsText.text = "Kills " + kills;
		grenadeText.text = "" + grenadeCount;
		currentWave = 1;
		WaveText.text = "Wave\n" + currentWave;
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameStarted && !gamePaused) {
			if (WaveText.enabled) {
				currentWaveEnabledTime += Time.deltaTime;
			}
			if (currentWaveEnabledTime >= waveEnabledTime) {
				WaveText.enabled = false;
			}
			currentSpawnTime += Time.deltaTime;
			currentPowerUpSpawnTime += Time.deltaTime;

			switch (kills) {
			case 100:
				if (towerCount == 0) {
					GameObject a = Instantiate (tower, towerSpawns [0].transform.position, Quaternion.Euler (-90f, 0f, 0f)) as GameObject;
					towerCount = 1;
				}
				break;
			case 200:
				if (towerCount == 1) {
					GameObject b = Instantiate (tower, towerSpawns [2].transform.position, Quaternion.Euler (-90f, 0f, 0f)) as GameObject;
					towerCount = 2;
				}
				break;
			case 300:
				if (towerCount == 2) {
					GameObject c = Instantiate (tower, towerSpawns [1].transform.position, Quaternion.Euler (-90f, 0f, 0f)) as GameObject;
					towerCount = 3;
				}
				break;
			case 400:
				if (towerCount == 3) {
					GameObject d = Instantiate (tower, towerSpawns [3].transform.position, Quaternion.Euler (-90f, 0f, 0f)) as GameObject;
					towerCount = 4;
				}
				break;
			default:
				break;
			}
		}
	}

	IEnumerator Spawn ()
	{
		if (!gameOver && !gamePaused && currentSpawnTime >= generatedSpawnTime) {
			currentSpawnTime = 0f;

			if (enemies.Count < currentLevel) {
				int randomNumber = Random.Range (0, spawnPoints.Length);
				GameObject spawnPoint = spawnPoints [randomNumber];
				newEnemy = Instantiate (orc, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
			}

			if (currentLevel == killedEnemies.Count) {
				enemies.Clear ();
				killedEnemies.Clear ();
				yield return new WaitForSeconds (3f);
				currentLevel += 5;
				currentWave++;
				WaveText.text = "Wave\n" + currentWave;
				WaveText.enabled = true;
				currentWaveEnabledTime = 0;
			}
		}
		yield return null;
		StartCoroutine (Spawn ());
	}

	IEnumerator PowerUpSpawn ()
	{
		if (!gameOver && !gamePaused && currentPowerUpSpawnTime > powerUpSpawnTime) {
			currentPowerUpSpawnTime = 0;

			if (powerups < maxPowerUps) {

				int randomNumber = Random.Range (0, powerUpSpawns.Length);
				GameObject spawnLocation = powerUpSpawns [randomNumber];
				int random = Random.Range (0, powerUpPrefabs.Length);
				newPowerup = Instantiate (powerUpPrefabs[random], spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
			}
		}

		yield return null;
		StartCoroutine (PowerUpSpawn ());
	}

	IEnumerator EndGame ()
	{
		inGameUI.enabled = false;

		int best = kills;
		bool exists = PlayerPrefs.HasKey ("best");
		if (exists) {
			best = PlayerPrefs.GetInt ("best");
			if (best < kills) {
				best = kills;
			}
		}
		myScore.text = "MY SCORE: " + kills;
		bestScore.text = "BEST SCORE: " + best;
		PlayerPrefs.SetInt ("best", best);

		endGameUI.enabled = true;

		yield return new WaitForSeconds (1f);

		if (Advertisement.IsReady()) {
			Advertisement.Show ();
		}
		yield return null;
	}

	public void RegisterEnemy (EnemyHealth enemy)
	{
		enemies.Add (enemy);
	}

	public void KillEnemy (EnemyHealth enemy)
	{
		killedEnemies.Add (enemy);
		kills++;
		killsText.text = "Kills " + kills;
	}

	public void RegisterPowerUp ()
	{
		powerups++;
	}

	public void UnRegisterPowerUp ()
	{
		powerups--;
	}

	public void AddGrenade ()
	{
		grenadeCount++;
		grenadeText.text = "" + grenadeCount;
	}

	public void DropGrenade(){
		if (grenadeCount > 0) {
			GameObject newGrenade = Instantiate (grenade, player.transform.position, player.transform.rotation);
			grenadeCount--;
			grenadeText.text = "" + grenadeCount;
		}
	}

	public void StartGame ()
	{
		gameStarted = true;
		menu.enabled = false;
		anim.enabled = true;
		gameOver = false;
		StartCoroutine (Spawn ());
		StartCoroutine (PowerUpSpawn ());
		audio.clip = clip;
		audio.Play ();
	}

	public void Exit ()
	{
		Application.Quit ();
	}

	public GameObject Player {
		get{ return player; }
	}

	public GameObject Orc {
		get{ return orc; }
	}

	public GameObject Arrow {
		get{ return arrow; }
	}

	public GameObject FireBall {
		get{ return fireBall; }
	}

	public bool GameOver {
		get{ return gameOver; }
	}

	public bool GamePaused {
		get{ return gamePaused; }
	}

	public bool GameStarted {
		get{ return gameStarted; }
	}

	public List<EnemyHealth> Enemies {
		get{ return enemies; }
	}

	public void PlayerHit (int currentHP)
	{

		if (currentHP > 0) {
			gameOver = false;
		} else {
			gameOver = true;
			gameStarted = false;
			StartCoroutine (EndGame ());
		}
	}

	public void Pause (){
		gamePaused = true;
		inGameUI.transform.FindChild ("Joysticks").gameObject.SetActive (false);
		inGameUI.transform.FindChild ("PauseMenu").gameObject.SetActive (true);
	}

	public void Resume (){
		gamePaused = false;
		inGameUI.transform.FindChild ("Joysticks").gameObject.SetActive (true);
		inGameUI.transform.FindChild ("PauseMenu").gameObject.SetActive (false);
	}

	public void Restart (){
		SceneManager.LoadScene ("Demo6");
	}

	public void ShareFB ()
	{
		FB.ShareLink (new System.Uri ("https://itunes.apple.com/us/app/no-mans-land-apocalypse/id1234116503"),
			"No Man's Land : Apocalypse",
			"I have killed " + kills + " orcs in \"No Man's Land : Apocalypse\".\n",
			new System.Uri ("https://firebasestorage.googleapis.com/v0/b/rent-car-tunisia-tizen.appspot.com/o/icon.jpg?alt=media&token=6df8072a-ca49-4ca6-8641-d4bf94a22d75"),
			OnShareFB);
	}

	private void OnShareFB (IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("ShareLink error: " + result.Error);
		} else if (!string.IsNullOrEmpty (result.PostId)) {
			Debug.Log (result.PostId);
		} else
			Debug.Log ("Share succed");
	}
}
