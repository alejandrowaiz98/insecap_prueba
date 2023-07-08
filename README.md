Hi, this project is an technical exam for INSECAP antofagasta!

Antes que nada, asegurese de tener Visual Studio instalado en su computador.

Luego, utilice 'git clone' en este proyecto y copielo en su carpeta local de preferencia (La solucion viene adjunta en el repo).

Al tener la carpeta descargada, abrala con Visual Studio y en "Explorador de soluciones" (por defecto a la derecha de la pantalla) haga
doble click en 'insecap_prueba.sln'.

Tras tener abierta la solucion, deberá conectarse a su base de datos SQL (Para este proyecto se utilizó SQL SERVER).

Al tener una conexion abierta ejecute el siguiente script en su base de datos:

CREATE TABLE students (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    rut VARCHAR (100) NOT NULL,
    name VARCHAR (150) NOT NULL UNIQUE,
    birthday VARCHAR(20) NOT NULL,
    gender VARCHAR(100) NOT NULL,
    courses VARCHAR(20) NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO students (rut, name, birhtday, gender)
VALUES
('19711806-7','Alejandro Waiz', '22/01/1998', 'masculino'),
('19953614-1','Lissette Vergara', '09/03/1998', 'femenino'),
('20623845-6','Arturo Prat', '03/04/1848', 'masculino'),
('25431685-2','Demi Lovato', '20/08/1992', 'no binario'),
('13524987-k','Diego Vera', '13/01/1997', 'omit')


Esto le permitirá crear y poblar la base de datos de Estudiantes. Luego, ejecute el siguiente script:

CREATE TABLE courses (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    code VARCHAR (100) NOT NULL,
    name VARCHAR (150) NOT NULL UNIQUE,
    classroom VARCHAR(20) NOT NULL,
    teacher VARCHAR(100) NOT NULL,
    bimester INT NOT NULL,
    year INT NOT NULL 
);

INSERT INTO courses (code, name, classroom, teacher, bimester, year)
VALUES
('A101','Epistemologia del frontend', 'F-14', 'Juan Perez','2','2023'),
('B202','Fenomenologia del backend', 'C-34', 'Luis Lopez','1','2023'),
('C303','MVC y la metafisica', 'F-24', 'Pablo Diaz','3','2023'),
('E404','Epifenomenos del POO', 'C-24', 'Lorenzo Cisterna','4','2023')

Con esto habrá creado y poblado la BBDD courses. Ahora está listo para ejecutar el programa. Para esto haga click en simbolo verde de Inicio en
el cuadrante central superior de la pantalla. 

Como podrá ver, esta pagina está creada utilizando Visual Studio como IDE y ASP.net con C# como base principal (entorno/lenguaje respectivamente). SQL SERVER en su licencia libre para desarrolladores ha sido la elección para crear la BBDD por su simpleza y eficacia y juntos se complementan en un patrón de arquitectura MVC (Modelo vista controlador) tradicional, esto ya que gracias a las paginas Razor se puede mantener Frontend y Backend en un mismo proyecto. Para la vista se han utilizado tags de HTML (lenguaje de etiquetado) y la lógica interna se administra con C#. 

Consideraciones importantes!!

Como se dará cuenta al abrir el proyecto, esto es una aplicacion básica y por esto he omitido varios detalles por simpleza pero que deben ser mencionados para que no se sigan algunas practicas poco recomendables dentro del codigo.

1.- Por cada llamada a una ruta HTTP se crea una nueva conexion con la BBDD. Idealmente deberia crearse un singleton de la conexion y, mediante un metodo getter, obtener dicha variable a través del proyecto, esto para reducir la cantidad de errores que pueden ser provocador por conectarse varias veces.

2.- Al ser un proyecto con pocas paginas se ha utilizado la libreria principal de C# para manejar las rutas HTTP. Idealmente deberian manejarse mediante un framework externo que facilite el enrutado y, al obtener y administrar todas las rutas, será mas facil poder implementar middlewares necesarios según lo pida el negocio (redirigir un 404, manejar un 400/500, etc)

3.- La conexion a la BBDD se ha escrito en duro en el codigo por simpleza, pero esto JAMAS deberia ser de esta forma. Idealmente deberian obtenerse las variables desde un .env a salvo de los repositorios publicos.

4.- Este proyecto, a pesar de implementar los tags y las clases para utilizar CSS, no se implementaron, esto ha sido principalmente por tiempo, pero a quien no le gusta que su pagina se vea bonita? Agregar CSS es un buen recurso cuando hablamos de UI/UX.

5.- Idealmente la comunicacion entre frontend y backend deberia ser mediante JSON o XML, pero al ser una pequeña pagina con MVC se ha omitido este intermediario en la comunicacion por la simpleza de acceder directamente a los campos requeridos. Es una buena practica desacoplar ambos mundos en caso de que hayan cambios de diseño en alguno de estos.

6.- Al ser una pagina pequeña con resultados comunes he omitido el manejo de las excepciones, esto ya que el funcionamiento de la pagina es "pretty straight-forward". Por favor, siempre manejen las excepciones. SIEMPRE.

Ahora... entrando hacia la imagen, este es un modelo relacional de entidades (o MER para los amigos).

En detalle:

La clase es nuestra entidad esencial, ya que ese es el negocio principal, las clases, por lo que será la base de todas las relaciones existentes. Ahora, sabemos que estas clases existen dentro de un espacio temporal, por lo que existe la tabla Bimestre para darle un vinculo temporal. Luego, para poder llevar a cabo una clase es necesario una sala (sino, a sentarse en el patio...) pero, por planteamiento del problema, sabemos que no todas las salas sirven para hacer la misma clase, por lo que ambas tablas se relacionan mediante la tabla 'Disponibilidad', el cual utiliza como clave foranea las claves primarias de ambas tablas. Además, esta tabla 'Disponibilidad' cubre una segunda necesidad que es desacoplar las tablas 'Cursos' y 'Salas', esto ya que entre estas existe una relación de muchos a muchos y por lo general las BBDD no permiten esta relacion directa ya que, al momento de buscar buscar un dato en particular, no habrá un identificador unico y esto llevaria a varios resultados, lo cual es malo si buscamos, por ejemplo, una factura unica de un cliente. Dicho esto, complementamos con el campo "disponibilidad" para saber si una sala está disponible para ser usada y "factibilidad" para saber si es posible usarla. Ahora, las salas pueden tener equipamientos, y como estos son varios los he separado en una tabla aparte con una relacion uno a muchos, permitiendo desacoplar la sala del equipamiento que requiera. Ahora, un curso puede tener varios profesores y viceversa, por lo que esta relacion muchos a muchos se desacopla con 'Asignacion_curso', permitiendo asi saber el estado de asignacion de un profesor X con un curso Y. Por ultimo lo esencial; Las clases son tomadas por alumnos, y una clase puede ser tomada por varios alumnos y un alumno puede tomar varias clases, si bien no dentro del mismo bimestre por su condicion de horario unico, si a través de varios semestres, y para tener un respaldo de los alumnos que han estado dentro del sistema (con fines comerciales, estadisticos, etc) se crea la tabla 'Alumno' y se desacopla de 'Clases' con la tabla 'Inscripcion', ya que así se puede tener un rastreo con un identificador unico de un alumno X en una clase Y en un bimestre Z. 
 
