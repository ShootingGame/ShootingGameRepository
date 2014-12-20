using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	// スコアを表示するGUIText
	public Text scoreGUIText;
	
	// ハイスコアを表示するGUIText
	public Text highScoreGUIText;
	
	//どうしてもうまくいかないので
	//ScoreのGUIに便乗する形で
	//Score.csに自機のライフを表示させる処理を書こうとしているところ
	public GUIText playerHpGUIText;

	//プレイヤー数
	public Text playerHpText;
	
	Life playerlife;
	
	// スコア
	private int score;
	
	// ハイスコア
	private int highScore;

	private int playerHp;
	
	// PlayerPrefsで保存するためのキー
	private string highScoreKey = "highScore";
	
	//倒した敵の数
	private int deadEnemyNum = 0;

	//倒したボスの数
	private int deadBossNum = 0;

	//倒した中ボスの数
	private int deadMiddleBossNum = 0;

	void Start ()
	{
		Initialize ();
	}
	
	void Update ()
	{
		// スコアがハイスコアより大きければ
		if (highScore < score) {
			highScore = score;
		}

		/*
		 * 11/8
		プレイヤーのライフの変数はstaticにしてある。
		Lifeクラスと、Guiクラスで共通化させるため。
		通常のメンバ変数だとライフが独立してしまい
		全く連携されない
		*/
		//playerHp = playerlife.getPlayerHp();
		playerHpGUIText.text = playerHp.ToString ();
		//print ("UPDATE >>>>>> " + playerlife.getPlayerHp());
		
		playerHpGUIText.text = playerlife.getPlayerHp().ToString ();
		scoreGUIText.text = score.ToString ();
		highScoreGUIText.text = "HighScore : " + highScore.ToString ();

		//TODO プレイヤーの残機
		playerHpText.text = playerlife.getPlayerHp().ToString ();

	}
	
	// ゲーム開始前の状態に戻す
	private void Initialize ()
	{
		//For test clear high score
		//PlayerPrefs.SetInt (highScoreKey, 0);

		highScoreGUIText = GameObject.Find ("HighScore").GetComponent<Text>();

		//倒した敵の数を0に戻す
		deadEnemyNum = 0;
		deadBossNum = 0;
		deadMiddleBossNum = 0;

		// ハイスコアを取得する。保存されてなければ0を取得する。
		highScore = PlayerPrefs.GetInt (highScoreKey, 0);

		if (Manager.isNextStage != true) {
			// スコアを0に戻す
			score = 0;
			//ライフを初期値に戻し
			playerlife = GetComponent<Life> ();
			playerlife.setPlayerHp (Manager.playerLifeDefault);
		} else {
			// スコア
			score = Manager.lastScore;
			Debug.Log ("last sore is " + score);
			//ライフを初期値に戻し
			playerlife = GetComponent<Life> ();
			playerlife.setPlayerHp (GetComponent<Life> ().getPlayerHp());
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


	public void setupScoreGui(){
		Manager managerObj = FindObjectOfType<Manager> ();

		//クリアGUIを表示
		managerObj.showClearGui();
		Manager.lastScore = getScore();
		
		//値を表示
		GameObject.Find ("totalScoreP").GetComponent<Text>().text = getScore().ToString();
		GameObject.Find ("bossP").GetComponent<Text>().text = "× " + getDeadBossNum().ToString();
		GameObject.Find ("enemySP").GetComponent<Text>().text = "× " + getDeadMiddleBossNum().ToString();
		GameObject.Find ("enemyNP").GetComponent<Text>().text = "× " + getDeadEnemyNum().ToString();
		
		string stageName = Application.loadedLevelName;
		
		if (stageName == managerObj.lastStageName) {
			GameObject.Find ("nextBtnText").GetComponent<Text>().text = "End";
		}

		Manager.isNextStage = true;
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

	//倒した敵の数を0に戻す
	public void initDeadEnemyNum(){
		deadEnemyNum = 0;
		deadBossNum = 0;
		deadMiddleBossNum = 0;
	}
}