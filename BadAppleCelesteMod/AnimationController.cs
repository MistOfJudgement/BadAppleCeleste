using System;
using System.Collections;

using Monocle;
namespace Celeste.Mod.BadAppleCelesteMod {
    struct Image {
        private int width, height;
        private BitArray data;
        public Image(int w, int h) {
            width = w;
            height = h;
            data = new BitArray(width * height);
        }
        
        public bool this[int a,int b] {
            get => data[b * width + a];
            set => data[b * width + a] = value;
        }
        public bool this[int a] {
            get => data[a];
            set => data[a] = value;
        }
        public static void FillAnimation(Image[] array, int x, int y) {
            for(int i = 0; i < array.Length; i++) {
                array[i] = new Image(x, y);
            }
        }
    }
    class AnimationController {
        private int width;
        private int height;
        AnimationPixelEntity[,] grid;
        public Image[] animation;
        double timer = 0;
        double timerOffset = -.7d;
        readonly double timerMod = 202d / 220d;
        readonly double millisecondsPerFrame = 1000/30d;
        public bool playing = false;
        public AnimationController() {
        
        }
        public void Initialize(int width, int height, int frames) {
            this.width = width;
            this.height = height;
            grid = new AnimationPixelEntity[width, height];
            animation = new Image[frames];
            Image.FillAnimation(animation, width, height);
            timer = timerOffset;
        }
        public void LoadScene(int dx, int dy, Scene scene) {
            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    grid[x, y] = new AnimationPixelEntity(new Microsoft.Xna.Framework.Vector2(dx+x*5, dy+y*10));
                    scene.Add(grid[x, y]);
                }
            }
        }
        public void Reset() {
            playing = false;
            timer = timerOffset;
        }
        public void Update(double dtime) {
            if(playing) {
                if(timer > 0) {
                    DrawFrame((int)(millisecondsPerFrame * timer));
                }
                    timer += dtime * timerMod;
                    if(timer * millisecondsPerFrame > animation.Length) {
                    playing = false;
                        timer = -2.5;
                    }

            }
        }
        public void DrawFrame(int index) {
          
            for(int y = 0; y < height; y++) {
                for(int x = 0; x < width; x++) {
                    grid[x, y].toggle(animation[index][x, y]);
                }
            }

        }
    }
}
