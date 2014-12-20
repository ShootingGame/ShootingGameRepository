using UnityEngine;
using System.Collections;

//********************************************************
//全ての通常エネミー共通
//ボスを含め、全てのエネミーで、当クラスを拡張して使用
//*******************************************************
public class Enemy : MonoBehaviour
{
	//========== 各エネミーで個別にインスペクターで設定する項目 ==========
	// ヒットポイント
	public int hp = 1;
	
	// スコアのポイント
	//public int point;
	
	//この値によってザコ敵に設定する弾丸を決めるメソッドをかえる
	public int shotFlag = 1;

    //最後のボスフラグ
    public bool isLastBoss = false;

    //特別な敵フラグ　(中ボスなど)
    public bool isSpecialEnemy = false;

	//========== 使用する各種コンポーネント ==========
	[System.NonSerialized]
	public EnemyMove enemyMove;

	[System.NonSerialized]
	public ShotType shottype;

	[System.NonSerialized]
	public Spaceship spaceship;

	[System.NonSerialized]
	public Manager managerObj;

	//========== Public 変数 ==========
	[System.NonSerialized]
	public GameObject playerObj;

	//停止フラグ
	[System.NonSerialized]
	public bool isEnemyPause = false;

	//DestroyArea 内侵入検知フラグ
	[System.NonSerialized]
	public bool isEnterDestroyArea = false;

	//item
	public GameObject Item;

	//========== メンバ 変数 ==========
	//停止位置
	public float pauseX, pauseY, pauseZ;
	
    //敵を倒した時のポイント
    int point;
    int LAST_BOSS_POINT = 1000;
    int SPECIAL_ENEMY_POINT = 500;
    int NORMAL_ENEMY_POINT = 100;

	//初期設定
	public void setUp ()
	{
		//使用する各種コンポーネントを設定
		enemyMove = GetComponent<EnemyMove> ();
		shottype = GetComponent<ShotType> ();
		spaceship = GetComponent<Spaceship> ();
		managerObj = FindObjectOfType<Manager> ();

		//プレイヤーを設定 || Player -> null, Player(Clone) -> OK
		playerObj = GameObject.Find (managerObj.selectedPlayer.name + "(Clone)");
		//managerObj.selectedPlayer.name + "(Clone)"
		// 倒した時のポイントを設定
		if( isLastBoss != false){
			point = LAST_BOSS_POINT;
		}else if( isSpecialEnemy != false){
			point = SPECIAL_ENEMY_POINT;
		}else{
			point = NORMAL_ENEMY_POINT;
		}
	}

	//ゲーム停止チェック処理
	public void checkPause ()
	{
		//TODO ================ 停止処理
		if (managerObj.IsGamePause () != true) {
			isEnemyPause = false;
		} else {
			//ポーズになったら現在の位置情報を取得し、エネミー停止フラグを変更
			if (isEnemyPause == false) {
				isEnemyPause = true;
				pauseX = transform.position.x;
				pauseY = transform.position.y;
				pauseZ = transform.position.z;
			}
			//エネミー停止の場合は、現在の位置に移動することで停止を表現
			if (isEnemyPause == true) {
				transform.position = new Vector3(pauseX, pauseY, pauseZ);
			}
		}
		//================
	}

	//衝突を確認処理
	public void checkCollision(Collider2D c)
	{

		// レイヤー名を取得
		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		//DestroyArea 内に入ったら検知するフラグを切り替え
		if (isEnterDestroyArea != true) {
			if (layerName == "DestroyArea") {
				isEnterDestroyArea = true;
			}
		}

		// レイヤー名がBullet (Player) or Bomb 以外の時は何も行わない
		if (layerName != "Bullet(Player)" && layerName != "Bomb") {
			return;		
		} else {

			// PlayerBulletのTransformを取得
			Transform playerBulletTransform = c.transform.parent;

			if(layerName != "Bomb"){
				//Player bullet ヒットポイントを減らす
				Bullet bullet =  playerBulletTransform.GetComponent<Bullet>();
				hp = hp - bullet.power;

				//貫通弾の処理
				if(shottype.getShotType() != 3){
					Destroy(c.gameObject);
				}
			}else{
				// Bomb ヒットポイントを減らす
				hp = hp - FindObjectOfType<BombManager> ().bombPower;
			}
			
			// ヒットポイントが0以下であれば
			if(hp <= 0 )
			{
				// スコアコンポーネントを取得してポイントを追加
				FindObjectOfType<Score>().AddPoint(point);
				
				// スコアコンポーネントを取得して倒した敵の数を加算
				if(isLastBoss){
					FindObjectOfType<Score>().setDeadBossNum();
				}else if(isSpecialEnemy){
					FindObjectOfType<Score>().setDeadMiddleBossNum();
				}else{
					FindObjectOfType<Score>().setDeadEnemyNum();
				}
				
				// 爆発
				spaceship.Explosion ();
				
				// エネミーの削除
				Destroy (gameObject);

				//ItemGet
				if(Item != null){
					Instantiate (Item , transform.position, Quaternion.identity);
				}

				//最後のボスの場合は、倒したらクリアGUIを表示
				if(isLastBoss != false){

                    //スコアGUIの呼び出し
                    FindObjectOfType<Score>().setupScoreGui();

                    //ゲームを停止
                    FindObjectOfType<Manager> ().isPause = true; //ゲーム停止する場合
                }
				
			}else{
				
				spaceship.GetAnimator().SetTrigger("Damage");
				
			}		
		}
	}
}