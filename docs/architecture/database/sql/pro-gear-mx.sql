USE ProGear;
GO

-- ==========================================
-- Tabla: Producto
-- ==========================================

CREATE TABLE Producto (
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Nombre VARCHAR(200) NOT NULL,
    Marca VARCHAR(20) NOT NULL,
    SKU VARCHAR(7) NOT NULL UNIQUE,

    CONSTRAINT CK_Producto_SKU_Formato
        CHECK (SKU LIKE '[A-Z][A-Z][A-Z]-[0-9][0-9][0-9]')
);
GO


-- ==========================================
-- Tabla: Inventario
-- ==========================================

CREATE TABLE Inventario (
    Id INT IDENTITY(1,1) PRIMARY KEY,

    IdProducto INT NOT NULL UNIQUE,

    Existencia INT NOT NULL DEFAULT 0,
    Reservado INT NOT NULL DEFAULT 0,

    CONSTRAINT CK_Inventario_Existencia_NoNegativa
        CHECK (Existencia >= 0),

    CONSTRAINT CK_Inventario_Reservado_NoNegativo
        CHECK (Reservado >= 0),

    CONSTRAINT CK_Inventario_Reservado_NoMayorExistencia
        CHECK (Reservado <= Existencia),

    CONSTRAINT FK_Inventario_Producto
        FOREIGN KEY (IdProducto)
        REFERENCES Producto(Id)
);
GO


-- ==========================================
-- Tabla: HistorialPrecio
-- ==========================================

CREATE TABLE HistorialPrecio (
    Id INT IDENTITY(1,1) PRIMARY KEY,

    IdProducto INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,

    FechaInicio DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaFin DATETIME2 NULL,

    CONSTRAINT CK_HistorialPrecio_Precio_NoNegativo
        CHECK (Precio >= 0),

    CONSTRAINT CK_HistorialPrecio_FechaFin_Valida
        CHECK (
            FechaFin IS NULL
            OR FechaFin > FechaInicio
        ),

    CONSTRAINT FK_HistorialPrecio_Producto
        FOREIGN KEY (IdProducto)
        REFERENCES Producto(Id)
);
GO


-- ==========================================
-- Índice: Un único precio vigente por producto
-- ==========================================

CREATE UNIQUE INDEX UX_HistorialPrecio_UnicoVigente
ON HistorialPrecio (IdProducto)
WHERE FechaFin IS NULL;
GO