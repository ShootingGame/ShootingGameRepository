using UnityEngine;

public class MissileBomb : MonoBehaviour
{
	
	//弾丸がヒットしたときのエフェクトを発動する
	//このMissileスクリプトをPlayerBulletのプレファブにアタッチして
	//爆風アニメーションの種類を選択する
	//エフェクトを出したくない時はNoneにすればいい
	//呼び出してるアニメーションはExplosionプレハブだが経路が違っている
	//Explosion.csとは無関係なので注意
	//アタッチするものを間違えないこと
	
	// 爆発のPrefab
	public GameObject explosion1;
	
	// アニメーターコンポーネント
	private Animator animator;
	
	void Start ()
	{
		// アニメーターコンポーネントを取得
		animator = GetComponent<Animator> ();
	}
	
	// 爆発の作成
	public void ExplosionType1()
	{
		Instantiate (explosion1, transform.position, transform.rotation);
	}
	
	// アニメーターコンポーネントの取得
	public Animator GetAnimator()
	{
		return animator;
	}
	
	
}