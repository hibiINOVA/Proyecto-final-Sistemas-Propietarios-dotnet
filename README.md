# Plataforma Web API para Gestión y Cotización de Impresiones 3D
## Descripción del Proyecto

El proyecto consiste en una Plataforma Web API diseñada para automatizar la gestión de recursos de impresión 3D (usuarios, impresoras, filamentos) y la generación de cotizaciones automáticas y precisas a partir de archivos G-code.

El propósito principal es solucionar el problema de calcular manualmente los costos reales de impresión (filamento, energía, desgaste, margen de error e impuestos). La API estandariza y agiliza este proceso, optimizando la rentabilidad y reduciendo errores humanos, lo cual es ideal tanto para el uso personal avanzado como para talleres de impresión medianos.

## 1.Funcionalidades Principales

La plataforma se centra en dos áreas clave: la gestión de recursos (CRUDS) y el cálculo de cotizaciones automáticas.

### 1.1. Gestión y Registro de Recursos (CRUD)

Permite realizar operaciones de Creación, Lectura, Actualización y Eliminación (CRUD) sobre las siguientes entidades:

    Usuario

    Impresora

    Filamento

    Cotización

Las entidades Usuario, Impresora y Filamento almacenan imágenes y sus rutas correspondientes en la base de datos.

### 1.2. Carga, Análisis y Cálculo de Cotizaciones

Esta es la funcionalidad central que optimiza el proceso de negocio:

Carga de G-code: El cliente sube el archivo G-code mediante una petición multipart/form-data.

Análisis de G-code: El servidor procesa el archivo para extraer datos cruciales, como el tiempo estimado de impresión en horas y la cantidad de filamento usado en gramos.

Cálculo de Costos: Utiliza la información extraída y los datos de la impresora/filamento seleccionados para calcular:

    Costo de Filamento: 1000PrecioPorKg​×gramosUsados 

    Costo de Energía: costoDeLuzPorHora(impresora)×tiempoHoras

    Desgaste de Máquina: Se utiliza una fórmula de ejemplo para la depreciación.

    Margen por Error: Subtotal ×0.15 (15%)

    IVA: Subtotal con margen ×100IVA​ (16% por defecto)

    Total: subtotal+margen+IVA

Seguridad: La API está protegida con Autenticación JWT para todas las rutas privadas, excluyendo login y crear un usuario.

## 1.3.Endpoints Implementados y Relevancia

Los siguientes son los endpoints principales que componen la funcionalidad de la API. La base para las rutas es http://localhost:5292/api/.

La URL base para todas las rutas es http://localhost:5292/api/.

### 1.3.1. Login

    Ruta Completa: /auth/login 

Método HTTP: POST

Descripción: Permite iniciar sesión al sistema mediante credenciales básicas (correo y contraseña). Si son correctas, genera un token JWT necesario para proteger las demás peticiones.

Cuerpo de la Petición (raw JSON):

JSON

{
  "correo": "josemanuel@gmail.com",
  "contrasena":"123456"
}

Ejemplo de Respuesta:

JSON

{
  "token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhMDVhOWIzMS1lZjg1LTRlMGItYTk4Yy1mN2IwMmRkYTA3ZWYiLCJlbWFpbCI6Impvc2VtYW51ZWxAZ21haWwuY29tIiwidXNlcklkIjoiYTA1YTliMzEtZWY4NS00ZTBiLWE5OGMtZjdiMDJkZGEwN2VmIiwiZXhwIjoxNzY0NDc5OTI4LCJpc3MiOiJ0dWFwaSIsImF1ZCI6InR1YXBpX3VzZXJzIn0.QvaNDcRD4oKyOibxIMRY5f6O_xa1aZxaZCH8gCnjUGw"
}

### 1.3.2. Crear Usuario

    Ruta Completa: /usuario 

Método HTTP: POST

Descripción: Permite registrar un nuevo usuario. Recibe nombre, apellido, correo, contraseña y foto. El sistema valida y guarda la información.

Parámetros (form-data):

    Nombre (Text) 

    Apellido (Text)

    Correo (Text)

    Contrasena (Text)

    Foto (File)

Ejemplo de Respuesta:

JSON

{
  "mensaje": "Usuario creado exitosamente",
  "usuario": {
    "id": "23450611-2562-41f3-8001-91cd9505e9e3",
    "nombre": "jose",
    "apellido": "garcia morales",
    "correo": "josemanuel@gmail.com",
    "foto": "/uploads/usuarios/9cf0e05b-bae6-409e-97fe-a6219b920573.jpg"
  }
}

