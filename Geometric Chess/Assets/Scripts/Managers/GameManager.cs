using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private Material pieceP1;
	[SerializeField]
	private Material pieceP2;
	[SerializeField]
	private Grid grid;
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private LayerMask clickableMask;
	[SerializeField]
	private Color pieceHighlightColor;
	[SerializeField]
	private Material moveHighlightMaterial;
	[SerializeField]
	private Material eatHighlightMaterial;

	private GCPlayer p1;
	private GCPlayer p2;
	private GCPlayer currentPlayer;

	private GameState gameState;

	private bool ready;

	public GCPlayer PlayerOponent {
		get {
			if (currentPlayer == null) return null;
			if (currentPlayer == p1) return p2;
			else return p1;
		}
	}

	public Material EatHighlightMaterial {
		get {return eatHighlightMaterial;}
	}

	public Color PieceHighlightColor {
		get {return pieceHighlightColor;}
	}

	public Material MoveHighlightMaterial {
		get {return moveHighlightMaterial;}
	}

	public GameState GameState {
		get {return gameState;}
	}

	public GCPlayer CurrentPlayer {
		get {return currentPlayer;}
	}

	public LayerMask CLickableMask {
		get {return clickableMask;}
	}

	public Camera MainCamera {
		get {return mainCamera;}
	}

	public GCPlayer P1 {
		get {return p1;}
	}

	public GCPlayer P2 {
		get {return p2;}
	}

	public bool IsReady {
		get {return ready;}
	}

	public Material PieceP1 {
		get {return pieceP1;}
	}

	public Material PieceP2 {
		get {return pieceP2;}
	}

	public Grid Grid {
		get {return grid;}
	}

	void Awake() {
		_destroyOnLoad = destroyOnLoad;
		p1 = new GCPlayer(PlayerType.P1);
		p2 = new GCPlayer(PlayerType.P2);
		gameState = new GameState();
		SwitchPlayer();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(init());
	}

	IEnumerator init() {
		Stopwatch timer = new Stopwatch();
		timer.Start();

		//wait for nodes
		while (!grid.IsReady) yield return null;
		//wait for pieces
		while (!grid.ArePiecesSpawned) yield return null; //wait till pieces are spawned
		while (!p1.IsReady) yield return null; //wait till all pieces are scaled in
		while (!p2.IsReady) yield return null;

		print("Time elapsed: " + timer.ElapsedMilliseconds / 1000.0 + "s");
		timer.Stop();

		//all objects are now ready
		ready = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!ready) return;
		if (gameState.IsGameOver) return;
		
	}

	public void SwitchPlayer() {
		if (currentPlayer != null) {
			currentPlayer.DisableInput();
		}

		if (currentPlayer == p2) {
			currentPlayer = p1;
			mainCamera.GetComponent<RotateCamera>().SwitchCamera(PlayerType.P1);
		} else if (currentPlayer == p1) {
			currentPlayer = p2;
			mainCamera.GetComponent<RotateCamera>().SwitchCamera(PlayerType.P2);
		} else {
			currentPlayer = p1;
		}

		currentPlayer.EnableInput();

		print(currentPlayer.Type); //show on screen 
	}
}
