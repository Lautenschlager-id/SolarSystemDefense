using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SolarSystemDefense
{
    static class Sound
    {
        public static SoundEffect MouseHover { get; private set; }
        public static SoundEffect Click { get; private set; }

        public static SoundEffect Place { get; private set; }
        public static SoundEffect Shift { get; private set; }

        public static SoundEffect Go { get; private set; }
        public static SoundEffect Select { get; private set; }
        public static SoundEffect GameOver { get; private set; }
        public static SoundEffect Shoot { get; private set; }
        public static SoundEffect Hit { get; private set; }
        public static SoundEffect Explosion { get; private set; }
        public static SoundEffect BlackHole { get; private set; }
        public static SoundEffect EarthDamage { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            MouseHover = content.Load<SoundEffect>("Sound/Components/mousehover");
            Click = content.Load<SoundEffect>("Sound/Components/click");

            Place = content.Load<SoundEffect>("Sound/Editor/place");
            Shift = content.Load<SoundEffect>("Sound/Editor/shift");

            Go = content.Load<SoundEffect>("Sound/Game/go");
            Select = content.Load<SoundEffect>("Sound/Game/select");
            GameOver = content.Load<SoundEffect>("Sound/Game/gameover");
            Shoot = content.Load<SoundEffect>("Sound/Game/shoot");
            Hit = content.Load<SoundEffect>("Sound/Game/hit");
            Explosion = content.Load<SoundEffect>("Sound/Game/explosion");
            BlackHole = content.Load<SoundEffect>("Sound/Game/blackhole");
            EarthDamage = content.Load<SoundEffect>("Sound/Game/earthdamage");
        }
    }
}
