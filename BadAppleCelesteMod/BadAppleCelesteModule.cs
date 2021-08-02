using System;
using System.IO;
using Celeste;
using Celeste.Mod.BadAppleCelesteMod;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BadAppleCeleseteMod
{
    public class BadAppleCelesteModule : EverestModule {

        public static EverestModule Instance;
        AnimationController animationController;
        EventInstance song;
        System.Diagnostics.Stopwatch stopwatch;
        Vector2 spawnLocation = new Vector2(0, 0);
        public BadAppleCelesteModule() {
            Instance = this;
            animationController = new AnimationController();

        }
        public override Type SettingsType  => typeof(BadAppleCelesteSettings);
        public static BadAppleCelesteSettings Settings => (BadAppleCelesteSettings)Instance._Settings;
        public override void Load() {
            On.Celeste.Player.Update += onUpdate;
            On.Celeste.Player.Die += Player_Die;
            Everest.Events.Level.OnTransitionTo += Level_OnTransitionTo;
            FileDecoder.getAnimation(animationController, Settings.DataPath);
            stopwatch = new System.Diagnostics.Stopwatch();
        }

        private void Level_OnTransitionTo(Level level, LevelData next, Vector2 direction) {
            spawnLocation = next.Position;
            if(song != null) {
                Audio.Stop(song);

            }
            animationController.Reset();
        }

        public override void Unload() {
            On.Celeste.Player.Update -= onUpdate;
            On.Celeste.Player.Die -= Player_Die;
            Everest.Events.Level.OnTransitionTo -= Level_OnTransitionTo;
        }
        
        private PlayerDeadBody Player_Die(On.Celeste.Player.orig_Die orig, Player self, Vector2 direction, bool evenIfInvincible, bool registerDeathInStats) {
            Audio.Stop(song);
            animationController.Reset();

            return orig(self, direction, evenIfInvincible, registerDeathInStats);
        }

        

        
        private void onUpdate(On.Celeste.Player.orig_Update orig, Player self) {
            orig(self);
            if (Settings.SpeedBoost.Check) {
                self.Speed.X *= 1.2f;
                self.Speed.Y *= 1.2f;
            }
            
            if(!animationController.playing && Settings.SpawnAnimationPixel.Pressed) {
                Logger.Log("BadApple", "spawning at " + self.Position.ToString());
                
                animationController.LoadScene(0, 0, self.Scene);
                animationController.playing = true;
                song = Audio.Play("event:/badapple/【東方】Bad Apple!! ＰＶ【影絵】");

            }

            if(!animationController.playing && Audio.IsPlaying(song)) {
                Audio.Stop(song);
            }
            stopwatch.Stop();
            animationController.Update(stopwatch.Elapsed.TotalSeconds);
            stopwatch.Restart();

        }

    }
}
