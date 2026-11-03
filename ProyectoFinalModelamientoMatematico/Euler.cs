using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalModelamientoMatematico
{
    internal class Euler : Algoritmo
    {
        public override List<(double x, double y)> Run(Func<double, double, double> f,
                                                      double x0, double y0, double h, double xFinal)
        {
            Validar(x0, h, xFinal, out int pasos);

            var puntos = new List<(double x, double y)>(pasos + 1) { (x0, y0) };
            double y = y0;

            for (int i = 0; i < pasos; i++)
            {
                double x = x0 + i * h;                   // evita acumular error de redondeo
                y += h * f(x, y);
                puntos.Add((x0 + (i + 1) * h, y));
            }
            return puntos;
        }
        public override List<(double x, double[] y)> RunSistema(Func<double, double[], double[]> f,
                                                               double x0, double[] y0, double h, double xFinal)
        {
            Validar(x0, h, xFinal, out int pasos);
            int m = y0.Length;

            var puntos = new List<(double x, double[] y)>(pasos + 1) { (x0, (double[])y0.Clone()) };
            double[] y = (double[])y0.Clone();

            for (int i = 0; i < pasos; i++)
            {
                double x = x0 + i * h;
                var k1 = f(x, y);

                var yn = new double[m];
                for (int j = 0; j < m; j++)
                    yn[j] = y[j] + h * k1[j];
                y = yn;
                puntos.Add((x0 + (i + 1) * h, (double[])y.Clone()));
            }
            return puntos;
        }
    }
}
