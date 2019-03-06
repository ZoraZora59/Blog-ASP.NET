use [Blog_ASPNETContext-20190306132606]
insert into UserLogin values ('zora759','12345678'),('abc123','12345678'),('qwe456','12345678')
insert into TextList (TextTitle, TextTag1, TextTag2, TextTag3, Account, Text) values ('SQLServer 约束','SQL','','','zora759','主键(primary key)约束、外键(foreign key)约束、唯一(unique)约束、检查(check)约束、默认值(default)约束实例

Oracle 有如下类型的约束:
NOT NULL(非空)、UNIQUE Key(唯一约束)、PRIMARY KEY(主键约束)、FOREIGN KEY(外键约束)、CHECK约束
Oracle使用SYS_Cn格式命名约束.
创建约束:在建表的同时创建、建表后创建
约束的类型有如下几种：
C (check constraint on a table)
P (primary key)
U (unique key)
R (Referential AKA Foreign Key)
V (with check option, on a view)
O (with read only, on a view)')
insert into CommitList(TextID, Account, CommitText) values (5000,'abc123','测试评论功能')
