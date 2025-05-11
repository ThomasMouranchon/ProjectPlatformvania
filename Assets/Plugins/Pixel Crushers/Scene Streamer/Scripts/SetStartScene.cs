using UnityEngine;
using System.Collections;

namespace PixelCrushers.SceneStreamer
{

	/// <summary>
	/// Sets the current scene at Start().
	/// </summary>
	[AddComponentMenu("Scene Streamer/Set Start Scene")]
	public class SetStartScene : MonoBehaviour 
	{

		/// <summary>
		/// The name of the scene to load at Start.
		/// </summary>
		[Tooltip("Load this scene at start")]
		public string startSceneName = "Scene 1";

		public void Start()
        {/*
            string currentScene = startSceneName;
            if (SaveManager.Instance.lastTeleportPoint != 0)
            {
                currentScene = SaveManager.Instance.activatedTeleportationsZone[SaveManager.Instance.lastTeleportPoint].ToString();
            }*/
            //SceneStreamer.SetCurrentScene(startSceneName);
			Destroy(this);
		}

	}

}
