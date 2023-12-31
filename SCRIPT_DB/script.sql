--SAIR DA BASE ATUAL
USE master
go
--CRIAR BASE CLINICA
CREATE DATABASE Clinica
GO
--USAR BASE CLINICA
USE Clinica
GO
 --CRIAR TABELA PERSON
Create Table dbo.Person(
	Id int identity(1,1) not null,
	 name   varchar(max) not null,
	 email   varchar(max) not null,
	PRIMARY KEY(Id),
 )
GO
--CRIAR TABELA USERS
Create Table dbo.Users(
Id int identity,
PersonId int,
UserName varchar(50) not null,
Password varchar(50) not null,
Creation_Date datetime not null,
Last_login datetime null,
Active bit,
PRIMARY KEY(Id),
FOREIGN KEY (PersonId) references dbo.Person(Id));
