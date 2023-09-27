using Laboratories.Application.Contracts;
using Laboratories.Domain;
using Laboratories.Persistence.Repositories;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Laboratories.Persistence
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
      
            container.RegisterType<IComplexRepository, ComplexRepository>();
            container.RegisterType<ISchoolRepository, SchoolRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType(typeof(IRepository<>), typeof(BaseRepository<>));
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IScheduleHedRepository, ScheduleHedRepository>();
            container.RegisterType<ITeacherMovmentRepository, TeacherMovmentRepository>();
            container.RegisterType<IExperimentRepository, ExperimentRepository>();
            container.RegisterType<IStudentGradesHedRepository, StudentGradesHedRepository>();
            container.RegisterType<IStudentGradesDetRepository, StudentGradesDetRepository>();
            container.RegisterType<IStudentRepository, StudentRepository>();
            container.RegisterType<IUserSchoolsRepository, UserSchoolsRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}