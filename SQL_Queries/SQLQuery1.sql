Select * from Roles

delete from users


DBCC CHECKIDENT ('Users', RESEED, 0);

INSERT INTO Users (UserName, Email, Password, RoleId)
VALUES ('Ravi', 'test@example.com', 'abc123', 1)