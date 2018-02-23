CREATE TABLE [dbo].[JobApplications](
	[Id] [int] NOT NULL,
	[CandidateId] [nvarchar](256) NOT NULL,
	[CandidateName] [nvarchar](256) NULL,
	[AppliedAt] [datetime] NULL,
	[RecorededAt] [datetime] NOT NULL,
	[AssignedToReviewerAt] [datetime] NULL,
	[RecentRemindAt] [datetime] NULL,
	[RejectAt] [datetime] NULL,
	[RejectionReason] [nvarchar](max) NULL,
	[ReviewerId] [int] NULL,
	[JobId] [nvarchar](256) NOT NULL,
	[JobName] [nvarchar](256) NULL,
	[ReviewStatus] [int] NOT NULL,
	[RemindCount] [int] NOT NULL,
	[Source] [nvarchar](256) NULL,
	[RejectedOrAcceptedAt] [datetime] NULL,
 CONSTRAINT [PK_JobApplications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]