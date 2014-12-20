using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
		//アニメーション終了時に実行
	void OnAnimationFinish ()
	{
		Destroy(gameObject);
	}

}
