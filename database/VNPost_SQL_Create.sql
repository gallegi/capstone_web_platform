create database VNPOST_Appointment
go
use VNPost_AppointMent
go

create table Customer(
	CustomerID bigint primary key identity,
	PasswordHash varchar(64) NOT NULL,
	PasswordSalt char(16) NOT NULL,
	FullName nvarchar(100) NOT NULL,
	Gender int NOT NULL,
	DOB datetime,
	Email varchar(100) NOT NULL UNIQUE,
	Phone varchar(20) NOT NULL UNIQUE
)


create table ResetPasswordToken(
	TokenID bigint primary key identity,
	CustomerID bigint foreign key references Customer(CustomerID) NOT NULL,
	Token varchar(64) NOT NULL,
	Status bit NOT NULL,
	CreatedDate datetime NOT NULL
)


create table PhoneVerification(
	PhoneVerificationID bigint primary key identity,
	Phone varchar(20) NOT NULL,
	VerificationCode varchar(6) NOT NULL,
	Status bit NOT NULL,
	CreatedTime datetime  NOT NULL
)

create table EmailVerification(
	EmailVerificationID bigint primary key identity,
	Email varchar(100) NOT NULL,
	VerificationCode varchar(6) NOT NULL,
	Status bit NOT NULL,
	CreatedTime datetime  NOT NULL
)

create table Province(
	PostalProvinceCode int primary key,
	PostalProvinceName nvarchar(MAX) NOT NULL,
	ProvinceCode int NOT NULL UNIQUE
)


create table District(
	PostalDistrictCode int primary key,
	PostalDistrictName nvarchar(MAX) NOT NULL,
	DistrictCode int NOT NULL UNIQUE,
	PostalProvinceCode int foreign key references Province(PostalProvinceCode) NOT NULL,
)

create table PersonalPaperType(
	PersonalPaperTypeID int primary key identity,
	PersonalPaperTypeName nvarchar(100) NOT NULL UNIQUE
)


create table ContactInfo(
	ContactInfoID bigint primary key identity,
	FullName nvarchar(100) NOT NULL,
	Phone varchar(20) NOT NULL,
	PostalDistrictCode int foreign key references District(PostalDistrictCode) NOT NULL,
	Street nvarchar(MAX) NOT NULL,
	PersonalPaperTypeID int foreign key references PersonalPaperType(PersonalPaperTypeID),
	PersonalPaperNumber varchar(20) NOT NULL,
	PersonalPaperIssuedDate datetime,
	PersonalPaperIssuedPlace nvarchar(MAX),
	CustomerID bigint foreign key references Customer(CustomerID) NOT NULL
)

create table PublicAdministration(
	PublicAdministrationLocationID bigint primary key,
	PublicAdministrationName nvarchar(100) NOT NULL,
	Address nvarchar(MAX) NOT NULL,
	Phone varchar(20) NOT NULL UNIQUE,
	PostalDistrictCode int foreign key references District(PostalDistrictCode) NOT NULL
)

create table PostOffice(
	PosCode bigint primary key,
	PosName nvarchar(100) NOT NULL,
	Address nvarchar(MAX) NOT NULL,
	Phone varchar(20) NOT NULL UNIQUE,
	PublicAdministrationLocationID bigint foreign key references PublicAdministration(PublicAdministrationLocationID) NOT NULL UNIQUE
)

create table Profile(
	ProfileID int primary key,
	ProfileName nvarchar(MAX) NOT NULL,
	PublicAdministrationLocationID bigint foreign key references PublicAdministration(PublicAdministrationLocationID) NOT NULL
)


create table Status(
	StatusID int primary key,
	StatusName nvarchar(100) NOT NULL UNIQUE
)

create table AdminRole(
	AdminRoleID int primary key identity,
	AdminRoleName nvarchar(100) NOT NULL UNIQUE
)

create table Admin(
	AdminID bigint primary key identity,
	AdminName nvarchar(100) NOT NULL,
	AdminUsername varchar(100)  NOT NULL UNIQUE,
	AdminPasswordHash varchar(64) NOT NULL,
	AdminPasswordSalt char(16) NOT NULL,
	Role int foreign key references AdminRole(AdminRoleID) NOT NULL,
	PostalProvinceCode int foreign key references Province(PostalProvinceCode),
	IsActive bit NOT NULL,
	CreatedBy bigint foreign key references Admin(AdminID) NOT NULL,
	CreatedTime datetime NOT NULL,
	ModifiedBy bigint foreign key references Admin(AdminID),
	ModifiedTime datetime
)


