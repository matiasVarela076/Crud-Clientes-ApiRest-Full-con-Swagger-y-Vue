# Clientes CRUD

Proyecto completo de gestión de clientes con arquitectura limpia y modular.


## Características Principales

### Backend (ClientesAPI)
- **Arquitectura Limpia**: Separación clara de capas
- **Patrón Repository**: Abstracción del acceso a datos
- **Inyección de Dependencias**: Desacoplamiento de componentes
- **DTOs**: Transferencia de datos tipada
- **Validaciones**: FluentValidation
- **AutoMapper**: Mapeo automático de objetos
- **Swagger**: Documentación interactiva de API
- **CORS**: Configuración para acceso desde frontend

### Frontend (ClientesFrontend)
- **Vue 3**: Framework
- **Composables**: Lógica reutilizable
- **Modular**: Componentes bien organizados
- **Responsive**: Diseño adaptable
- **Axios**: Cliente HTTP con interceptores
- **Vite**: Build tool rápido

## Patrones de Diseño Implementados

1. **Repository Pattern**: Abstracción del acceso a datos
2. **Dependency Injection**: Inyección de dependencias
3. **DTO Pattern**: Transferencia de datos
4. **Service Layer**: Lógica de negocio centralizada
5. **Composable Pattern**: Lógica reutilizable en Vue
6. **Store Pattern**: Gestión de estado global

## Instalación y Ejecución

### Backend
```bash
cd ClienteBackend
dotnet restore
dotnet run
# API disponible en http://localhost:5204
```

### Frontend
```bash
cd ClientesFrontend
npm install
npm run dev
# Frontend disponible en http://localhost:5173
```

## Configuración de Base de Datos

Actualizar la cadena de conexión en `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tu-servidor;Database=ClientesDB;User Id=usuario;Password=contraseña;TrustServerCertificate=true;"
}
```

Ejecutar migraciones:
```bash
dotnet ef database update
```

## API Endpoints

- `GET /api/clientes` - Obtener todos los clientes
- `GET /api/clientes/{id}` - Obtener cliente por ID
- `GET /api/clientes/search/{query}` - Buscar clientes
- `POST /api/clientes` - Crear nuevo cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente (borrado lógico)

## Colección de Postman

Se incluye una colección de Postman con todos los endpoints configurados:

- Ubicación: `/Utils/ClientesAPI.postman_collection.json`
- Incluye ejemplos de solicitud para todas las operaciones CRUD
- Configuración de variables de entorno para la URL base

Para importar en Postman:
1. Abre Postman
2. Haz clic en "Import"
3. Selecciona el archivo `Utils/ClientesAPI.postman_collection.json`
4. Asegúrate de configurar la variable `base_url` en el entorno de Postman con la URL de tu API
- `POST /api/clientes` - Crear nuevo cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente (borrado lógico)

## Documentación

- Swagger: http://localhost:5204/swagger
- Postman: Importar desde `/docs/postman-collection.json`

## Mejoras Implementadas

✅ Separación en capas  
✅ Patrón Repository  
✅ Inyección de dependencias  
✅ DTOs y validaciones  
✅ AutoMapper  
✅ Manejo global de errores  
✅ Frontend modular  
✅ Gestión de estado centralizada  
✅ Composables reutilizables  
✅ Documentación completa  
