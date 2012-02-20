using Microsoft.Xna.Framework.Audio;

namespace NNNA
{
    class Sons
    {
        private AudioEngine engine_menu;
        private WaveBank musique_menu;
        private SoundBank sons_menu; 
        private Cue musiquemenu;
        AudioCategory musicCategory;

        public void Initializesons(float musicVolume, float m_sound_music, float m_sound_general)
        {
            engine_menu = new AudioEngine("Content/sounds/son projet.xgs");
            musique_menu = new WaveBank(engine_menu, "Content/sounds/Wave Bank.xwb");
            sons_menu = new SoundBank(engine_menu, "Content/sounds/sound_menu.xsb");
            musiquemenu = sons_menu.GetCue("sonmenu");
            musiquemenu.Play();
            engine_menu.Update();
            musicCategory = engine_menu.GetCategory("Music");
            musicCategory.SetVolume(musicVolume * m_sound_music * (m_sound_general / 10));
        }
        
        public Cue Musiquemenu
        { get { return musiquemenu; } }

        public AudioEngine Engine_menu
        { get { return engine_menu; } }

        public AudioCategory MusicCategory
        { get { return musicCategory; } }
    }
}
