USE [master]
GO
CREATE DATABASE [Duck]
COLLATE Chinese_PRC_CI_AS;
GO
USE [Duck]
GO
CREATE TABLE CompanyCategories
(
	CompanyCategoryId nchar(1) primary key,
	CompanyCategoryName nvarchar(40) not null
)
GO
CREATE TABLE CompanyClasses
(
	CompanyClassId nchar(2) primary key,
	CompanyCategoryId nchar(1) not null
		references CompanyCategories(CompanyCategoryId),
	CompanyClassName nvarchar(40) not null
)
GO
CREATE TABLE Companies
(
    CompanyId int primary key identity,
	GUINumber nchar(8) not null,
	[Password] nvarchar(60) not null,
    CompanyName nvarchar(60) not null,
	CompanyClassId nchar(2)
		references CompanyClasses(CompanyClassId)
		on delete set null,
	[Address] nvarchar(100),
    Intro nvarchar(200),
	Benefits nvarchar(200),
    Picture varbinary(max),
    ContactName nvarchar(30) not null,
    ContactPhone nvarchar(24) not null,
    ContactEmail nvarchar(320) not null,
	[Status] bit not null default(0),
	[Date] datetime not null,
	Deadline datetime,
	OrderCount int not null default(0),
)
GO
CREATE TABLE TitleCategories
(
	TitleCategoryId int primary key identity(0,1),
	TitleCategoryName nvarchar(30) not null,
)
Go
CREATE TABLE TitleClasses
(
	TitleClassId int primary key identity,
	TitleCategoryId int default(0)
		references TitleCategories(TitleCategoryId)
		on delete set default,
	TitleClassName nvarchar(30) not null,
)
GO
CREATE TABLE Openings
(
	OpeningId int primary key identity,
	CompanyId int not null
		references Companies(CompanyId)
		on delete cascade,
	Title nvarchar(60) not null,
	[Address] nvarchar(100),
	[Description] nvarchar(300) not null,
	Degree nvarchar(20),
	Benefits nvarchar(200),
	InterviewYN bit not null default(0),
	SalaryMax money,
	SalaryMin money,
	[Time] nvarchar(60),
	ContactName nvarchar(30) not null,
    ContactPhone nvarchar(24) not null,
    ContactEmail nvarchar(320),
	ReleaseYN bit not null default(0),
	RequiredNumber int default(1)
		CHECK(RequiredNumber >= 1),
	ResumeNumber int default(0)
		CHECK(ResumeNumber >= 0),
)
GO
CREATE TABLE OpeningTitleClasses
(
	OpeningId int
		references Openings(OpeningId)
		on delete cascade,
	TitleClassId int
		references TitleClasses(TitleClassId)
		on delete cascade,
	primary key(OpeningId,TitleClassId)
)
GO
CREATE TABLE Candidates
(
	CandidateId int primary key identity,
	NationalId nchar(10) not null,
	Email nvarchar(320) not null,
	[Password] nvarchar(60) not null,
	[Name] nvarchar(30),
	Sex bit,
	Birthday date,
	Headshot varbinary(Max),
	TitleClass nvarchar(30),
	Phone nvarchar(24),
	[Address] nvarchar(100),
	Degree nvarchar(30),
	EmploymentStatus nvarchar(20),
	MilitaryService nvarchar(20),
	VerifyEmailYN bit not null default(0),
	RegisterTime datetime not null,
)
CREATE TABLE Resumes
(
	ResumeId int primary key identity,
	CandidateId int not null
		references Candidates(CandidateId)
		on delete cascade,
	Title nvarchar(60) not null,
	Intro nvarchar(200),
	Headshot varbinary(Max),
	Autobiography nvarchar(Max),
	WorkExperience nvarchar(Max),
	[Time] nvarchar(60),
	[Address] nvarchar(100),
	ReleaseYN bit not null default(1),
	LastEditTime datetime not null,
)
CREATE TABLE [dbo].[ResumeCertification] (
    [CertificationId]   INT             IDENTITY (1, 1) NOT NULL,
    [ResumeId]          INT             NOT NULL,
    [CertificationName] NVARCHAR (60)   NULL,
    [FileData]          VARBINARY (MAX) NULL,
    [ContentType]       NVARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([CertificationId] ASC),
    FOREIGN KEY ([ResumeId]) REFERENCES [dbo].[Resumes] ([ResumeId]) ON DELETE CASCADE
);
GO
CREATE TABLE ResumeTitleClasses
(
	ResumeId int
		references Resumes(ResumeId)
		on delete cascade,
	TitleClassId int
		references TitleClasses(TitleClassId)
		on delete cascade,
	primary key(ResumeId,TitleClassId)
)
GO
CREATE TABLE ResumeOpeningRecords
(
	ResumeOpeningRecordId int primary key identity,
	ResumeId int
		references Resumes(ResumeId)
		on delete cascade,
	OpeningId int
		references Openings(OpeningId)
		on delete cascade,
	OpeningTitle nvarchar(60),
	CompanyId int,
	CompanyName nvarchar(60),
	ApplyDate date,
	InterviewYN bit not null default(0),
	HireYN bit not null default(0)
)
GO
CREATE TABLE CandidateOpeningLikeRecords
(
	CandidateId int
		references Candidates(CandidateId)
		on delete cascade,
	OpeningId int
		references Openings(OpeningId)
		on delete cascade
	primary key(CandidateId,OpeningId)
)
GO
CREATE TABLE CompanyResumeLikeRecords
(
	CompanyId int
		references Companies(CompanyId)
		on delete cascade,
	ResumeId int
		references Resumes(ResumeId)
		on delete cascade
	primary key(CompanyId,ResumeId)
)
GO
CREATE TABLE TagClasses
(
	TagClassId int primary key identity(0,1),
	TagClassName nvarchar(30) not null
)
GO
CREATE TABLE Tags
(
	TagId int primary key identity,
	TagClassId int default(0)
		references TagClasses(TagClassId)
		on delete set default,
	TagName nvarchar(30) not null,
)
GO
CREATE TABLE OpeningTags
(
	OpeningId int
		references Openings(OpeningId)
		on delete cascade,
	TagId int
		references Tags(TagId)
		on delete cascade
	primary key(OpeningId,TagId)
)
GO
CREATE TABLE ResumeTags
(
	ResumeId int
		references Resumes(ResumeId)
		on delete cascade,
	TagId int
		references Tags(TagId)
		on delete cascade
	primary key(ResumeId,TagId)
)
GO
CREATE TABLE PricingPlans
(
	PlanId int primary key identity,
	Title nvarchar(40) not null,
	Intro nvarchar(100),
	Duration int not null,
	Price money not null
		CHECK(Price >= 0),
	Discount decimal(7,6) not null default(1)
		CHECK((Discount <= 1) AND (Discount > 0)),
	[Status] bit not null default(1),
)
GO
CREATE TABLE CompanyOrders
(
	OrderId nchar(22) primary key,
	CompanyId int
		references Companies(CompanyId)
		on delete set null,
	PlanId int
		references PricingPlans(PlanId)
		on delete set null,
	OrderNumber int not null,
	CompanyName nvarchar(60) not null,
	GUINumber nchar(8) not null,
	Title nvarchar(40) not null,
	Price money not null
		CHECK(Price >= 0), 
	OrderDate datetime not null,
	PayDate datetime,
	Duration int not null,
	[Status] bit not null default(0),
	StatusType nvarchar(20) not null,
	NewebPayStatus nvarchar(10),
	NewebPayMessage nvarchar(50),
	TradeNo nvarchar(20),
	PaymentType nvarchar(10),
	[IP] nvarchar(15),
	EscrowBank nvarchar(10),
)
GO
CREATE TABLE Notifications
(
	NotificationId int primary key identity,
	CompanyId int
		references Companies(CompanyId)
		on delete set null,
	CandidateId int
		references Candidates(CandidateId)
		on delete set null,
	OpeningId int,
	ResumeId int,
	[Status] nvarchar(10),
	SubjectLine nvarchar(60) not null,
	Content nvarchar(Max) not null,
	SendDate date not null,
	AppointmentDate date,
	AppointmentTime time,
	[Address] nvarchar(100),
	ReplyFirstYN bit not null default(0),
	ReplyYN bit not null default(0),
	Reply nvarchar(200), 
	ReplyTime datetime,
)
GO
CREATE TABLE Admins
(
	AdminId int primary key identity,
	PersonnelCode int not null,
	Email nvarchar(320) not null,
	[Password] nvarchar(16) not null,
	[Name] nvarchar(30) not null,
	Authority int,
	[Status] bit,
)
GO
CREATE TABLE OpinionLetters
(
	LetterId int primary key identity,
	CompanyId int
		references Companies(CompanyId)
		on delete set null,
	CandidateId int
		references Candidates(CandidateId)
		on delete set null,
	AdminId int
		references Admins(AdminId)
		on delete set null,
	Class nvarchar(30) not null,
	SubjectLine nvarchar(60) not null,
	Content nvarchar(Max) not null,
	Attachment varbinary(MAX),
	[Status] bit not null default(0),
	SendTime datetime not null,
)
GO
CREATE TABLE AdminRecords
(
	RecordId int primary key identity,
	AdminId int not null,
	Task nvarchar(50) not null,
	CRUD nvarchar(20) not null,
	[Time] datetime not null,
)