### 1.3.3. Crear Filamento

    Ruta Completa: /filamento/{IdUsuario} 

Método HTTP: POST

Descripción: Permite registrar un nuevo filamento asociado a un usuario mediante su ID. Recibe tipo, marca, color, diámetro, precio por kilogramo y foto.

Parámetros (form-data):

    Tipo (Text) 

    Marca (Text)

    Color (Text)

    Diametro (Text)

    PrecioPorKg (Text)

    Foto (File)

Ejemplo de Respuesta:

JSON

{
  "mensaje": "Filamento creado correctamente",
  "data": {
    "id": "136512ce-afca-4c72-ae7b-9c8528fda8a0",
    "tipo": "PETG",
    "marca": "eSun",
    "color": "Rojo",
    "diametro": 1.75,
    "precioPorKg": 420,
    "foto": "/uploads/filamentosFotos/68b84507-2d69-4580-a374-d7234d76fdfc.png",
    "usuarioId": "a05a9b31-ef85-4e0b-a98c-f7b02dda07ef"
  }
}

### 1.3.4. Crear Impresora

    Ruta Completa: /impresora/{IdUsuario} 

Método HTTP: POST

Descripción: Permite registrar una impresora 3D asociada a un usuario. Recibe modelo, marca, precio, tipo de extrusión, diámetro de la boquilla, velocidad de impresión, costo de luz por hora y foto.

Parámetros (form-data):

    Modelo (Text) 

    Marca (Text)

    Precio (Text)

    TipoExtrusion (Text)

    DiametroBoquilla (Text)

    VelocidadDeImpresion (Text)

    CostoDeLuzPorHora (Text)

    foto (File)

Ejemplo de Respuesta:

JSON

{
  "message": "Impresora creada correctamente",
  "impresora": {
    "id": "d457feb7-98fd-4d40-90e6-ea487772c1b5",
    "modelo": "Ender 3 v3 ke",
    "marca": "Creality",
    "precio": 5500,
    "tipoExtrusion": "Bowden",
    "diametroBoquilla": 0.4,
    "velocidadDeImpresion": 120,
    "costoDeLuzPorHora": 0.8,
    "foto": "/uploads/impresorasFotos/92144935-34ec-4ed8-be23-dd687bc36319.png",
    "usuarioId": "a05a9b31-ef85-4e0b-a98c-f7b02dda07ef"
  }
}

### 1.3.5. Crear Cotización

    Ruta Completa: /cotizacion/{IdUsuario}/{IdImpresora}/{IdFilamento} 

Método HTTP: POST

Descripción: Genera una cotización de los costos de una impresión 3D a partir de un archivo G-code. Analiza el archivo para obtener el tiempo y la cantidad de filamento, y calcula los costos (luz, material, desgaste, margen e IVA).

Parámetros (form-data):

    ArchivoGcode (File) 

    Nombre (Text)

    descripcion (Text)

    Iva (Text)

Ejemplo de Respuesta:

JSON

{
  "mensaje": "Cotización generada correctamente.",
  "cotizacion": {
    "id": "02a6b90d-4c66-429a-b866-03b9630a784e",
    "usuarioId": "a05a9b31-ef85-4e0b-a98c-f7b02dda07ef",
    "impresoraId": "ca044cde-a1c5-4449-bb90-b52766bbdaa6",
    "filamentoId": "45f5d726-6484-4c98-a738-20ab6991ae92",
    "nombre": "cute wolf",
    "descripcion": "llavero de un cute wolf",
    "tiempoHoras": 0.54,
    "filamentoGr": 4.00,
    "costoFilamento": 1.6800,
    "costoLuz": 0.4320,
    "subtotal": 2.225400,
    "margenError15": 0.33381000,
    "costoDesgarteImpresora": 0.113400,
    "iva": 0.41,
    "total": 2.96921000,
    "archivoGcode": "/uploads/gcodes/d1868c19-1c01-49c3-8958-5f22288053bd.gcode"
  }
}

# Instrucciones para ejecutar el proyecto
## Requerimientos del sistema

