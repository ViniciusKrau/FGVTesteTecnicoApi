CREATE DATABASE [TesteTecnicoDB]
GO

USE [TesteTecnicoDB]
GO

CREATE TABLE [dbo].[Cliente] (
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

CREATE TABLE [dbo].[Produto] (
 [CodProduto] INT IDENTITY (1, 1) NOT NULL,
 [Nome] VARCHAR (255) NOT NULL,
 [Preco] DECIMAL (10, 2) NOT NULL,
 [Estoque] INT NOT NULL,
 PRIMARY KEY CLUSTERED ([CodProduto] ASC)
);
GO

CREATE TABLE [dbo].[Pedido] (
 [CodPedido] INT IDENTITY (1, 1) NOT NULL,
 [CodCliente] INT NOT NULL,
 [DataPedido] DATETIME NOT NULL,
 [ValorTotal] DECIMAL (10, 2) NOT NULL,
 PRIMARY KEY CLUSTERED ([CodPedido] ASC),
 CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY ([CodCliente]) REFERENCES [dbo].[Cliente] ([CodCliente]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[ItensPedido] (
 [CodPedido] INT NOT NULL,
 [CodProduto] INT NOT NULL,
 [Quantidade] INT NOT NULL,
 [PrecoUnitario] DECIMAL (10, 2) NOT NULL,
 CONSTRAINT [PK_ItensPedido] PRIMARY KEY CLUSTERED ([CodPedido] ASC, [CodProduto] ASC),
 CONSTRAINT [FK_ItensPedido_Pedido] FOREIGN KEY ([CodPedido]) REFERENCES [dbo].[Pedido] ([CodPedido]) ON DELETE CASCADE,
 CONSTRAINT [FK_ItensPedido_Produto] FOREIGN KEY ([CodProduto]) REFERENCES [dbo].[Produto] ([CodProduto])
);
GO