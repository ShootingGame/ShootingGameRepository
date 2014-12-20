using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {

	public GameObject startGui;
	public GameObject charaGui;
	public GameObject menuGui;

	public Text modeText;
	public Text lifePoint;


	// Use this for initialization
	void Start () {

		//難易度とプレイヤーライフの初期表示を設定
		Manager.gameMode = 2;
		changeModeAndLife (Manager.gameMode);

		//GUIの初期表示を設定
		startGui.SetActive(true);
		charaGui.SetActive(false);
		menuGui.SetActive (false);
	}


	//スタートボタン押下時の処理
	public void onClickStart(){
		startGui.SetActive(false);
		charaGui.SetActive(true);
		menuGui.SetActive (false);
	}

	//メニューボタン押下時の処理
	public void onClickMenu(){
		startGui.SetActive(false);
		charaGui.SetActive(false);
		menuGui.SetActive (true);
	}

	//ゲーム難易度アップ処理
	public void onClickGameModeUp(){
		if (Manager.gameMode < 3) {
			Manager.gameMode ++;
		}
		changeModeAndLife (Manager.gameMode);
	}

	//ゲーム難易度ダウン処理
	public void onClickGameModeDown(){
		if (Manager.gameMode > 1) {
			Manager.gameMode --;
		}
		changeModeAndLife (Manager.gameMode);
	}

	//ゲームモードとライフを変更処理 | 引数にモード番号を設定
	public void changeModeAndLife(int argMode){

		switch (argMode) {
			case 1:
				Manager.playerLifeDefault = 20;
				lifePoint.text = "20";
				modeText.text = "Easy";
				break;
			case 2:
				Manager.playerLifeDefault = 10;
				lifePoint.text = "10";
				modeText.text = "Normal";
				break;
			case 3:
				Manager.playerLifeDefault = 5;
				lifePoint.text = "5";
				modeText.text = "Hard";
				break;
			default:
				Manager.playerLifeDefault = 10;
				modeText.text = "Normal";
				lifePoint.text = "10";
				break;
		}
	}

	//プレイヤーを設定 | 引数に機体番号を設定
	public void onClickSelectPlayer(int argNum){
		Manager.selectPlayer (argNum);
	}

	//スタートへ戻るボタン押下時の処理
	public void onClickBackToStart(){
		startGui.SetActive(true);
		charaGui.SetActive(false);
		menuGui.SetActive (false);
	}
}
