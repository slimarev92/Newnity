using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameFunctions {

	private static bool gameOver = false;
	private static bool showGUI = true;

	private static bool isDebbuging = false;

	private static int numOfDots = 0;

	private static GameObject axisOrigin;

	private static int score = 0;

	private static bool isWin = false;

	private static MoveMode currentMoveMode = MoveMode.Chase;

	private static float gameTime = 0;

	private static GameObject playerObject;

	public static GameObject GetAxisOrigin(){
		return axisOrigin;
	}

	public static bool IsDebbuging() {
		return isDebbuging;
	}

	public static void SetDebugging(bool debbugingStatus) {
		isDebbuging = debbugingStatus;
	}

	public static void SetAxisOrigin(GameObject giveAxisOrigin) {
		axisOrigin = giveAxisOrigin;
	}

	public static void SetPlayerObject(GameObject playersObject) {
		playerObject = playersObject;
	}

	public static GameObject GetPlayerObject() {
		return playerObject;
	}

	public static int GetNumOfDots() {
		return numOfDots;
	}

	public static void AddToNumOfDots() {
		numOfDots++;
	}

	public static void ShowGUI() {
		showGUI = true;
	}

	public static void FlipGUI() {
		showGUI = !showGUI;
	}

	public static MoveMode GetMoveMode() {
		return currentMoveMode;
	}
	
	public static void AddToScore(int num) {
		score += num;
	}

	public static void AddToScore() {
		score++;
	}

	public static int GetScore() {
		return score; 
	}

	public static void SetGameTime(float time) {
		gameTime = time;
	}

	public static void FlipMoveMode() {

		if(currentMoveMode == MoveMode.Chase) {
			currentMoveMode = MoveMode.Scatter;
		}
		else {
			currentMoveMode = MoveMode.Chase;
		}
	}

	public static float GetGameTime() {
		return gameTime;
	}

	public static bool isGUIOn() {
		return showGUI;
	}

	public static void HideGUI() {
		showGUI = false;
	}

	public static bool isGameOver() {
		return gameOver;
	}

	public static void EndGame() {
		gameOver = true;
	}

	public static void WonGame() {
		isWin = true;
	}

	public static bool GetWinStatus() {
		return isWin;
	}

	public static bool WinGame() {
		return isWin = true;
	}

	public static void StartGame() {
		SceneManager.LoadScene("Exp");
		gameTime = 0;
		score = 0;
		gameOver = false;
		isWin = false;
		numOfDots = 0;
		currentMoveMode = MoveMode.Chase;
	}
}
