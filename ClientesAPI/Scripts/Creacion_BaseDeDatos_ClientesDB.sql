-- Crear base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClientesDB')
BEGIN
    CREATE DATABASE ClientesDB;
END
GO

USE ClientesDB;
GO

-- Crear tabla Clientes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
BEGIN
    CREATE TABLE CLIENTES (
        ID INT PRIMARY KEY IDENTITY(1,1),
        NOMBRE NVARCHAR(100) NOT NULL,
        APELLIDO NVARCHAR(100) NOT NULL,
        FECHA_NACIMIENTO DATETIME NULL,
        CUIT NVARCHAR(20) NOT NULL UNIQUE,
        DOMICILIO NVARCHAR(200) NULL,
        TELEFONO NVARCHAR(20) NOT NULL,
        EMAIL NVARCHAR(100) NOT NULL,
        FECHA_CREACION DATETIME DEFAULT GETDATE(),
        FECHA_MODIFICACION DATETIME NULL,
        ACTIVO BIT NOT NULL DEFAULT 1
    );
END
GO

-- Crear índices para mejorar performance
CREATE NONCLUSTERED INDEX IX_Clientes_CUIT ON CLIENTES(CUIT);
CREATE NONCLUSTERED INDEX IX_Clientes_Email ON CLIENTES(EMAIL);
CREATE NONCLUSTERED INDEX IX_Clientes_Nombre_Apellido ON CLIENTES(NOMBRE, APELLIDO);
GO

-- Insertar datos de prueba
INSERT INTO Clientes (NOMBRE, APELLIDO, FECHA_NACIMIENTO, CUIT, DOMICILIO, TELEFONO, EMAIL, FECHA_CREACION, FECHA_MODIFICACION, ACTIVO)
VALUES 
    ('Juan', 'Pérez', '1985-03-15', '20-12345678-9', 'Calle Principal 123', '1123456789', 'juan.perez@email.com', GETDATE(), NULL, 1),
    ('María', 'García', '1990-07-22', '27-87654321-0', 'Avenida Central 456', '1187654321', 'maria.garcia@email.com', GETDATE(), NULL, 1),
    ('Carlos', 'López', '1988-11-08', '23-11111111-1', 'Calle Secundaria 789', '1111111111', 'carlos.lopez@email.com', GETDATE(), NULL, 1),
    ('Ana', 'Martínez', '1992-05-30', '25-22222222-2', 'Avenida Norte 321', '1122222222', 'ana.martinez@email.com', GETDATE(), NULL, 1),
    ('Roberto', 'Rodríguez', '1987-09-12', '20-33333333-3', 'Calle Sur 654', '1133333333', 'roberto.rodriguez@email.com', GETDATE(), NULL, 1);
GO