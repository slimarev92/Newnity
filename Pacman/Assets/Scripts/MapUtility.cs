using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Directions {Right, Left, Up, Down, None}; //Move direction enum

public  class MapUtility : MonoBehaviour{
	
	private static int[,] level;
	
	// Use this for initialization
	void Start () {
		level = LevelGenerator.getMap ();
	}

	public static bool[] checkMoves(GameObject obj) { //

		bool[] canMove = new bool[4];

		canMove [0] = MapUtility.CanMoveRight(obj.transform.gameObject);
		canMove [1] = MapUtility.CanMoveLeft(obj.transform.gameObject);
		canMove [2] = MapUtility.CanMoveUp(obj.transform.gameObject);
		canMove [3] = MapUtility.CanMoveDown(obj.transform.gameObject);

		return canMove;
	}

	public static bool[] checkMoves(int[] coordinates) { //checks which directions from the given coordinates can moved in

		bool[] canMove = new bool[4];

		canMove [0] = MapUtility.CanMoveRight(coordinates);
		canMove [1] = MapUtility.CanMoveLeft(coordinates);
		canMove [2] = MapUtility.CanMoveUp(coordinates);
		canMove [3] = MapUtility.CanMoveDown(coordinates);

		return canMove;
	}

	public static bool isNextJunction(int[] pos, Directions direction) {//returns true if the next cell (based on given movement direction) is a junction

		int[] checkPos = GetNext(pos, (int)direction);

		if(checkPos[0] >= LevelGenerator.GetRows() || checkPos[1] >= LevelGenerator.GetCols()) {

			return false;
		}
		else if(MapUtility.GetMapCell(checkPos) == 3) {

			return true;
		} 
			
			return false;
	}

	public static int[] FindNextJunction(int[] startingCoordinates, Directions direction) { //find nearest junction based on given location and direction
		
		if(isNextWall(startingCoordinates, (int)direction)) 
			return startingCoordinates;
		if(GetMapCell(GetNext(startingCoordinates, (int)direction)) == 3)
			return GetNext(startingCoordinates, (int)direction);	
		else 
			return FindNextJunction(GetNext(startingCoordinates, (int)direction), direction);
	}

	public static bool isJunction(int[] coordinates) { //returns true if this cell a junction

		if(GetMapCell(coordinates) == 3)
			return true;

		return false; 
	}

	public static bool CanMoveUp(GameObject obj) { //gets game object, returns true if object can move up one cell on the grid

		int[] coordinates = MapUtility.TranslateWorldToMapCoordinates (obj);
		int[] upCoordinates = new int[] {coordinates[0] - 1, coordinates[1]};

		if (coordinates [0] == 0) {
			return false;
		} else if (MapUtility.GetMapCell (upCoordinates) != 1) {
			return true;
		} else {
			return false;
		}
	}

	public static bool CanMoveUp(int[] coordinates) {  //gets game object, returns true if object can move up one cell on the grid

		int[] upCoordinates = new int[] {coordinates[0] - 1, coordinates[1]};

		if(upCoordinates[0] <= 0) {
			return false;
		}


		if (MapUtility.GetMapCell (upCoordinates) ==  1) {
			return false;
		} else {
			return true;
		}


	}

	public static bool CanMoveDown(GameObject obj) {  //gets game object, returns true if object can move down one cell on the grid

		int[] coordinates = MapUtility.TranslateWorldToMapCoordinates (obj);
		int[] downCoordinates = new int[] {coordinates[0] + 1, coordinates[1]};

		if(downCoordinates[0] >= LevelGenerator.GetRows())
			return false;
		
		if (coordinates [0] == (LevelGenerator.GetRows() - 1)) {
			return false;
		} else if (MapUtility.GetMapCell (downCoordinates) != 1) {
			return true;
		} else {
			return false;
		}
	}

