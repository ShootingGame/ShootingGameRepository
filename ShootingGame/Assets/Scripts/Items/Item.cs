using UnityEngine;
using System.Collections;

//アイテム移動用クラス
//切り替え処理はPlayer側でやる

public class Item : MonoBehaviour
{
	
	// Spaceshipコンポーネント
	Spaceship spaceship;

	IEnumerator Start ()

	{
		// Spaceshipコンポーネントを取得
		spaceship = GetComponent<Spaceship> ();

		// ローカル座標のY軸のマイナス方向に移動する
		itemMove (transform.up * -1);
		
		// canShotがfalseの場合、ここでコルーチンを終了させる
		if (spaceship.canShot == false) {
			yield break;
		}

	}
	
	// Itemの移動
	public void itemMove (Vector2 direction)
	{
		rigidbody2D.velocity = direction * spaceship.speed;

	}
	
}




