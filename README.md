# Elinext School App, core 2.2 mvc, ef

The app (School)
- Models: Students, teachers, classes
- Filter for students by selected parameters Gender and Class
- Ability to perform CRUD operations on teachers, classes and students
- 3 level application architecture
- Linking class-student models one to many
- Linking class-teacher models many to many
- As such, no design is needed, but the application should have a neat: look, layout, and style sheet.

Technologies used:
- .NET Core
- Web API + Razor
- Entity Framework (Code-First) + Migration

Basic scripts and screens:
1. Using login form log in as administrator (standard authentication)
2. Add, delete, edit subjects on the page with a list of subjects
3. on the page with the list of teachers add, delete, edit teachers (name, subject - choose from the list, position - principal, deputy principal, teacher) - operations on separate pages
4. On the page with the list of pupils we are adding, removing, editing the pupil (name, sex, day of birth, age - considered automatically)
5. Apply filter to the list of students by arbitrary part of the last name (not case sensitive)
6. Add/delete/edit class (name, class teacher from the list of teachers) on the page with class list - operations are made in popup windows
7. On the class page, edit the list of subjects and teachers of the class (the teacher is selected for the selected subject)
8. Make a logout
9. Use the login form to login as a teacher
10. See the list of classes the teacher teaches, and for each class we see a list of his students


Credentials:
- teacher's role credentials - login : teacher@gmail.com, password : Teacher_12345  

- administrator's credentials - login: admin@gmail.com, password : Admin_12345


