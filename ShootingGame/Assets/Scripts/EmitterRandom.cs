using UnityEngine;
using System.Collections;

public class EmitterRandom : MonoBehaviour {

	//enemyプレハブを格納する
	public GameObject enemy_prefab;

	//Animetorフラグ
	public bool create_clip = false;

	//敵機出現までの待機時間
	public float enemy_interval;

	//Animation再生回数
	private int animation_num = 0;

	//enemy_copyの速度
	public float enemy_speed;

	//EnemyMoveコンポーネント
	EnemyMove enemy_move;

	//Spaceshipコンポーネント
	Spaceship spaceship_clip;

	public float  EmitterRan_Pos_x = 0f;

	public float EmitterRan_Pos_y = 0f;


	IEnumerator RandomEnemy (){

		//
		for (int n = 0; n < 4; n++) {

//			//Animatorフラグをセット
//			create_clip = true;

			//Enemyを生成(オリジナルのposition位置)
			GameObject enemy_copy = this.Instantiate (enemy_prefab, new Vector3(EmitterRan_Pos_x, EmitterRan_Pos_y, 0f), Quaternion.identity) as GameObject;
			//Enemyの生成(EmitterRandom位置)
//			Instantiate(enemy_prefab, transform.position, Quaternion.identity);
//			Instantiate(enemy_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

//			//EnemyMoveコンポーネントを取得
//			enemy_move = Enemy.GetComponent<EnemyMove>();
			enemy_move = enemy_copy.GetComponent<EnemyMove>();
//
//			//Spaceshipコンポーネントを取得
//			spaceship_clip = Enemy.GetComponent<Spaceship>();
			spaceship_clip = enemy_copy.GetComponent<Spaceship>();
//
//			//Createアニメーションを呼び出す
			spaceship_clip.GetComponent<Animator>().SetBool("Create", create_clip);
//
//			//Enemy_プレファブのtransform.positionから修正
//			//enemy_copy.transform.Translate (3.5f, -6.4f, 0.1f);
//
//			//移動スピード
			enemy_move.enemySpeed = enemy_speed;

			//コルーチンで時間指定
			yield return new WaitForSeconds (enemy_interval);
		}
	}


	// Use this for initialization
	void Start () {
		StartCoroutine ("RandomEnemy");
	}


	// Update is called once per frame
	void Update () {

	}

}
