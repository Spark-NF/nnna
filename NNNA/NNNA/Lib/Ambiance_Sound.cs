using Microsoft.Xna.Framework.Audio;

namespace NNNA
{
	class Ambiance_Sound
	{
		private AudioEngine _engine;
        private WaveBank _musique;
		private SoundBank _sons; 
		private Cue _piste;
		private AudioCategory _musicCategory;
        string _current_sound;

		public void Initializesons(float musicVolume, float soundMusic, float soundGeneral)
		{
			_engine = new AudioEngine("Content/sounds/son projet.xgs");
			_musique = new WaveBank(_engine, "Content/sounds/Wave Bank.xwb");
			_sons = new SoundBank(_engine, "Content/sounds/sound_menu.xsb");
			_musicCategory = _engine.GetCategory("Music");
			_musicCategory.SetVolume(musicVolume * soundMusic * (soundGeneral / 10));
		}

        public void Set_Music(string name)
        {
            _engine.Update();
            _piste = _sons.GetCue(name);
            _current_sound = name;
        }

        public void Play()
        {
            _engine.Update();
            if (!_piste.IsPlaying)
            {
                _piste = _sons.GetCue(_current_sound);
                _piste.Play();
            }
        }

        public void Set_Volume(float volume)
        {
            _musicCategory.SetVolume(volume);
        }

        public void Pause()
        {
            if (_piste.IsPlaying && !_piste.IsPaused)
            {
                _piste.Pause();
            }
        }

        public void Resume()
        {
            if (_piste.IsPaused)
                _piste.Resume();
        }

        public void Stop()
        {
            _piste.Stop(AudioStopOptions.Immediate);
        }
	}
}
