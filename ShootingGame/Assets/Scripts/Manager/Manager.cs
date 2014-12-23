using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//全ステージ共通処理
public class Manager : MonoBehaviour
{
	//========== 個別にインスペクターで設定する項目 ==========
	// Playerプレハブ
	public GameObject player;
	public GameObject player2;
	public GameObject player3;
	public Sprite p1Icon;
	public Sprite p2Icon;
	public Sprite p3Icon;

	//========== 各種フラグ ==========
	//ゲーム停止フラグ
	[System.NonSerialized]
	public bool isPause;
	
	//エネミー停止フラグ
	[System.NonSerialized]
	public bool isEnemyPause = false;
	
	//ボム発射フラグ
	[System.NonSerialized]
	public bool isBomb = false;
	
	//スクリーンタッチ検知フラグ
	[System.NonSerialized]
	public bool isTouch = false;

	//ステージ遷移フラグ
	[System.NonSerialized]
	public static bool isNextStage;

	//ゲームリトライフラグ
	[System.NonSerialized]
	public bool retry = false;

	//========== その他の public 変数 ==========
	[HideInInspector]
	public GameObject selectedPlayer;
	
	public static int selectedPlayerNo;
		
	public static int lastScore;

	//選択した機体
	public static bool p1 = false;
	public static bool p2 = false;
	public static bool p3 = false;

	//現在のステージ（メニューから入れば１、あとはボスを倒すごとに＋＋）
	public static int Stage;

	//TODO from start stage
	public static int gameMode;
	public static int playerLifeDefault;

	//========== メンバ変数 ==========
	GameObject mainGui;
	GameObject continueGui;
	GameObject clearGui;

	//現在のステージ名
	string currentStageName;

	//TODO コンティニュー
	//コンティニューのカウント表示を格納する
	GameObject count1;
	GameObject count2;
	GameObject count3;
	GameObject count4;
	GameObject count5;
	GameObject gameover;
	GameObject conTitle;
	GameObject buttony;
	GameObject buttonn;

	public int continueCount = 0;
	public bool isEndByNoBtn = false;
	bool isContinuGui = false;

	//TODO for debug デバッグフラグ　リリースの際はfalseにすること。
	bool isDebug = true;


	public static string[] stagePatarns;

	void Start ()
	{

		//現在のステージ名を設定
		currentStageName = Application.loadedLevelName;

		//TODO ======================== for debug
		if (isDebug) {
			if (playerLifeDefault == 0) {
					playerLifeDefault = 20;
			}
			if(stagePatarns == null){
				stagePatarns = new string[] {currentStageName, "Stage03", "Thanks"};
			}
		}
		//TODO ======================== for debug

		//コンティニュ用オブジェクトセットアップ処理
		setContinueCountObj();

		//Guiを設定
		mainGui = GameObject.Find("mainGui");
		continueGui = GameObject.Find("continueGui");
		clearGui = GameObject.Find("clearGui");

		//ゲーム開始
		GameStart ();

	}
	
	void Update ()
	{
		//TODO ======================== for debug
		if (isDebug) {
			//ゲーム速度を下げる
			if (Input.GetKeyDown (KeyCode.Alpha1)){
				Time.timeScale = 0.3f;	
			}
			//ゲーム速度を戻す
			if (Input.GetKeyDown (KeyCode.Alpha2)){
				Time.timeScale = 1;	
			}
		}
		//TODO ======================== for debug

		//コンティニューカウントダウン処理
		if (isContinuGui) {
			continueCount++;
			if(isEndByNoBtn){
				continueCount = 300;
				isEndByNoBtn = false;
			}
			countinueCountDown(continueCount);
		}
	}

	//プレイヤーと、ステージの順番を設定 | 引数に機体番号を設定
	public static void selectPlayer(int argPlayerNum){
		switch (argPlayerNum) {
		case 1:
			Manager.p1 = true;
			stagePatarns = new string[] {"Stage11", "Stage12", "Stage13", "Thanks"};
			break;
		case 2:
			Manager.p2 = true;
			stagePatarns = new string[] {"Stage05", "Stage06", "Stage03", "Thanks"};
			break;
		case 3:
			stagePatarns = new string[] {"Stage05", "Stage11", "Stage06", "Stage03", "Stage12", "Stage13", "Thanks"};
			Manager.p3 = true;
			break;
		default:
			stagePatarns = new string[] {"Stage05", "Stage11", "Stage06", "Stage03", "Stage12", "Stage13", "Thanks"};
			Manager.p1 = true;
			break;
		}
		Application.LoadLevel (stagePatarns[0]);
	}

