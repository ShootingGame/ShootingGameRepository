using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

	public GameObject selectedPlayer;

	public static int selectedPlayerNo;

	//ゲーム停止フラグ
	public bool isPause;

	//エネミーフラグ
    public bool isEnemyPause = false;

	//TODO public??????
	public bool retry = false;

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

	//ステージ遷移フラグ
	public static bool isNextStage;

	public bool isBomb = false;

	//========== メンバ変数 ==========
	GameObject mainGui;
	GameObject continueGui;
	GameObject clearGui;

	//現在のステージ名
	string currentStageName;

	//最後のステージ名
	public string lastStageName = "Stage12";
	
	//TODO need order
	public Dictionary<string, int> stagePatarns = new Dictionary<string, int>();

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

	//TODO コンティニュー

	void Start ()
	{
		if (p1 == true) {
			lastStageName = "Stage12";
		}
		if (p2 == true) {
			lastStageName = "Stage06";
		}
		if (p3 == true) {
			lastStageName = "Stage06";
		}

		//TODO for DEBUG
		lastStageName = "Stage_Test";
		if (playerLifeDefault == 0) {
			playerLifeDefault = 20;
		}

		//コンティニュ用オブジェクトセットアップ処理
		setContinueCountObj();

		//現在のステージ名を設定
		currentStageName = Application.loadedLevelName;
		//機体によって最終ステージを決定
		if (Manager.p1 == true) {
						lastStageName = "Stage12";
				} else if (Manager.p2 == true) {
						lastStageName = "Stage 03";
				} else if (Manager.p3 == true) {
						lastStageName ="Stage12";
				}
		//Guiを設定
		mainGui = GameObject.Find("mainGui");
		continueGui = GameObject.Find("continueGui");
		clearGui = GameObject.Find("clearGui");

		//ゲーム開始
		GameStart ();

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

	//コンティニューカウントダウン処理
	void Update ()
	{
		if (isContinuGui) {
			continueCount++;
			if(isEndByNoBtn){
				continueCount = 300;
				isEndByNoBtn = false;
			}
			countinueCountDown(continueCount);
		}
	}

	//コンティニューカウントダウン処理
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
		}

		//player icon

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

	public void showClearGui(){
		clearGui.SetActive(true);
	}

	//ポーズフラグ取得処理
	public bool IsGamePause(){
		return this.isPause;
	}

	//TODO ゲームリトライフラグを設定
	public void setGameRetry(bool argFlg){
		retry = argFlg;
	}

	//TODO static method
	//プレイヤーを設定 | 引数に機体番号を設定
	public static void selectPlayer(int argPlayerNum){
		
		switch (argPlayerNum) {
		case 1:
			Manager.p1 = true;
			Application.LoadLevel ("Stage11");
			break;
		case 2:
			Manager.p2 = true;
			Application.LoadLevel("Stage05");
			break;
		case 3:
			Manager.p3 = true;
			Application.LoadLevel("Stage05");
			break;
		default:
			Manager.p1 = true;
			Application.LoadLevel ("Stage11");
			break;
		} 
	}

	//次のステージへ移行処理
	public void toNextStage(){

		Debug.Log ("from...toNextStage");
		if (Manager.p1 == true) {
				if (currentStageName == lastStageName) {
						Application.LoadLevel ("Thanks");
				} else {
						if (currentStageName == "Stage11") {
								Application.LoadLevel ("Stage12");
						}
				}
		} else if (Manager.p2 == true) {
				//kokonisyori
				if (currentStageName == lastStageName) {
						Application.LoadLevel ("Thanks");
				} else {
						if (currentStageName == "Stage05") {
								Application.LoadLevel ("Stage06");
						} else if (currentStageName == "Stage06") {
								Application.LoadLevel ("Stage03");
						}		
				}
		} else if (Manager.p3 == true) {
				//kokonisyori
				if (currentStageName == lastStageName) {
						Application.LoadLevel ("Thanks");
				} else {
						if (currentStageName == "Stage05") {
								Application.LoadLevel ("Stage11");
						} else if (currentStageName == "Stage11") {
								Application.LoadLevel ("Stage06");
						} else if (currentStageName == "Stage06") {
								Application.LoadLevel ("Stage03");
						} else if (currentStageName == "Stage03") {
								Application.LoadLevel ("Stage12");
						}
				}

		}
	}
}