using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//first line of csv file: num of cols, nums of rows, row of player, col of player, number of enemies, number of levels

//then a tale that describes the contents of each cell in the level. 

//1 - wall
//2 - blank space (with a dot)
//3 - junction
//4 - blank space with no dots

//then a line with chase, scatter, chase, sctatter (etc) cycle length (in seconds) for each level. After the game finishes all the specified cycles in the level and its not complete yet, it goes into permament chase mode.

public class LevelGenerator : MonoBehaviour {

	private GameObject _floor, _block;

	private static int[,] level;
	private static GameObject axisOrigin;

	public static int rows, cols;

	string[] numsInitial;
	string[] nums;

	int numOfLine = 0;
	[SerializeField] private GameObject floorPrefab;
	[SerializeField] private GameObject blockPrefab;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject enemyPrefabRed;
	[SerializeField] private GameObject enemyPrefabPink;	
	[SerializeField] private GameObject enemyPrefabLightBlue;	
	[SerializeField] private GameObject enemyPrefabOrange;
	[SerializeField] private GameObject dotPrefab;

	private List<int[]> enemyPositions = new List<int[]>();

 	void Awake() {
        Application.targetFrameRate = 120;
    }


	public static int[,] getMap() {
		return level;
	}

	public static Vector3 GetAxisOrigin() {
		return axisOrigin.transform.position;
	}

	public static int GetRows() {
		return rows;
	}

	public static int GetCols() {
		return cols;
	}

	public static void PlaceGameObject(GameObject obj, int[] coordinates) { //place in-game object at specific map coordinates
		GameObject obj1 = Instantiate (obj) as GameObject;
		obj1.transform.parent = axisOrigin.transform;

		float[] pos = MapUtility.TranslateMapToLevelCoordinates (coordinates);

		obj1.transform.localPosition = new Vector3 (pos [1], 2.55f, pos [0]); 

	}

	public static GameObject PlaceGameObject(GameObject obj, int[] coordinates, string name) { //place in-game object at specific map coordinates and name it
		GameObject obj1 = Instantiate (obj) as GameObject;
		obj1.transform.parent = axisOrigin.transform;

		float[] pos = MapUtility.TranslateMapToLevelCoordinates (coordinates);

		obj1.transform.localPosition = new Vector3 (pos [1], 2.55f, pos [0]); 

		obj1.name = name;

		return obj1;
	}

	 void Start () { //read level from file and generate an array representing it and the level itself in game

		TextAsset levelDataAsset = Resources.Load("level") as TextAsset;
		numsInitial = levelDataAsset.ToString().Split('\n');
	
		nums = numsInitial[numOfLine].Split(',');
		numOfLine++;
		
		rows = int.Parse (nums[0]);
		cols = int.Parse (nums[1]);

		int playerRow = int.Parse (nums [2]);
		int playerCol = int.Parse (nums [3]);

		int numOfEnemies = int.Parse (nums [4]);

		level = new int[rows, cols];

		axisOrigin = new GameObject ();
	
		axisOrigin.transform.position = new Vector3 ((float)((cols * -10) / 2), 2.55f, (float)((rows * -10) / 2));
		axisOrigin.name = "Axis Origin";

		GameFunctions.SetAxisOrigin(axisOrigin);

		_floor = Instantiate (floorPrefab) as GameObject;

		_floor.transform.localScale = new Vector3 (cols * 10, 0.1f, rows * 10);

		GameObject playerObject = PlaceGameObject(playerPrefab, new int[]{playerRow, playerCol}, "player"); //add player object

		GameFunctions.SetPlayerObject(playerObject);

		for (int i = 0; i < numOfEnemies; i++) { //add enemy objects

			nums = numsInitial[numOfLine].Split(',');
			numOfLine++;

			int[] coordinates = new int[] { int.Parse(nums [0]), int.Parse(nums [1]) };

			enemyPositions.Add(coordinates);

			string enemyColor = nums[2];

			if(enemyColor == "r") {
				PlaceGameObject (enemyPrefabRed, coordinates, "r_enemy" + i.ToString());
			}
			if(enemyColor == "p") {
				PlaceGameObject (enemyPrefabPink, coordinates, "p_enemy" + i.ToString());
			}
			if(enemyColor == "b") {
				PlaceGameObject (enemyPrefabLightBlue, coordinates, "lb_enemy" + i.ToString());
			}
			if(enemyColor == "o") {
				PlaceGameObject (enemyPrefabOrange, coordinates, "o_enemy" + i.ToString());
			}
		}

		for (int i = 0; i < rows; i++) { //place walls and dots

			nums = numsInitial[numOfLine].Split(',');
			numOfLine++;

			for (int j = 0; j < cols; j++) {

				int[] coordinates = new int[]{i, j};

				level [i,j] = int.Parse(nums [j]);

				if (int.Parse (nums [j]) == 1) {
					
					PlaceGameObject(blockPrefab, coordinates, "block" + i + "-" + j);			
				}
				if(int.Parse(nums[j]) == 2 || int.Parse(nums[j]) == 3 && !enemyPositions.Contains(coordinates)) {
					PlaceGameObject(dotPrefab, coordinates, "dot" + i + "-" + j);
					GameFunctions.AddToNumOfDots();
				}
			}

		}

		

	}
	

	 void Update () {
		
	}
}