	//次のステージへ移行処理
	public void toNextStage(){
		currentStageName = Application.loadedLevelName;
		for(int i=0; i < stagePatarns.Length; i++){
			// 現在のステージ名と、ステージ配列のステージ名が同じ場合に、次のステージをロードする。
			if(stagePatarns[i] == currentStageName){
				Application.LoadLevel (stagePatarns[i + 1]);
			}
		}
	}

	public void GameStart ()
	{
		//ゲームポーズを解除
		isPause = false;

		//Continueを解除
		continueCount= 0;
		isContinuGui = false;
		initContinueCount();

		//Guiを設定
		mainGui.SetActive (true);
		continueGui.SetActive(false);
		clearGui.SetActive(false);

		//プレイヤーを設定
		Image playerIcon = GameObject.Find ("PlayerImage").GetComponent<Image> ();
		if (p1 == true) {
			Instantiate (player, player.transform.position, player.transform.rotation);
			selectedPlayer = player;
			playerIcon.sprite = p1Icon;
		} else if (p2 == true) {
			Instantiate (player2, player2.transform.position, player2.transform.rotation);
			selectedPlayer = player2;
			playerIcon.sprite = p2Icon;
		} else if (p3 == true) {
			Instantiate (player3, player3.transform.position, player3.transform.rotation);
			selectedPlayer = player3;
			playerIcon.sprite = p3Icon;
		} else {
			Instantiate (player, player.transform.position, player.transform.rotation);
			selectedPlayer = player;
			playerIcon.sprite = p1Icon;
		}
	}

	//ゲーム終了処理
	public void GameOver ()
	{
		// ハイスコアの保存
		FindObjectOfType<Score>().Save();

		//倒した敵の数を0に戻す
		FindObjectOfType<Score> ().initDeadEnemyNum ();

		//ステージ遷移フラグを初期化
		isNextStage = false;

		//Gui表示設定
		mainGui.SetActive (false);
		clearGui.SetActive(false);

		//Continue Guiを表示
		continueGui.SetActive (true);
		isContinuGui = true;

	}

	//コンティニュー時のカウント用のオブジェクトを設定
	public void setContinueCountObj(){
		count1 = GameObject.Find ("c1");
		count2 = GameObject.Find ("c2");
		count3 = GameObject.Find ("c3");
		count4 = GameObject.Find ("c4");
		count5 = GameObject.Find ("c5");
		gameover = GameObject.Find("GameOver");
		conTitle = GameObject.Find ("continueTitle");
		buttony = GameObject.Find ("ButtonY");
		buttonn = GameObject.Find ("ButtonN");
		initContinueCount ();
	}
	
	//コンティニュー時のカウント用のオブジェクトを初期化
	public void initContinueCount(){
		count1.SetActive (false);
		count2.SetActive (false);
		count3.SetActive (false);
		count4.SetActive (false);
		count5.SetActive (false);
		gameover.SetActive (false);
	}
	
	//TODO コンティニューカウントダウン処理
	public void countinueCountDown(int argCount){
		switch (argCount) {
		case 1:
			count5.SetActive(true);
			break;
		case 60:
			count5.SetActive(false);
			count4.SetActive(true);
			break;
		case 120:
			count4.SetActive(false);
			count3.SetActive(true);
			break;
		case 180:
			count3.SetActive(false);
			count2.SetActive(true);
			break;
		case 240:
			count2.SetActive(false);
			count1.SetActive(true);
			break;
		case 300:
			//2秒間ゲームオーバー表示をして、その間コンティニューボタンを消す
			initContinueCount();
			gameover.SetActive(true);
			conTitle.SetActive (false);
			buttony.SetActive(false);
			buttonn.SetActive(false);
			break;
		case 420:
			//ゲーム最初に戻る
			continueCount= 0;
			Application.LoadLevel("01StartMenu");
			break;
		default:
			break;
		}
	}

	// ステージクリア時の GUI を表示
	public void showClearGui(){
		clearGui.SetActive(true);
	}

	// ゲーム停止フラグを取得
	public bool IsGamePause(){
		return this.isPause;
	}

	//TODO ゲームリトライフラグを設定
	public void setGameRetry(bool argFlg){
		retry = argFlg;
	}

	//スクリーンタッチを検出
	public void screenTouch(){
		isTouch = true;
	}

	//スクリーンタッチが離れたのを検出
	public void screenTouchOut(){
		isTouch = false;
	}
}