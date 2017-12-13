using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour
{
	public Button btnPlay, btnPause, btnStop;

	void Start ()
	{
	
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
			AudioManagerS.Instance.PlaySFX ("thunder", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
//			AudioManagerS.Instance.PlayOne ("ququSound", 1f);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			AudioManagerS.Instance.PlaySfxOneShot ("ququSound", 1f, true);
		}

	}
}
