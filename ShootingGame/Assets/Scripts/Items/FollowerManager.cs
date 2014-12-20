using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowerManager : MonoBehaviour {

	/** オプション最大数*/
	public int maxFollowers = 4;

	/** 現在のオプションの数*/
	public int followersNum = 0;

	/** オプション削除フラグ*/
	private bool isOptionDestroy = false;

	/** 現在のオプションオブジェクト*/
	private List<GameObject> _followers; // list of instansce

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	//現在のオプション数を設定
	public void setFollowersNum(int argFolloersNum){
		followersNum = argFolloersNum;
	}
	
	/**現在のオプション数を取得*/
	public int getFollowersNum(){
		return followersNum;
	}

	/**オプション削除かどうかを判別*/
	public bool IsOptionDestroy(){
		return isOptionDestroy;
	}

	/**オプション生成開始 = 削除フラグを立てない*/
	public void startFolloers(){
		isOptionDestroy = false;
	}

	/** 古いオプションを削除処理 */
	public void destroyOldFollowers(){
		if (_followers != null) {
			for(int i=0; i<_followers.Count; i++) {
				Destroy(_followers[i]);
			}
		}
	}

	/** オプションオブジェクトを設定*/
	public void setFollowersObj(List<GameObject> argObj){
		_followers = argObj;
	}

	/** オプションオブジェクトを取得*/
	public List<GameObject> getFollowersObj(){
		return _followers;
	}

	/**オプションが最大かどうかの判別*/
	public bool IsFollowerMax(){
		if (followersNum == maxFollowers) {
			return true;
		} else {
			return false;
		}
	}
}
