using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Application.Contracts
{
    public interface ITeacherMovmentRepository : IRepository<TeacherMovment>
    {
        IEnumerable<TeacherMovment> GetAllTeacherMovmentWithTeacher();
        TeacherMovment GetTeacherMovmentWithTeacherById(int id);
        TeacherMovment DeleteUsingMultiKey(TeacherMovment obj);
    }
}
