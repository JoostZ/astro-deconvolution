#pragma once

using namespace System;
using namespace System::Drawing;

namespace FftwConvolver {
    public ref class KernelData
    {
    public:
        KernelData(array<double,2>^ data, Point origin) :
          m_data(data), m_origin(origin)
        {
        }

        property array<double, 2>^ Data {
            array<double, 2>^ get() {
                return m_data;
            }
        }

        property Point Origin {
            Point get() {
                return m_origin;
            }
        }
    private:
        array<double, 2>^ m_data;
        Point m_origin;
    };
}
