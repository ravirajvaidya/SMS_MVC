select * from Users

select * from Teachers

DELETE FROM Teachers WHERE id = 1002;

DBCC CHECKIDENT ('Teachers', RESEED, 2);
