ALTER TABLE [membership].[UsersActionsLog]
    ADD CONSTRAINT [DF_CompanyUsersActionsLog_Date] DEFAULT (getdate()) FOR [Date];

