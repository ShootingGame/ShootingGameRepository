using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Followers : MonoBehaviour {

	//========== 個別にインスペクターで設定する項目 ==========
	public GameObject player;

	//Follower settings
	public FollowerManager flwMngObj;
	public GameObject follower;
	public int followersNum;
	public float controlGainP = 1f;
	public float controlGainD = 1f;
	public float followerDistance = 1f;
	
	//TODO　オプションからの弾の生成
	public GameObject bullet, bullet2, bullet3, bullet4;

	//========== 使用する各種コンポーネント ==========
	Manager managerObj;
	ShotType shotType;

	//========== メンバ変数 ==========
	private List<GameObject> _followers = new List<GameObject>(); // list of instance
	private float followerVel = 5f;


	// Use this for initialization
	void Start () {

		shotType = GetComponent<ShotType> ();

		//managerObj = GetComponent<Manager> ();
		managerObj = FindObjectOfType<Manager> ();

		//すでに生成されているオプションがある場合は削除
		if (FindObjectOfType<FollowerManager> ().getFollowersObj () != null) {
			FindObjectOfType<FollowerManager> ().destroyOldFollowers ();
		}

		//TODO プレーヤーを取得
		if (managerObj.selectedPlayer != null) {
			player = GameObject.Find (managerObj.selectedPlayer.name + "(Clone)");		
		} else {
			player = GameObject.Find ("Player(Clone)");
		}

		//オプション用のマネージャー設定
		flwMngObj = FindObjectOfType<FollowerManager> ();

		//数に合わせたオプションの作成開始
		flwMngObj.startFolloers ();
		followersNum = flwMngObj.getFollowersNum ();
		if (followersNum != flwMngObj.maxFollowers) {
			followersNum += 1;
		}
		flwMngObj.setFollowersNum (followersNum);
		buildInstance (followersNum);
		//Debug.Log (followersNum + "個のオプションを生成します。");

		// フォロワーのスピードを設定
		//followerVel = GetComponent<Spaceship>().speed; //public static
		StartCoroutine(Coroutine());
	}

	IEnumerator Coroutine()
	{
		while(true){

			for(int i=0; i<followersNum; i++) {
				if (_followers[i] != null && player != null) {
					if (managerObj.IsGamePause () != true) {
						if (managerObj.isTouch) {
							if (_followers[i] != null && player != null) {
								switch (shotType.getShotType())
								{
								case 1:
									ObjectPool.instance.GetGameObject (bullet, _followers[i].transform.position, _followers[i].transform.rotation);
									break;
								case 2:
									ObjectPool.instance.GetGameObject (bullet2, _followers[i].transform.position, _followers[i].transform.rotation);
									break;
								case 3:
									ObjectPool.instance.GetGameObject (bullet3, _followers[i].transform.position, _followers[i].transform.rotation);
									break;
								case 4:
									ObjectPool.instance.GetGameObject (bullet4, _followers[i].transform.position, _followers[i].transform.rotation);
									break;
								default:
									ObjectPool.instance.GetGameObject (bullet, _followers[i].transform.position, _followers[i].transform.rotation);
									break;
								}
							}
						}
					}
				}
			}
			yield return new WaitForSeconds (shotType.getPlayerShotDelay());	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//TODO オプション削除 (暫定 : キーが押された場合に現在のオプションを削除する)
		if (Input.GetKeyDown (KeyCode.Y)) {
			destroyMyself();
		}
		//オプション初期化処理
		if (flwMngObj.IsOptionDestroy()) {
			for(int i=0; i<_followers.Count; i++) {
				Destroy(_followers[i]);
			}
			flwMngObj.startFolloers();
		}
		
		//各オプションの移動
		for (int i=0; i<followersNum; i++) {
			if (_followers [i] != null && player != null) {
					//Velocity Follower vector2 follower[i] > (player or follower[i-1])
					Vector2 dir;
	
					if (i == 0) {
							dir = -1 * _followers [i].transform.position + player.transform.position;
					} else {
							dir = -1 * _followers [i].transform.position + _followers [i - 1].transform.position;
					}
	
					// move the follower by velocity if the distance was over followerDistance
					if (dir.magnitude > followerDistance) {
							_followers [i].rigidbody2D.velocity = dir.normalized * followerVel; //normalize the direction vector, and get the velocity vector
					} else {
							_followers [i].rigidbody2D.velocity = new Vector2 (0f, 0f);
					}
	
					//ItemManagerでオプションオブジェクトを保持
					FindObjectOfType<FollowerManager> ().setFollowersObj (_followers);

				//プレイヤーが存在しない場合はオプションオブジェクトを削除
				if(player == null){
					destroyMyself();
				}
			}
		}
	}
	
	//オプション群の生成
	public void buildInstance (int argFollowersNum) {
		for (int i=0; i<argFollowersNum; i++) {
			_followers.Add(Instantiate (follower, follower.transform.position, follower.transform.rotation) as GameObject);
		}
	}
	
	//オプションオブジェクトの削除　オプション数の初期化
	public void destroyMyself(){
		Destroy (this);
		flwMngObj.destroyOldFollowers();
		flwMngObj.followersNum = 0;
	}
}
