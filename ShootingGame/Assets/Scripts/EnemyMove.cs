using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

	// 移動スピード
	public float enemySpeed = 0.5f;

	//直進する
	public void EnemyMoveStraight (Vector2 direction)
	{
		rigidbody2D.velocity = direction * enemySpeed;
	}

	//引数に指定したオブジェクトを追尾する
	public void EnemyMoveHoming(GameObject target)
	{
		rigidbody2D.velocity = (target.transform.position - transform.position).normalized * enemySpeed;
	}

	//引数に指定したオブジェクトに合わせて向きを変える
	public void EnemyLookAt2D(GameObject target)
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

	//回転する
	public void EnemyRoring_1(){
		transform.Rotate(new Vector3(0, 0,90) * Time.deltaTime);

	}

	//高速回転する
	public void EnemyRoring_2(){
		transform.Rotate(new Vector3(0, 0, 220) * Time.deltaTime);
	}

}
