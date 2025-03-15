using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public float targetFreqF;

    [SyncVar]
    public float anotherVoiceF;
    [SyncVar]
    public float anotherVoiceI;
    [SyncVar]
    public float harmonicF;
    [SyncVar]
    public float harmonicI;

    public AudioSource asVoice;
    public AudioSource asHarmonic;
    private float sampleRate = 22050f;

    private float vf2;
    private float hf2;

    public float correctVoicePower;

    void Start()
    {
        if (isServer)
        {
            GameObject.FindGameObjectWithTag("Server").GetComponent<ServerController>().NewClient(this.netId);
        }

        targetFreqF = 165F;

        anotherVoiceF = 264;
        anotherVoiceI = 0;

        harmonicF = 1320;
        harmonicI = 0.0f;

        asVoice.clip = CreateSineWave(anotherVoiceF, sampleRate);
        asVoice.loop = true;
        asVoice.volume = 0.0f;
        asVoice.Play();

        asHarmonic.clip = CreateSineWave(harmonicF, sampleRate);
        asHarmonic.loop = true;
        asHarmonic.volume = 0.0f;
        asHarmonic.Play();

        vf2 = anotherVoiceF;
        hf2 = harmonicF;

        correctVoicePower = 0;
    }

    private void Update()
    {
        if( vf2 != anotherVoiceF )
        {
            asVoice.Stop();
            asVoice.clip = CreateSineWave(anotherVoiceF, sampleRate);
            asVoice.Play();
            vf2 = anotherVoiceF;
        }
        // anotherVoiceF の音を強さ anotherVoiceI で出す。
        asVoice.volume = anotherVoiceI;

        if(hf2 != harmonicF)
        {
            asHarmonic.Stop();
            asHarmonic.clip = CreateSineWave(harmonicF, sampleRate);
            asHarmonic.Play();
            hf2 = harmonicF;
        }

        // harmonicF の音を強さ harmonicI で出す。
        asHarmonic.volume = harmonicI;
    }

    private AudioClip CreateSineWave(float frequency, float sampleRate)
    {
        int sampleCount = (int)(sampleRate / frequency);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip audioClip = AudioClip.Create("SineWave", sampleCount, 1, (int)sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }

    [Command]
    public void CmdCurrentVoice(float st)
    {
        Debug.Log($"cmd {st}");
        if (isServer)
        {
            correctVoicePower = st;
        }
    }


}
