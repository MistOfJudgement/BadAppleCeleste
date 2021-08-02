using Celeste.Mod.BadAppleCelesteMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.BadAppleCelesteMod {
    class FileDecoder {
        public static void getAnimation(AnimationController anim, string filename) {
            StreamReader file = new StreamReader(filename);
            /*
             * Encoding Key -
             * File Header - numberOfFrames Width Height
             * W## - ## number of white pixels
             * B## - ## number of Black Pixels
             * new line is new frame
             */
            string[] header = file.ReadLine().Split(' ');
            anim.Initialize(int.Parse(header[1]), int.Parse(header[2]), int.Parse(header[0]));
            
            for(int i = 0; i < anim.animation.Length; i++) {
                string frameString = file.ReadLine();
                rleToImage(frameString, anim.animation[i]);
            }
            file.Close();
        }

        public static void rleToImage(string rleString, Image dest) {
            int i = 0;
            int counter = 0;
            int repeat;

            while(i < rleString.Length) {
                while(char.IsLetter(rleString[i])) {
                    dest[counter++] = (rleString[i++] == 'W');

                }
                repeat = 0;
                while(char.IsDigit(rleString[i])) {
                    repeat = 10 * repeat + (rleString[i++] - '0');
                }
                char temp = rleString[i++];
                while(repeat-->0) {
                    dest[counter++] = (temp == 'W');
                }

            }
        }
    }
}
