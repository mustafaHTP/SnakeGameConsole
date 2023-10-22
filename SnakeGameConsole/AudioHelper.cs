using NAudio.Wave;

namespace SnakeGameConsole
{
    internal static class AudioHelper
    {
        static string baseDirectoryPath = Environment.CurrentDirectory;
        static string soundEffectsPath = Path.Combine(baseDirectoryPath, "..", "..", "..", "SoundEffects");

        static string eatEffectFileName = "eat_effect.wav";
        static string gameOverEffectFileName = "game_over.wav";
        static string startGameEffectFileName = "start_game.wav";

        internal static void PlayEatEffect()
        {
            string eatEffectPath = Path.Combine(soundEffectsPath, eatEffectFileName);
            if (File.Exists(eatEffectPath))
            {
                using var audioFile = new AudioFileReader(eatEffectPath);
                using var outputDevice = new WaveOutEvent();

                outputDevice.Init(audioFile);
                outputDevice.Play();

                //Get the duration of audio file to 
                //prevent stopping audio file
                double duration = audioFile.TotalTime.TotalSeconds;
                double durationInMiliseconds = duration * 1000;

                Thread.Sleep((int)durationInMiliseconds);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        internal static void PlayGameOverEffect()
        {
            string gameOverEffectPath = Path.Combine(soundEffectsPath, gameOverEffectFileName);
            if (File.Exists(gameOverEffectPath))
            {
                using var audioFile = new AudioFileReader(gameOverEffectPath);
                using var outputDevice = new WaveOutEvent();

                outputDevice.Init(audioFile);
                outputDevice.Play();

                //Get the duration of audio file to 
                //prevent stopping audio file
                double duration = audioFile.TotalTime.TotalSeconds;
                double durationInMiliseconds = duration * 1000;

                Thread.Sleep((int)durationInMiliseconds);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        internal static void PlayStartGameEffect()
        {
            string startGameEffectPath = Path.Combine(soundEffectsPath, startGameEffectFileName);
            if (File.Exists(startGameEffectPath))
            {
                using var audioFile = new AudioFileReader(startGameEffectPath);
                using var outputDevice = new WaveOutEvent();

                outputDevice.Init(audioFile);
                outputDevice.Play();

                //Get the duration of audio file to 
                //prevent stopping audio file
                double duration = audioFile.TotalTime.TotalSeconds;
                double durationInMiliseconds = duration * 1000;

                Thread.Sleep((int)durationInMiliseconds);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