	public static bool CanMove(int[] coordinates, int checkDirection) { //returns true if movement from given cell to given direction is possible

		bool rValue = false;

		switch (checkDirection) {

		case 0:
			rValue = MapUtility.CanMoveRight (coordinates);
			break;
		case 1:
			rValue = MapUtility.CanMoveLeft (coordinates);
			break;
		case 2:
			rValue = MapUtility.CanMoveUp (coordinates);
			break;
		case 3:
			rValue = MapUtility.CanMoveDown (coordinates);
			break;
		}

		return rValue;
	}

	public static bool CanMove(GameObject obj, int checkDirection) { //returns true if movement of given object to given direction is possible

		int[] coordinates = MapUtility.TranslateWorldToMapCoordinates (obj);
		bool rValue = false;

		switch (checkDirection) {

		case 0:
			rValue = MapUtility.CanMoveRight (coordinates);
			break;
		case 1:
			rValue = MapUtility.CanMoveLeft (coordinates);
			break;
		case 2:
			rValue = MapUtility.CanMoveUp (coordinates);
			break;
		case 3:
			rValue = MapUtility.CanMoveDown (coordinates);
			break;
		}

		return rValue;
	}

	public static bool CanMoveDown(int[] coordinates) {  //gets coordinates, returns true if object can move down one cell on the grid

		int[] downCoordinates = new int[] {coordinates[0] + 1, coordinates[1]};

		if(downCoordinates[0] >= LevelGenerator.GetRows())
		 	return false;

	
		if (MapUtility.GetMapCell (downCoordinates) == 1) {
			return false;
		} else {
			return true;
		}
	}

	public static bool CanMoveLeft(GameObject obj) {  //gets game object, returns true if object can move left one cell on the grid

		int[] coordinates = MapUtility.TranslateWorldToMapCoordinates (obj);
		int[] leftCoordinates = new int[] {coordinates[0] , coordinates[1] - 1};

		if (coordinates [1] == 0) {
			return false;
		} else if (MapUtility.GetMapCell (leftCoordinates) != 1) {
			return true;
		} else {
			return false;
		}
	}

	public static bool CanMoveLeft(int[] coordinates) { //gets game object, returns true if object can move left one cell on the grid

		int[] leftCoordinates = new int[] {coordinates[0] , coordinates[1] - 1};

		if (coordinates [1] == 0) {
			return false;
		} else if (MapUtility.GetMapCell (leftCoordinates) == 1) {
			return false;
		} else {
			return true;
		}
	}
		
	public static bool CanMoveRight(GameObject obj) { //gets game object, returns true if object can move right one cell on the grid

		int[] coordinates = MapUtility.TranslateWorldToMapCoordinates (obj);
		int[] leftCoordinates = new int[] {coordinates[0] , coordinates[1] + 1};

		if(leftCoordinates[1] >= LevelGenerator.GetCols())
			return false;

		if (coordinates [1] == (LevelGenerator.GetRows() - 1)) {
			return false;
		} else if (MapUtility.GetMapCell (leftCoordinates) == 1) {
			return false;
		} else {
			return true;
		}
	}

	public static bool CanMoveRight(int[] coordinates) {  //gets game object, returns true if object can move right one cell on the grid

		int[] leftCoordinates = new int[] {coordinates[0] , coordinates[1] + 1};

		if(leftCoordinates[1] >= LevelGenerator.GetCols())
			return false;

		
		if (MapUtility.GetMapCell (leftCoordinates) == 1) {
			return false;
		} else {
			return true;
		}
	}
		
	public static int GetMapCell(int[] coordinates) { //return cell value from given coordinates

		int rValue = level [coordinates [0], coordinates [1]];
		
		return rValue;
	}

	public static Directions OppositeDirection(Directions direction) { //return opposite direction to given direction

		switch (direction) {
			case Directions.Right: 
				return Directions.Left;	
			case Directions.Left:
				return Directions.Right;
			case Directions.Up: 	
				return Directions.Down;
			case Directions.Down:
				return Directions.Up;
			default: 	
				return Directions.None;	
		}
	}

