CREATE DATABASE [TesteTecnicoDB]
GO

USE [TesteTecnicoDB]
GO

CREATE TABLE [dbo].[Cliente]
(
    [CodCliente] INT IDENTITY (1, 1) NOT NULL,
    [CNPJ] VARCHAR (18) NOT NULL,
    [Nome] VARCHAR (255) NOT NULL,
    [Email] VARCHAR (255) NOT NULL,
    [DataCadastro] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([CodCliente] ASC),
    UNIQUE NONCLUSTERED ([CNPJ] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
GO

CREATE TABLE [dbo].[Produto]
(
    [CodProduto] INT IDENTITY (1, 1) NOT NULL,
    [Nome] VARCHAR (255) NOT NULL,
    [Preco] DECIMAL (10, 2) NOT NULL,
    [Estoque] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([CodProduto] ASC)
);
GO

CREATE TABLE [dbo].[Pedido]
(
    [CodPedido] INT IDENTITY (1, 1) NOT NULL,
    [CodCliente] INT NOT NULL,
    [DataPedido] DATETIME NOT NULL,
    [ValorTotal] DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([CodPedido] ASC),
    CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY ([CodCliente]) REFERENCES [dbo].[Cliente] ([CodCliente]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[ItensPedido]
(
    [CodPedido] INT NOT NULL,
    [CodProduto] INT NOT NULL,
    [Quantidade] INT NOT NULL,
    [PrecoUnitario] DECIMAL (10, 2) NOT NULL,
    CONSTRAINT [PK_ItensPedido] PRIMARY KEY CLUSTERED ([CodPedido] ASC, [CodProduto] ASC),
    CONSTRAINT [FK_ItensPedido_Pedido] FOREIGN KEY ([CodPedido]) REFERENCES [dbo].[Pedido] ([CodPedido]) ON DELETE CASCADE,
    CONSTRAINT [FK_ItensPedido_Produto] FOREIGN KEY ([CodProduto]) REFERENCES [dbo].[Produto] ([CodProduto])
);
GO



INSERT INTO [dbo].[Cliente]
    ([CNPJ], [Nome], [Email], [DataCadastro])
VALUES
    ('1123456789123', 'Cliente 1', 'cliente1@email.com', GETDATE()),
    ('2123456789123', 'Cliente 2', 'cliente2@email.com', GETDATE()),
    ('3123456789123', 'Cliente 3', 'cliente3@email.com', GETDATE()),
    ('4123456789123', 'Cliente 4', 'cliente4@email.com', GETDATE()),
    ('5123456789123', 'Cliente 5', 'cliente5@email.com', GETDATE()),
    ('6123456789123', 'Cliente 6', 'cliente6@email.com', GETDATE()),
    ('7123456789123', 'Cliente 7', 'cliente7@email.com', GETDATE()),
    ('8123456789123', 'Cliente 8', 'cliente8@email.com', GETDATE()),
    ('9123456789123', 'Cliente 9', 'cliente9@email.com', GETDATE()),
    ('0123456789123', 'Cliente 10', 'cliente10@email.com', "2025-04-02T18:28:24.610Z"),
    ('1223456789123', 'Cliente 11', 'cliente11@email.com', "2025-04-02T18:28:24.610Z"),
    ('1323456789123', 'Cliente 12', 'cliente12@email.com', "2025-04-02T18:28:24.610Z"),
    ('1423456789123', 'Cliente 13', 'cliente13@email.com', "2025-04-02T18:28:24.610Z"),
    ('1623456789123', 'Cliente 15', 'cliente15@email.com', "2025-04-02T18:28:24.610Z"),
    ('1723456789123', 'Cliente 16', 'cliente16@email.com', "2025-04-02T18:28:24.610Z"),
    ('1823456789123', 'Cliente 17', 'cliente17@email.com', "2025-04-02T18:28:24.610Z"),
    ('1523456789123', 'Cliente 14', 'cliente14@email.com', "2025-04-02T18:28:24.610Z"),
    ('1923456789123', 'Cliente 18', 'cliente18@email.com', "2025-04-02T18:28:24.610Z"),
    ('1023456789123', 'Cliente 19', 'cliente19@email.com', "2025-04-02T18:28:24.610Z"),
    ('1123456789124', 'Cliente 20', 'cliente20@email.com', "2025-04-02T18:28:24.610Z");
GO

-- Seed Produtos
INSERT INTO [dbo].[Produto]
    ([Nome], [Preco], [Estoque])
VALUES
    ('Produto 1', 10.00, 1),
    ('Produto 2', 20.00, 2),
    ('Produto 3', 30.00, 3),
    ('Produto 4', 40.00, 4),
    ('Produto 5', 50.00, 5),
    ('Produto 6', 60.00, 6),
    ('Produto 7', 70.00, 7),
    ('Produto 8', 80.00, 8),
    ('Produto 9', 90.00, 9),
    ('Produto 10', 100.00, 10),
    ('Produto 11', 110.00, 11),
    ('Produto 12', 120.00, 12),
    ('Produto 13', 130.00, 13),
    ('Produto 14', 140.00, 14),
    ('Produto 15', 150.00, 15),
    ('Produto 16', 160.00, 16),
    ('Produto 17', 170.00, 17),
    ('Produto 18', 180.00, 18),
    ('Produto 19', 190.00, 19),
    ('Produto 20', 200.00, 20);
GO

-- Seed Pedidos (each for a different Cliente)
INSERT INTO [dbo].[Pedido]
    ([CodCliente], [DataPedido], [ValorTotal])
VALUES
    (1, GETDATE(), 100.00),
    (2, GETDATE(), 200.00),
    (3, GETDATE(), 300.00),
    (4, GETDATE(), 400.00),
    (5, GETDATE(), 500.00),
    (6, GETDATE(), 600.00),
    (7, GETDATE(), 700.00),
    (8, GETDATE(), 800.00),
    (9, GETDATE(), 900.00),
    (1, GETDATE(), 1000.00),
    (2, GETDATE(), 1100.00),
    (3, GETDATE(), 1200.00),
    (4, GETDATE(), 1300.00),
    (5, GETDATE(), 1400.00),
    (1, GETDATE(), 1500.00),
    (2, GETDATE(), 1600.00),
    (3, GETDATE(), 1700.00),
    (1, GETDATE(), 1800.00),
    (2, GETDATE(), 1900.00),
    (1, GETDATE(), 2000.00);
GO

-- Seed ItensPedido (each Pedido with 1 Produto for simplicity)
INSERT INTO [dbo].[ItensPedido]
    ([CodPedido], [CodProduto], [Quantidade], [PrecoUnitario])
VALUES
    (1, 1, 1, 10.00),
    (2, 2, 2, 20.00),
    (3, 3, 3, 30.00),
    (4, 4, 4, 40.00),
    (5, 5, 5, 50.00),
    (6, 6, 6, 60.00),
    (7, 7, 7, 70.00),
    (8, 8, 8, 80.00),
    (9, 9, 9, 90.00),
    (10, 10, 10, 100.00),
    (11, 11, 11, 110.00),
    (12, 12, 12, 120.00),
    (13, 13, 13, 130.00),
    (14, 14, 14, 140.00),
    (15, 15, 15, 150.00),
    (16, 16, 16, 160.00),
    (17, 17, 17, 170.00),
    (18, 18, 18, 180.00),
    (19, 19, 19, 190.00),
    (20, 20, 20, 200.00);
GO