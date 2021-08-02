using Microsoft.Xna.Framework.Input;
using YamlDotNet.Serialization;
namespace Celeste.Mod.BadAppleCeleseteMod {
    [SettingName("modoptions_badAppleMod")]
    public class BadAppleCelesteSettings :EverestModuleSettings{
        public ButtonBinding SpeedBoost { get; set; }
        public ButtonBinding SpawnAnimationPixel { get; set; }
        [SettingIgnore]
        public string DataPath { get; set; } = "badapple.rle";
    }
}
