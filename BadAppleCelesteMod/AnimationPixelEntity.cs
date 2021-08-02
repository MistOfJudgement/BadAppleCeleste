
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
namespace Celeste.Mod.BadAppleCelesteMod {
    [CustomEntity("BadAppleCelesteMod/AnimationPixelEntity")]
    class AnimationPixelEntity : Entity{


        public AnimationPixelEntity(Vector2 offset) : base(offset) {
            Logger.Log("BadApple", "Created Animation Pixel at" + offset.ToString());
            Visible = true;
            Sprite filledState = GFX.SpriteBank.Create("strawberrySeed");
            Logger.Log("BadApple", "created sprite:" + filledState.ToString());
            Add(filledState);
            Logger.Log("BadApple", "added sprite to Entity");
            filledState.Play("idle");
            Logger.Log("BadApple","Played Animation");
        }

        public void toggle(bool desiredState) {
            Visible = desiredState;
        } 
        public void toggle() {
            Visible = !Visible;
        }

    }
}