	public static int[] TranslateWorldToMapCoordinates(GameObject obj) { //translates in-level coordinates to map coordinates (first element is row, second element is column)

		int[] mapCoordinates = new int[2];

		Vector3 coordinates = obj.transform.localPosition;

		mapCoordinates [0] = (int)(LevelGenerator.GetRows() - (coordinates.z / 10));

		mapCoordinates [1] = (int)(coordinates.x / 10);

		return mapCoordinates;
	}
	public static int[] TranslateWorldToMapCoordinates(Vector3 coordinates) { //translates in-level coordinates to map coordinates (first element is row, second element is column)

		int[] mapCoordinates = new int[2];

		mapCoordinates [0] = (int)(LevelGenerator.GetRows() - (coordinates.z / 10));

		mapCoordinates [1] = (int)(coordinates.x / 10);

		return mapCoordinates;
	}

	public static int[] TranslateWorldToMapCoordinates(float[] coordinates) { //translates in-level coordinates to map coordinates (first element is row, second element is column)

		int[] mapCoordinates = new int[2];

		mapCoordinates [0] = (int)(LevelGenerator.GetRows() - (coordinates[0] / 10));

		mapCoordinates [1] = (int)(coordinates[1] / 10);

		return mapCoordinates;
	}

	public static float[] TranslateMapToLevelCoordinates(int[] coordinates) { //translate map coordinates to world coordinates

		float[] rValue = new float[2];

	
		rValue [1] = (float)((coordinates [1] * 10) + 5);
		rValue [0] = (float)((coordinates [0] * -10) + ((LevelGenerator.GetRows () * 10) - 5));


		return rValue;
	}

	public static float[] TranslateMapToLevelCoordinatesGlobal(int[] coordinates) {//translate map coordinates to world coordinates

		float[] rValue = MapUtility.TranslateMapToLevelCoordinates(coordinates);

		rValue[0] = rValue[1] - 140;
		rValue[1] = rValue[0] - 155;
		
		

		return rValue;
	}

	public static Vector3 TranslateMapToLevelCoordinatesVector(int[] coordinates) {//translate level coordinates to map coordinates

		Vector3 rValue = new Vector3(coordinates[1], 2.55f, coordinates[0]);
		
		return rValue;
	}	

	public static Vector3 GetJunctionPosition(int[] startingPosition, Directions currentDirection) {//get position of next junction based on given starting positiong and movement direction

		int[] nextJunction = MapUtility.FindNextJunction(startingPosition, currentDirection);	
		
		float[] nextJunctionCoordinates = MapUtility.TranslateMapToLevelCoordinates(nextJunction);

		Vector3 movePosition = new Vector3(nextJunctionCoordinates[1], 2.55f, nextJunctionCoordinates[0]);

		return movePosition;
	}

	public static Vector3 GetDeadEndPosition(int[] startingPosition, GameObject obj, Directions currentDirection) { //get position of next dead end based on given starting positiong and movement direction

		int[] nextJunction = MapUtility.FindNextDeadend(MapUtility.TranslateWorldToMapCoordinates(obj), currentDirection);	

		float[] nextDeadendCoordinates = MapUtility.TranslateMapToLevelCoordinates(nextJunction);
		
		Vector3 deadendPosition = new Vector3(nextDeadendCoordinates[1], 2.55f, nextDeadendCoordinates[0]);

		return deadendPosition;
	}


	public static int[] GetNext(int[] pos, int currentDirection) { //returns coordinates of next cell given the current position and current movement direction

		int[] checkPos = new int[2];

		switch(currentDirection) {
			case 0:
				checkPos[0] = pos[0];
				checkPos[1] = pos[1] + 1;
			break;

			case 1:
				checkPos[0] = pos[0];
				checkPos[1] = pos[1] - 1;
			break;	

			case 2: 
				checkPos[0] = pos[0] - 1;
				checkPos[1] = pos[1];
			break;

			case 3: 	
				checkPos[0] = pos[0] + 1;
				checkPos[1] = pos[1];
			break;

		}

		return checkPos;
	}

