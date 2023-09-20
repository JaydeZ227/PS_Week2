using UnityEngine;

public class MicrophoneVolumeReader : MonoBehaviour
{
    public string microphoneDeviceName; // ���ڴ洢��˷��豸����
    public float sensitivity = 100f; // �������ж�

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // ��ȡ���õ���˷��豸�б�
        string[] microphoneDevices = Microphone.devices;

        if (microphoneDevices.Length > 0)
        {
            // ʹ�õ�һ�����õ���˷��豸
            microphoneDeviceName = microphoneDevices[0];
            Debug.Log("Using microphone: " + microphoneDeviceName);

            // ������˷����
            audioSource.clip = Microphone.Start(microphoneDeviceName, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(microphoneDeviceName) > 0)) { } // �ȴ���˷�����׼����
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone devices found.");
        }
    }

    void Update()
    {
        // ��ȡ��ǰ��˷�����
        float volume = GetMicrophoneVolume();
        Debug.Log("Microphone Volume: " + volume);
    }

    public float GetMicrophoneVolume()
    {
        // ��ȡ��˷���Ƶ����
        float[] audioData = new float[audioSource.clip.samples];
        audioSource.clip.GetData(audioData, 0);

        // ��������
        float sum = 0f;
        for (int i = 0; i < audioData.Length; i++)
        {
            sum += Mathf.Abs(audioData[i]);
        }

        // ����ƽ���������������ж�
        float averageVolume = sum / audioData.Length * sensitivity;

        return averageVolume;
    }
}