Asegúrate de tener instalado lo siguiente en tu sistema:

    Lenguaje/Framework: .NET 8.0 SDK (o superior).

    Base de Datos: Docker (para ejecutar el contenedor de SQL Server).

    Dependencias Adicionales:

        Docker

        Git (para clonar el repositorio)

    Nota: El proyecto fue desarrollado y probado en un entorno Linux (Debian GNU/Linux 12 - bookworm) en una Raspberry Pi 5, pero al usar .NET y Docker, debería funcionar sin problemas en Windows, macOS o cualquier otra distribución de Linux.

## 2. Configuración Inicial (Base de Datos y Entorno)
## 2.1. Iniciar el Servidor de Base de Datos (Docker)

La base de datos se ejecuta utilizando la imagen de Azure SQL Edge en un contenedor de Docker.

    Ejecuta el contenedor de SQL Server: Utiliza el siguiente comando para levantar el contenedor. Esto mapea el puerto 1433 de tu máquina al puerto 1433 del contenedor.
    Bash

docker run -d \
--name proyectoSistemasPropietarios \
-e 'ACCEPT_EULA=Y' \
-e 'MSSQL_SA_PASSWORD=P1aSSwOrd!' \
-p 1433:1433 \
mcr.microsoft.com/azure-sql-edge

    Usuario (User Id): sa

    Contraseña (Password): P1aSSwOrd!

    Servidor (Server): localhost,1433

    Nombre de la BD (Database): proyectoImpresion3d

Verificar la conexión (Opcional): Puedes usar el siguiente comando para verificar que la base de datos está corriendo y es accesible:
Bash

    sqlcmd -S localhost,1433 -U sa -P P1aSSwOrd! -C

## 2.2. Configuración de la Conexión (appsettings.json)

La cadena de conexión y la configuración JWT ya están definidas en el archivo appsettings.json.

    Cadena de Conexión: Asegúrate de que la sección DefaultConnection en appsettings.json apunte al servidor Docker:
    JSON

"DefaultConnection": "Server=localhost,1433;Database=proyectoImpresion3d;User Id=sa;Password=P1aSSwOrd!;TrustServerCertificate=True"

Configuración JWT: La configuración para la autenticación está lista para usarse, con un tiempo de expiración de 120 minutos:
JSON

    "Jwt": {
        "Key": "J8f9K2mQ4sA7vT1zN0xP3gR6bW5cH9uE2tY4lS8oB1nD7qF0rV3jL6mP8aT2wX5yZ",
        "Issuer": "tuapi",
        "Audience": "tuapi_users",
        "ExpireMinutes": 120
    }

## 3. Instalación de Dependencias

Una vez que el proyecto esté clonado en tu máquina, navega al directorio principal (proyectoSistemaPropietarios.csproj) e instala todas las dependencias:

    Restaurar Paquetes: Este comando asegura que todos los paquetes NuGet definidos en el archivo .csproj (como Microsoft.EntityFrameworkCore.SqlServer, FluentValidation, y Microsoft.AspNetCore.Authentication.JwtBearer versión 8.0.0) sean descargados e instalados.
    Bash

dotnet restore

(Nota: Los comandos dotnet add package... listados son para el desarrollo inicial; dotnet restore es el comando que el usuario final debe ejecutar. Los archivos code deben generados en slicer como cura o plusaslicer.)

Ejecutar Migraciones de EF Core: Una vez que las dependencias se hayan restaurado, ejecuta las migraciones para crear la base de datos proyectoImpresion3d y sus tablas en el contenedor de SQL Server: (Asume que las migraciones ya existen en el proyecto.)
Bash

    dotnet ef database update

## 4. Comandos para Iniciar el Proyecto

Para iniciar la API, utiliza el siguiente comando en el directorio del proyecto:
Bash

dotnet run

    La API se ejecutará, y los endpoints estarán disponibles en http://localhost:5292/api/.

    El sistema incluye Swagger/OpenAPI para la documentación y prueba de endpoints en desarrollo.

## 5. Consideraciones Especiales

    Archivos Estáticos: La API sirve archivos estáticos (fotos y gcodes) desde el directorio wwwroot. Asegúrate de que este directorio exista y sea accesible.

    CORS: La API tiene configurada una política CORS (AllowAngular) para permitir peticiones desde http://localhost:4200 (para la aplicación frontend de Angular). Si usas otro puerto o un frontend diferente, ajusta esta configuración.

    Validaciones: Se utiliza FluentValidation en todos los endpoints para garantizar la calidad y validez de los datos recibidos.

# 4. Colección de Postman

[Uploading proyecto sistemas propietarios.postman_collection.json…]()
