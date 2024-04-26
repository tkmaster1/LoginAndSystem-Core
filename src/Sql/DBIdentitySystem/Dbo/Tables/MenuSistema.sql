 CREATE TABLE [dbo].[MenuSistema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](256) NOT NULL,
	[Descricao] [varchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_MenuSistema] PRIMARY KEY CLUSTERED ([Id] ASC)
 )