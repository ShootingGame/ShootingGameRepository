using UnityEngine;
using System.Collections;

public class EnemyBoss : MonoBehaviour
{
	// ヒットポイント
	// staticにするとインスペクターで設定できなくなる
	// 全ての敵で同一のHPになってしまうので注意
	//同じ種類の敵で別々のHPを設定するならHPのstaticは禁止
	//現在は敵の種類だけスクリプトを用意する仕様なので
	//static化すると全部の敵に同じHPが適用されてしまう
	public int hp = 1;
	
	// スコアのポイント
	public int point = 100;

	//ボス移動追加分
	public int count = 0; 
	//ボス移動追加分
	public float delta = 1;

	//クリアしたかのフラグ
	//private bool clear = false;

	//インスペクターから設定する項目
	//この値によってザコ敵に設定する弾丸を決めるメソッドをかえる
	public int shotFlag = 1;
	
	// Spaceshipコンポーネント
	Spaceship spaceship;

	//EnemyMoveンポーネント
	//EnemyMove enemyMove;

	//Player
	//private GameObject player_obj;

	//大量に出てくるザコ敵のMove処理はUpdate(){}の中に書いてはいけない
	//激しく処理落ちする
	//ボス追加分(軌道部分)
	void Update(){
		Move (transform.right * delta);
		count++;
		if(count >= 20){
			delta = delta * -1;
			count = -40;
		}
		/*
		if(hp <= 110 && hp >= 90){
			delta = delta + 0.1f;
			return;
		}
		*/
	}

	// 機体の移動
	public void Move (Vector2 direction)
	{
		rigidbody2D.velocity = direction * spaceship.speed;
	}

	IEnumerator Start ()

	{
		
		// Spaceshipコンポーネントを取得
		spaceship = GetComponent<Spaceship> ();

		//enemyMove = GetComponent<EnemyMove> ();

		//直進する場合はこちらを使用
		//enemyMove.EnemyMoveStraight (transform.up * -1);

		//Player -> null 
		//Player(Clone) -> OK
		//player_obj = GameObject.Find ("Player(Clone)");

		//プレイヤーに向かって突撃させる場合はこちらを使用
		/*
		if (player_obj != null) {
			enemyMove.EnemyLookAt2D (player_obj);
			enemyMove.EnemyMoveHoming (player_obj);
		}*/




		// canShotがfalseの場合、ここでコルーチンを終了させる
		if (spaceship.canShot == false) {
			yield break;
		}


		while (true) {
			
			// 子要素を全て取得する
			for (int i = 0; i < transform.childCount; i++) {
				
				Transform shotPosition = transform.GetChild (i);


				// ShotPositionの位置/角度で弾を撃つ

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
		// レイヤー名を取得
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		
		// レイヤー名がBullet (Player)以外の時は何も行わない
		if (layerName != "Bullet(Player)") return;
		
		// PlayerBulletのTransformを取得
		Transform playerBulletTransform = c.transform.parent;
		
		// Bulletコンポーネントを取得
		Bullet bullet =  playerBulletTransform.GetComponent<Bullet>();

		// ヒットポイントを減らす
		hp = hp - bullet.power;
		
		// ヒットポイントが0以下であれば
		if(hp <= 0 )
		{
			// スコアコンポーネントを取得してポイントを追加
			FindObjectOfType<Score>().AddPoint(point);
			// 爆発
			spaceship.Explosion ();
			// エネミーの削除
			Destroy (gameObject);
			//フラグ（ステージクリア）
			//clear = true;

			//スコアGUIの呼び出し
			FindObjectOfType<Score>().setupScoreGui();

		}else{
			
			spaceship.GetAnimator().SetTrigger("Damage");
			Destroy (c.gameObject);
		}
	}
}