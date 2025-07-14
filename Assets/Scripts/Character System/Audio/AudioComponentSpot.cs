using UnityEngine;

namespace Character_System.Audio
{
    // Extends AudioComponent to play a single clip on demand without looping.
    public class AudioComponentSpot : AudioComponent
    {
        // Plays the provided AudioClip if not already playing.
        protected override void CallAudio(object[] param)
        {
            _xAudioSource.clip = (AudioClip)param[0];
            if (_xAudioSource.isPlaying) return;
            _xAudioSource.Play();
        }
    }
}
