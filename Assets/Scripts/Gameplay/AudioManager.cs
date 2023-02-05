using System;
using Samples.Basic.Scripts;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;

        public bool playOnAwake = false;
        public bool loop = false;
    }
    
    public class AudioManager : Singleton<AudioManager>
    {
        public Sound[] sounds;

        protected void Awake()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume * 3f;
                sound.source.pitch = sound.pitch;
                sound.source.playOnAwake = sound.playOnAwake;
                sound.source.loop = sound.loop;
                if (sound.playOnAwake) sound.source.Play();
            }
        }

        public void Play(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
            }
        }

        public void AdjustVolume(string name, float value)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
                s.source.volume = value;
        }

        public void Stop(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
                s.source.Stop();
        }

        public void StopAll()
        {
            foreach (var sound in sounds)
            {
                sound.source.Stop();
            }
        }
    }
}