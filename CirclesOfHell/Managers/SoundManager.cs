using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    sealed class SoundManager
    {
        private static SoundManager __instance = new SoundManager();

        private Dictionary<string, SoundEffect> __soundList = new Dictionary<string, SoundEffect>();

        private Song __currentSong;

        public static SoundManager Instance
        {
            get
            {
                return __instance;
            }
        }
        private SoundManager()
        { }

        public void LoadSound(String _key, String _path, ContentManager _content)
        {
            SoundEffect tempSound = _content.Load<SoundEffect>(_path);
            //SoundEffectInstance instance = tempSound.CreateInstance();
            __soundList.Add(_key, tempSound);
        }
        public void PlaySound(String _key)
        {
            __soundList[_key].Play();
            
        }



        public void LoadSong(String _path, ContentManager _content)
        {
            __currentSong = _content.Load<Song>(_path);
        }
        public void PlaySong()
        {
            
            MediaPlayer.Play(__currentSong);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true; 
            
            
        }
    }
}
