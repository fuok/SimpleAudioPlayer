using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//音乐播放分为几种不同情况:
//1,同一首音乐，则继续播放。2,同一首音乐，从头播放。
//3,新的音乐，停止原先的播放新的。4,新的音乐，但同时播放。5,音效，无论相同的还是不同的，都同时播放
//但这个方法仍存在问题，比如人物语音，如何判断该停止播放哪段音源，除非把特定人物语音每次都挂载到特定AudioResource上。
//那么这里能处理的，就只有背景音乐和特效了
public class AudioManagerS : MonoBehaviour
{
	public static AudioManagerS Instance{ get; private set; }

	//音频剪辑数组
	public AudioClip[] mAuClipArray;
	//音频剪辑索引
	private Dictionary<string,AudioClip> mDicClip;

	private string bgmName;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

	void Start ()
	{
		////保存声音数据到Dictionary
		//if (mAuSourceArray.Length > 0)
		//{
		//    mDicSource = new Dictionary<string, AudioSource>();
		//    foreach (AudioSource audioSource in mAuSourceArray)
		//    {
		//        mDicSource.Add(audioSource.name, audioSource);
		//    }
		//}

		if (mAuClipArray.Length > 0) {
			mDicClip = new Dictionary<string, AudioClip> ();
			foreach (AudioClip audioClip in mAuClipArray) {
				mDicClip.Add (audioClip.name, audioClip);
			}
		}
	}

	/// <summary>
	///传值 “音乐剪辑” 播放 “背景音乐”
	/// </summary>
	/// <param name="audiClip">游戏音效剪辑</param>
	//public static void PlayEffAudio(AudioClip audiClip)
	//{
	//    if (audiClip)
	//    {//播放
	//        AudioSource[] AS = mAudioManagerInstance.GetComponents<AudioSource>();//先获取当前对象下的AudioSource
	//        for (int i = 1; i < AS.Length; i++)
	//        {//跳过下标0,0位预留给背景音乐
	//            if (!AS[i].isPlaying)
	//            {
	//                AS[i].clip = audiClip;//音频剪辑的赋值
	//                AS[i].Play();//播放背景音乐
	//                return;
	//            }
	//            else
	//            {
	//                //元素被占用
	//            }
	//        }
	//        //当需要同时播放多个音效或当前对象的AudioSource不足,添加新AudioSource
	//        AudioSource NewAS = mAudioManagerInstance.AddComponent<AudioSource>();
	//        NewAS.loop = false;
	//        NewAS.clip = audiClip;
	//        NewAS.Play();
	//    }
	//}

	public void PlayBGM (string audioSourceName, float Volume)
	{
//		if (!string.IsNullOrEmpty (audioSourceName)) {
//			if (mDicSource [audioSourceName]) {
//				if (mDicSource [audioSourceName].isPlaying) {
//					//如果当前bgm在播放，就什么也不做.
//					return;
//				}
//				foreach (var item in transform.Find("bgm").GetComponentsInChildren<AudioSource>()) {
//					//关闭其他bgm
//					item.Stop ();
//				}
//				//AudioSource必须是组件形式才能播放，只是拿到对象（例如从字典里取出）是不行的。prefab的source也是不能播的,注意引用关系
//				mDicSource [audioSourceName].Play ();
//			}
//		}
	}

	//PlayScheduled暂时还不太明白,TODO
	//	public void PlayS (string audioClipName, double time)
	//	{
	//		if (!string.IsNullOrEmpty (audioClipName)) {
	//			if (mDicClip [audioClipName]) {
	//				AudioSource aSource = gameObject.AddComponent<AudioSource> ();
	//				aSource.clip = mDicClip [audioClipName];
	//				aSource.PlayScheduled (time);
	//			}
	//		}
	//	}


	/// <summary>
	/// 可以同时播放相同Clip，适合播放音效,使用不断添加删除组件的方式
	/// </summary>
	/// <param name="audioName">Audio name.</param>
	public void PlaySFX (string audioClipName, float Volume, bool autoDestroy = false)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = gameObject.AddComponent<AudioSource> ();
				aSource.loop = false;
				aSource.volume = Volume;
				aSource.clip = mDicClip [audioClipName];
				aSource.Play ();
				if (autoDestroy) {//这里我默认不销毁，目的是复用组件
					Destroy (aSource, aSource.clip.length);//播放完销毁
				}
			}
		}
	}

	/// <summary>
	/// 循环播放的音效如警报声
	/// </summary>
	/// <param name="audioClipName">Audio clip name.</param>
	/// <param name="Volume">Volume.</param>
	/// <param name="time">Time.</param>
	public void PlaySFX (string audioClipName, float Volume, float time)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = gameObject.AddComponent<AudioSource> ();
				aSource.loop = true;
				aSource.volume = Volume;
				aSource.clip = mDicClip [audioClipName];
				aSource.Play ();
				Destroy (aSource, time);
			}
		}
	}

	//PlayOneShot搞得这么麻烦就和前面没什么区别了，如果要用这个还是直接传clip比较好
	public void PlaySfxOneShot (string audioClipName, float Volume, bool autoDestroy = false)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = gameObject.AddComponent<AudioSource> ();
				AudioClip clip = mDicClip [audioClipName];
				if (clip) {
					aSource.PlayOneShot (clip, Volume);
					if (autoDestroy) {
						Destroy (aSource, clip.length);
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------

	public void PlayAll ()
	{
		AudioSource[] AS = gameObject.GetComponents<AudioSource> ();
		for (int i = 0; i < AS.Length; i++) {
			AS [i].UnPause ();
		}
	}

	public void PauseAll ()
	{
		AudioSource[] AS = gameObject.GetComponents<AudioSource> ();
		for (int i = 0; i < AS.Length; i++) {
			AS [i].Pause ();
		}
	}

	public void StopAll ()
	{
		AudioSource[] AS = gameObject.GetComponents<AudioSource> ();
		for (int i = 0; i < AS.Length; i++) {
			AS [i].Stop ();
		}
	}
}