// VideoToRaw.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <fstream>
#include <opencv2/core.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <string>
//#define ShowDisplay
#define FileOutput


using namespace cv;
const int desiredWidth = 64;
const int desiredHeight = 18;
std::string outputDir = "G:\\Celeste";
std::string outputFilename = "badapple.rle";
std::string inputFilename = "C:\\Users\\User\\Downloads\\badapple.mp4";

std::string matToRLE(Mat & input) {
    int i = input.cols * input.rows;
    uchar* image = input.ptr();
    std::string letters;
    for (int j = 0; j < i; j++) {
        int count = 1;
        while ((image[j*3] > 127) == (image[j*3 + 3] > 127) && j+1 < i) {
            count++;
            j++;
        }
        letters += std::to_string(count);
        if (image[j*3] > 127) {
            letters.push_back('W');
        } else {
            letters.push_back('B');
        }
    }
    return letters;
}

void RLEToMat(std::string input, Mat & dest) {
    int i = 0;
    int counter = 0;
    int repeat;
    uchar* image = dest.ptr();
    while (i < input.length()) {
        while (input[i] == 'B' || input[i] == 'W') {
            uchar value = 255 * (input[i++] == 'W');
            image[counter++] = value;
            image[counter++] = value;
            image[counter++] = value;
        }

        repeat = 0;
        while (std::isdigit(input[i])) {
            repeat = 10 * repeat + (input[i++] - '0');
        }
        char temp = input[i++];
        while (repeat-- > 1) {
            uchar value = 255 * (input[i++] == 'W');
            image[counter++] = value;
            image[counter++] = value;
            image[counter++] = value;
        }
    }
}
int main()
{

    std::cout << "Start\n";
    Mat frame;
    Mat resizedFrame;
    VideoCapture capture = VideoCapture(inputFilename);
    std::cout << "Capture Started\n";
    
    std::cout << capture.get(CAP_PROP_FRAME_COUNT) << std::endl;

#ifdef FileOutput
    int i = 0;
    std::fstream output;
    output.open(outputDir + "\\" + outputFilename, std::fstream::out);
    output << capture.get(CAP_PROP_FRAME_COUNT) << " " << std::to_string(desiredWidth) << " " << std::to_string(desiredHeight) << std::endl;
    while (capture.isOpened() || i < capture.get(CAP_PROP_FRAME_COUNT)) {
        capture.read(frame);
        if (frame.empty()) {
            std::cerr << "error: blank frame";
            break;
        }
        resize(frame, resizedFrame, Size(desiredWidth, desiredHeight));

        output << matToRLE(resizedFrame);
        std::cout << "printed frame:" << std::to_string(i++) << std::endl;
        if (capture.isOpened()) {
            output << std::endl;
        }

    }
    output.close();
#endif // FileOutput

#ifdef ShowDisplay

    while (capture.isOpened()) {
        capture.read(frame);
        resize(frame, resizedFrame, Size(desiredWidth, desiredHeight), 0, 0, INTER_NEAREST);
        resize(resizedFrame, frame, Size(frame.cols, frame.rows), 0, 0, INTER_NEAREST);
        if (frame.empty()) {
            std::cerr << "ERROR: blank frame";
            break;
        }
        imshow("badapple", frame);
        if (waitKey(5) >= 0) {
            std::cout << "recieved input: exiting\n";
            break;
        }
    }
#endif
    return 0;
}

