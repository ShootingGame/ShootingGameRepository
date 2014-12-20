using UnityEngine;
using System.Collections;

public class RoundShot : MonoBehaviour {
	
	//弾幕指定専用スクリプト
	//使用にあたってはEnemyにEnemy2スクリプトをアタッチ

	//逆回転
	public bool RoundR = false;
	//回転なし
	public bool off = false;
	//ランダムshot
	public bool random = false;
	//public GameObject sp;
	//private Vector3 targetPos;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (off == true) {
			
		} else {
			
			if (RoundR == false) {
				transform.Rotate (0, 0, 1);
			} else {
				transform.Rotate (0, 0, -1);
			}
		}
		if(random == true){
			float num = Random.Range(0.0f, 360f);
			this.transform.Rotate (0, 0, num);

		}
	}
}