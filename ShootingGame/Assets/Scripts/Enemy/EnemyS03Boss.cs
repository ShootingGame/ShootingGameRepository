using UnityEngine;
using System.Collections;

public class EnemyS03Boss : Enemy {

	//ボス移動追加分
	public int count = 0; 
	//ボス移動追加分
	public float delta = 1;
	
	//ボス追加分(軌道部分)
	void Update(){
		Move (transform.right * delta);
		count++;
		if(count >= 10){
			delta = delta * -1;
			count = -30;
		}
	}
	
	// 機体の移動
	public void Move (Vector2 direction)
	{
		rigidbody2D.velocity = direction * (spaceship.speed / 1.5f);
	}
	
	IEnumerator Start ()
	{
		this.setUp ();
		
		// canShotがfalseの場合、ここでコルーチンを終了させる
		if (spaceship.canShot == false) {
			yield break;
		}
		while (true) {
			// 子要素を全て取得する
			for (int i = 0; i < transform.childCount; i++) {
				Transform shotPosition = transform.GetChild (i);
				spaceship.EnemyShot(shotPosition);
			}
			// shotDelay秒待つ
			yield return new WaitForSeconds (spaceship.shotDelay);
		}
	}
	
	//ぶつかってくるのは自機の弾丸のみ想定
	//自機がダメージを受ける処理はプレイヤー側に記述
	void OnTriggerEnter2D (Collider2D c)
	{
		//共通処理
		this.checkCollision (c);
	}
}
