﻿CREATE TABLE [dbo].[Seniority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Seniority] [nchar](30) NOT NULL,
 CONSTRAINT [PK_Seniority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]