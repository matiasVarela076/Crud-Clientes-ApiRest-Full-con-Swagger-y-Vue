# API de Gestión de Clientes - Intuit Challenge

## Descripción
API REST desarrollada en .NET 8 con Entity Framework Core para gestionar un ABMC (Alta, Baja, Modificación, Consulta) de clientes. La API se conecta a SQL Server con autenticación de Windows y proporciona endpoints para crear, leer, actualizar, eliminar (borrado lógico) y buscar clientes.

## Requisitos Previos
- .NET 8 SDK instalado
- SQL Server 2019 o superior
- Postman o similar para testear los endpoints

## Configuración

### 1. Crear la Base de Datos
Ejecutar el script SQL en SQL Server Management Studio:
```
Scripts/Creacion_BaseDeDatos_ClientesDB.sql
```

Este script:
- Crea la base de datos `ClientesDB`
- Crea la tabla `Clientes` con todos los campos requeridos
- Inserta 5 registros de prueba

### 2. Configuración de Conexión
```
Server: localhost
Database: ClientesDB
Autenticación: Windows (Integrated Security)
```

La API utiliza autenticación de Windows. La cadena de conexión está configurada en `Program.cs`.


## Campos de la Tabla Clientes

| Campo | Tipo | Obligatorio | Descripción |
|-------|------|-------------|-------------|
| ID | int | Sí | Identificador único (auto-incremento) |
| NOMBRE | string(100) | Sí | Nombre del cliente |
| APELLIDO | string(100) | Sí | Apellido del cliente |
| FECHA_NACIMIENTO | datetime | No | Fecha de nacimiento |
| CUIT | string(20) | Sí | CUIT único (validación de 11 dígitos) |
| DOMICILIO | string(200) | No | Dirección del cliente |
| TELEFONO | string(20) | Sí | Teléfono celular |
| EMAIL | string(100) | Sí | Email válido |
| FECHA_CREACION | datetime | Sí | Fecha de creación (auto-generada) |
| FECHA_MODIFICACION | datetime | No | Fecha de última modificación |
| ACTIVO | bit | Sí | Estado del cliente (true=activo, false=eliminado) |

## Endpoints

### 1. GetAll - Obtener todos los clientes activos
```
GET /api/clientes
```
**Nota:** Solo devuelve clientes con `ACTIVO = true`

**Respuesta exitosa (200):**
```json
[
  {
    "id": 1,
    "nombre": "Juan",
    "apellido": "Pérez",
    "fecha_nacimiento": "1985-03-15T00:00:00",
    "cuit": "20-12345678-9",
    "domicilio": "Calle Principal 123",
    "telefono": "1123456789",
    "email": "juan.perez@email.com",
    "fecha_creacion": "2025-10-23T00:41:00",
    "fecha_modificacion": null,
    "activo": true
  }
]
```

### 2. Get by ID - Obtener cliente por ID
```
GET /api/clientes/{id}
```
**Ejemplo:** `GET /api/clientes/1`

**Respuesta exitosa (200):**
```json
{
  "id": 1,
  "nombres": "Juan",
  "apellidos": "Pérez",
  ...
}
```

**Respuesta error (404):**
```json
{
  "error": "Cliente con ID 999 no encontrado"
}
```

### 3. Search - Buscar clientes por nombre
```
GET /api/clientes/search/{nombre}
```
**Ejemplo:** `GET /api/clientes/search/Juan`

Busca clientes cuyo nombre o apellido contenga el texto especificado.

**Respuesta exitosa (200):**
```json
[
  {
    "id": 1,
    "nombres": "Juan",
    "apellidos": "Pérez",
    ...
  }
]
```

### 4. Insert - Crear nuevo cliente
```
POST /api/clientes
Content-Type: application/json
```

**Body:**
```json
{
  "nombres": "Pedro",
  "apellidos": "Sánchez",
  "fechaNacimiento": "1995-06-18",
  "cuit": "20-44444444-4",
  "domicilio": "Calle Este 987",
  "telefonoCelular": "1144444444",
  "email": "pedro.sanchez@email.com"
}
```

**Respuesta exitosa (201):**
```json
{
  "id": 6,
  "nombres": "Pedro",
  "apellidos": "Sánchez",
  ...
}
```

