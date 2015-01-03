using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
		
	//========== 各プレイヤーで個別にインスペクターで設定する項目 ==========
	//None


	//========== 使用する各種コンポーネント ==========
	FollowerManager followerMng;
	Life playerlife;
	Manager managerObj;
	ShotType shotTypeObj;
	Spaceship spaceship;
	SuperShot superShot;
	AudioManager audioObj;
	ShotTypeChanger shotChangerObj;

	//========== メンバ変数 ==========
	//None


	IEnumerator Start ()
	{

		// コンポーネントを取得
		followerMng = FindObjectOfType<FollowerManager> ();
		playerlife = GetComponent<Life> ();
		managerObj = FindObjectOfType<Manager> ();
		shotTypeObj = GetComponent<ShotType> ();
		spaceship = GetComponent<Spaceship> ();
		superShot = GetComponent<SuperShot> ();
		audioObj = FindObjectOfType<AudioManager> ();
		shotChangerObj = FindObjectOfType<ShotTypeChanger> ();


		//初期の弾の種類を設定
		shotTypeObj.setDefaultShot ();

		// 弾をプレイヤーと同じ位置/角度で作成
		while(true){
			int selectedShotType = shotTypeObj.getShotType();
			Attack(selectedShotType);
			yield return new WaitForSeconds (shotTypeObj.getPlayerShotDelay());	
		}

	}
	
	void Update ()
	{
		if (managerObj.IsGamePause () != true) {
			// 右・左
			float x = Input.GetAxisRaw ("Horizontal");
			
			// 上・下
			float y = Input.GetAxisRaw ("Vertical");
			
			// 移動する向きを求める
			Vector2 direction = new Vector2 (x, y).normalized;
			
			// 移動の制限
			Move (direction);		
		}
	}

	void Attack (int argShotType)
	{

		if (managerObj.IsGamePause () != true) {

			//画面にタッチされた場合に弾を発射
			if(managerObj.isTouch || managerObj.isAlwaysShoot != false){
			//if (Input.GetKey ("space")) {

				switch (argShotType)
				{
				case 1:
					spaceship.Shot (transform);
					audioObj.playShotSound (1);
					break;
				case 2:
					spaceship.Shot2 (transform);
					audioObj.playShotSound (2);
					break;
				case 3:
					spaceship.Shot3 (transform);
					audioObj.playShotSound (3);
					break;
				case 4:
					spaceship.Shot4 (transform);
					audioObj.playShotSound (4);
					break;
				default:
					spaceship.Shot (transform);
					audioObj.playShotSound (1);
					break;
				}
			}
		}
	}
	
	void Move (Vector2 direction)
	{
		// 画面左下のワールド座標をビューポートから取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		
		// 画面右上のワールド座標をビューポートから取得
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		
		// プレイヤーの座標を取得
		Vector2 pos = transform.position;
		
		// 移動量を加える
		pos += direction  * spaceship.speed * Time.deltaTime;
		
		// プレイヤーの位置が画面内に収まるように制限をかける
		pos.x = Mathf.Clamp (pos.x, min.x, max.x);
		pos.y = Mathf.Clamp (pos.y, min.y, max.y);
		
		// 制限をかけた値をプレイヤーの位置とする
		transform.position = pos;
	}

	// ぶつかった瞬間に呼び出される cはぶつかってきた物体のコライダ
	// 敵ならばダメージ アイテムならば取得処理を記載
	void OnTriggerEnter2D (Collider2D c)
	{

		if (managerObj != null && managerObj.IsGamePause () != true) {
			// レイヤー名を取得
			string layerName = LayerMask.LayerToName(c.gameObject.layer);
			
			// レイヤー名がBullet(Enemy)の時は弾を削除
			if( layerName == "Bullet(Enemy)")
			{
				// エネミーの弾の削除。実際には非アクティブにする
				//Destroy(c.gameObject);
				ObjectPool.instance.shootingGamePool(c.gameObject);
			}
			
			// レイヤー名がBullet(Enemy)またはEnemy,Boss1の場合は damage
			if( layerName == "Bullet(Enemy)" || layerName == "Enemy" || layerName == "Boss1")
			{
				spaceship.GetAnimator().SetTrigger("Damage");

				playerlife.damagePlayerHp();
				
				spaceship.Explosion();
				
				//Game Over
				if(playerlife.getPlayerHp() <= 0 )
				{
					// Manager GameOverメソッドを呼び出す
					managerObj.GameOver();
					
					// 爆発する
					spaceship.Explosion();
					
					//オプションを削除
					followerMng.destroyOldFollowers();
					
					//ゲームを停止
					managerObj.isPause = true;
					
					// プレイヤーを削除
					Destroy (gameObject);
				}
			}
			
			// ショット切り替えアイテムの取得時の処理
			int nextShotType = 0;
			if (layerName == "ItemN"){
				nextShotType = 1;
			} else if (layerName == "ItemE"){
				nextShotType = 2;
			} else if (layerName == "ItemL"){
				nextShotType = 3;
			} else if (layerName == "ItemM"){
				nextShotType = 4;
			}
			if (nextShotType != 0){
				shotTypeObj.setShotType(nextShotType);
				shotTypeObj.setPlayerShotDelay(nextShotType);
				shotChangerObj.changeShot(nextShotType);
				Destroy(c.gameObject);
			}

			// オプション取得時の処理
			if (layerName == "ItemOpt"){
				if (followerMng.IsFollowerMax() == false){
					FindObjectOfType<BombManager>().increaseBombNum(); //ボムの数を加算
					GameObject prefab = (GameObject)Resources.Load ("followers");
					Instantiate (prefab, transform.position, Quaternion.identity);
				}
				Destroy(c.gameObject);
			}

			// バリア取得時の処理
			if (layerName == "ItemBarrier") {
				GameObject.Find("ItemBarrier").renderer.enabled = true;
				Destroy (c.gameObject);
			}

			// スペシャル弾の取得時の処理
			if (layerName == "ItemSuperShot") {
				GameObject.Find("ItemSuperShot").renderer.enabled = true;
				superShot.superShotFlg = 1;
				Destroy (c.gameObject);
			}
		}
	}
}