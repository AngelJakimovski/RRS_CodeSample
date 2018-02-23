CREATE TABLE [dbo].[Reviewers](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[AssignedCount] [int] NOT NULL,
	[RejectCount] [int] NOT NULL,
	[AdvanceCount] [int] NOT NULL,
	[RecentAssignedAt] [datetime] NULL,
	[TimeZone] [nvarchar](64) NULL,
 CONSTRAINT [PK_Reviewers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Reviewers] ADD  CONSTRAINT [DF_Reviewers_TimeZone]  DEFAULT ('UTC') FOR [TimeZone]