using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager:MonoBehaviour{

	[Serializable]
	public class Count{
		public int minimum;
		public int maximum;

		public Count(int min, int max){
			minimum = min;
			maximum  = max;
		}
	}
	// a bunch of self explanitory variables
	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount =  new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	// so we dont have a bunch of game objects cluttering the hierarchy
	private Transform boardHolder;
	// list of valid locations
	private List <Vector3> gridPositions = new List<Vector3>();

	void InitialiseList(){
		gridPositions.Clear();

		// loop along the x and y axis, -1 so we dont create impassable levels
		for(int x=1; x<columns-1; x++){
			for(int y=1; y<rows-1; y++){
				// add coordinates to the list of valid locations
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject("Board").transform;

		// loop along the x and y axis, -1 and +1 to reach outerwall tiles
		for(int x=-1; x<columns+1; x++){
			for(int y=-1; y<rows+1; y++){
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
				// check if we are at an outer wall tile
				if(x==-1 || x==columns || y==-1 || y==rows){
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}
				// instantiate the chosen tile
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                // adds the created object to the boardHolder
                instance.transform.SetParent(boardHolder);
			}
		}
	}

    Vector3 RandomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max){
        int objectCount = Random.Range(min, max+1);

        for (int i=0; i<objectCount; i++){
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level){
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        // more difficult as time goes by, 1 enemy at level 2, 2 at level 4, ...
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

		int randomExit = Random.Range(0, 2);

		if(randomExit == 0){
			Instantiate(exit, new Vector3(0, rows-1, 0f), Quaternion.identity);

		}else if(randomExit == 1){
			Instantiate(exit, new Vector3(columns-1, 0, 0f), Quaternion.identity);

		}else{
			Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
		}
    }
}