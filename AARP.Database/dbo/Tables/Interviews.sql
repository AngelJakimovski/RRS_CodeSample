CREATE TABLE [dbo].[Interviews](
	[Id] [int] NOT NULL,
	[InterviewerId] [int] NOT NULL,
	[IntervieweeId] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[InterviewStageId] [int] NOT NULL,
	[SeniorityId] [int] NOT NULL,
	[TechnologyId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[RatingId] [int] NOT NULL,
	[InterviewerAttitude] [nchar](100) NULL,
	[TestTask] [bit] NULL,
	[TaskDifficultyLevel] [nchar](100) NULL,
	[InterviewLengthId] [int] NULL,
	[NoteFromInterviewee] [nvarchar](500) NULL,
	[SurveyStatus] [nchar](100) NULL,
	[FeedbackLink] [nvarchar](250) NULL,
 CONSTRAINT [PK_Interviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]