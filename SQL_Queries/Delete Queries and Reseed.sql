Delete from Subjects;
Delete from Grades;
Delete from Section;
Delete from Users;
Delete from Teachers;
Delete from Classes;
Delete from TeacherAndClass;
Delete from ClassAndSubject;

DBCC CHECKIDENT ('Classes', RESEED, 0);