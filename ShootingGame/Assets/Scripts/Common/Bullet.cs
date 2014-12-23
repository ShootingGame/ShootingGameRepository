using UnityEngine;

/*
このクラスではショットの弾道のみを制御する
武器の変更についてはShotType.csにて管理する
*/

public class Bullet : MonoBehaviour
{
	//========== 個別にインスペクターで設定する項目 ==========
	// 弾の移動スピード
	public int speed = 10;

	// ゲームオブジェクト生成から削除するまでの時間
	public float lifeTime = 5;

	// 攻撃力
	public int power = 1;

	//========== 使用する各種コンポーネント ==========
	ShotType shotType;
	MissileBomb missileBomb;  //ミサイル爆風発生用コンポネント
	Manager managerObj;  //ミサイル爆風発生用コンポネント

	//========== メンバ変数 ==========
	//敵を探して追尾させる処理の準備
	private GameObject enemy_obj;

	// 弾が表示された時に呼び出される
	void OnEnable ()
	{

		shotType = GetComponent<ShotType> ();
		missileBomb = GetComponent<MissileBomb> (); //爆風発生用コンポネント
		managerObj = FindObjectOfType<Manager> ();

		if(shotType != null){

			//shotTypeの値によって弾道を変更する ここでは弾丸の軌道を管理する 基本は前方へ射出する
			int selectedShotType = shotType.getShotType ();
			switch (selectedShotType)
			{
			case 1:
			case 2:
			case 3:
				rigidbody2D.velocity = transform.up.normalized * speed;
				break;
			case 4:
				//ミサイルの処理
				//serchTagメソッドで索敵する際の注意!! タグ名をつけないとミサイルが参照先を失ってその場に停滞してしまう
				enemy_obj = serchTag(gameObject,"Enemy");
				if(enemy_obj != null){
					Missile_Homing (enemy_obj);
					LookAt2D(enemy_obj);
				}else{
					//敵が見つからない場合はまっすぐ飛ぶ
					rigidbody2D.velocity = transform.up.normalized * speed;
				}
				
				//誘導がうまくできなければまっすぐ飛ぶ
				rigidbody2D.velocity = transform.up.normalized * speed;
				break;
			default:
				rigidbody2D.velocity = transform.up.normalized * speed;
				break;
			}

		}else{
			//shotTypeに依存しない処理 何かしらの問題でshotType取得に失敗した場合まっすぐ飛ぶ
			rigidbody2D.velocity = transform.up.normalized * speed;
		}
	}

	/*
	void Update () {
	}
	*/

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

		if(tagName == "Enemy"){
			//爆風発生
			if(missileBomb != null){
				//shotTypeによって呼び出す爆風を変化させる予定
				missileBomb.ExplosionType1();
			}else{
				//print ("Because....Null!!!!");
			}
		}else{
			//tagが『Enemy』以外の時は何もしない
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