using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalModelamientoMatematico
{
    internal class EulerMejorado : Algoritmo
    {        public override List<(double x, double y)> Run(Func<double, double, double> f,
                                                      double x0, double y0, double h, double xFinal)
        {
            Validar(x0, h, xFinal, out int pasos);

            var puntos = new List<(double x, double y)>(pasos + 1) { (x0, y0) };
            double y = y0;

            for (int i = 0; i < pasos; i++)
            {
                double x = x0 + i * h;
                double k1 = f(x, y);
                double yPred = y + h * k1;               // predictor (Euler)
                double k2 = f(x + h, yPred);             // pendiente en el punto predicho

                y += h / 2.0 * (k1 + k2);                // corrector (promedio de pendientes)
                puntos.Add((x0 + (i + 1) * h, y));
            }
            return puntos;
        }

        // Versión vectorial del mismo esquema.
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

                var yPred = new double[m];
                for (int j = 0; j < m; j++)
                    yPred[j] = y[j] + h * k1[j];         // predictor (Euler)

                var k2 = f(x + h, yPred);

                var yn = new double[m];
                for (int j = 0; j < m; j++)
                    yn[j] = y[j] + h / 2.0 * (k1[j] + k2[j]);   // corrector
                y = yn;
                puntos.Add((x0 + (i + 1) * h, (double[])y.Clone()));
            }
            return puntos;
        }
    }
}
