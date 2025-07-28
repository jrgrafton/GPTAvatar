using UnityEngine;
using System;
using System.Linq;

public class MicRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private int recordingStartPosition = 0;
 
    bool isRecording = false;
    string micDevice;

    void Start()
    {
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("[MicRecorder] Initializing continuous microphone buffer - BEGIN");
        
        // Initialize microphone device after audio system is ready
        micDevice = Microphone.devices.FirstOrDefault();
        Debug.Log($"[MicRecorder] Selected microphone device: {micDevice ?? "NULL"}");
        
        // Start continuous 30s circular buffer - accept the 1s freeze HERE at startup
        audioClip = Microphone.Start(micDevice, true, 15, 48000); // loop=true, 30s buffer, 16kHz optimal for speech
        while (Microphone.GetPosition(micDevice) <= 0) {}
        Debug.Log($"[MicRecorder] Continuous microphone initialization took: {(Time.realtimeSinceStartup - startTime) * 1000:F1}ms");
        Debug.Log("[MicRecorder] Microphone now running in continuous mode");
    }

    public void StartRecording()
    {
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("[MicRecorder] Starting recording system - BEGIN");
        
        // Just mark where we are in the continuous buffer - NO FREEZE!
        recordingStartPosition = Microphone.GetPosition(micDevice);
        isRecording = true;
        
        Debug.Log($"[MicRecorder] Recording marked at buffer position: {recordingStartPosition}");
        Debug.Log($"[MicRecorder] StartRecording took: {(Time.realtimeSinceStartup - startTime) * 1000:F1}ms");
    }

    public void Destroy()
    {
        if (IsRecording())
        {
            Debug.Log("Recording stopped");
            isRecording = false;
        }
        
        // Optionally stop the continuous microphone on cleanup
        if (Microphone.IsRecording(micDevice))
        {
            Microphone.End(micDevice);
            Debug.Log("[MicRecorder] Continuous microphone stopped");
        }
    }
    
    public void StopRecordingAndProcess(string outputFileName)
    {
        if (!isRecording)
            return;

        float startTime = Time.realtimeSinceStartup;
        Debug.Log("[MicRecorder] Stopping recording and processing - BEGIN");

        int currentPosition = Microphone.GetPosition(micDevice);
        isRecording = false;

        // Calculate recorded length, handling wraparound
        int recordedSamples = currentPosition - recordingStartPosition;
        if (recordedSamples < 0) // Handle buffer wraparound
            recordedSamples += audioClip.samples;

        Debug.Log($"[MicRecorder] Recorded {recordedSamples} samples (start: {recordingStartPosition}, end: {currentPosition})");

        // Extract audio data from circular buffer
        float[] allData = new float[audioClip.samples];
        audioClip.GetData(allData, 0);

        float[] recordedData = new float[recordedSamples];

        if (currentPosition < recordingStartPosition) // Wraparound case
        {
            Debug.Log("[MicRecorder] Handling buffer wraparound");
            // Copy end part: from recordingStartPosition to buffer end
            int endSamples = audioClip.samples - recordingStartPosition;
            Array.Copy(allData, recordingStartPosition, recordedData, 0, endSamples);
            
            // Copy start part: from buffer start to currentPosition
            Array.Copy(allData, 0, recordedData, endSamples, currentPosition);
        }
        else // Normal case: no wraparound
        {
            Array.Copy(allData, recordingStartPosition, recordedData, 0, recordedSamples);
        }

        // Create AudioClip with just the recorded segment
        AudioClip recordedClip = AudioClip.Create("Recording", recordedSamples, 
            audioClip.channels, audioClip.frequency, false);
        recordedClip.SetData(recordedData, 0);

        // Save and process as before
        SavWav.Save(outputFileName, recordedClip, true);

        AIManager aiScript = GetComponent<AIManager>();
        aiScript.ProcessMicAudioByFileName(outputFileName);
        
        Debug.Log($"[MicRecorder] StopRecordingAndProcess took: {(Time.realtimeSinceStartup - startTime) * 1000:F1}ms");
    }

   public bool IsRecording() { return isRecording; }
    private void Update()
    {

        /*
        if (!isRecording && Input.GetKey(KeyCode.M))
        {
            Debug.Log("Recording started");
            StartRecording();
        }

        if (isRecording && !Input.GetKey(KeyCode.M))
        {
           StopRecordingAndProcess();
        }
        */
    }
}
//#endif