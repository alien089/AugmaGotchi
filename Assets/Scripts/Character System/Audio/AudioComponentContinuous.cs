using Misc;
using UnityEngine;

namespace Character_System.Audio
{
    // Extends AudioComponent to handle continuous looping playback with random clip switching.
    public class AudioComponentContinuous : AudioComponent
    {
        private int _iLastSamples = 0;
        private AudioClip[] _xClipList;

        // Checks per-frame if the loop has completed to swap to a random clip seamlessly.
        void Update()
        {
            if (_xAudioSource.isPlaying && _xAudioSource.loop)
            {
                // Detect loop restart to switch to a new random clip.
                if (_xAudioSource.timeSamples < _iLastSamples)
                {
                    int rnd = Random.Range(0, _xClipList.Length);
                    _xAudioSource.clip = _xClipList[rnd];
                    if (_xAudioSource.isPlaying) return;
                    _xAudioSource.Play();
                }
                _iLastSamples = _xAudioSource.timeSamples;
            }
        }

        // Handles starting or stopping continuous playback based on event parameters.
        protected override void CallAudio(object[] param)
        {
            bool value = (bool)param[0];

            if (value)
            {
                // Start playing a random clip from the list.
                AudioSO so = (AudioSO)param[1];
                _xClipList = so.AudioClips;
                int rnd = Random.Range(0, _xClipList.Length);
                if (_xAudioSource.isPlaying) return;
                _xAudioSource.clip = _xClipList[rnd];
                _xAudioSource.Play();
            }
            else
            {
                // Stop playback if the value is false.
                _xAudioSource.Stop();
            }
        }
    }
}
