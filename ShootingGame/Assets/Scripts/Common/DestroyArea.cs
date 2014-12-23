using UnityEngine;

public class DestroyArea : MonoBehaviour
{
	void OnTriggerExit2D (Collider2D c)
	{
		//レイヤー名を取得
		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		if(c.gameObject.CompareTag("Caution")){
			// タグが Caution の場合はオブジェクト削除
			Destroy (c.gameObject);
		} else if (layerName == "Bullet(Enemy)" || layerName == "Bullet(Player)") {
			//プレイヤーとエネミーの弾はプーリング
			ObjectPool.instance.shootingGamePool(c.gameObject);
		} else {
			// それ以外は削除
			Destroy (c.gameObject);
		}
	}
}