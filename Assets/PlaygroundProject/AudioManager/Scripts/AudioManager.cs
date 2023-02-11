using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("bang");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
            Debug.LogWarning("<color=red>Sound " + name + " not found!</color>");
    }

    public void PlayDelayed(string name, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.PlayDelayed(delay);
        else
            Debug.LogWarning("<color=red>Sound " + name + " not found!</color>");
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.PlayOneShot(s.source.clip);
        else
            Debug.LogWarning("<color=red>Sound " + name + " not found!</color>");
    }

    //public void PlayOnGamepad(string name, int slot)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    if (s != null)
    //        s.source.PlayOnGamepad(slot);
    //    else
    //        Debug.LogWarning("<color=red>Sound " + name + " not found!</color>");
    //}

    public void PlayScheduled(string name, double time)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.PlayScheduled(time);
        else
            Debug.LogWarning("<color=red>Sound " + name + " not found!</color>");
    }
}
