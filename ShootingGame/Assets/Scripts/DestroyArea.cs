using UnityEngine;

public class DestroyArea : MonoBehaviour
{

	ShotType shotType;
	
	void Start(){

		//不要だったっぽいのでコメントアウト
		//ShotTypeコンポネント取得
		//shotType = GetComponent<ShotType> ();

	}


	void OnTriggerExit2D (Collider2D c)
	{
		//ここに中心点がふれたオブジェクトは無条件で削除される
		/*
		レーザーなどは画面の奥にとどなければ不自然なので
		shotTypeが3の時はこれを実行しない
		触れても消えないようにする
		※shotType変数はstaticなのでアタッチして呼べばよい
		*/
		//不要だったっぽいのでコメントアウト
		//if(shotType.getShotType() != 3){
		Destroy (c.gameObject);
		//}
	}
}