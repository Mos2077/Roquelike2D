using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float turnDelay = 0.1f;
	public float levelStartDelay = 2f;

	public static GameManager instance = null;
	private BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;


	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

	
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	//OnlevelWasLoaded is part of Unity UI. It is called every time a scene is loaded
	private void OnLevelWasLoaded(int index){
	
		level++;

		InitGame ();
	}

	void InitGame(){
		Debug.Log ("InitGame() is called");

		doingSetup = true;
		
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		enemies.Clear (); //clear any enemies out from the last level
		boardScript.SetupScene (level);
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver(){
		levelText.text = "After " + level + " days, you starved";
		levelImage.SetActive (true);
		enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if(playersTurn || enemiesMoving || doingSetup){
			return;
		}

		StartCoroutine (MoveEnemies());
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if(enemies.Count == 0){
			yield return new WaitForSeconds(turnDelay);
		}

		for(int i = 0; i < enemies.Count; i++){
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
		 
		playersTurn = true;
		enemiesMoving = false;
	}
}
