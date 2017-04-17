using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	private Material highlightMoveMaterial;
	[SerializeField]
	private Material highlightEatMaterial;
	[SerializeField]
	private Material highlightCheckMaterial;

	public Text winnerText;
	public Text blackScoreText;
	public Text whiteScoreText;
	public Text gamesPlayedText;
	public GameObject winnerCanvas;
	public GameObject continueButton;
	public GameObject winnerButton;

	public const string PLAYER_WHITE = "PLAYER_WHITE";
	public const string PLAYER_BLACK = "PLAYER_BLACK";
	public const string SCORE_MAX = "SCORE_MAX";
	public const string GAME_MAX = "GAME_MAX";
	public const string GAME_CURRENT = "GAME_CURRENT";
	public const string CAMERA_VIEW = "CAMERA_VIEW";

	public delegate void SwitchedPlayer();
	public static event SwitchedPlayer SwitchedEvent;

	private GCPlayer p1 = new GCPlayer(PlayerType.P1);
	private GCPlayer p2 = new GCPlayer(PlayerType.P2);
	private GCPlayer currentPlayer;

	private GameState gameState;

	private bool ready;

	public GCPlayer PlayerOponent {
		get {
			return Opponent(currentPlayer);
		}
	}

	public Material HighlightCheckMaterial {
		get {return highlightCheckMaterial;}
	}

	public Material HighlightEatMaterial {
		get {return highlightEatMaterial;}
	}

	public Color PieceHighlightColor {
		get {return pieceHighlightColor;}
	}

	public Material HighlightMoveMaterial {
		get {return highlightMoveMaterial;}
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
		gameState = new GameState();
		LoadScores();
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

		//IMPORTANT
		p1.ComputePieces();
		p2.ComputePieces(); 
		SwitchPlayer(); //if null current player = p1

		//all objects are now ready
		ready = true;
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Z)) {
			GameOver(p1,GameOverType.CHECKMATE); //TODO delete
		} else if (Input.GetKeyDown(KeyCode.X)) {
			GameOver(p2,GameOverType.CHECKMATE); //TODO delete
		} else if (Input.GetKeyDown(KeyCode.C)) {
			GameOver(p2,GameOverType.STALEMATE);
		}
#endif

		if (!ready) return;
		if (gameState.IsGameOver) return;
	}

	public void SwitchPlayer() {
		if (currentPlayer != null) {
			currentPlayer.DisableInput();
		}

		if (currentPlayer == p2) {
			currentPlayer = p1;
			mainCamera.GetComponent<SwitchAngle>().SwitchCamera(PlayerType.P1);
		} else if (currentPlayer == p1) {
			currentPlayer = p2;
			mainCamera.GetComponent<SwitchAngle>().SwitchCamera(PlayerType.P2);
		} else {
			currentPlayer = p1;
		}

		//IF checkmate
		if (Rules.HasNoMove()) {	
			InputManager.Instance.UnHighlightTile();
			if (currentPlayer.IsChecked) {
				GameManager.Instance.GameState.Checkmate();
				GameOver(PlayerOponent, GameOverType.CHECKMATE);
				print("CHECKMATE");
			} else {
				GameManager.Instance.GameState.Stalemate();
				print("STALEMATE");
				GameOver(currentPlayer, GameOverType.STALEMATE);
			}
		}

		currentPlayer.EnableInput();
		if (SwitchedEvent != null) {
			SwitchedEvent(); //EXPERIMENTAL
		}

		print("Turn of: " + currentPlayer.Type); //show on screen 
	}

	public GCPlayer Opponent(GCPlayer player) {
		if (player == null) return null;
		if (player == p1) return p2;
		else return p1;
	}

	public void GameOver(GCPlayer winner, GameOverType gameOverType) {
		AddGame();
		switch (gameOverType) {
			case GameOverType.CHECKMATE:
				if (winner == p2) {
					winnerText.text = "CHECKMATE: BLACK wins";
					AddScore(PLAYER_BLACK);
				} else if (winner == p1) {
					winnerText.text = "CHECKMATE: WHITE wins";
					AddScore(PLAYER_WHITE);
				}
			break;
			case GameOverType.STALEMATE:
				winnerText.text = "STALEMATE: It's a tie"; 
			break;
		}
		continueButton.SetActive(true);
	}

	public void End() {
		continueButton.SetActive(false);
		winnerButton.SetActive(true);
	}

	public void AddGame() {
		int newGame = PlayerPrefs.GetInt(GAME_CURRENT,0) + 1;
		PlayerPrefs.SetInt(GAME_CURRENT,newGame);

		int maxGame = PlayerPrefs.GetInt(GAME_MAX, 0);
		if (newGame >= maxGame) {
			End();
		}
	}

	public void AddScore(string playerString) {
		int newScore = PlayerPrefs.GetInt(playerString,0) + 1;
		PlayerPrefs.SetInt(playerString,newScore);

		int maxScore = PlayerPrefs.GetInt(SCORE_MAX,0);
		if (newScore >= maxScore) {
			End();
		}
	}

	public void LoadScores() {
		int white = PlayerPrefs.GetInt(PLAYER_WHITE,0);
		int black = PlayerPrefs.GetInt(PLAYER_BLACK,0);
		whiteScoreText.text = white.ToString();
		blackScoreText.text = black.ToString();

		int gamesPlayed = PlayerPrefs.GetInt(GAME_CURRENT,0);
		gamesPlayedText.text = gamesPlayed.ToString();
	}

	public static void ResetScores() {
		PlayerPrefs.DeleteKey(PLAYER_WHITE);
		PlayerPrefs.DeleteKey(PLAYER_BLACK);
		PlayerPrefs.DeleteKey(GAME_CURRENT);
	}
}
