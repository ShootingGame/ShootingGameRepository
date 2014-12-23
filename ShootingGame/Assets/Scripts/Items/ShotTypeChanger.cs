using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// ショット切り替えボタン制御クラス
public class ShotTypeChanger : MonoBehaviour {

	//========== 個別にインスペクターで設定する項目 ==========
	//スプライト画像
	public Sprite sp1, sp2, sp3, sp4;

	//Imageボタン
	public GameObject shotChanger1, shotChanger2, shotChanger3, shotChanger4;

	//弾の画像
	public GameObject shotImg1, shotImg2, shotImg3, shotImg4;

	//現在のショット番号
	public int currentShotNum = 1;

	//========== 使用する各種コンポーネント ==========
	ShotType shotTypeObj;

	//========== メンバ変数 ==========
	Button btn1, btn2, btn3, btn4;

	ColorBlock cb1, cb2, cb3, cb4;

	bool is1done = false;
	bool is2done = false;
	bool is3done = false;
	bool is4done = false;

	//弾の種類リスト
	Dictionary<string, int> shotList = new Dictionary<string, int>();

	void Start(){

		shotTypeObj = GetComponent<ShotType> ();

		//各種ボタンを設定
		btn1 = shotChanger1.GetComponent<Button>();
		btn2 = shotChanger2.GetComponent<Button>();
		btn3 = shotChanger3.GetComponent<Button>();
		btn4 = shotChanger4.GetComponent<Button>();

		//初期状態ではノーマール弾の画像のみを表示
		shotChanger1.SetActive(true);
		shotChanger2.SetActive(false);
		shotChanger3.SetActive(false);
		shotChanger4.SetActive(false);

		//デフォルトの弾を設定
		shotList.Add("shot1",1);

	}
	
	/*
	void Update(){
	}
	*/

	//弾の切り替え処理
	public void changeShot (int argShotType) {

		//同じ武器をすでに取得してないかどうかを確認
		bool isOnlyOne = true;
		switch (argShotType) {
		case 1:
			if(is1done != true){
				is1done = true;
			}else{
				isOnlyOne = false;
			}
			break;
		case 2:
			if(is2done != true){
				is2done = true;
			}else{
				isOnlyOne = false;
			}
			break;
		case 3:
			if(is3done != true){
				is3done = true;
			}else{
				isOnlyOne = false;
			}
			break;
		case 4:
			if(is4done != true){
				is4done = true;
			}else{
				isOnlyOne = false;
			}
			break;
		default:
			break;
		}

		//同じ武器をすでに取得してない場合のみ弾ボタンと弾画像を表示
		if (isOnlyOne) {
			Image targetImg;

			if(argShotType != 1){
				switch (currentShotNum) {
				case 1:
					//if(argShotType != 1){
					shotChanger2.SetActive (true);
					changeColor (2);
					targetImg = GameObject.Find ("shotImage2").GetComponent<Image> ();
					changeImg (argShotType, targetImg);

					shotList.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
					break;
				case 2:
					shotChanger3.SetActive (true);
					changeColor (3);
					targetImg = GameObject.Find ("shotImage3").GetComponent<Image> ();
					changeImg (argShotType, targetImg);
					
					shotList.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
					break;
				case 3:
					shotChanger4.SetActive (true);
					changeColor (4);
					targetImg = GameObject.Find ("shotImage4").GetComponent<Image> ();
					changeImg (argShotType, targetImg);
					
					shotList.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
					break;
				default:
					break;
				}

				currentShotNum ++;
			}
		}

	}

	//ボタンの画像を変更する
	public void changeImg(int argShotType, Image argImg){
		switch (argShotType) {
		case 1:
			argImg.sprite = sp1;
			break;
		case 2:
			argImg.sprite = sp2;
			break;
		case 3:
			argImg.sprite = sp3;
			break;
		case 4:
			argImg.sprite = sp4;
			break;
		default:
			break;
		}
	}

	//ボタンの色を変更する。
	public void changeColor(int argBtnNum){
		
		switch (argBtnNum) {
		case 1:
			cb1 = btn1.colors;
			cb1.normalColor = Color.red;
			btn1.colors = cb1;
			cb2 = btn2.colors;
			cb2.normalColor = Color.white;
			btn2.colors = cb2;
			cb3 = btn3.colors;
			cb3.normalColor = Color.white;
			btn3.colors = cb3;
			cb4 = btn4.colors;
			cb4.normalColor = Color.white;
			btn4.colors = cb4;
			break;
		case 2:
			cb1 = btn1.colors;
			cb1.normalColor = Color.white;
			btn1.colors = cb1;
			cb2 = btn2.colors;
			cb2.normalColor = Color.red;
			btn2.colors = cb2;
			cb3 = btn3.colors;
			cb3.normalColor = Color.white;
			btn3.colors = cb3;
			cb4 = btn4.colors;
			cb4.normalColor = Color.white;
			btn4.colors = cb4;
			break;
		case 3:
			cb1 = btn1.colors;
			cb1.normalColor = Color.white;
			btn1.colors = cb1;
			cb2 = btn2.colors;
			cb2.normalColor = Color.white;
			btn2.colors = cb2;
			cb3 = btn3.colors;
			cb3.normalColor = Color.red;
			btn3.colors = cb3;
			cb4 = btn4.colors;
			cb4.normalColor = Color.white;
			btn4.colors = cb4;
			break;
		case 4:
			cb1 = btn1.colors;
			cb1.normalColor = Color.white;
			btn1.colors = cb1;
			cb2 = btn2.colors;
			cb2.normalColor = Color.white;
			btn2.colors = cb2;
			cb3 = btn3.colors;
			cb3.normalColor = Color.white;
			btn3.colors = cb3;
			cb4 = btn4.colors;
			cb4.normalColor = Color.red;
			btn4.colors = cb4;
			break;
		default:
			break;
		}
	}

	//ショット切り替えボタン押下時の処理
	public void shotChangerOnClick(int argBtnNum){

		// ボタンの色を変更
		changeColor (argBtnNum);

		// 押されたボタンの弾に切り替える
		List<KeyValuePair<string, int>> sPair = new List<KeyValuePair<string, int>>(shotList);
		foreach (var item in sPair)
		{
			if(item.Value == argBtnNum){
				if(item.Key == "shot1"){
					shotTypeObj.setShotType(1);
					shotTypeObj.setPlayerShotDelay(1);
				}else if(item.Key == "shot2"){
					shotTypeObj.setShotType(2);
					shotTypeObj.setPlayerShotDelay(2);
				}else if(item.Key == "shot3"){
					shotTypeObj.setShotType(3);
					shotTypeObj.setPlayerShotDelay(3);
				}else if(item.Key == "shot4"){
					shotTypeObj.setShotType(4);
					shotTypeObj.setPlayerShotDelay(4);
				}else{
					shotTypeObj.setShotType(1);
					shotTypeObj.setPlayerShotDelay(1);
				}
			}
		}
	}
}