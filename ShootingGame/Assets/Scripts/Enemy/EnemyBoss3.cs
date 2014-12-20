using UnityEngine;
using System.Collections;

public class EnemyBoss3 : MonoBehaviour
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
	
	//private float delta = Vector3(Random.Range(0.1f,3.0f),Random.Range(0.1f,3.0f),0.0f);
	private float delta = -0.25f;
	private float deltab = 0.0f;
	public int count = 1; 
	private float enemyY;
	//private float enemyX;
	//インスペクターから設定する項目
	//この値によってザコ敵に設定する弾丸を決めるメソッドをかえる
	public int shotFlag = 1;
	
	// Spaceshipコンポーネント
	Spaceship spaceship;
	
	//EnemyMoveンポーネント
	EnemyMove enemyMove;
	
	//Player
	private GameObject player_obj;
	
	//大量に出てくるザコ敵のMove処理はUpdate(){}の中sに書いてはいけない
	//激しく処理落ちする
	void Update () {
		Move (transform.up * delta + transform.right * deltab);
		//enemyX = transform.position.x;
		enemyY = transform.position.y;
		count++;
		if(enemyY <= 2.0f)
		{
			delta = 0.0f;
			if (count >= 0) {
				deltab = 0.1f;
			}
			if (count >= 120) {
				deltab = -0.1f;
			}
			if (count >= 240){
				count = 0;
			}
			if(hp <= 500){
				if (count >= 0) {
					deltab = 0.5f;
				}if (count >= 20) {
					deltab = -0.5f;
				}if (count >= 60) {
					deltab = 0.5f;
				}
				if (count >= 80)count =0;
			}
		}
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
		
		enemyMove = GetComponent<EnemyMove> ();
		
		//直進する場合はこちらを使用
		//enemyMove.EnemyMoveStraight (transform.up * -1);
		
		//Player -> null 
		//Player(Clone) -> OK
		player_obj = GameObject.Find ("Player(Clone)");
		
		//プレイヤーに向かって突撃させる場合はこちらを使用
		if (player_obj != null) {
			enemyMove.EnemyLookAt2D (player_obj);
			enemyMove.EnemyMoveHoming (player_obj);
		}
		
		
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
			
		}else{
			
			spaceship.GetAnimator().SetTrigger("Damage");
			
		}
	}
}