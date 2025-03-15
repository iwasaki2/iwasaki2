using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections.Generic;

public class SoundManager : NetworkBehaviour
{
    private AudioSource audioSource;
    public float frequency = 1320f;
    private float duration = 1f;
    private float sampleRate = 22050f;
    public float threshold = 1000f;


    private bool isPlaying = false;
    private bool isPlayingForFourSeconds = false;
    private float latestIntensity = 0f;

    private Dictionary<int, float> voices;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 1f;
        audioSource.loop = true;
        UpdateSineWave();
        voices = new Dictionary<int, float>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            frequency = 1320f;
            UpdateSineWave();
            audioSource.Stop();
            audioSource.Play();
        }
        else if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            frequency = 660f;
            UpdateSineWave();
            audioSource.Stop();
            audioSource.Play();
        }

        if (Keyboard.current.vKey.isPressed)
        {
            if (!isPlaying)
            {
                UpdateSineWave();
                audioSource.volume = 0.8f;
                audioSource.Play();
                isPlaying = true;
                StartCoroutine(CheckIfPlayingForFourSeconds());

                CmdPlaySound(0.8f);
            }
        }
        else if (Keyboard.current.oKey.isPressed)
        {
            if (!isPlaying)
            {
                UpdateSineWave();
                audioSource.volume = 1.0f;
                audioSource.Play();
                isPlaying = true;
                StartCoroutine(CheckIfPlayingForFourSeconds());

                CmdPlaySound(1.0f);
            }
        }

        if (!Keyboard.current.vKey.isPressed && !Keyboard.current.oKey.isPressed)
        {
            if (isPlaying)
            {
                audioSource.Stop();
                isPlaying = false;
                StopCoroutine(CheckIfPlayingForFourSeconds());
                isPlayingForFourSeconds = false;
            }
        }
    }

    [Command]
    private void CmdPlaySound(float volume)
    {
        Debug.Log($"?????? {connectionToClient.connectionId} ?????????");
        RpcPlaySound(volume);
    }

    [ClientRpc]
    private void RpcPlaySound(float volume)
    {
        if (!isPlaying)
        {
            audioSource.volume = volume;
            audioSource.Play();
            isPlaying = true;
            StartCoroutine(CheckIfPlayingForFourSeconds());
        }
    }

    [Command]
    public void CmdCurrentVoice(float st)
    {
        if (isServer)
        {
            voices[connectionToClient.connectionId] = st;
            foreach (var (cid, str) in voices)
            {
                //                Debug.Log($"{cid} {str}");
            }
        }
        //RpcPlaySound(volume);
    }

    private System.Collections.IEnumerator CheckIfPlayingForFourSeconds()
    {
        yield return new WaitForSeconds(4);
        isPlaying = false;
        isPlayingForFourSeconds = true;
        Debug.Log("??4??????????");
    }

    private void UpdateSineWave()
    {
        AudioClip sineWaveClip = CreateSineWave(frequency, duration, sampleRate);
        audioSource.clip = sineWaveClip;
    }

    private AudioClip CreateSineWave(float frequency, float duration, float sampleRate)
    {
        int sampleCount = (int)(duration * sampleRate);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip audioClip = AudioClip.Create("SineWave", sampleCount, 1, (int)sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }
}