using UnityEngine;
using System.Collections;

public class CautionNormal : Enemy {

	// Use this for initialization
	IEnumerator Start () {
		
		//共通処理
		this.setUp ();
		enemyMove.EnemyMoveStraight (transform.right * -1);
		yield break;
	}
	
	//大量に出てくるザコ敵のMove処理はUpdate(){}の中に書いてはいけない 激しく処理落ちする
	void Update () 
	{
		//共通処理
		this.checkPause ();
	}
	
	
	void OnTriggerEnter2D (Collider2D c)
	{
		//共通処理
		this.checkCollision (c);
	}
}
