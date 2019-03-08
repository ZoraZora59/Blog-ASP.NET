create table User
(
  Account varchar(8) primary key ,
  Password varchar(11) not null
)
create table TextList
(
  TextID  int identity(5000,1) primary key ,
  TextTitle nvarchar(40) not null,
  Tag varchar(12) not null,
  Account varchar(8) not null foreign key references UserLogin(Account),
  Text nvarchar(4000) not null,
  TextChangeDate datetime not null default getdate()
)
create table CommitList
(
  TextID int not null foreign key references TextList(TextID),
  CommitID int identity(10000,1) primary key ,
  Account varchar(8) not null foreign key references UserLogin(Account),
  CommitText nvarchar(100) not null ,
  CommitChangeDate datetime not null default getdate()
)