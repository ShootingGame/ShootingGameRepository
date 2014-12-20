using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy
{
	//Enemy2 : 弾幕指定専用スクリプト
	//使用にあたっては各Wave内ShotPositionにRoundShotのアタッチ必須
	//回転方向・回転のオンオフ等、一部ShotPositionのInspectorから個々に直指定
	//インスペクターから設定する項目
	//この値によってザコ敵に設定する弾丸を決めるメソッドをかえる
	//public int shotFlag = 1;

	//////////////////////以下弾幕パターンを指定。処理はコルーチンにて分岐

	//WaitShotしたい時はtrueにセット
	public bool WaitShot = false;
	public bool Reverse = false;
	
	//WaitShot使う場合shotflameとtimeに同じ数をセット
	public int shotflame = 30;
	public int time = 30; 

	//敵移動方向変更（横移動の場合はチェック）
	public bool moveL = false;
	public bool moveR = false;
	
	//////////////////////以下BOSS専用の設定項目

	//弾幕を変化させるBOSSの場合チェック
	public bool shotChangeEnemy = false;
	public bool isMidleBoss = false;

	//インスペクターからボスのパターンのプレファブをぶち込む
	public GameObject[] BossPatern;

	//配列のカウント用の変数
	int arr =0;

	//弾吐いたら消えるパターンのクローンが入る
	private GameObject nextDestroy;
	
	//フレームの数を数える変数
	private int flamecount = 0;

	//BOSSの止まりたいフレーム数をセットする
	public int setcount = 150;

	//ゲームの停止ではなく、ボスの動きとしてのポーズフラグ
	public bool isEnemyPause2;

	//inst()を1回だけ呼ぶためのフラグ
	bool isDanAFinish = false;
	bool isDanBFinish = false;
	bool isDanCFinish = false;
	bool isDanDFinish = false;
	bool isDanEFinish = false;

	//////////////////////以下アップデート処理
	void Update () {

		this.checkPause ();

		//BOSSの弾幕バリエーションをチェンジ
		if (shotChangeEnemy == true) {
			danmakuChange(hp);
		}

		//ゲームポーズではない場合にフレームを加算
		if (isEnemyPause != true) {
			flamecount++;
		}

		//BOSSの止まりたいフレーム数がきたら、止まる位置を保持
		if (flamecount == setcount) {
			if (shotChangeEnemy == true) {
				isEnemyPause2 = true;
				pauseX = transform.position.x;
				pauseY = transform.position.y;
				pauseZ = transform.position.z;
			}
			flamecount = 0;
		}

		//エネミー停止の場合は、現在の位置に移動することで停止を表現 
		//(ポーズボタンでの停止ではなく、ボスの動きの停止)
		if (isEnemyPause2 == true) {
			transform.position = new Vector3(pauseX, pauseY, pauseZ);
		}

	}//Updateここまで

	//////////////////////////////////////ここからコルーチン内の処理
	IEnumerator Start ()
	{
		this.setUp ();
		
		//移動方向変更
		if (moveR == false && moveL == false) {
			enemyMove.EnemyMoveStraight (transform.up * -1);
		} else if (moveR == true && moveL == false) {
			enemyMove.EnemyMoveStraight (transform.right * 1);
		} else if (moveR == false && moveL == true) {
			enemyMove.EnemyMoveStraight (transform.right * -1);
		}

		// canShotがfalseの場合、ここでコルーチンを終了させる
		if (spaceship.canShot == false) {
			yield break;
		}

		//弾幕パターン分岐（コルーチン直下）・WaitShot処理
		if (WaitShot == true) {
			while (true) {
				if (this.isEnemyPause != true && this.isEnterDestroyArea != false) {
					// 子要素を全て取得する
					for (int i = 0; i < transform.childCount; i++) {
						Transform shotPosition = transform.GetChild (i);
						// ShotPositionの位置/角度で弾を撃つ
						spaceship.EnemyShot (shotPosition);
						time--;
					}
				}
				yield return new WaitForSeconds (spaceship.shotDelay);

				if (time > 0) {
					// shotDelay秒待つ					
					yield return new WaitForSeconds (spaceship.shotDelay);
				} else {
					yield return new WaitForSeconds (1);
					time = shotflame;
				}

			}  //waitShotオンの無限ループはここまで
			
		}else{     //弾幕パターン分岐（コルーチン直下）・Normalshot 
			while (true) {
				if (this.isEnemyPause != true && this.isEnterDestroyArea != false) {
				// 子要素を全て取得する
				for (int i = 0; i < transform.childCount; i++) {
					Transform shotPosition = transform.GetChild (i);
					// ShotPositionの位置/角度で弾を撃つ
					spaceship.EnemyShot (shotPosition);
					}
				}
				// shotDelay秒待つ
				yield return new WaitForSeconds (spaceship.shotDelay);
				
			}//Normalショットの無限ループここまで
			
		}//弾幕パターン分岐（コルーチン直下）ここまで
		
	}//以上がコルーチンの中身
	
	//////////////////////////////////////ここからその他の関数

	//弾幕バリエーションをチェンジ
	public void danmakuChange(int hp){
		if(hp <= 5000){
			
			if(isDanAFinish == false){
				nextDestroy = inst (BossPatern[arr]);
				arr++;
				isDanAFinish = true;
				Debug.Log("arr =" + arr +"  hp" + hp );
			}
		}
		
		if(hp <= 4000){
			if(isDanBFinish == false){
				Destroy(nextDestroy);
				nextDestroy = inst (BossPatern[arr]);
				arr++;
				isDanBFinish = true;
				Debug.Log("arr =" + arr +"  hp" + hp );
			}
		}
		
		if(hp <= 3000){
			if(isDanCFinish == false){
				Destroy(nextDestroy);
				nextDestroy = inst (BossPatern[arr]);
				arr++;
				isDanCFinish = true;
				Debug.Log("arr =" + arr +"  hp" + hp );
			}
		}
		if(hp <= 2000){
			if(isDanDFinish == false){
				Destroy(nextDestroy);
				nextDestroy = inst (BossPatern[arr]);
				arr++;
				isDanDFinish = true;
				Debug.Log("arr =" + arr +"  hp" + hp );
			}
		}
		if(hp <= 1000){
			if(isDanEFinish == false){
				Destroy(nextDestroy);
				nextDestroy = inst (BossPatern[arr]);
				arr++;
				isDanEFinish = true;
				Debug.Log("arr =" + arr +"  hp" + hp );
			}
		}
	}
	
	//BOSS専用・攻撃パターンを生成する関数
	//パターンが入った配列の中のパターンオブジェクトを受け取ってパターンのEnemyを生成。
	//次にデストロイする予定のパターンオブジェクトを返して、送り側でデストロイ。さらに次の弾幕チェンジに備える
	GameObject inst(GameObject g){

		Debug.Log ("inst!!!! arr =" + arr);
		g = (GameObject)Instantiate (BossPatern[arr], transform.position, Quaternion.identity);
		g.transform.parent = transform;
		return g;

	}

	//////////////////////////////////////	
	
	void OnTriggerEnter2D (Collider2D c)
	{
		//共通処理
		this.checkCollision (c);
	}
}