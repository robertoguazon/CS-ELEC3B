using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType {
	WAITING,
	HOLDING,
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

	public bool IsHolding {
		get {return state == GameStateType.HOLDING;}
	}

	public void Grabbed() {
		state = GameStateType.HOLDING;
	}

	public void Placed() {
		state = GameStateType.WAITING;
	}

	public void Released() {
		Placed();
	}

	public bool IsGameOver {
		get {
			if (state == GameStateType.GAME_OVER) return true;
			return false;
		}
	}
}
