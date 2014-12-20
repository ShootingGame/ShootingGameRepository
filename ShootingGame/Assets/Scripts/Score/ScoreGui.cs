using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGui : MonoBehaviour {

	public Text totalScoreP;
	public Text bossP;
	public Text enemySP;
	public Text enemyNP;
	public Text nextBtnText;

	int score = 0;
	int enemyNum = 0;
	int enemySNum = 0;
	int bossNum = 0;

	public string stageName;
	public string lastStageName;



	// Use this for initialization
	void Start () {

		lastStageName = "Stage01";

		//Application.LoadLevel("Stage05");

		//各スコア値を設定
		score = FindObjectOfType<Score>().getScore();
		enemyNum = FindObjectOfType<Score>().getDeadEnemyNum();
		enemySNum = FindObjectOfType<Score>().getDeadMiddleBossNum();
		bossNum = FindObjectOfType<Score>().getDeadBossNum();

		//値を表示
		totalScoreP.text = score.ToString();
		bossP.text = "× " + bossNum.ToString();
		enemySP.text = "× " + enemySNum.ToString();
		enemyNP.text = "× " + enemyNum.ToString();

		stageName = Application.loadedLevelName;

		if (stageName == lastStageName) {
			nextBtnText.text = "End";
		}

	}
	
	// Update is called once per frame
	/*
	void Update () {
	}
	*/

	public void toNextStage(){

		if (stageName == lastStageName) {
			Application.LoadLevel ("Thanks");
		} else {
			if (stageName == "Stage01") {
				Application.LoadLevel("Stage11");
			}else if (stageName == "Stage11") {
				Application.LoadLevel("Stage05");
			}else if (stageName == "Stage05") {
				Application.LoadLevel("Stage03");
			}else if (stageName == "Stage03") {
				Application.LoadLevel("Stage06");
			}else if (stageName == "Stage06") {
				Application.LoadLevel("Stage12");
			}else if (stageName == "Stage12") {
				Application.LoadLevel("lastStageName");
			}
		}



	}
}
