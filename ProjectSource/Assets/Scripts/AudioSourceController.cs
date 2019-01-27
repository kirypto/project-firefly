using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class AudioSourceController : MonoBehaviour {

    private AudioSource _source;

    public AudioClip Clip;

    [Range(0, 1)]
    public float Volume = 1f;

    [Range(.25f, 3)]
    public float Pitch = 1f;

    public bool Loop = false;

    [Range(0f, 1f)]
    public float SpacialBlend = 1f;

    public string[] RandomNames = { "GrannySound" };

    public string[] RandomTags = { "SpawnLocation" };


    void Awake() {
        _source = GetComponent<AudioSource>();
        if (_source == null) {
            _source = gameObject.AddComponent<AudioSource>();
        }
    }


    void Start() {
        Play();
    }


    public void SetSourceProperties(AudioClip clip, float volume, float picth, bool loop, float spacialBlend) {
        _source.clip = clip;
        _source.volume = volume;
        _source.pitch = picth;
        _source.loop = loop;
        _source.spatialBlend = spacialBlend;
    }

    IEnumerator WaitSeconds() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(15, 60));
            _source.Play();
        }
    }

    public void Play(string Name) {
        if (Name == "Granny") {
            print("Granny sound");
            Clip = (AudioClip)Resources.Load("g3");
        } else if (Name == "Theo") {
            print("Theo sound");
            Clip = (AudioClip)Resources.Load("kidtalking4");
        }
        print(Clip);

        Play();
    }

    public void Play() {
        SetSourceProperties(Clip, Volume, Pitch, Loop, SpacialBlend);
        if(System.Array.IndexOf(RandomTags, transform.tag) != -1) {
            StartCoroutine("WaitSeconds");
            return;
        }
        if (System.Array.IndexOf(RandomNames, transform.name) != -1) {
            StartCoroutine("WaitSeconds");
            return;
        }
        _source.Play();
    }

}