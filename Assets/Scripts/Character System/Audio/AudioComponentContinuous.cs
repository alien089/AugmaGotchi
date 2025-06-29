using Misc;
using UnityEngine;

namespace Character_System.Audio
{
    public class AudioComponentContinuous : AudioComponent
    {
        private int _iLastSamples = 0;
        private AudioClip[] _xClipList;
        
        // Update is called once per frame
        void Update()
        {
            if (_xAudioSource.isPlaying && _xAudioSource.loop)
            {
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
        
        protected override void CallAudio(object[] param)
        {
            bool value = (bool)param[0];

            if (value)
            {
                AudioSO so = (AudioSO)param[1];
                _xClipList = so.AudioClips;
                int rnd = Random.Range(0, _xClipList.Length);
                if (_xAudioSource.isPlaying) return;
                _xAudioSource.clip = _xClipList[rnd];
                _xAudioSource.Play();
            }
            else
            {
                _xAudioSource.Stop();
            }
        }
    }
}
