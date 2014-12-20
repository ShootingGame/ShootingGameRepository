using UnityEngine;
using System.Collections;

public class EnemyBoss2 : MonoBehaviour
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
	private float delta = 0.0f;
	private float deltab = 0.0f;

	//private float enemyY;
	//private float enemyX;
	int random = Random.Range(0, 6);

	
	//クリアしたかのフラグ
	//private bool clear = false;

	// Spaceshipコンポーネント
	Spaceship spaceship;
	
	//EnemyMoveンポーネント
	EnemyMove enemyMove;
	
	//Player
	private GameObject player_obj;
	
	//インスペクターから設定する項目
	//この値によってザコ敵に設定する弾丸を決めるメソッドをかえる
	public int shotFlag = 1;





	
	//大量に出てくるザコ敵のMove処理はUpdate(){}の中に書いてはいけない
	//激しく処理落ちする
	//ボス追加分(軌道部分)

	void Update(){

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

				Move (transform.up * delta + transform.right * deltab);
				count++;
				//enemyY = transform.position.y;
				//enemyX = transform.position.x;
				switch(random){
				case 0:
					print (count);
					if(count == 0)
						{if(transform.position != new Vector3( 0, -2, 0)){
							transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
							}
					}

					random = 0;	
					if(count >= 1){
						deltab = 0.5f;
						}
					if(count >= 4)deltab = -0.5f;
					if(count >= 10)deltab = 0.5f;
					if(count >= 13){
					deltab = 0.0f;
					count = 0;
					random = Random.Range(2, 6);	
					}
					break;

				case 1:
					print (count);
					if(count == 0)
					{if(transform.position != new Vector3( 0, -2, 0)){
							transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
						}
					}

					random = 1;
					if(count >= 1){
						delta = 0.0f;
						deltab = 0.25f;
						}
					if (player_obj != null) {
						enemyMove.EnemyMoveHoming (player_obj);
					}
					if(count >= 3){if (player_obj != null) {
							enemyMove.EnemyLookAt2D (player_obj);
						}
						deltab = 0.0f;
						delta = 0.1f;
					}
					if(count >= 5){
						delta = -1.0f;
					}
					if(count >= 7){
						delta = 0.0f;
					}
					if(count >= 10){
						delta = 0.1f;
					}
					if(count >= 20){
						delta = 0.0f;
						deltab = 0.0f;
						transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, -2, 0) , 5.0f);
						count = 0;
						random = Random.Range(2, 6);
					}
					break;

				case 2:
					/*if(count == 0 && transform.position != new Vector3( 0, 2, 0)){
							transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
					}*/
					print (count);
					random = 2;
					if(count >= 1)
						{delta = -0.1f;
						deltab = -0.5f;
						}
					if(count >= 4)
					{
						deltab = 0.5f;
					}
					if(count >= 9)
					{
						deltab = -0.5f;
					}
					if(count >= 15)
					{
						deltab = 0.5f;
					}
					if(count >= 21)
					{
						deltab = -0.5f;
					}
					if(count >= 27)
					{
						deltab = 0.5f;
					}
					if(count >= 30){
						delta =0.0f;
						deltab = 0.0f;
						transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
					}
					if(count >= 39){
						random = Random.Range(0, 6);
						count = 0;
						}
					break;

				case 3:
					print (count);
					random = 3;
					if(count >= 1)
						{
						delta = -0.5f;
						deltab = -0.5f;
						}
					if(count >= 4){
						deltab = 0.5f;
					}
					if(count >= 7)
					{
						delta = 0.5f;
						deltab = 0.5f;
					}
					if(count >= 10){
						deltab = -0.5f;
					}
					if(count >= 13)
					{	
						delta = 0.0f;
						deltab = 0.0f;
					}
					if(count >= 20){
						{
							if(transform.position != new Vector3( 0, -2, 0)){
								transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
							}
						}
					}
					if(count >= 14){
					delta = 0.0f;
					deltab = 0.0f;
					count = 0;
					random = Random.Range(0, 6);
					}
					break;

				case 4:
					print (count);
					if(count == 0)
					{if(transform.position != new Vector3( 0, -2, 0)){
							transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, 2, 0) , 5.0f);
						}
					}
					
					random = 4;
					if(count >= 1){
						delta = 0.0f;
						deltab = -0.25f;
					}
					if (player_obj != null) {
						enemyMove.EnemyMoveHoming (player_obj);
					}
					if(count >= 3){if (player_obj != null) {
							enemyMove.EnemyLookAt2D (player_obj);
						}
						deltab = 0.0f;
						delta = 0.1f;
					}
					if(count >= 5){
						delta = -1.0f;
					}
					if(count >= 7){
						delta = 0.0f;
					}
					if(count >= 10){
						delta = 0.1f;
					}
					if(count >= 20){
						delta = 0.0f;
						deltab = 0.0f;
						transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, -2, 0) , 5.0f);
						count = 0;
						random = Random.Range(2, 6);
					}
					break;

				case 5:
					print (count);
					random = 5;
					if(count >= 1)
					{
						delta = -0.5f;
						deltab = 0.5f;
					}
					if(count >= 4){
						deltab = -0.5f;
					}
					if(count >= 7)
					{
						delta = 0.5f;
						deltab = -0.5f;
					}
					if(count >= 10){
						deltab = 0.5f;
					}
					if(count >= 13)
					{	
						delta = 0.0f;
						deltab = 0.0f;
					}
					if(count >= 14){
						{
							if(transform.position != new Vector3( 0, -2, 0)){
								transform.position = Vector3.MoveTowards (transform.position , new Vector3( 0, -2, 0) , 5.0f);
							}
						}
					}
					if(count >= 14){
						delta = 0.0f;
						deltab = 0.0f;
						count = 0;
						random = Random.Range(0, 6);
					}
					break;

				default:
					count = 0;
					break;
				}
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
		
		
	}else{
		
		spaceship.GetAnimator().SetTrigger("Damage");
		Destroy (c.gameObject);
	}
}
}