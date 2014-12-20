using UnityEngine;

/*
このクラスではショットの弾道のみを制御する
武器の変更についてはShotType.csにて管理する
*/

public class Bullet : MonoBehaviour
{
	// 弾の移動スピード
	public int speed = 10;

	// ゲームオブジェクト生成から削除するまでの時間
	public float lifeTime = 5;

	// 攻撃力
	public int power = 1;

	ShotType shotType;

	//ミサイル爆風発生用コンポネント
	MissileBomb missileBomb;

	//敵を探して追尾させる処理の準備
	private GameObject enemy_obj;

	void Start ()
	{
		/*
		static変数、shotTypeの値によって弾道を変更する
		*/
		shotType = GetComponent<ShotType> ();
		/*
		爆風発生用コンポネント
		 */
		missileBomb = GetComponent<MissileBomb> ();

		if(shotType != null){

			//ここでは弾丸の軌道を管理する
			//基本は前方へ射出する
			if(shotType.getShotType () == 1){
				rigidbody2D.velocity = transform.up.normalized * speed;
				Destroy (gameObject, lifeTime);
			}else if(shotType.getShotType () == 2){
				rigidbody2D.velocity = transform.up.normalized * speed;
				Destroy (gameObject, lifeTime);
			}else if(shotType.getShotType () == 3){
				rigidbody2D.velocity = transform.up.normalized * speed;
				Destroy (gameObject, lifeTime);
			}else if(shotType.getShotType () == 4){			

				//serchTagメソッドで索敵する際の注意!!
				//タグ名をつけないとミサイルが参照先を失ってその場に停滞してしまう

					enemy_obj = serchTag(gameObject,"Enemy");
				if(enemy_obj != null){
					Missile_Homing (enemy_obj);
					LookAt2D(enemy_obj);
					Destroy (gameObject, lifeTime);
				}else{
					//敵が見つからない場合はまっすぐ飛ぶ
					//print ("missile none target !!");
					rigidbody2D.velocity = transform.up.normalized * speed;
					Destroy (gameObject, lifeTime);
				}
				
				//誘導がうまくできなければまっすぐ飛ぶ
				rigidbody2D.velocity = transform.up.normalized * speed;
				// lifeTime秒後に削除
				Destroy (gameObject, lifeTime);
			}
			//ここまでミサイルの処理
		}else{
		//何かしらの問題でshotType取得に失敗した場合まっすぐ飛ぶ
		//shotTypeに依存しない処理
		//print ("Bullet.cs... shotType NONE!");
		rigidbody2D.velocity = transform.up.normalized * speed;
		// lifeTime秒後に削除
		Destroy (gameObject, lifeTime);
		}
	}

	/*
	指定されたタグの中で最も近いものを取得
	nowObj......ミサイル(弾丸)オブジェクト
	tagName.....敵のタグ名（レイヤーではない）
	タグがNullだとエラーになるのでホーミングする相手には必ずタグをつける
	タグは基本的に「Enemy」以外使用しない
	新しい敵を作ったら必ずインスペクターウインドウで付加しておくこと
	*/
	GameObject serchTag(GameObject nowObj,string tagName){
		float tmpDis = 0;           //距離用一時変数
		float nearDis = 0;          //最も近いオブジェクトの距離
		//string nearObjName = "";    //オブジェクト名称
		GameObject targetObj = null; //オブジェクト
		
		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(tagName)){
			//自身と取得したオブジェクトの距離を取得
			tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);
			
			//オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
			//一時変数に距離を格納
			if (nearDis == 0 || nearDis > tmpDis){
				nearDis = tmpDis;
				//nearObjName = obs.name;
				targetObj = obs;
			}	
		}
		//最も近かったオブジェクトを返す
		return targetObj;
	}

	//弾丸がヒットしたときのエフェクトをExplosion.cs発動する
	//現在は一種類のみ
	void OnTriggerEnter2D (Collider2D c)
	{
		// 弾丸がヒットした相手のtag名を取得
		string tagName = c.gameObject.tag;
		//print ("Hit !!! TagName is ... " + tagName);

		if(tagName == "Enemy"){
			//print ("Hit !!! TagName is Enemy? ===> " + tagName);

			//爆風発生
			if(missileBomb != null){
				//shotTypeによって呼び出す爆風を変化させる予定
				//print ("EXPLODE!! Now Shot Type is ..." + shotType.getShotType ());
				missileBomb.ExplosionType1();
			}else{
				//print ("Because....Null!!!!");
			}

		//tagが『Enemy』以外の時は何もしない
		}if (tagName != "Enemy") {
			//print ("None Other tag name !!");
			return;
		}
	}

	//敵を探して追尾
	public void Missile_Homing (GameObject target)
	{
		//target.transform...敵
		//transform..........射出された弾丸
		rigidbody2D.velocity = (target.transform.position - transform.position).normalized * shotType.getPlayerShotDelay();
	}

	//敵を探して向きを変える
	void LookAt2D(GameObject target)
	{
		// 指定オブジェクトと回転さすオブジェクトの位置の差分(ベクトル)
		Vector3 pos = target.transform.position - transform.position;
		// ベクトルのX,Yを使い回転角を求める
		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		Quaternion rotation = new Quaternion();
		// 回転角は右方向が0度なので-90
		rotation.eulerAngles = new Vector3(0, 0, angle - 90);
		// 回転
		transform.rotation = rotation;
	}

}