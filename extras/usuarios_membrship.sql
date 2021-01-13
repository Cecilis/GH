USE [GH]
GO
INSERT [dbo].[aspnet_Applications] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (N'/', N'/', N'50421a42-1195-4761-913f-0748c997806b', NULL)
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'8e8159fc-5645-44a5-89d5-2d7e3248b27b', N'Administrador', N'administrador', NULL, 0, CAST(N'2018-08-26 05:24:26.703' AS DateTime))
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'6abe7ba8-9b69-48ed-9297-15d21388402d', N'Cecilis', N'cecilis', NULL, 0, CAST(N'2018-08-26 05:28:51.023' AS DateTime))
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'189a5b0b-4861-47f0-91a1-88be7b4fc420', N'Cotizador', N'cotizador', NULL, 0, CAST(N'2018-08-26 05:24:57.953' AS DateTime))
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'8e8159fc-5645-44a5-89d5-2d7e3248b27b', N'vaxXqJ2F4dnp3IGWrrUgyVuTQdc=', 1, N'NdbwtKzyZnLH/goWkIaZcA==', NULL, N'ligiapuertas@gmail.com', N'ligiapuertas@gmail.com', N'mascota', N'R/gEfAq9KOaX+5qGK0sF+FlpdFk=', 1, 0, CAST(N'2018-08-26 05:24:26.000' AS DateTime), CAST(N'2018-08-26 05:24:26.703' AS DateTime), CAST(N'2018-08-26 05:24:26.000' AS DateTime), CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'189a5b0b-4861-47f0-91a1-88be7b4fc420', N'NrctWsKaaM/bo63LIY5Xtr9eTRU=', 1, N'1g685Dvu53IMfNfpd/+gCw==', NULL, N'ligiapuertas@gmail.com', N'ligiapuertas@gmail.com', N'mascota', N'WT9ehZLaQPx6EOgZ38sZbd22j0M=', 1, 0, CAST(N'2018-08-26 05:24:57.000' AS DateTime), CAST(N'2018-08-26 05:24:57.953' AS DateTime), CAST(N'2018-08-26 05:24:57.000' AS DateTime), CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'6abe7ba8-9b69-48ed-9297-15d21388402d', N'M9kjXZ2r92ZnN0+KY3BIooP7p3o=', 1, N'NSy6WyFIa92sr1LTau+mmg==', NULL, N'ligiapuertas@gmail.com', N'ligiapuertas@gmail.com', N'mascota', N'dfymY6BicOOeY1IARcjeYdJh0jk=', 1, 0, CAST(N'2018-08-26 05:25:21.000' AS DateTime), CAST(N'2018-08-26 05:28:51.023' AS DateTime), CAST(N'2018-08-26 05:25:21.000' AS DateTime), CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), 0, CAST(N'1754-01-01 00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'e787afd1-d0ed-42b4-803a-f9628ff86bc4', N'Administrador', N'administrador', NULL)
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'50421a42-1195-4761-913f-0748c997806b', N'4a63a8b3-07fc-46c1-9578-f645b801dfda', N'Cotizador', N'cotizador', NULL)
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'6abe7ba8-9b69-48ed-9297-15d21388402d', N'4a63a8b3-07fc-46c1-9578-f645b801dfda')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'189a5b0b-4861-47f0-91a1-88be7b4fc420', N'4a63a8b3-07fc-46c1-9578-f645b801dfda')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'6abe7ba8-9b69-48ed-9297-15d21388402d', N'e787afd1-d0ed-42b4-803a-f9628ff86bc4')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'8e8159fc-5645-44a5-89d5-2d7e3248b27b', N'e787afd1-d0ed-42b4-803a-f9628ff86bc4')
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'common', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'health monitoring', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'membership', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'personalization', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'profile', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'role manager', N'1', 1)
