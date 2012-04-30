using Microsoft.Xna.Framework.Audio;

namespace NNNA
{
	class Sons
	{
		private AudioEngine _engineMenu;
		private SoundBank _sonsMenu; 
		private Cue _musiqueMenu;
		AudioCategory _musicCategory;

		public void Initializesons(float musicVolume, float soundMusic, float soundGeneral, bool play = true)
		{
			_engineMenu = new AudioEngine("Content/sounds/son projet.xgs");
			new WaveBank(_engineMenu, "Content/sounds/Wave Bank.xwb");
			_sonsMenu = new SoundBank(_engineMenu, "Content/sounds/sound_menu.xsb");
			_musiqueMenu = _sonsMenu.GetCue("sonmenu");
            if (play)
            {
                MusiqueMenu.Play();
            }
			_engineMenu.Update();
			_musicCategory = _engineMenu.GetCategory("Music");
			_musicCategory.SetVolume(musicVolume * soundMusic * (soundGeneral / 10));
		}
		
		public Cue MusiqueMenu
		{ get { return _musiqueMenu; } }

		public AudioEngine EngineMenu
		{ get { return _engineMenu; } }

		public AudioCategory MusicCategory
		{ get { return _musicCategory; } }
	}
}
