using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineResetter : MonoBehaviour
{
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void RestartTimeline()
    {
        director.Stop();
        director.time = 0;
        director.Play();
    }
}
