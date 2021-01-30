using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
	[SerializeField]
	private VideoPlayer m_videoPlayer = null;

	[SerializeField]
	private List<VideoClip> m_clips = null;

	private void Start()
	{
		if (m_clips == null || m_clips.Count == 0)
		{
			return;
		}

		int rand = Random.Range(0, m_clips.Count);
		m_videoPlayer.clip = m_clips[rand];
	}
}
