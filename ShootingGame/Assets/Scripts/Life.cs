using UnityEngine;

public class Life : MonoBehaviour
{
	// 表示するGUIText
	//public GUIText lifeGUIText;

		// ヒットポイント
		// セッターで何もしなければこの数字が設定される
		/*
		プレイヤーのライフの変数はstaticにしてある
		LifeクラスとGuiクラスで共通化させるため
		*/
		//HPの初期値はScore.csにてセットしている（GUI表示のため）
	private static int playerHp = 1;

	public int getPlayerHp(){
		return playerHp;
	}

	public void addPlayerHp(){
		playerHp++;
		print ("PLAYER REPARE !!  " + playerHp);
	}

	public void damagePlayerHp(){

		if(playerHp <= 0){
			playerHp = 0;
		}
		else{
		playerHp = playerHp -1;
		print ("PLAYER DAMAGE !!  " + playerHp);
		}
	}

	//初期値
	public void setPlayerHp(int num){
		playerHp = num;
	}
}