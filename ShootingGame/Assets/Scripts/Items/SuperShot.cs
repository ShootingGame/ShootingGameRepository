using UnityEngine;
using System.Collections;

public class SuperShot : MonoBehaviour
{
	public int superShotFlg = 0;

	// コンポーネント
	Spaceship spaceship;
	Life playerlife;
	Manager managerObj;

	IEnumerator Start (){
		// コンポーネントを取得
		spaceship = GetComponent<Spaceship> ();
		managerObj = FindObjectOfType<Manager> ();

		while(true){
			if (superShotFlg == 1 && managerObj.IsGamePause () != true) {
				SuperAttack();
			}
			yield return new WaitForEndOfFrame ();
		}

	}
	
	void SuperAttack(){
		// 弾をプレイヤーと同じ位置/角度で作成
		for (int i = 0; i < transform.childCount; i++) {
			Transform shotPosition = transform.GetChild (i);
			//shotPositionから弾を発射する
			spaceship.SuperShot (shotPosition);
		}
		// ショット音を鳴らす
		audio.Play ();
	}

}