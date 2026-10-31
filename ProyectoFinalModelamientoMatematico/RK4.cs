using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalModelamientoMatematico
{
    public class RK4 : Algoritmo
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
                double k1 = f(x, y);
                double k2 = f(x + h / 2.0, y + h / 2.0 * k1);
                double k3 = f(x + h / 2.0, y + h / 2.0 * k2);
                double k4 = f(x + h, y + h * k3);

                y += h / 6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
                puntos.Add((x0 + (i + 1) * h, y));
            }
            return puntos;
        }

      
        public override List<(double x, double[] y)> RunSistema(Func<double, double[], double[]> f,
                                                       double x0, double[] y0, double h, double xFinal)
        {
            Validar(x0, h, xFinal, out int pasos);
            int m = y0.Length;

            double[] Mas(double[] a, double[] k, double c)
            {
                var r = new double[m];
                for (int j = 0; j < m; j++) r[j] = a[j] + c * k[j];
                return r;
            }

            var puntos = new List<(double x, double[] y)>(pasos + 1) { (x0, (double[])y0.Clone()) };
            double[] y = (double[])y0.Clone();

            for (int i = 0; i < pasos; i++)
            {
                double x = x0 + i * h;
                var k1 = f(x, y);
                var k2 = f(x + h / 2.0, Mas(y, k1, h / 2.0));
                var k3 = f(x + h / 2.0, Mas(y, k2, h / 2.0));
                var k4 = f(x + h, Mas(y, k3, h));

                var yn = new double[m];
                for (int j = 0; j < m; j++)
                    yn[j] = y[j] + h / 6.0 * (k1[j] + 2.0 * k2[j] + 2.0 * k3[j] + k4[j]);
                y = yn;
                puntos.Add((x0 + (i + 1) * h, (double[])y.Clone()));
            }
            return puntos;
        }

        private static void Validar(double x0, double h, double xFinal, out int pasos)
        {
            if (h <= 0) throw new ArgumentException("El paso h debe ser mayor que 0.");
            if (xFinal <= x0) throw new ArgumentException("xFinal debe ser mayor que x0.");
            pasos = (int)Math.Round((xFinal - x0) / h);
            if (pasos < 1) pasos = 1;
        }
    }
}
