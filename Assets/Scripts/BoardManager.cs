using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {


	//Count class is used to specify and keep track the number of floors, walls....
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
	
		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	//varible declaration for a game board
	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9); // a random range to generate number of walls
	public Count foodCount = new Count(1, 5); // a randome range to generate number of food

	public GameObject exit; //use to hold exit prefeb
	public GameObject[] floorTiles; //store floor prefabs
	public GameObject[] wallTiles; //store wall prefabs
	public GameObject[] foodTiles; //store food prefabs
	public GameObject[] enemyTiles; //enemyTiles;
	public GameObject[] outerWallTiles; //store outer wall prefabs;

	private Transform boardHolder; //used to child spawned game objects - keep the hierarchy clean
	private List<Vector3> gridPositions = new List<Vector3>(); //a list of postions where game objects will be created 

	//a method to generate a list of positions
	private void InitialiseList()
	{
		gridPositions.Clear(); //reset the length of the list to 0

		//a loop to create a list of postions.
		for(int x = 1; x < columns; x++)
		{
			for(int y = 1; y < rows; y++)
			{
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}

	//a method to setup the outer wall and floor for the game
	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform; //create a gameobject named "Board"

		for(int x = -1; x < columns + 1; x++)
		{
			for(int y = -1; y < rows + 1; y++)
			{
				//choose a random floor tile from floor tile prefabs
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
				//a condition to choose the outer wall to instantiate instead of floorTiles.
				if(x == -1 || x == columns || y == -1 || y == rows){
					toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];
				}

				//instantiate a game object 
				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;

				//child the game object to boardHolder to keep the hierarchy clean.
				instance.transform.SetParent(boardHolder);
			}
		}
		
	}

	//Choose a random position from the list of grid postions.
	Vector3 RandomPostion(){
		int randomIndex = Random.Range (0, gridPositions.Count); //random in index in the list
		Vector3 randomPostion = gridPositions [randomIndex]; // store the random position of the gridPostions 
		gridPositions.RemoveAt (randomIndex); //remove the random postion from the list so we wont instantiate objects at the same position.
		return randomPostion; 
	}

	//choose the number of min and max of objects that need to be spawned.
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1); //keep track the number of objects that will be spawned

		for(int i = 0; i < objectCount; i++)
		{
			Vector3 randomPosition = RandomPostion();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	//will be called from the game manager class to set up the board
	public void SetupScene(int level)
	{
		Debug.Log ("I was about to set up a scene");
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}

}
