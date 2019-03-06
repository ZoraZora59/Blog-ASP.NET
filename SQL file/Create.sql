create table UserLogin
(
  Account varchar(8) primary key ,
  Password varchar(11) not null
)
create table TextList
(
  TextID  int identity(5000,1) primary key ,
  TextTitle nvarchar(40) not null,
  Tag varchar(12) not null foreign key references TagList(Tag),
  Account varchar(8) not null foreign key references UserLogin(Account),
  Text nvarchar(4000) not null,
  Hot int not null default 0,
  TextChangeDate datetime not null default getdate()
)
create table TagList
(
  Tag varchar(12) not null primary key ,
  Hot int not null default 0,
  TextID int not null foreign key references TextList(TextID)
)
create table CommitList
(
  TextID int not null foreign key references TextList(TextID),
  CommitID int identity(10000,1) primary key ,
  Account varchar(8) not null foreign key references UserLogin(Account),
  CommitText nvarchar(100) not null ,
  CommitChangeDate datetime not null default getdate()
)
ALTER TABLE TextList
ADD FOREIGN KEY (Tag)
REFERENCES TagList(Tag)