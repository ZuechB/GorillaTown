using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace QuestCam
{
    public static class QuestCamNative
    {
        public const string Assembly = @"QuestCamLib";
        
        public enum Status : int {
            Ok                  = 0,
            InvalidArgument     = 1,
            InvalidOperation    = 2,
            NotImplemented      = 3,
            InvalidSession      = 4,
        }
        
        public delegate void RecorderCreatedHandler (IntPtr context, IntPtr recorder, Status status, int w, int h);
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderCreateMP4")]
        public static extern Status CreateMP4Recorder (
            [MarshalAs(UnmanagedType.LPUTF8Str)] string gameToken,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string path,
            int width,
            int height,
            float frameRate,
            int sampleRate,
            int channelCount,
            int videoBitrate,
            int keyframeInterval,
            int audioBitRate,
            IntPtr context,
            RecorderCreatedHandler handler
        );
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderCommitFrame")]
        public static extern unsafe Status CommitFrame (
            this IntPtr recorder,
            void* pixelBuffer,
            long timestamp
        );

        [DllImport(Assembly, EntryPoint = @"QCRecorderCommitAudioSamples")]
        public static extern unsafe Status CommitAudioSamples(this IntPtr recorder, float* samples, int sampleCount);
        
        public delegate void RecordingHandler (IntPtr context, IntPtr path);
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderFinishWriting")]
        public static extern Status FinishWriting (
            this IntPtr recorder,
            RecordingHandler handler,
            IntPtr context
        );
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderSetUnityAudioVolume")]
        public static extern void SetUnityAudioVolume(this IntPtr recorder, float volume);
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderSetMicrophoneAudioVolume")]
        public static extern void SetMicrophoneAudioVolume(this IntPtr recorder, float volume);
        
        [DllImport(Assembly, EntryPoint = @"QCRecorderSetPaused")]
        public static extern void SetPaused(this IntPtr recorder, bool paused);
    }
}