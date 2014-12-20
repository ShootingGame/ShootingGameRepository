using UnityEngine;
using System.Collections;

//********************************************************
//通常のボスの挙動
//このスクリプトを参考にして、各自のボスの挙動を設定
//*******************************************************
public class BossNormal : Enemy {

	// Use this for initialization
	IEnumerator Start () {
		
		//共通処理
		this.setUp ();
		
		//ボスの移動を指定
		if (this.playerObj != null) {
		}
		
		// 弾を発射しないボスの場合はここで処理を終了
		if (spaceship.canShot == false) {
			yield break;
		}
		
		// 弾を発射するボスの場合は発射
		while (true) {
			
			// エネミーが停止していない場合に、子要素を全て取得し、ShotPositionの位置/角度で弾を撃つ
			if (this.isEnemyPause != true && this.isEnterDestroyArea != false) {
				for (int i = 0; i < transform.childCount; i++) {
					Transform shotPosition = transform.GetChild (i);
					spaceship.EnemyShot(shotPosition);
				}
			}
			
			yield return new WaitForSeconds (this.spaceship.shotDelay); // shotDelay秒待つ
		}
	}

	//大量に出てくるザコ敵のMove処理はUpdate(){}の中に書いてはいけない 激しく処理落ちする
	void Update ()
	{
		//共通処理
		this.checkPause ();

		//TODO Sample ボス個別の処理を記載
		//ボスの追尾 || Player(Clone)がNullでない場合のみ追尾
		if (playerObj != null) {
			enemyMove.EnemyLookAt2D (playerObj);
			enemyMove.EnemyMoveHoming (playerObj);
		}
	}
	
	void OnTriggerEnter2D (Collider2D c)
	{
		//共通処理
		this.checkCollision (c);
	}

}