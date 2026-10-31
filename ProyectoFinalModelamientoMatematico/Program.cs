using ScottPlot;
using System.Globalization;

namespace ProyectoFinalModelamientoMatematico
{
    internal class Program
    {
        // Ecuación única: y' = f(x, y). Cambiar según el problema.
        static double F(double x, double y) => 2 * x - 3 * y + 1;

        // Sistema: Y' = G(x, Y). Cambiar según el problema.
        // Ejemplo: y'' + y = 0  ->  y1 = y, y2 = y'  ->  y1' = y2, y2' = -y1
        static double[] G(double x, double[] y) => new[] { y[1], -y[0] };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sistema de aproximación de ecuaciones por métodos numéricos");
                Console.WriteLine("Tipo de problema:\n1. Ecuación única y' = f(x, y)\n2. Sistema de ecuaciones Y' = G(x, Y)");
                string? tipo = Console.ReadLine();
                if (tipo != "1" && tipo != "2") { Pausa("Opción inválida."); continue; }
                bool esSistema = tipo == "2";

                Console.WriteLine("Escoja un algoritmo: \n1. Euler \n2. Euler mejorado \n3. Runge-Kutta de cuarto orden \n4. Multipaso");
                string? op = Console.ReadLine();

                double x0 = LeerDouble("Ingresa el valor de x0: ");
                double[] y0 = esSistema
                    ? LeerVector("Ingresa y0 (un valor por ecuación, separados por espacios): ")
                    : new[] { LeerDouble("Ingresa el valor de y0: ") };
                double h = LeerDouble("Ingresa el valor del paso (h): ");
                double xf = LeerDouble("Ingresa el valor final de x: ");

                if (h <= 0 || xf <= x0) { Pausa("Valores inválidos."); continue; }

                List<(double x, double[] y)> puntos;
                try
                {
                    puntos = Resolver(op, esSistema, x0, y0, h, xf);
                }
                catch (Exception ex)
                {
                    Pausa($"Error al implementar el algoritmo: {ex.Message}");
                    continue;
                }

                Mostrar(puntos);

                try
                {
                    Graficar(puntos);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al dibujar el gráfico: {ex.Message}");
                }
                Pausa("");
            }
        }

        // Despacha al método numérico elegido y devuelve siempre el formato común.
        static List<(double x, double[] y)> Resolver(string? op, bool esSistema,
                                                     double x0, double[] y0, double h, double xf)
        {
            if (esSistema)
            {
                if (G(x0, y0).Length != y0.Length)
                    throw new ArgumentException("La cantidad de valores de y0 no coincide con el número de ecuaciones de G.");

                switch (op)
                {
                    case "3": return new RK4().RunSistema(G, x0, y0, h, xf);
                    // case "1": return new Euler().RunSistema(G, x0, y0, h, xf);
                    // case "2": return new EulerMejorado().RunSistema(G, x0, y0, h, xf);
                    // case "4": return new Multipaso().RunSistema(G, x0, y0, h, xf);
                    default: throw new NotImplementedException("Método aún no disponible para sistemas.");
                }
            }

            switch (op)
            {
                case "3":
                    return new RK4().Run(F, x0, y0[0], h, xf)
                                    .Select(p => (x: p.x, y: new[] { p.y })).ToList();
                // case "1": return new Euler().Run(F, x0, y0[0], h, xf).Select(p => (x: p.x, y: new[] { p.y })).ToList();
                // case "2": ...
                // case "4": ...
                default: throw new NotImplementedException("Método aún no disponible.");
            }
        }

        static void Mostrar(List<(double x, double[] y)> puntos)
        {
            int m = puntos[0].y.Length;
            Console.WriteLine("Puntos obtenidos:");
            Console.WriteLine("x\t\t\t" + string.Join("\t\t\t", Enumerable.Range(1, m).Select(j => m == 1 ? "y" : $"y{j}")));
            foreach (var p in puntos)
                Console.WriteLine(Fmt(p.x) + "\t\t\t" + string.Join("\t\t\t", p.y.Select(Fmt)));
        }

        static string Fmt(double v) => double.IsNaN(v) ? "0" : v.ToString("F4");

        static void Graficar(List<(double x, double[] y)> puntos)
        {
            string dir = Directory.CreateDirectory(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ProyectoFinalModelamientoMatematico")).FullName;

            int m = puntos[0].y.Length;
            double[] xs = puntos.Select(p => p.x).ToArray();

            // Una curva por componente, en función de x
            Plot plot = new();
            for (int j = 0; j < m; j++)
            {
                double[] yj = puntos.Select(p => p.y[j]).ToArray();
                plot.Add.Scatter(xs, yj).LegendText = m == 1 ? "y" : $"y{j + 1}";
            }
            if (m > 1) plot.ShowLegend();
            plot.XLabel("x");
            plot.YLabel("y");
            plot.SavePng(RutaLibre(dir, "grafico"), 800, 600);

            // Si el sistema es de dos ecuaciones, también el plano de fase (y2 vs y1)
            if (m == 2)
            {
                Plot fase = new();
                fase.Add.Scatter(puntos.Select(p => p.y[0]).ToArray(),
                                 puntos.Select(p => p.y[1]).ToArray());
                fase.Title("Plano de fase");
                fase.XLabel("y1");
                fase.YLabel("y2");
                fase.SavePng(RutaLibre(dir, "fase"), 800, 600);
            }

            Console.WriteLine($"Gráfico(s) guardado(s) en la carpeta {dir}");
        }

        static string RutaLibre(string dir, string nombre)
        {
            string ruta = Path.Combine(dir, $"{nombre}.png");
            for (int i = 1; File.Exists(ruta); i++)
                ruta = Path.Combine(dir, $"{nombre}_{i}.png");
            return ruta;
        }

        // Lectura numérica tolerante: acepta "0.1" y "0,1" sin depender de la configuración regional.
        static bool TryLeer(string? s, out double v) =>
            double.TryParse(s?.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out v);

        static double LeerDouble(string mensaje)
        {
            while (true)
            {
                Console.WriteLine(mensaje);
                if (TryLeer(Console.ReadLine(), out double v)) return v;
                Console.WriteLine("Valor numérico inválido.");
            }
        }

        static double[] LeerVector(string mensaje)
        {
            while (true)
            {
                Console.WriteLine(mensaje);
                var partes = (Console.ReadLine() ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var v = new double[partes.Length];
                bool ok = partes.Length > 0;
                for (int i = 0; ok && i < partes.Length; i++) ok = TryLeer(partes[i], out v[i]);
                if (ok) return v;
                Console.WriteLine("Entrada inválida. Use números separados por espacios.");
            }
        }

        static void Pausa(string mensaje)
        {
            Console.WriteLine($"{mensaje} Presione cualquier tecla para continuar...".Trim());
            Console.ReadKey();
        }
    }
}