using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePlayerUse : WorldUsage
{
    public PlayableDirector playableDirector;

    // Override the Use method to play the timeline clip
    public override void Use()
    {
        base.Use();

        if (playableDirector != null)
        {
            playableDirector.Play();
        }
        else
        {
            Debug.LogError("TimelinePlayer: PlayableDirector is not assigned!");
        }
    }
}
