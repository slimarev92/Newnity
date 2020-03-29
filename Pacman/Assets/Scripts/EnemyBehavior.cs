using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType {Aggressive, Front, Chance, Avoid};

public enum MoveMode {Chase, Scatter}; 


public class EnemyBehavior : MonoBehaviour
{
	public Directions currentDirection; //current movement direction

	[SerializeField] private EnemyType type; //enemy type

	public float speed; //movement speed per second
	
	public float moveX, moveZ; //raw input from axes
	public int[] targetCoordinates; //coordinates of next junction

	public Vector3 movePosition;

	public string id; //unique id

	[SerializeField] private GameObject targetPrefab;

	private GameObject currentTarget;

	private GameObject axisOrigin;

	void Start() {
		currentDirection =  PickRandomAvailableDirection();

		currentTarget = new GameObject();

		speed = 40.0f;

		id = transform.gameObject.name;

		axisOrigin = GameFunctions.GetAxisOrigin();

		MoveToPositionJunction();
	}

	void MoveToPositionJunction() { //if player is currently not moving, moves him current direction to next junction

		try {
			movePosition =  MapUtility.GetJunctionPosition(MapUtility.TranslateWorldToMapCoordinates(transform.gameObject), currentDirection);
		}
		catch(Exception e) { 
			e.GetType();
			movePosition = MapUtility.GetDeadEndPosition(MapUtility.TranslateWorldToMapCoordinates(transform.gameObject), transform.gameObject, currentDirection);
		}

		Destroy(currentTarget);

		currentTarget = Instantiate (targetPrefab) as GameObject;

		currentTarget.transform.parent = GameFunctions.GetAxisOrigin().transform;

		if(!GameFunctions.IsDebbuging()) {
			currentTarget.SetActive(false);
		}

		currentTarget.transform.localPosition = movePosition;

		

		iTween.MoveTo(transform.gameObject, iTween.Hash("position", movePosition, "speed", speed, "islocal", true, "easetype", "linear", "oncomplete", "StopMovingJunction", "name", id));
	}

	void StopMovingJunction() {

		if(GameFunctions.GetMoveMode() == MoveMode.Chase && type != EnemyType.Chance) {
			currentDirection =  PickBestDirection();
		}
		else {
			currentDirection = PickRandomAvailableDirection();
		}
		
		MoveToPositionJunction();
	} 

	List<int> GetDistances(List<Directions> availableDirs, GameObject target) {
		
		List<int> rList = new List<int>();
		
		int len = availableDirs.Count;

		int[] targetPosition;
		
		PlayerMotion targetMotion = target.GetComponent<PlayerMotion>();

		if(type == EnemyType.Aggressive || targetMotion.GetMoveDirection() == Directions.None) {
			targetPosition = MapUtility.TranslateWorldToMapCoordinates(target);
		}
		else { 
			targetPosition = MapUtility.FindNextJunction(targetMotion.GetPosition(), targetMotion.GetCurrentDirection());
		}
		 
		int[] currentPosition = MapUtility.TranslateWorldToMapCoordinates(transform.gameObject);

		int[] junction = new int[2];
		
		Vector3 junction1 = new Vector3();

		for(int i = 0; i < len; i++) {

			try {
				junction = MapUtility.FindNextJunction(currentPosition, availableDirs[i]);
			} catch(Exception e) {
				e.GetType();
				junction1 = MapUtility.GetDeadEndPosition(MapUtility.TranslateWorldToMapCoordinates(transform.gameObject), transform.gameObject,  availableDirs[i]);

				junction = MapUtility.TranslateWorldToMapCoordinates(junction1);
			}
			
			rList.Add(MapUtility.GetDistance(targetPosition, junction));			
		}

		return rList;
	}

	Directions PickRandomAvailableDirection() {
	List<Directions> availableDirs1 = new List<Directions>();

		for(int i = 0; i < 4; i++) {
			if(MapUtility.CanMove(transform.gameObject, i) && MapUtility.OppositeDirection((Directions)i) != currentDirection) {
				availableDirs1.Add((Directions)i);
			}
		}

		System.Random rand = new System.Random();

		int count = availableDirs1.Count;

		if(count > 0) {
			return availableDirs1[rand.Next(availableDirs1.Count)];
		}
		else{
			return MapUtility.OppositeDirection(currentDirection);
		}	
	}

	Directions PickBestDirection() {

		Directions rValue = Directions.Right;	

		List<Directions> availableDirs = new List<Directions>();

		for(int i = 0; i < 4; i++) {
			if(MapUtility.CanMove(transform.gameObject, i) && MapUtility.OppositeDirection((Directions)i) != currentDirection) {
				availableDirs.Add((Directions)i);
			}
		}

		List<int> distances = GetDistances(availableDirs, GameFunctions.GetPlayerObject());

		if(type == EnemyType.Aggressive || type == EnemyType.Front) {
			int min = 10000;

			for(int i = 0; i < distances.Count; i++) {
				if(distances[i] < min) {
					min = distances[i];
					rValue = availableDirs[i];
				}
			}
		}
		else {
			int max = 0;

			for(int i = 0; i < distances.Count; i++) {
				if(distances[i] > max) {
					max = distances[i];
					rValue = availableDirs[i];
				}
			}
		}
	
		return rValue;
	}

	void MoveToPositionDeadend() { //if player is currently not moving, moves him current direction to next junction

		Vector3 movePosition = MapUtility.GetDeadEndPosition(MapUtility.TranslateWorldToMapCoordinates(transform.gameObject), transform.gameObject, currentDirection);
		
		iTween.MoveTo(transform.gameObject,iTween.Hash("position", movePosition, "speed", speed, "islocal", true, "easetype", iTween.EaseType.linear, "oncomplete", "StopMovingJunction", "name", id));
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.GetComponent<PlayerMotion>() != null) {
			iTween.StopByName(id);
		}

		if(other.gameObject.GetComponent<LeftTeleport>() != null) {
			iTween.StopByName(id);
			float[] newPosition = MapUtility.TranslateMapToLevelCoordinates(new int[]{14, 26});

			transform.localPosition = new Vector3(newPosition[1], 2.55f, newPosition[0]);
			currentDirection = Directions.Left;
			MoveToPositionJunction();

		}	
		if(other.gameObject.GetComponent<RightTeleport>() != null) {

			float[] newPosition = MapUtility.TranslateMapToLevelCoordinates(new int[]{14, 1});

			transform.localPosition = new Vector3(newPosition[1], 2.55f, newPosition[0]);
			currentDirection = Directions.Right;
			MoveToPositionJunction();
		}	
	}
	
	void Update () { // Update is called once per frame
		if(GameFunctions.isGameOver()) { 		
			iTween.StopByName(id);
			return;
		}	
	}
}