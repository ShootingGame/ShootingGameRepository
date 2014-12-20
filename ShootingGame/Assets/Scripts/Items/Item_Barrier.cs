using UnityEngine;
using System.Collections;

public class Item_Barrier : MonoBehaviour {

	public int hp = 3;
	public GameObject Barrier;

	Spaceship spaceship;

	// ゲーム始めはバリアはない状態にしておく
	void Start () {
		//Barrier.SetActive(false);
		GameObject.Find("ItemBarrier").renderer.enabled = false;
	}
	
	
	void Update () {
	
	}

	//３回当たるとなくなる処理
	void OnTriggerEnter2D (Collider2D c)
	{
		//TODO 仮実装
		if (GameObject.Find ("ItemBarrier").renderer.enabled != false) {
			// レイヤー名を取得
			string layerName = LayerMask.LayerToName (c.gameObject.layer);
			
			// レイヤー名がBullet (Player)の時は何も行わない
			if (layerName == "Bullet(Player)") return;
			if (layerName == "ItemBarrier") hp = 3;
			if (layerName == "Item") return;
			
			//敵またはボスに当たった場合はオブジェクトを削除
			if (layerName == "Bullet(Enemy)") {
				hp--;
				Destroy (c.gameObject);
				//spaceship.GetAnimator().SetTrigger("Damage");
			}
			
			// ヒットポイントが0以下であればバリアを非表示
			if(hp <= 0 ){
				GameObject.Find("ItemBarrier").renderer.enabled = false;
			}		
		}
	}
}
