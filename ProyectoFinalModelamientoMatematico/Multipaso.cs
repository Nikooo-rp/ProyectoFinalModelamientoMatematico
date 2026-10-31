using ProyectoFinalModelamientoMatematico;

internal class Multipaso : Algoritmo
{
    public override List<(double x, double y)> Run(Func<double, double, double> f,
                                                  double x0, double y0, double h, double xFinal)
    {
        Validar(x0, h, xFinal, out int pasos);

        // Con menos de 4 pasos no hay puntos suficientes para el método multipaso.
        if (pasos <= 3) return new RK4().Run(f, x0, y0, h, xFinal);

        // Punto inicial + 3 puntos adicionales calculados con RK4 (4 puntos en total).
        List<(double x, double y)> puntos = new RK4().Run(f, x0, y0, h, x0 + 3 * h);

        // Derivadas f_k = f(x_k, y_k), calculadas una sola vez por punto.
        var fs = new List<double>(pasos + 1);
        foreach (var p in puntos) fs.Add(f(p.x, p.y));

        for (int n = 3; n < pasos; n++)
        {
            double xn1 = x0 + (n + 1) * h;
            double yn = puntos[n].y;

            // Predictor
            double yPred = yn + h / 24.0 * (55 * fs[n] - 59 * fs[n - 1] + 37 * fs[n - 2] - 9 * fs[n - 3]);

            // Corrector
            double fPred = f(xn1, yPred);
            double yCorr = yn + h / 24.0 * (9 * fPred + 19 * fs[n] - 5 * fs[n - 1] + fs[n - 2]);

            puntos.Add((xn1, yCorr));
            fs.Add(f(xn1, yCorr)); // derivada en el nuevo punto, para los pasos siguientes
        }
        return puntos;
    }

    public override List<(double x, double[] y)> RunSistema(Func<double, double[], double[]> f,
                                                            double x0, double[] y0, double h, double xFinal)
    {
        Validar(x0, h, xFinal, out int pasos);

        if (pasos <= 3) return new RK4().RunSistema(f, x0, y0, h, xFinal);

        int m = y0.Length;
        List<(double x, double[] y)> puntos = new RK4().RunSistema(f, x0, y0, h, x0 + 3 * h);

        var fs = new List<double[]>(pasos + 1);
        foreach (var p in puntos) fs.Add(f(p.x, p.y));

        for (int n = 3; n < pasos; n++)
        {
            double xn1 = x0 + (n + 1) * h;
            double[] yn = puntos[n].y;

            // Predictor
            double[] yPred = new double[m];
            for (int j = 0; j < m; j++)
                yPred[j] = yn[j] + h / 24.0 * (55 * fs[n][j] - 59 * fs[n - 1][j] + 37 * fs[n - 2][j] - 9 * fs[n - 3][j]);

            // Corrector
            double[] fPred = f(xn1, yPred);
            double[] yCorr = new double[m];
            for (int j = 0; j < m; j++)
                yCorr[j] = yn[j] + h / 24.0 * (9 * fPred[j] + 19 * fs[n][j] - 5 * fs[n - 1][j] + fs[n - 2][j]);

            puntos.Add((xn1, yCorr));
            fs.Add(f(xn1, yCorr));
        }
        return puntos;
    }
}