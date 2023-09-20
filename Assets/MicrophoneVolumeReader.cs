using UnityEngine;

public class MicrophoneVolumeReader : MonoBehaviour
{
    public string microphoneDeviceName; // 用于存储麦克风设备名称
    public float sensitivity = 100f; // 音量敏感度

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // 获取可用的麦克风设备列表
        string[] microphoneDevices = Microphone.devices;

        if (microphoneDevices.Length > 0)
        {
            // 使用第一个可用的麦克风设备
            microphoneDeviceName = microphoneDevices[0];
            Debug.Log("Using microphone: " + microphoneDeviceName);

            // 启动麦克风监听
            audioSource.clip = Microphone.Start(microphoneDeviceName, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(microphoneDeviceName) > 0)) { } // 等待麦克风数据准备好
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone devices found.");
        }
    }

    void Update()
    {
        // 获取当前麦克风音量
        float volume = GetMicrophoneVolume();
        Debug.Log("Microphone Volume: " + volume);
    }

    public float GetMicrophoneVolume()
    {
        // 获取麦克风音频数据
        float[] audioData = new float[audioSource.clip.samples];
        audioSource.clip.GetData(audioData, 0);

        // 计算音量
        float sum = 0f;
        for (int i = 0; i < audioData.Length; i++)
        {
            sum += Mathf.Abs(audioData[i]);
        }

        // 计算平均音量并乘以敏感度
        float averageVolume = sum / audioData.Length * sensitivity;

        return averageVolume;
    }
}
