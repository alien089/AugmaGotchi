using UnityEngine;

namespace Character_System.Audio
{
    public class AudioComponentContinuous : AudioComponent
    {
        protected override void CallAudio(object[] param)
        {
            bool value = (bool)param[0];

            if (value)
            {
                AudioClip clip = (AudioClip)param[1];
                _xAudioSource.clip = clip;
                _xAudioSource.Play();
            }
            else
            {
                _xAudioSource.Stop();
            }
        }
    }
}
