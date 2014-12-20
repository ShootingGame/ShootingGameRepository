using UnityEngine;
using System.Collections;

//********************************************************
//通常のエネミーの挙動
//このスクリプトを参考にして、各自のエネミーの挙動を設定
//*******************************************************
public class EnemyNormal : Enemy {

	// Use this for initialization
	IEnumerator Start () {

		//共通処理
		this.setUp ();

		//エネミーの移動を指定
		if (this.playerObj != null) {
			//直進する場合はこちらを使用
			//enemyMove.EnemyMoveStraight (transform.up * -1);

			//プレイヤーに向かって突撃させる場合はこちらを使用
			this.enemyMove.EnemyLookAt2D (playerObj);
			this.enemyMove.EnemyMoveHoming (playerObj);
		}

		// 弾を発射しないエネミーの場合はここで処理を終了
		if (spaceship.canShot == false) {
			yield break;
		}

        // 弾を発射するエネミーの場合は発射
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
	}


	void OnTriggerEnter2D (Collider2D c)
	{
		//共通処理
		this.checkCollision (c);
	}
}
