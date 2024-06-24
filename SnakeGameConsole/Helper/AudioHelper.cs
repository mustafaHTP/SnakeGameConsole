using NAudio.Wave;

namespace SnakeGameConsole
{
    internal static class AudioHelper
    {
        static readonly string BaseDirectoryPath = Environment.CurrentDirectory;
        static readonly string SfxFolderName = "SFX";
        static readonly string SfxFolderPath = Path.Combine(BaseDirectoryPath, SfxFolderName);

        static readonly string EatSfxFileName = "sfx_eat.wav";
        static readonly string EatSfxAIFileName = "sfx_eat_ai.wav";
        static readonly string GameOverSfxFileName = "sfx_game_over.wav";
        static readonly string StartGameSfxFileName = "sfx_start_game.wav";

        internal static void PlayEatEffect(bool isMute)
        {
            if (isMute) return;

            string eatEffectPath = Path.Combine(SfxFolderPath, EatSfxFileName);
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

        internal static void PlayEatEffectAI(bool isMute)
        {
            if (isMute) return;

            string eatEffectAIPath = Path.Combine(SfxFolderPath, EatSfxAIFileName);
            if (File.Exists(eatEffectAIPath))
            {
                using var audioFile = new AudioFileReader(eatEffectAIPath);
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

        internal static void PlayGameOverEffect(bool isMute)
        {
            if (isMute) return;

            string gameOverEffectPath = Path.Combine(SfxFolderPath, GameOverSfxFileName);
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

        internal static void PlayStartGameEffect(bool isMute)
        {
            if (isMute) return;

            string startGameEffectPath = Path.Combine(SfxFolderPath, StartGameSfxFileName);
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