create table [Order](
	OrderID bigint primary key identity,
	CustomerID bigint foreign key references Customer(CustomerID) NOT NULL,
	OrderDate datetime NOT NULL,
	ShipDate datetime,
	ProfileID int foreign key references Profile(ProfileID) NOT NULL,
	AppointmentLetterCode varchar(100) NOT NULL,
	AppointLetterImageLink varchar(200) NOT NULL,
	ProcedurerFullName nvarchar(100)  NOT NULL,
	ProcedurerPhone varchar(20)  NOT NULL,
	ProcedurerPostalDistrictCode int foreign key references District(PostalDistrictCode) NOT NULL,
	ProcedurerStreet nvarchar(MAX) NOT NULL,
	ProcedurerPersonalPaperTypeID int foreign key references PersonalPaperType(PersonalPaperTypeID) NOT NULL,
	ProcedurerPersonalPaperNumber varchar(20) NOT NULL,
	ProcedurerPersonalPaperIssuedDate datetime NOT NULL,
	ProcedurerPersonalPaperIssuedPlace nvarchar(MAX) NOT NULL,
	SenderFullName nvarchar(100) NOT NULL,
	SenderPhone varchar(20) NOT NULL,
	SenderPostalDistrictCode int foreign key references District(PostalDistrictCode) NOT NULL,
	SenderStreet nvarchar(MAX) NOT NULL,
	ReceiverFullName nvarchar(100) NOT NULL,
	ReceiverPhone varchar(20)  NOT NULL,
	ReceiverPostalDistrictCode int foreign key references District(PostalDistrictCode) NOT NULL,
	ReceiverStreet nvarchar(MAX) NOT NULL,
	OrderNote nvarchar(MAX),
	CreatedTime datetime NOT NULL,
	Amount float,
	StatusID int foreign key references Status(StatusID) NOT NULL,
	ItemCode nvarchar(100) UNIQUE,
	ProcessedBy bigint foreign key references Admin(AdminID),
	ModifiedBy bigint foreign key references Admin(AdminID),
	ModifiedTime datetime
)

create table PaymentMethod(
	PaymentMethodID int primary key identity,
	PaymentMethodName nvarchar(100) NOT NULL UNIQUE
)

create table PaymentStatus(
	PaymentStatusID int primary key identity,
	PaymentStatusDescription nvarchar(100) NOT NULL UNIQUE
)

create table Payment(
	PaymentID bigint primary key identity,
	OrderID bigint foreign key references [Order](OrderID) NOT NULL,
	PaymentMethodID int foreign key references PaymentMethod(PaymentMethodID) NOT NULL,
	PaymentStatusID int foreign key references PaymentStatus(PaymentStatusID) NOT NULL,
	PaymentDate datetime NOT NULL,
	OtherDetails nvarchar(MAX)
)

create table OrderStatusDetail(
	OrderID bigint foreign key references [Order](OrderID),
	StatusID int foreign key references Status(StatusID) NOT NULL,
	Note nvarchar(MAX),
	PosCode bigint foreign key references PostOffice(PosCode)  NOT NULL,
	CreatedTime datetime NOT NULL,
	primary key (OrderID, StatusID, CreatedTime)
)


create table API (
	APIID int primary key identity,
	APIMethod varchar(10) NOT NULL,
	APIEndPoint varchar(MAX) NOT NULL,
	APIDescription nvarchar(MAX),
	LastMofifiedTime datetime NOT NULL
)


create table APIInputParam(
	APIID int foreign key references API(APIID),
	APIInputParamName varchar(50),
	APIInputParamType varchar(50) NOT NULL,
	APIInputParamDescription nvarchar(MAX),
	LastMofifiedTime datetime NOT NULL,
	primary key (APIID, APIInputParamName)
)

create table APIOutputParam(
	APIID int foreign key references API(APIID),
	APIOutputParamName varchar(50),
	APIOutputParamType varchar(50) NOT NULL,
	APIOutputParamDescription nvarchar(MAX),
	LastMofifiedTime datetime NOT NULL,
	primary key (APIID, APIOutputParamName)
)
