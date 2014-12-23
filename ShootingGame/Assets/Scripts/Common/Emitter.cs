using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour
{
	// Waveプレハブを格納する
	public GameObject[] waves;
	
	// 現在のWave
	public int currentWave;
	
	// Managerコンポーネント
	//private Manager manager;

	private GameObject g;
	
/*
	void Update(){
		//リトライを押されたことがManagerから伝わったら、画面上の敵を消す
		if (manager.retry == true) {
			// Waveの削除
			Destroy (g);
			manager.retry = false;
		}
	}
*/

	IEnumerator Start ()
	{
		// Waveが存在しなければコルーチンを終了する
		if (waves.Length == 0) {
			yield break;
		}
		
		// Managerコンポーネントをシーン内から探して取得する
		//manager = FindObjectOfType<Manager>();

		while (true) {
			/*
			// タイトル表示中は待機(タイトル表示をなくしたのでとりあえずコメントアウト)
			while(manager.IsPlaying() == false) {
				yield return new WaitForEndOfFrame ();
			}
			*/

			// Waveを作成する
			g = (GameObject)Instantiate (waves [currentWave], transform.position, Quaternion.identity);
			
			// WaveをEmitterの子要素にする
			g.transform.parent = transform;


			// Waveの子要素のEnemyが全て削除されるまで待機する
			//Waveが階層構造になってると待機し続けてしまう
			//Waveに直接Enemyが入ってないと先に進まなくなる
			//childCountが0になるまでループし続ける
				while (g != null && g.transform.childCount != 0) {
				yield return new WaitForEndOfFrame ();

				//yield return new WaitForSeconds (1);
			}

			
			// Waveの削除
			// 消えないことがある･･･
			Destroy (g);
			
			// 格納されているWaveを全て実行したら
			if (waves.Length <= ++currentWave) {

				//最後は空のエネミーを表示する　|| エネミーをループする場合　currentWaveを0にする（最初から -> ループ）
				currentWave = waves.Length - 1;
				
				//TODO ボスを倒した場合の処理
				/*
				Manager.Stage++;
				Application.LoadLevel("Stage02");
				*/

				//TODO スコアGUIの呼び出し
				/*
				GameObject prefab = (GameObject)Resources.Load ("scoreGUI");
				Instantiate (prefab, transform.position, Quaternion.identity);
				*/
			}
		}
	}
}