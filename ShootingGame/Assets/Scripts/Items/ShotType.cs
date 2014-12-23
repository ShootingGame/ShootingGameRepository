using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
ショットの種類を shotType で制御
武器の弾道についてはBullet.csにて管理
全てのPlayerBulletと配下のBulletにこのスクリプトをアタッチする
武器が変更される処理が呼び出されるのはPlayer.cs
 */
public class ShotType: MonoBehaviour {
	
	//武器ショットの種類
	private static int shotType = 1;

	//初期値は通常弾
	private static float playerShotDelay = 0.02f;

	//デフォルトの弾を設定
	public void setDefaultShot(){
		shotType = 1;
		playerShotDelay = 0.02f;
	}

	// 弾の種類を変更処理
	public void setShotType(int argType){
		shotType = argType;
	}

	// 弾の速さを設定 shotTypeが0の場合、引数のスピードを設定
	public void setPlayerShotDelay(int argShotType, float argSpeed = 0.02f){

		switch (argShotType) {
		case 0:
			playerShotDelay = argSpeed;
			break;
		case 1:
			playerShotDelay = 0.01f;
			break;
		case 2:
			playerShotDelay = 0.08f;
			break;
		case 3:
			playerShotDelay = 0.6f;
			break;
		case 4:
			playerShotDelay = 0.3f;
			break;
		case 5:
			playerShotDelay = 0.5f;
			break;
		default:
			playerShotDelay = 0.02f;
			break;
		}
	}

	// 弾の速さを取得
	public float getPlayerShotDelay(){
		return playerShotDelay;
	}

	// 弾の種類を取得
	public int getShotType(){
		return shotType;
	}
	
}
