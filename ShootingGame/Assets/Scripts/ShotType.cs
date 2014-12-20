using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
このクラスではショットの種類のみをstatic変数shotTypeで制御する
武器の弾道についてはBullet.csにて管理する
全てのPlayerBulletと配下のBulletにこのスクリプトをアタッチする

武器が変更される処理が呼び出されるのはPlayer.cs
*/
public class ShotType: MonoBehaviour {
	
	//武器の切り替え用変数
	//種類は1～5を予定
	private static int shotType = 1;

	//初期値は通常弾を想定している
	private static float playerShotDelay = 0.02f;

	public int getShotType(){
		return shotType;
	}
	
	public void setShotType_1(){
		shotType = 1;
	}
	
	public void setShotType_2(){
		shotType = 2;
	}
	
	public void setShotType_3(){
		shotType = 3;
	}
	
	public void setShotType_4(){
		shotType = 4;
	}

	public void setShotType_5(){
		shotType = 5;
	}
	
	
	
	public float getPlayerShotDelay(){
		return playerShotDelay;
	}
	
	public void setPlayerShotDelay_1(){
		playerShotDelay = 0.01f;
	}
	
	public void setPlayerShotDelay_2(){
		playerShotDelay = 0.08f;
	}
	
	public void setPlayerShotDelay_3(){
		playerShotDelay = 0.6f;
	}
	
	public void setPlayerShotDelay_4(){
		playerShotDelay = 0.3f;
	}

	public void setPlayerShotDelay_5(){
		playerShotDelay = 0.5f;
	}
	
}
