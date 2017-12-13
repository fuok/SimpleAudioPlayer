using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour
{
	public Button btnSFX1, btnSFX2, btnSFX3;
	public Button btnBGM1, btnBGM2;
	public Button btnPlay, btnPause, btnStop;

	void Start ()
	{
		btnSFX1.onClick.AddListener (() => {
			AudioManagerS.Instance.PlaySFX ("ququSound", 1f);
		});
		btnSFX2.onClick.AddListener (() => {
			AudioManagerS.Instance.PlaySFX ("thunder", 1f);
		});
		btnSFX3.onClick.AddListener (() => {
			AudioManagerS.Instance.PlaySFX ("AudioEffect_C3_Fire(ShortGun)", 1f);
		});

		btnBGM1.onClick.AddListener (() => {
			AudioManagerS.Instance.PlayBGM ("GameGUI_BackgroundAudio", 1f);
		});
		btnBGM2.onClick.AddListener (() => {
			AudioManagerS.Instance.PlayBGM ("GamePlaying_BackgroundAudio", 1f);
		});
	
		btnPlay.onClick.AddListener (() => {
			AudioManagerS.Instance.PlayAll ();
		});
		btnPause.onClick.AddListener (() => {
			AudioManagerS.Instance.PauseAll ();
		});
		btnStop.onClick.AddListener (() => {
			AudioManagerS.Instance.StopAll ();
		});
	}

	void Update ()
	{
	
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {

		}

	}
}
