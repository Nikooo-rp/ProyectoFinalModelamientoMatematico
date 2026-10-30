La idea para hacer esto funcionar es la siguiente:

Como se va a trabajar en distintos algoritmos por separado, __lo ideal sería que cada uno sea una clase con una función RUN()__, llamada igual dentro de cada clase, y con parámetros para cada valor de entrada necesario para aplicar el algoritmo. Adicionalmente,
RUN() deberá devolver los resultados como un array de tuplas de x e y de cada punto calculado. Un array de tuplas se declara asi: `(int x, int y)[] puntos = new[] {(0, 0) (1, 1)}` 

Las clases son las siguientes:
- Program: Con el main(), pondremos aquí un menu para elegir el algoritmo a utilizar, instanciarlo y utilizarlo. También se encargará de imprimir en consola los resultados de cada  (Niko)
- Euler.
- EulerMejorado.
- RK4.
- Multipaso.

Euler y EulerMejorado muy seguramente deban de tener algoritmos separados, dado que el paso extra de EulerMejorado va en la mitad del Euler y no creo que sea posible reutilizar el código de Euler haciendo una llamada a esa clase,
de todas formas, se puede simplemente copiar y pegar y añadir el paso extra.

RK4 es seguramente el más largo de todos.

Multipaso es el más fácil, puede simplemente llamar al RUN() de RK4 con el paso indicado para generar los primeros cuatro datos, y luego operar encima de eso.
