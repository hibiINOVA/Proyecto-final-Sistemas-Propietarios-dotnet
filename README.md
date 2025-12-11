# Plataforma Web API para Gestión y Cotización de Impresiones 3D
## Descripción del Proyecto

El proyecto consiste en una Plataforma Web API diseñada para automatizar la gestión de recursos de impresión 3D (usuarios, impresoras, filamentos) y la generación de cotizaciones automáticas y precisas a partir de archivos G-code.

El propósito principal es solucionar el problema de calcular manualmente los costos reales de impresión (filamento, energía, desgaste, margen de error e impuestos). La API estandariza y agiliza este proceso, optimizando la rentabilidad y reduciendo errores humanos, lo cual es ideal tanto para el uso personal avanzado como para talleres de impresión medianos.

## Funcionalidades Principales

La plataforma se centra en dos áreas clave: la gestión de recursos (CRUDS) y el cálculo de cotizaciones automáticas.

1. Gestión y Registro de Recursos (CRUD)

Permite realizar operaciones de Creación, Lectura, Actualización y Eliminación (CRUD) sobre las siguientes entidades:

    Usuario

    Impresora

    Filamento

    Cotización

Las entidades Usuario, Impresora y Filamento almacenan imágenes y sus rutas correspondientes en la base de datos.

2. Carga, Análisis y Cálculo de Cotizaciones

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

Endpoints Implementados y Relevancia

Los siguientes son los endpoints principales que componen la funcionalidad de la API. La base para las rutas es http://localhost:5292/api/.

Nombre del Endpoint,Ruta Completa,Método HTTP,Descripción y Relevancia
Login,/auth/login,POST,"Permite iniciar sesión y genera un token JWT , esencial para acceder a las rutas protegidas y garantizar la seguridad del sistema."
Crear Usuario,/usuario,POST,"Permite el registro de nuevos usuarios en el sistema , la única ruta de gestión de recursos no protegida."
Crear Filamento,/filamento/{IdUsuario},POST,"Registra un nuevo filamento asociado a un usuario. Relevante para el cálculo de costos, ya que provee el PrecioPorKg."
Crear Impresora,/impresora/{IdUsuario},POST,"Registra una impresora 3D asociada a un usuario. Crítico para el cálculo de costos, ya que provee el CostoDeLuzPorHora y el Precio para el desgaste."
Crear Cotización,/impresora/{IdUsuario}/{IdImpresora}/{IdFilamento},POST,"Endpoint central del proyecto. Genera la cotización a partir de un archivo G-code , los datos de la impresora y el filamento seleccionados. Implementa la lógica de análisis y cálculo financiero para la automatización."
