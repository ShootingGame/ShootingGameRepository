using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//TODO item changer
public class ShotTypeChanger : MonoBehaviour {


	//スプライト画像
	public Sprite sp1;
	public Sprite sp2;
	public Sprite sp3;
	public Sprite sp4;

	//Imageオブジェクト
	public GameObject shotChanger1;
	public GameObject shotChanger2;
	public GameObject shotChanger3;
	public GameObject shotChanger4;

	public GameObject shotImg1;
	public GameObject shotImg2;
	public GameObject shotImg3;
	public GameObject shotImg4;

	//現在のショット番号
	public int currentShotNum = 1;
	
	Button btn1;
	Button btn2;
	Button btn3;
	Button btn4;

	ColorBlock cb1;
	ColorBlock cb2;
	ColorBlock cb3;
	ColorBlock cb4;

	bool is1done = false;
	bool is2done = false;
	bool is3done = false;
	bool is4done = false;

	public ShotType shotType;

	public Dictionary<string, int> dicta = new Dictionary<string, int>();

	void Start(){

		shotType = GetComponent<ShotType> ();

		//各種ボタンを保持
		btn1 = shotChanger1.GetComponent<Button>();
		btn2 = shotChanger2.GetComponent<Button>();
		btn3 = shotChanger3.GetComponent<Button>();
		btn4 = shotChanger4.GetComponent<Button>();

		//初期状態ではノーマール弾の画像のみを表示
		shotChanger1.SetActive(true);
		shotChanger2.SetActive(false);
		shotChanger3.SetActive(false);
		shotChanger4.SetActive(false);

		dicta.Add("shot1",1);

	}

	void Update(){

	}

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

		//同じ武器をすでに取得してない場合のみ表示
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

					dicta.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
					break;
				case 2:
					shotChanger3.SetActive (true);
					changeColor (3);
					targetImg = GameObject.Find ("shotImage3").GetComponent<Image> ();
					changeImg (argShotType, targetImg);
					
					dicta.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
					break;
				case 3:
					shotChanger4.SetActive (true);
					changeColor (4);
					targetImg = GameObject.Find ("shotImage4").GetComponent<Image> ();
					changeImg (argShotType, targetImg);
					
					dicta.Add("shot" + argShotType.ToString(),currentShotNum + 1);//TODO
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
		changeColor (argBtnNum);
		List<KeyValuePair<string, int>> sPair = new List<KeyValuePair<string, int>>(dicta);
		foreach (var item in sPair)
		{
			if(item.Value == argBtnNum){
				Debug.Log (item.Key + "pressed...");
				if(item.Key == "shot1"){
					shotType.setShotType_1();
					shotType.setPlayerShotDelay_1();
				}else if(item.Key == "shot2"){
					shotType.setShotType_2();
					shotType.setPlayerShotDelay_2();
				}else if(item.Key == "shot3"){
					shotType.setShotType_3();
					shotType.setPlayerShotDelay_3();
				}else if(item.Key == "shot4"){
					shotType.setShotType_4();
					shotType.setPlayerShotDelay_4();
				}
			}
		}


	}
}