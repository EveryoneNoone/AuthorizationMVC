CREATE TABLE [dbo].[Users]
(
	[IdUser] INT NOT NULL PRIMARY KEY identity(1, 1)
	,[Name] varchar(50) not null
	,[Email] varchar(100) not null
,[Password] nvarchar(max) not null
)
go
create table [dbo].[Roles]
(
	[IdRole] int not null primary key identity(1, 1)
	,[Role] varchar(20) not null
)
go
create table [dbo].[UserRole]
(
	[Id] int not null primary key identity(1, 1)
	,[IdUser] int not null constraint FK_UserRole_IdUser foreign key(IdUser) references [dbo].[Users](IdUser)
	,[IdRole] int not null constraint FK_UserRole_IdRole foreign key(IdRole) references [dbo].[Roles](IdRole)
)
go
CREATE TABLE [dbo].[UserData]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1)
	,[IdUser] int not null constraint FK_UserData_IdUser foreign key(IdUser) references dbo.Users(IdUser)
	,[Sex] char(1) not null
	,[Age] int not null
	,[Earning] decimal not null
	,[IdPosition] int not null constraint FK_UserData_IdPosition foreign key(IdPosition) references dbo.Positions(IdPosition)
)
go
create table [dbo].[Departments]
(
	[IdDepartment] int not null primary key identity(1, 1)
	,[Name] varchar(50) not null 
)
go
create table [dbo].[Positions]
(
	[IdPosition] int not null primary key identity(1,1)
	,[Name] varchar(50) not null
	,[IdDepartment] int not null constraint FK_Positions_IdDepartment foreign key(IdDepartment) references dbo.Departments(IdDepartment)
)



