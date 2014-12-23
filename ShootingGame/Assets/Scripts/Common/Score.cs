using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	//========== 個別にインスペクターで設定する項目 ==========
	// スコアを表示するGUIText
	public Text scoreGUIText;
	
	// ハイスコアを表示するGUIText
	public Text highScoreGUIText;

	//プレイヤー数
	public Text playerHpText;

	//========== メンバ変数 ==========
	// スコア
	private int score, oldScore;
	
	// ハイスコア
	private int highScore;

	// 自機ライフオブジェクト
	private Life playerlifeObj;

	// 残機
	private int playerLife, oldPlayerLife;
	
	// ハイスコアをPlayerPrefsで保存するためのキー
	private string highScoreKey = "highScore";
	
	// 倒した敵の数
	private int deadEnemyNum = 0;

	// 倒したボスの数
	private int deadBossNum = 0;

	// 倒した中ボスの数
	private int deadMiddleBossNum = 0;

	//GUI表示タイマー
	private float timer;

	//GUI表示Flag
	private bool isShowGui = false;

	//GUI表示までの時間
	private float showTime = 1;


	void Start ()
	{
		Initialize ();
	}

	void Update ()
	{
		playerLife = playerlifeObj.getPlayerHp();

		// スコアがハイスコアより大きければ保存
		if (highScore < score) {
			highScore = score;
		}

		//スコアが変わっていたら新しいスコアを表示
		if (score != oldScore) {
			scoreGUIText.text = score.ToString ();
			oldScore = score;
		}

		//プレイヤーの残機数が変わっていたら、新しい残機を表示
		if (playerLife != oldPlayerLife){
			playerHpText.text = playerlifeObj.getPlayerHp().ToString ();
			oldPlayerLife = playerLife;
		}

		//フラグが立っている場合、指定秒後にGUIを表示
		if (isShowGui) {
			timer += Time.deltaTime;
			if(timer > showTime){
				showScoreGui();
			}
		}
	}
	
	// ゲーム開始前の状態に戻す
	private void Initialize ()
	{
		//PlayerPrefs.SetInt (highScoreKey, 0); //テスト用に、ハイスコアを初期化

		//ゲームスピードを初期化
		Time.timeScale = 1;

		//倒した敵の数を初期化
		initDeadEnemyNum();

		// ハイスコアを取得し表示する。保存されてなければ0を取得する。
		highScore = PlayerPrefs.GetInt (highScoreKey, 0);
		highScoreGUIText = GameObject.Find ("HighScore").GetComponent<Text>();
		highScoreGUIText.text = "HighScore : " + highScore.ToString ();

		//自機ライフオブジェクトを取得し、残機数を設定
		playerlifeObj = GetComponent<Life> ();
		playerLife = playerlifeObj.getPlayerHp();

		//ステージクリア時の表示であるかどうかを判別
		if (Manager.isNextStage != true) {
			// スコアと残機を初期化
			score = 0;
			playerlifeObj.setPlayerHp (Manager.playerLifeDefault);
		} else {
			// クリア時のスコアと残機を設定
			score = Manager.lastScore;
			playerlifeObj.setPlayerHp (playerLife);

			//次ステージフラグを初期化
			Manager.isNextStage = false;
		}
	}
	
	// ポイントの追加
	public void AddPoint (int point)
	{
		score = score + point;
		setDeadEnemyNum();
	}
	
	// ハイスコアの保存
	public void Save ()
	{
		// ハイスコアを保存する
		PlayerPrefs.SetInt (highScoreKey, highScore);
		PlayerPrefs.Save ();
		
		// ゲーム開始前の状態に戻す
		Initialize ();
	}

	//クリアGUI表示フラグを切り替え
	public void setupScoreGui(){
		isShowGui = true;
	}

	//クリアGUI表示
	public void showScoreGui(){

		//クリアGUIを表示
		Manager managerObj = FindObjectOfType<Manager> ();
		managerObj.showClearGui();

		//クリア時のスコアを表示
		Manager.lastScore = getScore();
		
		//倒した敵の数を表示
		GameObject.Find ("totalScoreP").GetComponent<Text>().text = getScore().ToString();
		GameObject.Find ("bossP").GetComponent<Text>().text = "× " + getDeadBossNum().ToString();
		GameObject.Find ("enemySP").GetComponent<Text>().text = "× " + getDeadMiddleBossNum().ToString();
		GameObject.Find ("enemyNP").GetComponent<Text>().text = "× " + getDeadEnemyNum().ToString();

		//最終ステージの場合はENDボタンを表示
		string stageName = Application.loadedLevelName;
		string lastStageName = Manager.stagePatarns[Manager.stagePatarns.Length - 2];
		if (stageName == lastStageName) {
			GameObject.Find ("nextBtnText").GetComponent<Text>().text = "End";
		}

		//次ステージへ遷移フラグを立てる
		Manager.isNextStage = true;

		//ゲームを停止
		Time.timeScale = 0; //TODO Manager側で停止したい。
		managerObj.isPause = true;
	}

	//倒した敵の数を初期化
	public void initDeadEnemyNum(){
		deadEnemyNum = 0;
		deadBossNum = 0;
		deadMiddleBossNum = 0;
	}

	/**
	 * ==========================================
	 * Setter and Getter
	 * ==========================================
	 */
	//スコアを取得
	public int getScore(){
		return score;
	}
	//倒した敵の数を設定
	public void setDeadEnemyNum(){
		this.deadEnemyNum ++;
	}
	//倒した敵の数を取得
	public int getDeadEnemyNum(){
		return this.deadEnemyNum;
	}
	//倒したボスの数を設定
	public void setDeadBossNum(){
		this.deadBossNum ++;
	}
	//倒したボスの数を取得
	public int getDeadBossNum(){
		return this.deadBossNum;
	}
	//倒した中ボスの数を設定
	public void setDeadMiddleBossNum(){
		this.deadMiddleBossNum ++;
	}
	//倒した中ボスの数を取得
	public int getDeadMiddleBossNum(){
		return this.deadMiddleBossNum;
	}
}