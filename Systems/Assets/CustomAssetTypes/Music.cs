using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System;

namespace ECS.Systems.Assets.CustomAssetTypes
{
    public class Music : IDisposable
    {
        private readonly VorbisReader vorbis;

        private DynamicSoundEffectInstance instance;

        private readonly int loopStart;
        private readonly int loopEnd;
        private readonly int channels;
        private readonly int sampleRate;

        private const int bufferLength = 4096;
        private const int bufferMin = 4;

        public TimeSpan CurrentTime => vorbis.DecodedTime;

        public bool IsPaused => instance != null && instance.State == SoundState.Paused;

        internal Music(string path)
        {
            vorbis = new VorbisReader(path);

            channels = vorbis.Channels;
            sampleRate = vorbis.SampleRate;

            string[] comments = vorbis.Comments;

            for (int i = 0; i < comments.Length; i++)
            {
                if (comments[i].StartsWith("LOOPSTART"))
                {
                    int.TryParse(comments[i].Split('=')[1], out loopStart);
                }
                else if (comments[i].StartsWith("LOOPEND"))
                {
                    int.TryParse(comments[i].Split('=')[1], out loopEnd);
                }
            }
        }

        /// <summary>
        /// Plays the music encapsulated by this track.
        /// </summary>
        public void Play()
        {
            instance = new DynamicSoundEffectInstance(sampleRate, (AudioChannels)channels);

            vorbis.DecodedPosition = loopStart;

            TrySubmittingBuffer();
        }

        /// <summary>
        /// Positions the reader at a specific time point.
        /// </summary>
        /// <param name="time">The target time position.</param>
        public void GoToTime(TimeSpan time) => vorbis.DecodedTime = time < TimeSpan.Zero ? TimeSpan.Zero : time;

        /// <summary>
        /// Pauses the music encapsulated by this track.
        /// </summary>
        public void Pause() => instance?.Pause();

        /// <summary>
        /// Resumes the music encapsulated by this track.
        /// </summary>
        public void Resume() => instance?.Resume();

        /// <summary>
        /// Stops the music encapsulated by this track.
        /// </summary>
        public void Stop()
        {
            instance?.Stop();
            instance?.Dispose();

            instance = null;
        }

        /// <summary>
        /// Sets the volume of the music encapsulated by this track.
        /// </summary>
        /// <param name="volume">The target volume.</param>
        public void SetVolume(float volume)
        {
            if (instance != null)
            {
                instance.Volume = volume;
            }
        }

        /// <summary>
        /// Sets the pitch of the music encapsulated by this track.
        /// </summary>
        /// <param name="pitch">The target pitch.</param>
        public void SetPitch(float pitch)
        {
            if (instance != null)
            {
                instance.Pitch = pitch;
            }
        }

        /// <summary>
        /// Sets the pan of the music encapsulated by this track.
        /// </summary>
        /// <param name="pan">The target pan.</param>
        public void SetPan(float pan)
        {
            if (instance != null)
            {
                instance.Pan = pan;
            }
        }

        /// <summary>
        /// Keeps the audio playing by submitting a new buffer (intended to be called every tick).
        /// </summary>
        public void TrySubmittingBuffer()
        {
            if (instance == null)
            {
                return;
            }

            if (instance.PendingBufferCount < bufferMin)
            {
                float[] readBuffer = new float[bufferLength];

                int samplesRead = vorbis.ReadSamples(readBuffer, 0, readBuffer.Length);

                if ((loopEnd > 0 && vorbis.DecodedPosition >= loopEnd) || samplesRead < readBuffer.Length) // If the music has ended or passed the loop endpoint, send to loop start point (defaults to the beginning of the music).
                {
                    vorbis.DecodedPosition = loopStart;
                    vorbis.ReadSamples(readBuffer, samplesRead, readBuffer.Length - samplesRead);
                }

                instance.SubmitFloatBufferEXT(readBuffer);
            }

            if (!IsPaused)
            {
                instance.Play();
            }
        }

        public void Dispose()
        {
            vorbis?.Dispose();
            instance?.Dispose();
        }
    }
}
