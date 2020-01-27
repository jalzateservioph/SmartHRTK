using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK = TKProcessor.Models.TK;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // application wide

            CreateMap<User, TK.User>();
            CreateMap<TK.User, User>();

            // Maintenance - Employee related mappings

            CreateMap<Employee, TK.Employee>();
            CreateMap<TK.Employee, Employee>().AfterMap((empDto, emp) =>
            {
                emp.IsDirty = false;

                if (emp.EmployeeWorkSites == null)
                    emp.EmployeeWorkSites = new System.Collections.ObjectModel.ObservableCollection<EmployeeWorkSite>();
            });

            CreateMap<TK.EmployeeWorkSite, EmployeeWorkSite>();
            CreateMap<EmployeeWorkSite, TK.EmployeeWorkSite>();

            CreateMap<WorkSite, TK.WorkSite>();
            CreateMap<TK.WorkSite, WorkSite>();


            // Maintenance - Shift related mappings

            CreateMap<Shift, TK.Shift>();
            CreateMap<TK.Shift, Shift>()
               .AfterMap((model, appmodel) => { appmodel.IsDirty = false; });

            CreateMap<Shift, Shift>();


            // Maintenance - Work Schedule related mappings

            CreateMap<WorkSchedule, TK.WorkSchedule>();
            CreateMap<TK.WorkSchedule, WorkSchedule>();

            CreateMap<WorkSchedule, WorkSchedule>();

            CreateMap<Employee, Employee>();
            CreateMap<Employee, Employee>();

            CreateMap<Shift, Shift>();
            CreateMap<Shift, Shift>();

            CreateMap<User, User>();
            CreateMap<User, User>();
        }
    }
}
