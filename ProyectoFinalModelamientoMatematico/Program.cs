using ScottPlot;
using System.Numerics;

namespace ProyectoFinalModelamientoMatematico
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            while (true)
            {
                List<(double x, double y)> puntos = new List<(double x, double y)>(); // Para almacenar los puntos obtenidos y poderlos dibujar.
                static double F(double x, double y) => -2 * y + x; // Función a aproximar (y'), cambiar según el problema a resolver.

                Console.Clear();
                Console.WriteLine("Sistema de aproximación de ecuaciones por métodos numéricos\nEscoja un algoritmo: \n1. Euler \n2. Euler mejorado \n3. Runge-Kutta de cuarto orden \n4. Multipaso");

                string? op;
                op = Console.ReadLine();

                // Todos necesitan la funcion f(x,y), el punto inicial (x0,y0), el valor de h y el valor final de x. Multipaso necesita de RK4.

                Console.WriteLine("Ingresa el valor de x0: ");
                double x0 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Ingresa el valor de y0: ");
                double y0 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Ingresa el valor del paso (h): ");
                double h = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Ingresa el valor final de x: ");
                double xf = Convert.ToDouble(Console.ReadLine());

                // Los valores se envían en el orden x0, y0, h, xf, F(x,y) a la función del método seleccionado. Se espera recibir una lista de puntos (x,y).

                if (h <= 0 || xf <= x0)
                {
                    Console.WriteLine("Valores inválidos. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (op)
                    {
                        case "0":
                            // testeo, añade puntos de prueba para dibujar un gráfico.
                            List<double> xs = new List<double>();
                            for (int i = 0; i < MathF.Floor((float)xf / (float)h); i++)
                            {
                                xs.Add(x0 + i * h);
                            }
                            List<double> ys = new List<double>();
                            Random r = new Random();
                            for (int i = 0; i < MathF.Floor((float)xf / (float)h); i++)
                            {
                                ys.Add(y0 + r.NextDouble() * 10);
                            }
                            for (int i = 0; i < xs.Count; i++)
                            {
                                puntos.Add((xs[i], ys[i]));
                            }
                            break;
                        case "1":
                            //Euler e = new Euler();
                            //puntos = e.RUN(x0, y0, h, xf, F);
                            break;
                        case "2":
                            //EulerMejorado em = new EulerMejorado();
                            //puntos = em.RUN(x0, y0, h, xf, F);
                            break;
                        case "3":
                            //RK4 rk4 = new RK4();
                            //puntos = rk4.RUN(x0, y0, h, xf, F);
                            break;
                        case "4":
                            //Multipaso mp = new Multipaso();
                            //puntos = mp.RUN(x0, y0, h, xf, F);
                            break;
                        default:
                            Console.WriteLine("Opción inválida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al implementar el algoritmo: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                // Poner los datos en una tabla
                Console.WriteLine("Puntos obtenidos:");
                Console.WriteLine("x\t\t\ty");
                foreach (var punto in puntos)
                {
                    string x4 = punto.x.ToString("F4") == "NaN" ? "0" : punto.x.ToString("F4"); // Si el valor es NaN, se reemplaza por 0 para que no se vea mal en la tabla.
                    string y4 = punto.y.ToString("F4") == "NaN" ? "0" : punto.y.ToString("F4");
                    Console.WriteLine($"{x4}\t\t\t{y4}");
                }

                // Dibujar los puntos obtenidos en un gráfico.
                try
                {
                    string dir = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProyectoFinalModelamientoMatematico")).ToString();
                    Plot plot = new Plot();
                    double[] xs = puntos.Select(p => p.x).ToArray();
                    double[] ys = puntos.Select(p => p.y).ToArray();

                    plot.Add.Scatter(xs, ys);

                    string filePath = Path.Combine(dir, "grafico.png");
                    int i = 1;
                    while (File.Exists(filePath))
                    {
                        filePath = Path.Combine(dir, $"grafico_{i}.png");
                        i++;
                    }
                    plot.SavePng(filePath, 800, 600);
                    Console.WriteLine($"Gráfico guardado en la carpeta {dir} \nPresione cualquier tecla para continuar");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al dibujar el gráfico: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
            }
        }
    }
}
