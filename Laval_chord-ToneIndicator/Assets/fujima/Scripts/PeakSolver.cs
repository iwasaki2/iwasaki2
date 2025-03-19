using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class PeakSolver : MonoBehaviour
{
    Matrix<double> m = Matrix<double>.Build.Dense(3,3);
    Vector<double> a = Vector<double>.Build.Dense(3);
    Vector<double> c = Vector<double>.Build.Dense(3);

    void Start()
    {
        Vector<double> x = Vector<double>.Build.Dense(10);
        Vector<double> y = Vector<double>.Build.Dense(10);
        for(int i=0; i<10; i++)
        {
            x[i] = i;
            y[i] = 2*i*i - 4*i + 3;
        }
        Debug.Log(GetPeak(x,y));
    }

    double GetPeak( Vector<double> x, Vector<double> y )
    {
        double x4, x3, x2, x1, x0;
        double x2y, x1y, x0y;
        x4 = x3 = x2 = x1 = x0 = 0;
        x2y = x1y = x0y = 0;

        for(int i=0; i<x.Count; i++)
        {
            double tmp = 1;
            x0 += tmp;
            x0y += tmp*y[i];

            tmp *= x[i];
            x1 += tmp;
            x1y += tmp*y[i];

            tmp *= x[i];
            x2 += tmp;
            x2y += tmp*y[i];
            
            tmp *= x[i];
            x3 += tmp;
            
            tmp *= x[i];
            x4 += tmp;
        }

        m[0,0] = x4;
        m[1,0] = m[0,1] = x3;
        m[2,0] = m[1,1] = m[0,2] = x2;
        m[2,1] = m[1,2] = x1;
        m[2,2] = x0;

        a[0] = x2y;
        a[1] = x1y;
        a[2] = x0y; 

        m.Inverse().Multiply(a, c);
        Debug.Log($"{c[0]:0.00} x^2 + {c[1]:0.00} x + {c[2]:0.00}");
        double peak = -c[1]/(2*c[0]);
        double peakvalue = c[0]*peak*peak + c[1]*peak + c[2];
        Debug.Log($"Peak {peakvalue:0.00} at x={peak:0.00}");
        return peak;
    }

}