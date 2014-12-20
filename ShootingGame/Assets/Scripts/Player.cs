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
	ShotType shotType;
	Spaceship spaceship;
	SuperShot superShot;
	AudioManager audioObj;

	//========== メンバ変数 ==========
	//None

	IEnumerator Start ()
	{

		// コンポーネントを取得
		followerMng = FindObjectOfType<FollowerManager> ();
		playerlife = GetComponent<Life> ();
		managerObj = FindObjectOfType<Manager> ();
		shotType = GetComponent<ShotType> ();
		spaceship = GetComponent<Spaceship> ();
		superShot = GetComponent<SuperShot> ();
		audioObj = FindObjectOfType<AudioManager> ();

		while(true){
			int selectedShotType = shotType.getShotType();
			Attack(selectedShotType);
			// 弾をプレイヤーと同じ位置/角度で作成
			yield return new WaitForSeconds (shotType.getPlayerShotDelay());	
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
			// スペースを押したかを判定
			if (Input.GetKey ("space")) {

				switch (argShotType)
				{
				case 1:
					spaceship.Shot (transform);
					break;
				case 2:
					spaceship.Shot2 (transform);
					break;
				case 3:
					spaceship.Shot3 (transform);
					break;
				case 4:
					spaceship.Shot4 (transform);
					break;
				default:
					spaceship.Shot (transform);
					break;
				}

				// TODO ショット音を鳴らす || ショットによって振り分けたい
				audioObj.playShotSound (1);
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
			// 以下、コライダのレイヤー名で判定する
			string layerName = LayerMask.LayerToName(c.gameObject.layer);
			
			// レイヤー名がBullet(Enemy)の時は弾を削除
			if( layerName == "Bullet(Enemy)")
			{
				// 弾の削除
				Destroy(c.gameObject);
			}
			
			// レイヤー名がBullet(Enemy)またはEnemy,Boss1の場合は damage
			if( layerName == "Bullet(Enemy)" || layerName == "Enemy" || layerName == "Boss1")
			{
				playerlife.damagePlayerHp();
				Debug.Log ("Damage(Player.cs) == >" + playerlife.getPlayerHp());
				
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
			
			//TODO アイテム獲得時の分岐
			if(layerName == "ItemN"){
				Debug.Log ("ItemN GET");
				shotType.setShotType_1();
				shotType.setPlayerShotDelay_1();
				Destroy(c.gameObject);

				FindObjectOfType<ShotTypeChanger> ().changeShot(1);

			}else if(layerName == "ItemE"){
				Debug.Log ("ItemE GET");
				shotType.setShotType_2();
				shotType.setPlayerShotDelay_2();
				Destroy(c.gameObject);

				FindObjectOfType<ShotTypeChanger> ().changeShot(2);
				
			}else if(layerName == "ItemL"){
				Debug.Log ("ItemL GET");
				shotType.setShotType_3();
				shotType.setPlayerShotDelay_3();
				Destroy(c.gameObject);

				FindObjectOfType<ShotTypeChanger> ().changeShot(3);
			}else if(layerName == "ItemM"){
				Debug.Log ("ItemM GET");
				shotType.setShotType_4();
				shotType.setPlayerShotDelay_4();
				Destroy(c.gameObject);

				FindObjectOfType<ShotTypeChanger> ().changeShot(4);
			}else if(layerName == "ItemOpt"){
				//TODO オプションを生成処理
				if(followerMng.IsFollowerMax() == false){
					FindObjectOfType<BombManager>().increaseBombNum(); //ボムの数を加算
					GameObject prefab = (GameObject)Resources.Load ("followers");
					Instantiate (prefab, transform.position, Quaternion.identity);
				}
				Destroy(c.gameObject);
			}else if (layerName == "ItemBarrier") {
				GameObject.Find("ItemBarrier").renderer.enabled = true;
				Destroy (c.gameObject);
				
			}else if (layerName == "ItemSuperShot") {
				GameObject.Find("ItemSuperShot").renderer.enabled = true;
				superShot.superShotFlg = 1;
				Destroy (c.gameObject);
			}
		}
	}
}