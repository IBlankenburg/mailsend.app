/****** Object:  Table [dbo].[MetaBuilders_Polling_Polls]    Script Date: 09/22/2007 02:43:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MetaBuilders_Polling_Polls]') AND type in (N'U'))
BEGIN
CREATE TABLE [MetaBuilders_Polling_Polls](
	[PollID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[VoteMode] [nvarchar](50) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[AllowWriteIns] [bit] NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_MetaBuilders_Polling_Polls] PRIMARY KEY CLUSTERED 
(
	[PollID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MetaBuilders_Polling_Options]    Script Date: 09/22/2007 02:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MetaBuilders_Polling_Options]') AND type in (N'U'))
BEGIN
CREATE TABLE [MetaBuilders_Polling_Options](
	[OptionID] [int] IDENTITY(1,1) NOT NULL,
	[PollID] [int] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_MetaBuilders_Polling_Options] PRIMARY KEY CLUSTERED 
(
	[OptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MetaBuilders_Polling_Votes]    Script Date: 09/22/2007 02:43:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MetaBuilders_Polling_Votes]') AND type in (N'U'))
BEGIN
CREATE TABLE [MetaBuilders_Polling_Votes](
	[VoteID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_MetaBuilders_Polling_Votes_VoteID]  DEFAULT (newid()),
	[Member] [nvarchar](255) NULL,
	[PollID] [int] NOT NULL,
	[OptionID] [int] NULL,
	[WriteInText] [nvarchar](255) NULL,
	[DateVoted] [datetime] NOT NULL,
	[VoteRating] [int] NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MetaBuilders_Polling_Votes_1] PRIMARY KEY CLUSTERED 
(
	[VoteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  View [dbo].[MetaBuilders_Polling_Options_VoteAmounts]    Script Date: 09/22/2007 02:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[MetaBuilders_Polling_Options_VoteAmounts]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [MetaBuilders_Polling_Options_VoteAmounts]
AS
SELECT DISTINCT 
                         dbo.MetaBuilders_Polling_Options.OptionID, dbo.MetaBuilders_Polling_Options.PollID, dbo.MetaBuilders_Polling_Options.Text, 
                         dbo.MetaBuilders_Polling_Options.DisplayOrder,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.MetaBuilders_Polling_Votes
                               WHERE        (OptionID = dbo.MetaBuilders_Polling_Options.OptionID)) * COALESCE (MetaBuilders_Polling_Votes_1.VoteRating, 1) AS VoteAmount
FROM            dbo.MetaBuilders_Polling_Options LEFT OUTER JOIN
                         dbo.MetaBuilders_Polling_Votes AS MetaBuilders_Polling_Votes_1 ON dbo.MetaBuilders_Polling_Options.OptionID = MetaBuilders_Polling_Votes_1.OptionID
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'MetaBuilders_Polling_Options_VoteAmounts', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "MetaBuilders_Polling_Options"
            Begin Extent = 
               Top = 6
               Left = 16
               Bottom = 129
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MetaBuilders_Polling_Votes_1"
            Begin Extent = 
               Top = 8
               Left = 312
               Bottom = 209
               Right = 529
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1215
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MetaBuilders_Polling_Options_VoteAmounts'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'MetaBuilders_Polling_Options_VoteAmounts', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MetaBuilders_Polling_Options_VoteAmounts'
GO
/****** Object:  ForeignKey [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Options]    Script Date: 09/22/2007 02:43:36 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Options]') AND parent_object_id = OBJECT_ID(N'[MetaBuilders_Polling_Votes]'))
ALTER TABLE [MetaBuilders_Polling_Votes]  WITH CHECK ADD  CONSTRAINT [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Options] FOREIGN KEY([OptionID])
REFERENCES [MetaBuilders_Polling_Options] ([OptionID])
GO
ALTER TABLE [MetaBuilders_Polling_Votes] CHECK CONSTRAINT [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Options]
GO
/****** Object:  ForeignKey [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Polls]    Script Date: 09/22/2007 02:43:36 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Polls]') AND parent_object_id = OBJECT_ID(N'[MetaBuilders_Polling_Votes]'))
ALTER TABLE [MetaBuilders_Polling_Votes]  WITH CHECK ADD  CONSTRAINT [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Polls] FOREIGN KEY([PollID])
REFERENCES [MetaBuilders_Polling_Polls] ([PollID])
GO
ALTER TABLE [MetaBuilders_Polling_Votes] CHECK CONSTRAINT [FK_MetaBuilders_Polling_Votes_MetaBuilders_Polling_Polls]
GO
/****** Object:  ForeignKey [FK_MetaBuilders_Polling_Options_MetaBuilders_Polling_Polls]    Script Date: 09/22/2007 02:43:41 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_MetaBuilders_Polling_Options_MetaBuilders_Polling_Polls]') AND parent_object_id = OBJECT_ID(N'[MetaBuilders_Polling_Options]'))
ALTER TABLE [MetaBuilders_Polling_Options]  WITH CHECK ADD  CONSTRAINT [FK_MetaBuilders_Polling_Options_MetaBuilders_Polling_Polls] FOREIGN KEY([PollID])
REFERENCES [MetaBuilders_Polling_Polls] ([PollID])
GO
ALTER TABLE [MetaBuilders_Polling_Options] CHECK CONSTRAINT [FK_MetaBuilders_Polling_Options_MetaBuilders_Polling_Polls]
GO
