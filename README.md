# API  en .Net Core 8
> Es necesario instalar Microsoft.Data.SqlClient para conectar a la base de datos de SQl server

```sh
PM> Install-Package System.Data.SqlClient
```

En el archivo "appsettings.json" agregar:
```sh
  "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLExpress;Database=Personas;Trusted_Connection=true;TrustServerCertificate=true"
  }
  ```
  > Script para crear la BD y los procedimientos a utilizar en el ejemplo:
  
  ```sh
  
-- 1. Crear la base de datos
CREATE DATABASE Personas;
GO

-- 2. Usar la base de datos
USE Personas;
GO

-- 3. Crear la tabla Persona
CREATE TABLE Persona (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100),
    Edad INT
);
GO

-- 4. Procedimiento para obtener todas las personas
CREATE PROCEDURE sp_GetAllPersons
AS
BEGIN
    SELECT * FROM Persona;
END;
GO

-- 5. Procedimiento para obtener una persona por ID
CREATE PROCEDURE sp_GetPersonById
    @Id INT
AS
BEGIN
    SELECT * FROM Persona
    WHERE Id = @Id;
END;
GO

-- 6. Procedimiento para editar persona por ID
CREATE PROCEDURE sp_UpdatePersonById
    @Id INT,
    @Name NVARCHAR(100),
    @Age INT
AS
BEGIN
    UPDATE Persona
    SET Nombre = @Name,
        Edad = @Age
    WHERE Id = @Id;
END;
GO

-- 7. Procedimiento para eliminar persona por ID
CREATE PROCEDURE sp_DeletePersonById
    @Id INT
AS
BEGIN
    DELETE FROM Persona
    WHERE Id = @Id;
END;
GO


CREATE PROCEDURE sp_InsertPerson
    @Name NVARCHAR(100),
    @Age INT
AS
BEGIN
insert into Persona (Nombre,Edad) values (@Name,@Age);

END;
GO
  
  ```


