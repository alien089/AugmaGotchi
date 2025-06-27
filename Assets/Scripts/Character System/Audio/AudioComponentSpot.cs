using UnityEngine;

namespace Character_System.Audio
{
    public class AudioComponentSpot : AudioComponent
    {
        protected override void CallAudio(object[] param)
        {
            _xAudioSource.clip = (AudioClip)param[0];
            _xAudioSource.Play();
        }
    }
}
