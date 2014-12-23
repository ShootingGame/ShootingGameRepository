using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//========== 個別にインスペクターで設定する項目 ==========
	//音声ファイル格納
	public AudioClip simpleShoot;
	public AudioClip soundExplosion;
	public AudioClip soundBomb;
	public AudioClip soundBoom;
	public AudioClip soundChase;
	public AudioClip soundShoot;
	public AudioClip soundSht1;
	public AudioClip soundSht2;
	public AudioClip soundBGM1;
	public AudioClip soundBGM2;


	//ショット音を設定処理
	public void playShotSound(int argSoundNum){
		switch(argSoundNum){
		case 1:
			audio.PlayOneShot (simpleShoot);
			break;
		case 2:
			audio.PlayOneShot (soundSht2);
			break;
		case 3:
			audio.PlayOneShot (soundBoom);
			break;
		case 4:
			audio.PlayOneShot (soundChase);
			break;
		default:
			audio.PlayOneShot (simpleShoot);
			break;
		}
	}

	//BGMを再生処理
	public void playBgm(int argBgmNo){
		//BGM play
	}

	//音楽をストップ処理
	public void stopAudio(){
		audio.Stop ();
	}
}