﻿CREATE TABLE [dbo].[JobTechnologies](
	[Id] [int] NOT NULL,
	[Technology] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_JobTechnologies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]