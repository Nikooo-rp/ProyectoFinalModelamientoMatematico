using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalModelamientoMatematico
{
    public abstract class Algoritmo
    {
        public abstract List<(double x, double y)> Run(Func<double, double, double> f,
                                       double x0, double y0, double h, double xFinal);
        public abstract List<(double x, double[] y)> RunSistema(Func<double, double[], double[]> f,
                                                double x0, double[] y0, double h, double xFinal);

        protected static void Validar(double x0, double h, double xFinal, out int pasos)
        {
            if (h <= 0) throw new ArgumentException("El paso h debe ser mayor que 0.");
            if (xFinal <= x0) throw new ArgumentException("xFinal debe ser mayor que x0.");
            pasos = (int)Math.Round((xFinal - x0) / h);
            if (pasos < 1) pasos = 1;
        }
    }
}