**Validaciones:**
- Nombres, Apellidos, CUIT, TelefonoCelular y Email son obligatorios
- CUIT debe tener 11 dígitos
- Email debe ser válido
- Teléfono debe tener al menos 7 dígitos
- CUIT debe ser único (no puede haber duplicados)

### 5. Update - Actualizar cliente
```
PUT /api/clientes/{id}
Content-Type: application/json
```

**Body:**
```json
{
  "id": 1,
  "nombres": "Juan Carlos",
  "apellidos": "Pérez García",
  "fechaNacimiento": "1985-03-15",
  "cuit": "20-12345678-9",
  "domicilio": "Calle Principal 123 - Apartamento 5",
  "telefonoCelular": "1123456789",
  "email": "juan.perez.nuevo@email.com"
}
```

**Respuesta exitosa (200):**
```json
{
  "mensaje": "Cliente actualizado correctamente",
  "cliente": {
    "id": 1,
    "nombres": "Juan Carlos",
    ...
  }
}
```

**Validaciones:**
- El ID en la URL debe coincidir con el ID en el body
- El cliente debe existir y estar activo
- Se aplican las mismas validaciones que en Insert
- No se puede cambiar el CUIT a uno que ya existe en otro cliente activo

### 6. Delete - Eliminar cliente (borrado lógico)
```
DELETE /api/clientes/{id}
```
**Ejemplo:** `DELETE /api/clientes/1`

**Respuesta exitosa (200):**
```json
{
  "mensaje": "Cliente eliminado correctamente",
  "id": 1
}
```
## Validaciones Implementadas

Todas las validaciones están en la clase `ClientesHelper`.

### Campos Obligatorios
- **NOMBRE**: Obligatorio, máximo 100 caracteres
- **APELLIDO**: Obligatorio, máximo 100 caracteres
- **CUIT**: Obligatorio, máximo 20 caracteres
- **TELEFONO**: Obligatorio, máximo 20 caracteres
- **EMAIL**: Obligatorio, máximo 100 caracteres

**Códigos HTTP utilizados:**
- `200 OK` - Solicitud exitosa
- `201 Created` - Recurso creado exitosamente
- `400 Bad Request` - Datos inválidos o validación fallida
- `404 Not Found` - Recurso no encontrado
- `500 Internal Server Error` - Error del servidor

## Logging

Los logs se muestran en la consola durante el desarrollo y pueden configurarse para escribirse en archivos o sistemas externos en producción.

La API estará disponible en:

- **HTTP:** `http://localhost:5204`

- **swagger:** `http://localhost:5204/swagger`

### Seguridad
- Validación de entrada en todos los endpoints
- Prevención de duplicados (CUIT único)
- Borrado lógico para mantener historial
- Manejo seguro de excepciones sin exponer detalles internos

## Notas Importantes

- La API utiliza **autenticación de Windows** para conectarse a SQL Server
- CORS está habilitado para todas las solicitudes (AllowAll)
- Los datos de prueba se cargan automáticamente al ejecutar el script SQL
- La API es stateless y puede escalarse horizontalmente
- El borrado es siempre lógico, nunca físico. Ya que al usar un cliente, lo damo de baja con Activo pero queda persistiendo en la base de atos 
- Agregue los campos fecha creación y fecha modificación para saber cuando un cliente fue dado de alta, y modificacion para saber cuando se dio   de baja
- El campo activo se usa para saber si un cliente esta dado de baja o no
- Para darlo de alta nuevamente se debe usar el update: 
UPDATE CLIENTES
SET ACTIVO = 1
WHERE ID = ID_CLIENTE;

# Frontend - Gestión de Clientes con vue

Frontend en **Vue 3** con **Axios** para consumir la API REST de gestión de clientes.

### 1. Instalar node y dependencias

```bash
npm install
```

### 2. Ejecutar en desarrollo

```bash
npm run dev
```

El frontend estará disponible en: **http://localhost:3000**

### 3. Build para producción

```bash
npm run build
```

## 🔧 Configuración

### Variables de Entorno

Edita el archivo `.env`:

```env
VITE_API_URL=http://localhost:5204/api


