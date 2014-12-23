using UnityEngine;

public class Life : MonoBehaviour
{
	// HPの初期値はScore.csにてセットしている（GUI表示のため）

	private static int playerHp = 1;

	public int getPlayerHp(){
		return playerHp;
	}

	public void addPlayerHp(){
		playerHp++;
		//print ("PLAYER REPARE !!  " + playerHp);
	}

	public void damagePlayerHp(){

		if(playerHp <= 0){
			playerHp = 0;
		}
		else{
			playerHp = playerHp -1;
		}
	}

	//初期値
	public void setPlayerHp(int num){
		playerHp = num;
	}
}