	public static int[] FindNextDeadend(int[] startingCoordinates, Directions direction) { //find next dead-end cell based on starting coordinates and movement direction
		switch(direction) {
			case Directions.Right:
				return FindNextDeadendRight(startingCoordinates);
			case Directions.Left:
				return FindNextDeadendLeft(startingCoordinates);
			case Directions.Up:
				return FindNextDeadendUp(startingCoordinates);	
			default: 
				return FindNextDeadendDown(startingCoordinates);		
		}
	}

	public static int[] FindNextDeadendRight(int[] startingCoordinates) { //returns map coordinates of next dead-end cell to the right

		int[] rValue = startingCoordinates;

		if(CanMoveRight(rValue)) {
			return FindNextDeadendRight(MapUtility.GetNext(rValue, (int)Directions.Right));
		}
		else {
			return rValue;
		}
	}

	public static int[] FindNextDeadendLeft(int[] startingCoordinates) {//returns map coordinates of next dead-end cell to the left

		int[] rValue = startingCoordinates;

		if(CanMoveLeft(rValue)) {
			rValue = MapUtility.GetNext(rValue, (int)Directions.Left);
			return FindNextDeadendLeft(rValue);
		}
		else {
			return rValue;
		}
	}
		
	public static int[] FindNextDeadendDown(int[] startingCoordinates) {//returns map coordinates of next dead-end cell below

		int[] rValue = startingCoordinates;

		if(CanMoveDown(rValue)) {
			rValue = MapUtility.GetNext(rValue, (int)Directions.Down);
			return FindNextDeadendDown(rValue);
		}
		else {
			return rValue;
		}
	}
		
	public static int[] FindNextDeadendUp(int[] startingCoordinates) { //returns map coordinates of next dead-end cell to the above

		int[] rValue = startingCoordinates;

		if(CanMoveUp(rValue)) {
			rValue = MapUtility.GetNext(rValue, (int)Directions.Up);
			return FindNextDeadendUp(rValue);
		}
		else {
			return rValue;
		}
	}
		
	public static bool isNextWall(int[] pos, int currentDirection) { //returns true if next cell in the current movement direction is a wall

		int[] checkPos = new int[2];

		switch(currentDirection) {
			case 0:
				checkPos[0] = pos[0];
				checkPos[1] = pos[1] + 1;
			break;

			case 1:
				checkPos[0] = pos[0];
				checkPos[1] = pos[1] - 1;
			break;	

			case 2: 
				checkPos[0] = pos[0] - 1;
				checkPos[1] = pos[1];
			break;

			case 3: 	
				checkPos[0] = pos[0] + 1;
				checkPos[1] = pos[1];
			break;

		}

			if(checkPos[0] >= LevelGenerator.GetRows() || checkPos[1] >= LevelGenerator.GetCols()) {

				return false;
			}
			else if(MapUtility.GetMapCell(checkPos) == 1) {

				return true;
			} 
			
				return false;
		




		}


	public static int[] getBackPoint(GameObject obj, Directions moveDirection) { //get back point map coordinates of player depending on movement direction
		
		float modify = obj.transform.localScale.x / 2;
		float[] coordinates = {obj.transform.localPosition.z, obj.transform.localPosition.x};

		if(moveDirection == Directions.Right) {
			coordinates[1] -= modify;
		}
		if(moveDirection == Directions.Left) {
			coordinates[1] += modify;
		}
		if(moveDirection == Directions.Up) {
			coordinates[0] -= modify;
		}
		if(moveDirection == Directions.Down) {
			coordinates[0] += modify;
		}

		return TranslateWorldToMapCoordinates(coordinates);
	}

	public static int GetDistance(int[] firstCell, int[] secondCell) { //returns total vertical + horizontal distance between two cells, regardless of walls

		int distance = 0;

		

		int distance1 = System.Math.Abs(firstCell[0] - secondCell[0]);
		int distance2 = System.Math.Abs(firstCell[1] - secondCell[1]);	

		distance = (int)Math.Sqrt((distance1*distance1) +(distance2*distance2));

		return distance;
	}
}
