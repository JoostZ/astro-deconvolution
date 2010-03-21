#include "stdafx.h"
#include "form1.h"
#include <math.h>

namespace Tester {

System::Void Form1::button1_Click(System::Object^  sender, System::EventArgs^  e) {
    double sigma = 0.5;
    double sigma2 = sigma * sigma;

    Point origin;
    origin.X = (int)(3 * sigma) + 1;
    origin.Y = origin.X;
    int width = 2 * origin.X + 1;
    int height = width;

    
    array<double,2>^ psf = gcnew array<double,2>(width, height);

    double integral = 0.0;
    for (int x = 0; x < width; x++)
    {
        int x1 = x - origin.X;
        for (int y = 0; y < height; y++)
        {
            int y1 = y - origin.Y;
            double radius = sqrt((double)(x1 * x1 + y1 * y1));
            double value = exp(-0.5 * radius / sigma2);
            integral += value;
            psf[x, y] = value;
        }
    }

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            psf[x,y] /= integral;
        }
    }

    System::Drawing::Size size;
    size.Width = 5;
    size.Height = 5;
    
    array<double,2>^ image = gcnew array<double,2>(size.Width, size.Height);

    image[3, 3] = 1.0;

    FftwConvolver::FftwConvolver^ convolver = gcnew FftwConvolver::FftwConvolver(size, psf, origin);

    array<double,2>^ result = convolver->Convolve(image);
}
}