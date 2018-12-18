create database QuanLyQuanCafe
go
use QuanLyQuanCafe
go
--food
--table 
--foodcategory
--Account
--Bill
--BillInfo
create table TableFood
(
	id int IDENTITY primary key,
	name nvarchar(100) not null default N'Bàn chưa co tên',
	status nvarchar(100) not null default N'Trống'
	--Troongs||co nguoi

)
go

create table Account
(
	UserName nvarchar(100) primary key,
	DisplayName nvarchar(100) not null default N'Admin',
	PassWord nvarchar(100) not null default 0,
	Type int not null default 0
)
go

create table FoodCategory
(
	id int IDENTITY primary key,
	name nvarchar(100) not null default N'Chưa đặt tên'
)
go

create table Food
(
	id int IDENTITY primary key,
	name nvarchar(100) not null default N'Chưa đặt tên',
	idCategory int not null,
	price float not null default 0
	foreign key (idCategory) references dbo.FoodCategory(id)
)
go

create table Bill
(
	id int IDENTITY primary key,
	DateCheckIn date not null default GETDATE(),
	DateCheckOut date,
	idTable int not null,
	status int not null default 0
	foreign key(idTable) references dbo.TableFood(id)
)
go

create table Billinfo
(
	id int IDENTITY primary key,
	idBill int not null,
	idFood int not null,
	count int not null default 0
	foreign key(idBill) references dbo.Bill(id),
	foreign key(idFood) references dbo.Food(id)
)
go
