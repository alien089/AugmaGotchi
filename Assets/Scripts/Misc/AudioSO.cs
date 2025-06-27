using UnityEngine;

namespace Misc
{
    [CreateAssetMenu(menuName = "Custom Assets/Audio Package")]
    public class AudioSO : ScriptableObject
    {
        public AudioClip[] AudioClips;
    }
}
