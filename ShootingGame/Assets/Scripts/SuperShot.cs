using UnityEngine;
using System.Collections;

public class SuperShot : MonoBehaviour
{
	
	// Spaceshipコンポーネント
	Spaceship spaceship;
	
	Life playerlife;
	
	public int superShotFlg = 0;
	
	Manager managerObj;

	void Update ()
	{
		// コンポーネントを取得
		spaceship = GetComponent<Spaceship> ();
		managerObj = FindObjectOfType<Manager> ();

			SuperAttack();

	}

	
	void SuperAttack(){


		if (superShotFlg == 1) {
						if (managerObj.IsGamePause () != true) {

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
	}

}