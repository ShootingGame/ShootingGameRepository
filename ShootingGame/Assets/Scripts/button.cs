using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class button : MonoBehaviour {


	Emitter emitter;
	//public Manager managerObj;
	Manager managerObj;

	// Use this for initialization
	void Start () {
		managerObj = FindObjectOfType<Manager> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	//continue yes btn
	public void YesOnClick(){
		managerObj.retry = true;
		//Waveの最初からプレイ
		//FindObjectOfType<Emitter> ().currentWave = 0;
		managerObj.GameStart ();
	}

	//continue no btn
	public void NoOnClick(){
		managerObj.isEndByNoBtn = true;
		//Application.LoadLevel("01StartMenu");
	}

	//TODO pause btn
	public void pauseBtnClick(){

		GameObject pauseMark = GameObject.Find ("PauseMarkText");

		if(managerObj.isPause){
			managerObj.isPause = false;
			pauseMark.GetComponent<Text>().color = Color.white;
		}else{
			managerObj.isPause = true;
			pauseMark.GetComponent<Text>().color = Color.red;
		}
	}

	//次のステージへ遷移ボタン押下処理
	public void onClickNextStage(){
		managerObj.toNextStage ();
	}

	//ボムボタン押下時の処理
	public void onClickBomb(){
		managerObj.isBomb = true;
	}

}
