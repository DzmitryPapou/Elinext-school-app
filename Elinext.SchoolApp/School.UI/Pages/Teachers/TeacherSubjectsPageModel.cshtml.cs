using School.UI.Data;
using School.UI.Models;
using School.UI.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace School.UI.Pages.Teachers
{
    public class TeacherSubjectsPageModel : PageModel
    {

        public List<AssignedSubjectData> AssignedSubjectDataList;

        public void PopulateAssignedSubjectData(SchoolContext context,
                                               Teacher teacher)
        {
            var allSubjects = context.Subjects;
            var teacherSubjects = new HashSet<int>(
                teacher.SubjectAssignments.Select(c => c.SubjectID));
            AssignedSubjectDataList = new List<AssignedSubjectData>();
            foreach (var subject in allSubjects)
            {
                AssignedSubjectDataList.Add(new AssignedSubjectData
                {
                    SubjectID = subject.SubjectID,
                    Title = subject.Title,
                    Assigned = teacherSubjects.Contains(subject.SubjectID)
                });
            }
        }

        public void UpdateTeacherSubjects(SchoolContext context,
            string[] selectedSubjects, Teacher teacherToUpdate)
        {
            if (selectedSubjects == null)
            {
                teacherToUpdate.SubjectAssignments = new List<SubjectAssignment>();
                return;
            }

            var selectedSubjectsHS = new HashSet<string>(selectedSubjects);
            var teacherSubjects = new HashSet<int>
                (teacherToUpdate.SubjectAssignments.Select(c => c.Subject.SubjectID));
            foreach (var subject in context.Subjects)
            {
                if (selectedSubjectsHS.Contains(subject.SubjectID.ToString()))
                {
                    if (!teacherSubjects.Contains(subject.SubjectID))
                    {
                        teacherToUpdate.SubjectAssignments.Add(
                            new SubjectAssignment
                            {
                                TeacherID = teacherToUpdate.ID,
                                SubjectID = subject.SubjectID
                            });
                    }
                }
                else
                {
                    if (teacherSubjects.Contains(subject.SubjectID))
                    {
                        SubjectAssignment subjectToRemove
                            = teacherToUpdate
                                .SubjectAssignments
                                .SingleOrDefault(i => i.SubjectID == subject.SubjectID);
                        context.Remove(subjectToRemove);
                    }
                }
            }
        }
    }
}