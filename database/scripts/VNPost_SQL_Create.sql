USE [master]
GO
CREATE DATABASE [VNPOST_Appointment]
GO
USE [VNPOST_Appointment]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 11/16/2019 10:00:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminID] [bigint] IDENTITY(1,1) NOT NULL,
	[AdminName] [nvarchar](100) NOT NULL,
	[AdminUsername] [varchar](100) NOT NULL,
	[AdminPasswordHash] [varchar](64) NOT NULL,
	[AdminPasswordSalt] [char](16) NOT NULL,
	[Role] [int] NOT NULL,
	[PostalProvinceCode] [varchar](2) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedTime] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AdminUsername] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminRole]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminRole](
	[AdminRoleID] [int] IDENTITY(1,1) NOT NULL,
	[AdminRoleName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AdminRoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[API]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[API](
	[APIID] [int] IDENTITY(1,1) NOT NULL,
	[APIMethodID] [int] NOT NULL,
	[APIUri] [varchar](max) NOT NULL,
	[APIDescription] [nvarchar](max) NULL,
	[LastMofifiedTime] [datetime] NOT NULL,
	[SampleRespone] [nvarchar](4000) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
 CONSTRAINT [PK__API__ABCD70F20CBAE877] PRIMARY KEY CLUSTERED 
(
	[APIID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APIInputParam]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APIInputParam](
	[APIID] [int] NOT NULL,
	[APIInputParamName] [varchar](50) NOT NULL,
	[APIInputParamType] [varchar](50) NOT NULL,
	[APIInputParamDescription] [nvarchar](max) NULL,
	[LastMofifiedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[APIID] ASC,
	[APIInputParamName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APIMethod]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APIMethod](
	[MethodID] [int] IDENTITY(1,1) NOT NULL,
	[MethodName] [varchar](10) NOT NULL,
	[LastMofifiedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_APIMethod] PRIMARY KEY CLUSTERED 
(
	[MethodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APIOutputParam]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APIOutputParam](
	[APIID] [int] NOT NULL,
	[APIOutputParamName] [varchar](50) NOT NULL,
	[APIOutputParamType] [varchar](50) NOT NULL,
	[APIOutputParamDescription] [nvarchar](max) NULL,
	[LastMofifiedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[APIID] ASC,
	[APIOutputParamName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactInfo](
	[ContactInfoID] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
	[PostalDistrictCode] [varchar](4) NOT NULL,
	[Street] [nvarchar](max) NOT NULL,
	[PersonalPaperTypeID] [int] NULL,
	[PersonalPaperNumber] [varchar](20) NULL,
	[PersonalPaperIssuedDate] [datetime] NULL,
	[PersonalPaperIssuedPlace] [nvarchar](max) NULL,
	[CustomerID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [bigint] IDENTITY(1,1) NOT NULL,
	[PasswordHash] [varchar](64) NOT NULL,
	[PasswordSalt] [char](16) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Email] [varchar](100) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
	[PostalDistrictID] [varchar](4) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[PostalDistrictCode] [varchar](4) NOT NULL,
	[PostalDistrictName] [nvarchar](100) NOT NULL,
	[DistrictCode] [varchar](4) NOT NULL,
	[PostalProvinceCode] [varchar](2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PostalDistrictCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[DistrictCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailVerification]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailVerification](
	[EmailVerificationID] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[VerificationCode] [varchar](6) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmailVerificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerID] [bigint] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[ShipDate] [datetime] NULL,
	[ProfileID] [int] NOT NULL,
	[AppointmentLetterCode] [varchar](100) NOT NULL,
	[AppointLetterImageLink] [varchar](200) NOT NULL,
	[ProcedurerFullName] [nvarchar](100) NOT NULL,
	[ProcedurerPhone] [varchar](50) NOT NULL,
	[ProcedurerPostalDistrictCode] [varchar](4) NOT NULL,
	[ProcedurerStreet] [nvarchar](max) NOT NULL,
	[ProcedurerPersonalPaperTypeID] [int] NOT NULL,
	[ProcedurerPersonalPaperNumber] [varchar](20) NOT NULL,
	[ProcedurerPersonalPaperIssuedDate] [datetime] NOT NULL,
	[ProcedurerPersonalPaperIssuedPlace] [nvarchar](max) NOT NULL,
	[SenderFullName] [nvarchar](100) NOT NULL,
	[SenderPhone] [varchar](50) NOT NULL,
	[SenderPostalDistrictCode] [varchar](4) NOT NULL,
	[SenderStreet] [nvarchar](max) NOT NULL,
	[ReceiverFullName] [nvarchar](100) NOT NULL,
	[ReceiverPhone] [varchar](50) NOT NULL,
	[ReceiverPostalDistrictCode] [varchar](4) NOT NULL,
	[ReceiverStreet] [nvarchar](max) NOT NULL,
	[OrderNote] [nvarchar](max) NULL,
	[Amount] [float] NULL,
	[TotalAmountInWords] [nvarchar](100) NULL,
	[StatusID] [int] NULL,
	[ItemCode] [nvarchar](100) NULL,
	[ProcessedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedTime] [datetime] NULL,
 CONSTRAINT [PK__Order__C3905BAF300424B4] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__Order__3ECC0FEA32E0915F] UNIQUE NONCLUSTERED 
(
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatusDetail]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatusDetail](
	[OrderStatusDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderID] [bigint] NOT NULL,
	[StatusID] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[PosCode] [bigint] NULL,
	[CreatedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderStatusDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderID] [bigint] NOT NULL,
	[PaymentMethodID] [int] NOT NULL,
	[PaymentStatusID] [int] NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[OtherDetails] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentMethodName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentMethodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PaymentMethodName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatus]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStatus](
	[PaymentStatusID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentStatusDescription] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PaymentStatusDescription] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonalPaperType]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalPaperType](
	[PersonalPaperTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PersonalPaperTypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonalPaperTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PersonalPaperTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneVerification]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneVerification](
	[PhoneVerificationID] [bigint] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
	[VerificationCode] [varchar](6) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PhoneVerificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostOffice]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostOffice](
	[PosCode] [bigint] NOT NULL,
	[PosName] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [varchar](50) NULL,
	[DistrictCode] [varchar](4) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PosCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileID] [int] NOT NULL,
	[ProfileName] [nvarchar](2000) NOT NULL,
	[PublicAdministrationLocationID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Province](
	[PostalProvinceCode] [varchar](2) NOT NULL,
	[PostalProvinceName] [nvarchar](100) NOT NULL,
	[ProvinceCode] [varchar](2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PostalProvinceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ProvinceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublicAdministration]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublicAdministration](
	[PublicAdministrationLocationID] [int] NOT NULL,
	[PublicAdministrationName] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [varchar](50) NULL,
	[PosCode] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PublicAdministrationLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResetPasswordToken]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResetPasswordToken](
	[TokenID] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerID] [bigint] NOT NULL,
	[Token] [varchar](64) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusID] [int] NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[StatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Admin] ([AdminID])
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Admin] ([AdminID])
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD FOREIGN KEY([PostalProvinceCode])
REFERENCES [dbo].[Province] ([PostalProvinceCode])
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD FOREIGN KEY([Role])
REFERENCES [dbo].[AdminRole] ([AdminRoleID])
GO
ALTER TABLE [dbo].[API]  WITH CHECK ADD  CONSTRAINT [FK_API_APIMethod] FOREIGN KEY([APIMethodID])
REFERENCES [dbo].[APIMethod] ([MethodID])
GO
ALTER TABLE [dbo].[API] CHECK CONSTRAINT [FK_API_APIMethod]
GO
ALTER TABLE [dbo].[APIInputParam]  WITH CHECK ADD  CONSTRAINT [FK__APIInputP__APIID__74AE54BC] FOREIGN KEY([APIID])
REFERENCES [dbo].[API] ([APIID])
GO
ALTER TABLE [dbo].[APIInputParam] CHECK CONSTRAINT [FK__APIInputP__APIID__74AE54BC]
GO
ALTER TABLE [dbo].[APIOutputParam]  WITH CHECK ADD  CONSTRAINT [FK__APIOutput__APIID__75A278F5] FOREIGN KEY([APIID])
REFERENCES [dbo].[API] ([APIID])
GO
ALTER TABLE [dbo].[APIOutputParam] CHECK CONSTRAINT [FK__APIOutput__APIID__75A278F5]
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([PersonalPaperTypeID])
REFERENCES [dbo].[PersonalPaperType] ([PersonalPaperTypeID])
GO
ALTER TABLE [dbo].[ContactInfo]  WITH CHECK ADD FOREIGN KEY([PostalDistrictCode])
REFERENCES [dbo].[District] ([PostalDistrictCode])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_District] FOREIGN KEY([PostalDistrictID])
REFERENCES [dbo].[District] ([PostalDistrictCode])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_District]
GO
ALTER TABLE [dbo].[District]  WITH CHECK ADD FOREIGN KEY([PostalProvinceCode])
REFERENCES [dbo].[Province] ([PostalProvinceCode])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__CustomerI__7A672E12] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__CustomerI__7A672E12]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__ModifiedB__7B5B524B] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Admin] ([AdminID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__ModifiedB__7B5B524B]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__Procedure__7C4F7684] FOREIGN KEY([ProcedurerPostalDistrictCode])
REFERENCES [dbo].[District] ([PostalDistrictCode])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__Procedure__7C4F7684]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__Procedure__7D439ABD] FOREIGN KEY([ProcedurerPersonalPaperTypeID])
REFERENCES [dbo].[PersonalPaperType] ([PersonalPaperTypeID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__Procedure__7D439ABD]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__Processed__7E37BEF6] FOREIGN KEY([ProcessedBy])
REFERENCES [dbo].[Admin] ([AdminID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__Processed__7E37BEF6]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__ProfileID__7F2BE32F] FOREIGN KEY([ProfileID])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__ProfileID__7F2BE32F]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__ReceiverP__00200768] FOREIGN KEY([ReceiverPostalDistrictCode])
REFERENCES [dbo].[District] ([PostalDistrictCode])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__ReceiverP__00200768]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__SenderPos__01142BA1] FOREIGN KEY([SenderPostalDistrictCode])
REFERENCES [dbo].[District] ([PostalDistrictCode])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__SenderPos__01142BA1]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__StatusID__02084FDA] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__StatusID__02084FDA]
GO
ALTER TABLE [dbo].[OrderStatusDetail]  WITH CHECK ADD  CONSTRAINT [FK__OrderStat__Order__02FC7413] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderStatusDetail] CHECK CONSTRAINT [FK__OrderStat__Order__02FC7413]
GO
ALTER TABLE [dbo].[OrderStatusDetail]  WITH CHECK ADD FOREIGN KEY([PosCode])
REFERENCES [dbo].[PostOffice] ([PosCode])
GO
ALTER TABLE [dbo].[OrderStatusDetail]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK__Payment__OrderID__05D8E0BE] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK__Payment__OrderID__05D8E0BE]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([PaymentMethodID])
REFERENCES [dbo].[PaymentMethod] ([PaymentMethodID])
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([PaymentStatusID])
REFERENCES [dbo].[PaymentStatus] ([PaymentStatusID])
GO
ALTER TABLE [dbo].[PostOffice]  WITH CHECK ADD FOREIGN KEY([DistrictCode])
REFERENCES [dbo].[District] ([DistrictCode])
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD FOREIGN KEY([PublicAdministrationLocationID])
REFERENCES [dbo].[PublicAdministration] ([PublicAdministrationLocationID])
GO
ALTER TABLE [dbo].[PublicAdministration]  WITH CHECK ADD FOREIGN KEY([PosCode])
REFERENCES [dbo].[PostOffice] ([PosCode])
GO
ALTER TABLE [dbo].[ResetPasswordToken]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
/****** Object:  Trigger [dbo].[updateStatus]    Script Date: 11/16/2019 10:00:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [dbo].[updateStatus] on [dbo].[OrderStatusDetail]
after insert,update as
begin
declare @status int, @orderid int;
select @status = (select StatusID from inserted);
select @orderid = (select OrderID from inserted);
if @status in (2,3,12)
begin
update [Order]
set StatusID = -1
where OrderID = @orderid
end
else 
begin
update [Order]
set StatusID = @status
where OrderID = @orderid
end
end
GO
ALTER TABLE [dbo].[OrderStatusDetail] ENABLE TRIGGER [updateStatus]
GO
