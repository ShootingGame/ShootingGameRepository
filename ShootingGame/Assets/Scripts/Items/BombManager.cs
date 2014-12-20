using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombManager : MonoBehaviour {

	//ボム数 表示テキスト
	public Text bombNumText;

	public GameObject bombBtnObj;

	//ボムのダメージ数
	public int bombPower = 300;

	//ボムの初期値
	static int defaultBombNum = 1;

	Manager managerObj;

	//ボムの数
	int bombNum;

	void Start () {
		bombNum = defaultBombNum;
		managerObj = FindObjectOfType<Manager>();
	}

	void Update () {
		//if (Input.GetKeyDown (KeyCode.B) && bombNum > 0) {
		if (Input.GetKeyDown (KeyCode.B)) {
			managerObj.isBomb = true;	
		}
		if (managerObj.isBomb && bombNum > 0) {
			//ボム爆発処理
			GameObject prefabBomb = (GameObject)Resources.Load ("Explode_Bomb");
			Instantiate (prefabBomb, transform.position, Quaternion.identity);

			//ボム数を減算
			subtractBombNum();

			//オプションを減らす
			int flwNum = FindObjectOfType<FollowerManager> ().getFollowersNum();
			if(flwNum >0){
				FindObjectOfType<FollowerManager> ().setFollowersNum(flwNum -2);
				GameObject prefab = (GameObject)Resources.Load ("followers");
				Instantiate (prefab, transform.position, Quaternion.identity);
			}
			managerObj.isBomb = false;
		}
		bombNumText.text = getBombNum().ToString();	

		//bomb color change
		Button bombBtn = bombBtnObj.GetComponent<Button>();
		ColorBlock cb1;
		cb1 = bombBtn.colors;
		if (bombNum != 0) {
			cb1.normalColor = Color.red;
		} else {
			cb1.normalColor = Color.white;
		}
		bombBtn.colors = cb1;
	}

	//ボムの減算
	public void subtractBombNum(){
		bombNum --;
	}

	//ボムの加算
	public void increaseBombNum(){
		bombNum ++;
	}

	//ボム数を取得
	public int getBombNum(){
		return bombNum;
	}
}
