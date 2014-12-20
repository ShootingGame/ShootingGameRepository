using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Spaceship : MonoBehaviour
{
	// 移動スピード
	public float speed;
	
	// 通常弾を撃つ間隔
	public float shotDelay = 0.5f;
	
	public float getShotDelay(){
		return shotDelay;
	}

	//通常弾のPrefab
	public GameObject bullet;
	public GameObject bullet2;
	public GameObject bullet3;
	public GameObject bullet4;
	public GameObject super_bullet;

	//敵通常弾のPrefab
	public GameObject enemyBullet;

	//自機のオブジェクト"Player(Clone)
	private GameObject player_obj;
	
	// 弾を撃つかどうか
	public bool canShot;

	
	// 爆発のPrefab
	public GameObject explosion;
	
	// アニメーターコンポーネント
	private Animator animator;
	
	void Start ()
	{
		// アニメーターコンポーネントを取得
		animator = GetComponent<Animator> ();
	}
	
	// 爆発の作成
	public void Explosion ()
	{
		Instantiate (explosion, transform.position, transform.rotation);
	}
	
	// 弾の作成
	public void Shot (Transform origin)
	{
		Instantiate (bullet, origin.position, origin.rotation);
	}

	public void Shot2 (Transform origin)
	{
		Instantiate (bullet2, origin.position, origin.rotation);
	}

	public void Shot3 (Transform origin)
	{
		Instantiate (bullet3, origin.position, origin.rotation);
	}

	public void Shot4 (Transform origin)
	{
		Instantiate (bullet4, origin.position, origin.rotation);
	}

	public void SuperShot (Transform origin)
	{
		Instantiate (super_bullet, origin.position, origin.rotation);
	}

	//敵の弾の作成
	public void EnemyShot(Transform origin)
	{
		Instantiate (enemyBullet, origin.position, origin.rotation);
	}
	
	// アニメーターコンポーネントの取得
	public Animator GetAnimator()
	{
		return animator;
	}
	
}