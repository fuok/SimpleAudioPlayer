using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//音乐播放分为几种不同情况:
//1,同一音源，继续播放。如背景音乐，切换场景时如果音乐相同，就没必要重新播放。
//2,同一音源，重新播放。如格斗游戏的打击音效，一旦连击，即使上一次声音没播完，也马上结束播放新的。
//3,不同音源，替代其他音源播放。同上，不同的打击效果音。
//4,不同音源，和其他音源同时播放。最常见的音效。
//情况1、2目前通过BGM方法可以实现，如果在其他地方使用要和情况3一起考虑。
//但这个方法仍存在问题，比如人物语音，如何判断该停止播放哪段音源，除非把特定人物语音每次都挂载到特定AudioResource上。
//那么这里能处理的，就只有背景音乐和特效了
public class AudioManagerS : MonoBehaviour
{
	public static AudioManagerS Instance{ get; private set; }

	//音频剪辑数组
	public AudioClip[] mAuClipArray;
	//音频剪辑索引
	private Dictionary<string,AudioClip> mDicClip;

	private string currentBGM;

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
	/// Plaies the background.
	/// </summary>
	/// <param name="audioSourceName">Audio source name.</param>
	/// <param name="Volume">Volume.</param>
	/// <param name="restartSame">If set to <c>true</c> restart same.</param>
	public void PlayBGM (string audioSourceName, float Volume, bool restartSame = false)
	{
		if (!string.IsNullOrEmpty (audioSourceName)) {
			if (mDicClip [audioSourceName]) {
				//AudioSource必须是组件形式才能播放，只是拿到对象（例如从字典里取出）是不行的。prefab的source也是不能播的,注意引用关系
				AudioSource aSource = null;
				foreach (var item in gameObject.GetComponents<AudioSource>()) {
					if (currentBGM == currentBGM) {
						aSource = item;
						break;
					}
				}
				aSource = aSource ?? GetLeisureAS ();

				if (aSource.clip) {
					if (aSource.clip.name == audioSourceName && aSource.isPlaying && !restartSame) {//判断是否正在播放、是否重新开始
						//如果当前bgm在播放，就什么也不做.
						print ("已经存在BGM，保持不变");
						return;
					} else {
						print ("已经存在BGM，用当前AudioSource播放新BGM");
					}

				} else {
					print ("不存在BGM，使用新AudioSource");
				}

				//关闭之前的bgm
				aSource.Stop ();
				//播放新的
//				AudioSource aSource = GetLeisureAS ();
				aSource.loop = true;
				aSource.volume = Volume;
				aSource.clip = mDicClip [audioSourceName];
				aSource.Play ();
				currentBGM = audioSourceName;
			}
		}
	}

	/// <summary>
	/// 可以同时播放相同Clip，适合播放音效,使用不断添加删除组件的方式
	/// </summary>
	/// <param name="audioName">Audio name.</param>
	public void PlaySFX (string audioClipName, float Volume, bool autoDestroy = false)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = GetLeisureAS ();
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
				AudioSource aSource = GetLeisureAS ();
				aSource.loop = true;
				aSource.volume = Volume;
				aSource.clip = mDicClip [audioClipName];
				aSource.Play ();
				Destroy (aSource, time);
			}
		}
	}

	//PlayOneShot搞得这么麻烦就和前面没什么区别了，如果要用这个还是直接传clip比较好。PlayOneShot的特点是播放完后会把clip置空，但不会删除AudioSource
	public void PlaySfxOneShot (string audioClipName, float Volume, bool autoDestroy = false)
	{
		if (!string.IsNullOrEmpty (audioClipName)) {
			if (mDicClip [audioClipName]) {
				AudioSource aSource = GetLeisureAS ();
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

	//----------------------------------------------------------------------------

	//重复使用AudioSource组件
	private AudioSource GetLeisureAS ()
	{
		foreach (var item in gameObject.GetComponents<AudioSource>()) {
			if (!item.isPlaying) {
				return item;
			}
		}
		return gameObject.AddComponent<AudioSource> ();
	}
}