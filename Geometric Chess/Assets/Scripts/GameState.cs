using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType {
	WAITING,
	HOLDING,
	PLACING,
	GAME_OVER
}

public class GameState {

	private GameStateType state;

	public GameState() {
		state = GameStateType.WAITING;
	}

	public GameStateType State {
		get {return state;}
		set {
			state = value;
		}
	}

	public bool IsWaiting {
		get {return state == GameStateType.WAITING;}
	}

	public bool IsPlacing {
		get {return state == GameStateType.PLACING;}
	}

	public bool IsHolding {
		get {return state == GameStateType.HOLDING;}
	}

	public void Grab() {
		state = GameStateType.HOLDING;
	}

	public void Place() {
		state = GameStateType.PLACING;
	}

	public void Release() {
		state = GameStateType.WAITING;
		GameManager.Instance.SwitchPlayer();
	}

	public void Cancel() {
		state = GameStateType.WAITING;
	}

	public bool IsGameOver {
		get {
			if (state == GameStateType.GAME_OVER) return true;
			return false;
		}
	}
}
