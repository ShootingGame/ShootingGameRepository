using UnityEngine;
using System.Collections;

//********************************************************
//このクラスではショットの弾道のみを制御する
//武器の変更についてはShotType.csにて管理する
//*******************************************************

public class EnemyBullet : MonoBehaviour
{
	//========== 各エネミーで個別にインスペクターで設定する項目 ==========
	// 弾の移動スピード
	public int speed = 10;

	// ゲームオブジェクト生成から削除するまでの時間
	public float lifeTime = 5;

	// 攻撃力
	public int power = 1;

	//========== 使用する各種コンポーネント ==========
	Manager managerObj;

	//========== メンバ変数 ==========
	//敵を探して追尾させるため
	GameObject player_obj;

	//停止フラグ
	bool isEnemyPause = false;
	
	//停止位置
	float pauseX, pauseY, pauseZ;

	bool isEnterDestroyArea = false;

	void Update () {

		//DestroyArea 外の場合は削除
		if (isEnterDestroyArea == false) {
			Destroy (gameObject);
		}

		//TODO ================ 停止処理
		if (managerObj.IsGamePause () != true) {
			isEnemyPause = false;
		} else {
			//敵を止める場合
			if (isEnemyPause == false) {
				isEnemyPause = true;
				pauseX = transform.position.x;
				pauseY = transform.position.y;
				pauseZ = transform.position.z;
			}
			
			//エネミーが止まっている場合
			if (isEnemyPause == true) {
				transform.position = new Vector3(pauseX, pauseY, pauseZ);
			}
		}
		//TODO ================

	}

	IEnumerator Start ()
	{

		managerObj = FindObjectOfType<Manager> ();

		while (true) {

			if(isEnemyPause != true){
				//まっすぐ飛ぶ
				rigidbody2D.velocity = transform.up.normalized * speed;
				// lifeTime秒後に削除
				//Destroy (gameObject, lifeTime);
			}else{
				//Debug.Log("bullet stop");
				transform.position = new Vector3(pauseX, pauseY, pauseZ);
			}

			// shotDelay秒待つ
			yield return new WaitForSeconds (1f);
		}
	}

	//敵を探して向きを変える
	void LookAt2D(GameObject target)
	{
		// 指定オブジェクトと回転さすオブジェクトの位置の差分(ベクトル)
		Vector3 pos = target.transform.position - transform.position;
		// ベクトルのX,Yを使い回転角を求める
		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		Quaternion rotation = new Quaternion();
		// 回転角は右方向が0度なので-90と...言いたいところだけど敵は
		//さかさまを向いているので+90度に変更...
		rotation.eulerAngles = new Vector3(0, 0, angle - 90);
		//rotation.eulerAngles = new Vector3(0, 0, angle + 90);
		// 回転
		transform.rotation = rotation;
	}

	//敵を探して追尾
	public void Missile_Homing (GameObject target)
	{
		//target.transform...敵
		//transform..........射出された弾丸
		rigidbody2D.velocity = (target.transform.position - transform.position).normalized * 2.0f;
	}

	/*
	指定されたタグの中で最も近いものを取得
	nowObj......ミサイル(弾丸)オブジェクト
	tagName.....敵のタグ名（レイヤーではない）
	タグがNullだとエラーになるのでホーミングする相手には必ずタグをつける
	タグは基本的に「Player」以外使用しない
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
	
	void OnTriggerEnter2D (Collider2D c)
	{
		// レイヤー名を取得
		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		//DestroyArea 内に入ったら検知するフラグを切り替え
		if (isEnterDestroyArea != true) {
			if (layerName == "DestroyArea") {
				isEnterDestroyArea = true;
			}
		}
	}
}