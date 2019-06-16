#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main(int argc, char* argv[])
{
    if (argc != 3) {
        cout << "Usage: PROGRAM input output " << endl;
        return -1;
    }
    Mat bitmap = imread("s1.jpg", CV_LOAD_IMAGE_COLOR);	


    if (!bitmap.data) {
        cout << "Unable to load image" << endl;
        return -1;
    }
    int ww = bitmap.cols - 2 * bitmap.cols / 8;
    int hh = bitmap.rows - 2 * bitmap.rows / 8;
    int x1 = bitmap.cols / 8;
    int y1 = bitmap.rows / 8;

    int pixelSize = bitmap.channels();
    int stride = pixelSize * bitmap.cols;
    uchar* scan0 = bitmap.ptr<uchar>(0) + (y1  * stride) + x1 * pixelSize;

    uchar* row = scan0, *bb = row;
    for (    int yy = 0; yy < hh; yy++, bb = (row += stride)) {
        for (int xx = 0; xx < ww; xx++, bb += pixelSize     ) {
            // *(bb + 0) is B (Blue ) component of the pixel
            // *(bb + 1) is G (Green) component of the pixel
            // *(bb + 2) is R (Red  ) component of the pixel
            // *(bb + 3) is A (Alpha) component of the pixel ( for 32bpp )
            uchar gray = ((1140 * *(bb + 0) + 5870 * *(bb + 1) + 2989 * *(bb + 2)) / 10000);
            *(bb + 0) = *(bb + 1) = *(bb + 2) = gray;
        }
    }
    vector<int> compression_params;
    compression_params.push_back(CV_IMWRITE_JPEG_QUALITY);
    compression_params.push_back(50);
    imwrite(argv[2], bitmap, compression_params);
    return 0;
}

