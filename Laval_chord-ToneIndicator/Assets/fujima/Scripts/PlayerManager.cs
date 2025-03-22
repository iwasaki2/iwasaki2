using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static OVRManager;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public bool isLower;

    [SyncVar]
    public float targetFreq;

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

    private float vf2 = 1000;
    private float hf2 = 1000;

    public float correctVoicePower;

    public bool isFemale;
    public GameObject indicator;

    void Start()
    {
        isLower = true;

        if (isServer)
        {
            GameObject.FindGameObjectWithTag("Server").GetComponent<ServerController>().NewClient(this);
        }

        asVoice.clip = CreateSineWave(1000, sampleRate);
        asVoice.loop = true;
        asVoice.volume = 0.0f;
        asVoice.Play();

        asHarmonic.clip = CreateSineWave(1000, sampleRate);
        asHarmonic.loop = true;
        asHarmonic.volume = 0.0f;
        asHarmonic.Play();

        correctVoicePower = 0;
        isFemale = false;

        OVRManager.HMDUnmounted += CmdRestart;
    }

    private void Update()
    {

        if (!isLocalPlayer)
        {
            indicator.SetActive(false);
            return;
        }

        asVoice.volume = anotherVoiceI;
        if ( vf2 != anotherVoiceF )
        {
            asVoice.Stop();
            asVoice.clip = CreateSineWave(anotherVoiceF, sampleRate);
            asVoice.Play();
            vf2 = anotherVoiceF;

        }


        // harmonicF の音を強さ harmonicI で出す。
        asHarmonic.volume = harmonicI;
        if(hf2 != harmonicF)
        {
            asHarmonic.Stop();
            asHarmonic.clip = CreateSineWave(harmonicF, sampleRate);
            asHarmonic.Play();
            hf2 = harmonicF;
        }

        
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            CmdSetSex('F');
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            CmdSetSex('M');
        }
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

    [Command]
    public void CmdSetSex(char s)
    {
        Debug.Log($"cmd {s}");
        if (isServer)
        {
            isFemale = ( s == 'F' );
        }
    }

    [Command]
    void CmdRestart()
    {
        Debug.Log("Unmount detected");
        GameObject.FindGameObjectWithTag("Server").GetComponent<ServerController>().RestartGame();
    }

}
