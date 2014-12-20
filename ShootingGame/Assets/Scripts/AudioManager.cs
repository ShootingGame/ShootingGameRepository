using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//音声ファイル格納用変数
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

	/*
	void Update () {
	}
	*/

	public void playShotSound(int argSoundNum){
		switch(argSoundNum){
		case 1:
			audio.PlayOneShot (simpleShoot);
			/*
			 * audio.PlayOneShot (soundShoot);
			audio.PlayOneShot (soundSht1);
			audio.PlayOneShot (soundSht2);
			audio.PlayOneShot (soundExplosion);
			audio.PlayOneShot (soundBomb);
			audio.PlayOneShot (soundBoom);
			audio.PlayOneShot (soundChase);
			audio.PlayOneShot (soundSht1);
			audio.PlayOneShot (soundSht2);
			*/
			break;
		default:
			break;
		}
		audio.PlayOneShot (soundExplosion);
	}

	//音楽をストップする
	public void stopAudio(){
		Debug.Log ("stop music.Equals..");
		audio.Stop ();
	}
}