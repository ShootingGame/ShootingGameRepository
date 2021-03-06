﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	private static ObjectPool _instance;

	// シングルトン
	public static ObjectPool instance {
		get {
			if (_instance == null) {

				// シーン上から取得する
				_instance = FindObjectOfType<ObjectPool> ();

				if (_instance == null) {

					// ゲームオブジェクトを作成しObjectPoolコンポーネントを追加する
					_instance = new GameObject ("ObjectPool").AddComponent<ObjectPool> ();
				}
			}
			return _instance;
		}
	}

	//TODO 一定時間ごとに、オブジェクトプールの使用しなくなった弾丸を破棄する
	int cleanInterval = 3;
	void OnEnable ()
	{
		StartCoroutine (RemoveObjectCheck ());
	}
	IEnumerator RemoveObjectCheck ()
	{
		while (true) {

			foreach( KeyValuePair<int, List<GameObject>> pobj in pooledGameObjects )
			{
				bool isAllDisabled = true;
				foreach (GameObject pooledObjList in pooledGameObjects[pobj.Key]) {
					if(pooledObjList != null && pooledObjList.activeInHierarchy != false){
						//アクティブな弾が一つもない弾種をチェック
						isAllDisabled = false;
					}
					if(isAllDisabled != true){
						break;
					}
				}
				if(isAllDisabled != false){
					//全て非アクティブの弾種を削除
					foreach (GameObject pooledObjList in pooledGameObjects[pobj.Key]) {
						Destroy(pooledObjList);
					}
					pooledGameObjects.Remove(pobj.Key);
					//TODO 一気に壊すのは負担かもしれないので、1回ずつ、1種類ずつ削除
					break;
				}
			}

			//RemoveObject (prepareCount);
			yield return new WaitForSeconds (cleanInterval);
		}
	}

	// ゲームオブジェクトのDictionary
	private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>> ();

	// ゲームオブジェクトをpooledGameObjectsから取得する。必要であれば新たに生成する
	public GameObject GetGameObject (GameObject prefab, Vector2 position, Quaternion rotation)
	{
		// プレハブのインスタンスIDをkeyとする
		int key = prefab.GetInstanceID ();

		// Dictionaryにkeyが存在しなければ作成する
		if (pooledGameObjects.ContainsKey (key) == false) {

			pooledGameObjects.Add (key, new List<GameObject> ());
		}

		List<GameObject> gameObjects = pooledGameObjects [key];

		GameObject go = null;

		for (int i = 0; i < gameObjects.Count; i++) {

			go = gameObjects [i];

			// 現在非アクティブ（未使用）であれば
			if (go != null && go.activeInHierarchy == false) {

				// 位置を設定する
				go.transform.position = position;

				// 角度を設定する
				go.transform.rotation = rotation;

				// これから使用するのでアクティブにする
				go.SetActive (true);

				return go;
			}
		}

		// 使用できるものがないので新たに生成する
		go = (GameObject)Instantiate (prefab, position, rotation);
		
		// ObjectPoolゲームオブジェクトの子要素にする
		go.transform.parent = transform;

		// リストに追加
		gameObjects.Add (go);

		return go;
	}

	// ゲームオブジェクトを非アクティブにする。こうすることで再利用可能状態にする
	public void ReleaseGameObject (GameObject go)
	{
		// 非アクティブにする
		go.SetActive (false);
	}

	//TODO シューティングゲーム用の弾プーリング制御
	public void shootingGamePool(GameObject argObj){
		if(argObj.transform.parent.gameObject.name != "ObjectPool"){
			if(argObj.transform.parent.gameObject.activeInHierarchy == true){
				// 弾の親オブジェクトを削除。実際には非アクティブにする
				ObjectPool.instance.ReleaseGameObject (argObj.transform.parent.gameObject);
			}
		}else{
			ObjectPool.instance.ReleaseGameObject (argObj);
		}
	}
}
