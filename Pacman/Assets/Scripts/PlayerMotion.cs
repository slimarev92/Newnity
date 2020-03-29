using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMotion : MonoBehaviour
{
	public Directions currentDirection; //current movement direction
	public float speed; //movement speed per second

	private float timeSinceFlip = 0; //time since last behavior flip

	public Directions move; //desired movement direction

	public float moveX, moveZ; //movement along the X and Z axes - based on input

	private int timeToFlip = 20; //time between flips of chase/scatter mode

	public int[] targetCoordinates; //coordinates of player's current move target

	public string id; //unique id

	void Start () {	// Use this for initialization
		currentDirection = Directions.Right;
		move = currentDirection;

		speed = 60.0f;

		id = transform.gameObject.name;

		MoveToPositionDeadend();
	}

	public int[] GetPosition() { //get current position
		return MapUtility.TranslateWorldToMapCoordinates(transform.gameObject);
	}

	public Directions GetCurrentDirection() {
		return currentDirection;
	}

	void StopMovingDeadend() { //what happends when a tween to a deadend is complete
		currentDirection = Directions.None;
	} 

	void StopMovingJunction() { //what happends when a tween to a junction is complete
		if(MapUtility.CanMove(transform.gameObject, (int)move)) {
			currentDirection = move;
			MoveToPositionDeadend();
		}
		else {
			MoveToPositionJunction(MapUtility.getBackPoint(transform.gameObject, currentDirection));
		}
	}

	void MoveToPositionDeadend() { //if player is currently not moving, moves him current direction to next junction
	
		Vector3 movePosition = MapUtility.GetDeadEndPosition(MapUtility.TranslateWorldToMapCoordinates(transform.gameObject), transform.gameObject, currentDirection);

		targetCoordinates = MapUtility.TranslateWorldToMapCoordinates(movePosition);

		iTween.MoveTo(transform.gameObject,iTween.Hash("position", movePosition, "speed", speed, "islocal", true, "easetype", iTween.EaseType.linear, "oncomplete", "StopMovingDeadend", "name", id));

	}

	public int[] GetTargetCoordinates() { //returns the coordinates of the player's current move target
		return targetCoordinates;
	}

	public Directions GetMoveDirection() {//returns the current direction the player is moving in
		return currentDirection;
	}

	void MoveToPositionJunction(int[] startingPosition) { //if player is currently not moving, moves him current direction to next junction

		Vector3 movePosition =  MapUtility.GetJunctionPosition(startingPosition, currentDirection);

		targetCoordinates = MapUtility.TranslateWorldToMapCoordinates(movePosition);

		iTween.MoveTo(transform.gameObject, iTween.Hash("position", movePosition, "speed", speed, "islocal", true, "easetype", "linear", "oncomplete", "StopMovingJunction", "name", id));
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.GetComponent<EnemyBehavior> () != null) {
			iTween.StopByName(id);
			EndGame();

		}	
		if(other.gameObject.GetComponent<LeftTeleport>() != null) {

			float[] newPosition = MapUtility.TranslateMapToLevelCoordinates(new int[]{14, 26});

			transform.localPosition = new Vector3(newPosition[1], 2.55f, newPosition[0]);
			currentDirection = Directions.Left;
			MoveToPositionDeadend();

		}	
		if(other.gameObject.GetComponent<RightTeleport>() != null) {

			float[] newPosition = MapUtility.TranslateMapToLevelCoordinates(new int[]{14, 1});

			transform.localPosition = new Vector3(newPosition[1], 2.55f, newPosition[0]);
			currentDirection = Directions.Right;
			MoveToPositionDeadend();
		}	
	}
		
	void OnGUI() { //draw GUI (if it was set as visible)
		if(GameFunctions.isGUIOn()) {
			
			float posX, posY;
			GUI.skin.label.fontSize = 30;
		
			if(!GameFunctions.isGameOver()) {

				posX = 50;
				posY = 50;

				GUI.Label(new Rect(posX, posY, 900, 200), "Score: " + GameFunctions.GetScore() +"\n" + GameFunctions.GetMoveMode());
			} else if(!GameFunctions.GetWinStatus()){
					posX = Screen.width / 2 - 190;
					posY = Screen.height / 2 - 150;

					GUI.Label(new Rect(posX, posY, 900, 200), "Game over, press r to restart \nYour score " + GameFunctions.GetScore());
			}			
			else {
					posX = Screen.width / 2 - 190;
					posY = Screen.height / 2 - 150;

					GUI.Label(new Rect(posX, posY, 900, 200), "You won! Press r to restart.");
			}
		}	
	}

	public void EndGame() {
		GameFunctions.EndGame();
		iTween.StopByName(id);
	}

	void Update () { //called once per frame 

		if(GameFunctions.GetScore() == GameFunctions.GetNumOfDots()) {
			GameFunctions.WinGame();
		 	EndGame();
		}

		if (GameFunctions.isGameOver()) { //if game is over, allow player to press r to restart the scene

			GameFunctions.ShowGUI();

			if(Input.GetKeyDown(KeyCode.R)) {
				GameFunctions.StartGame();
			}
			return;
		}	

		GameFunctions.SetGameTime(Time.timeSinceLevelLoad);

		if((int)GameFunctions.GetGameTime() - (int)timeSinceFlip > timeToFlip) {
			timeSinceFlip = GameFunctions.GetGameTime();
			GameFunctions.FlipMoveMode();
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)) { //turn on debugging mode
		 		GameFunctions.SetDebugging(true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) { //turn off debugging mode
		 		GameFunctions.SetDebugging(false);
		}
		if(Input.GetKeyDown(KeyCode.O)) { //if the o key was pressed, show/hide GUI
		 		GameFunctions.FlipGUI();
		}
	
		if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKeyDown(KeyCode.O) && !Input.GetKeyDown(KeyCode.R)) { //if an arrow key was pressed
			moveX = Input.GetAxis ("Horizontal"); //check for horizontal input
			moveZ = Input.GetAxis ("Vertical"); //check for vertical input

			if (moveX > 0) { //if the player pressed the right arrow key, set desired direction to "right"
				move = Directions.Right;
			} else if (moveX < 0) { //if the player pressed the left arrow key, set desired direction to "left"
				move = Directions.Left;
			} 
			
			if (moveZ > 0) { //if the player pressed the up arrow key, set desired direction to "up"
				move = Directions.Up;
			} else if (moveZ < 0) {  //if the player pressed the down arrow key, set desired direction to "down"
				move = Directions.Down;
			}

			if(currentDirection == Directions.None) //if theres no active tween, set moving direction to whatever was pressed
			{
				currentDirection = move;
				MoveToPositionDeadend();
		
			}	
			else if(move != currentDirection) { //if theres an active tween but the direction is the opposite of current moving direction, start moving to opposite direction
				
				if(MapUtility.OppositeDirection(move) == currentDirection) {
					currentDirection = move;
					MoveToPositionDeadend();
				}
				else { //if new direction is perpendicular to current direction, move to nearest junction and start checking if player can move to the desired direction at that junction
					MoveToPositionJunction(MapUtility.getBackPoint(transform.gameObject, currentDirection));
				}
			}
		}
	}
}