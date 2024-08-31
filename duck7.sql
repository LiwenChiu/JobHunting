USE [master]
GO
CREATE DATABASE [Duck]
GO
USE [Duck]
GO
CREATE TABLE TitleCategories
(
	TitleCategoryID nchar(1) primary key,
	TitleCategoryName nvarchar(40) not null
)
GO
CREATE TABLE TitleClasses
(
	TitleClassID nchar(2) primary key,
	TitleCategoryID nchar(1) not null
		references TitleCategories(TitleCategoryID),
	TitleClassName nvarchar(40) not null
)
GO
CREATE TABLE Companies
(
    CompanyID int primary key identity,
	GUINumber int not null,
	[Password] nvarchar(16) not null,
    CompanyName nvarchar(40) not null,
	[Address] nvarchar(100),
    Intro nvarchar(200),
	Benefits nvarchar(200),
    Picture varbinary(max),
    ContactName nvarchar(30) not null,
    ContactPhone nvarchar(24) not null,
    ContactEmail nvarchar(320) not null,
	[Status] bit not null default(0),
	[Date] datetime not null
)
GO
CREATE TABLE Openings
(
	OpeningID int primary key identity,
	CompanyID int not null
		references Companies(CompanyID)
		on delete cascade,
	Title nvarchar(60) not null,
	TitleClassID nchar(2)
		references TitleClasses(TitleClassID)
		on delete set null,
	[Address] nvarchar(100),
	[Description] nvarchar(300) not null,
	Benefits nvarchar(200),
	InterviewYN bit not null default(0),
	SalaryMax money,
	SalaryMin money,
	[Time] nvarchar(60),
	ContactName nvarchar(30) not null,
    ContactPhone nvarchar(24) not null,
    ContactEmail nvarchar(320),
	ReleaseYN bit not null default(0),
)
GO
CREATE TABLE Candidates
(
	CandidateID int primary key identity,
	NationalID nchar(10) not null,
	Email nvarchar(320) not null,
	[Password] nvarchar(16) not null,
	[Name] nvarchar(30),
	Sex bit,
	Birthday date,
	Phone nvarchar(24),
	[Address] nvarchar(100),
	Degree nvarchar(30),
	EmploymentStatus nvarchar(20),
	MilitaryService nvarchar(20),
)
CREATE TABLE Resumes
(
	ResumeID int primary key identity,
	CandidateID int not null
		references Candidates(CandidateID)
		on delete cascade,
	Title nvarchar(60) not null,
	Intro nvarchar(200),
	Autobiography nvarchar(Max),
	WorkExperience nvarchar(Max),
	Certification varbinary(Max),
	[Time] nvarchar(60),
	[Address] nvarchar(100),
	ReleaseYN bit not null default(1),
)
GO
CREATE TABLE ResumeOpeningRecords
(
	ResumeOpeningRecordID int primary key identity,
	ResumeID int
		references Resumes(ResumeID)
		on delete cascade,
	OpeningID int
		references Openings(OpeningID)
		on delete set null,
	CompanyID int not null,
	CompanyName nvarchar(40) not null,
	OpeningTitle nvarchar(60) not null,
	ApplyDate date,
	LikeYN bit not null default(0),
	InterviewYN bit not null default(0),
	HireYN bit not null default(0)
)
GO
CREATE TABLE CompanyResumeRecords
(
	CompanyID int not null
		references Companies(CompanyID)
		on delete cascade,
	ResumeID int not null
		references Resumes(ResumeID)
		on delete cascade,
	LikeYN bit not null default(0),
	InterviewYN bit,
	HireYN bit
	primary key(CompanyID,ResumeID)
)
GO
CREATE TABLE TagClasses
(
	TagClassID int primary key identity,
	TagClass nvarchar(30) not null
)
GO
CREATE TABLE Tags
(
	TagID int primary key identity,
	TagName nvarchar(30) not null,
	TagClassID int not null default(0)
		references TagClasses(TagClassID)
		on delete set default
)
GO
CREATE TABLE OpeningTags
(
	OpeningID int
		references Openings(OpeningID)
		on delete cascade,
	TagID int
		references Tags(TagID)
		on delete cascade
	primary key(OpeningID,TagID)
)
GO
CREATE TABLE ResumeTags
(
	ResumeID int
		references Resumes(ResumeID)
		on delete cascade,
	TagID int
		references Tags(TagID)
		on delete cascade
	primary key(ResumeID,TagID)
)
GO
CREATE TABLE PricingPlans
(
	PlanID int primary key identity,
	Title nvarchar(40) not null,
	Intro nvarchar(100),
	Duration int not null,
	Price money
		CHECK(Price >= 0),
	Discount decimal(5,4) default(1)
		CHECK((Discount <= 1) AND (Discount > 0))
)
GO
CREATE TABLE CompanyOrders
(
	OrderID int primary key identity,
	CompanyID int
		references Companies(CompanyID)
		on delete set null,
	PlanID int
		references PricingPlans(PlanID)
		on delete set null,
	CompanyName nvarchar(40) not null,
	GUINumber int not null,
	PlanTitle nvarchar(40) not null,
	Price money
		CHECK(Price >= 0), 
	OrderDate datetime not null,
	[Status] bit not null default(0),
)
GO
CREATE TABLE Notifications
(
	NotificationID int primary key identity,
	CompanyID int
		references Companies(CompanyID)
		on delete set null,
	CandidateID int
		references Candidates(CandidateID)
		on delete set null,
	ResumeID int,
	OpeningID int,
	[Status] nvarchar(10),
	SubjectLine nvarchar(60) not null,
	Content nvarchar(Max) not null,
	SendDate date not null,
	AppointmentTime datetime,
)
GO
CREATE TABLE Admins
(
	AdminID int primary key identity,
	PersonnelCode int not null,
	Email nvarchar(320) not null,
	[Password] nvarchar(16) not null,
	[Name] nvarchar(30) not null,
	Authority int
)
GO
CREATE TABLE OpinionLetters
(
	LetterID int primary key identity,
	CompanyID int
		references Companies(CompanyID)
		on delete set null,
	CandidateID int
		references Candidates(CandidateID)
		on delete set null,
	AdminID int
		references Admins(AdminID)
		on delete set null,
	Class nvarchar(30) not null,
	SubjectLine nvarchar(60) not null,
	Content nvarchar(Max) not null,
	Attachment varbinary(MAX),
	[Status] bit not null default(0)
)