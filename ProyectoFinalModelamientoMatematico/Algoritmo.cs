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
    }
}
