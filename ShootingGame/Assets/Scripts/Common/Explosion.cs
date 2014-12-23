using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
		//アニメーション終了時に実行
	void OnAnimationFinish ()
	{
		// 削除。実際には非アクティブにする
		//Destroy(gameObject);
		ObjectPool.instance.ReleaseGameObject (gameObject);
	}

}
