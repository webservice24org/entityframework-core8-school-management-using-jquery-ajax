-- Insert data into Instructor table
INSERT INTO Instructors (InstructorID, FirstName, LastName, JoinDate, InstructorPicture)
VALUES 
(1, 'John', 'Doe', '2020-01-15', 'john_doe.jpg'),
(2, 'Jane', 'Smith', '2019-03-22', 'jane_smith.jpg');

-- Insert data into Department table
INSERT INTO Departments (DepartmentID, DepartmentName, Budget, StartDate, InstructorID)
VALUES 
(1, 'Computer Science', 200000.00, '2018-09-01', 1),
(2, 'Mathematics', 150000.00, '2019-01-01', 2);

-- Insert data into Course table
INSERT INTO Courses (CourseId, CourseName, DepartmentID)
VALUES 
(1, 'Introduction to Programming', 1),
(2, 'Advanced Mathematics', 2),
(3, 'Data Structures', 1);

-- Insert data into Student table
INSERT INTO Students (StudentId, StudentName, Dob, Mobile, ImageUrl, CourseId)
VALUES 
(1, 'Alice Johnson', '2000-05-12', '1234567890', 'alice_johnson.jpg', 1),
(2, 'Bob Brown', '1999-08-23', '0987654321', 'bob_brown.jpg', 2);

-- Insert data into Enrollment table
INSERT INTO Enrollments (EnrollmentId, CourseId, StudentId)
VALUES 
(1, 1, 1),
(2, 2, 2);

-- Insert data into OfficeAssignment table
INSERT INTO OfficeAssignments (InstructorID, Location)
VALUES 
(1, 'Room 101'),
(2, 'Room 102');

-- Insert data into CourseInstructor table
INSERT INTO CourseInstructors (CourseId, InstructorID)
VALUES 
(1, 1),
(2, 2),
(3, 1);
