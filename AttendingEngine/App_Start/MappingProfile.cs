using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendingEngine.Dtos;
using AttendingEngine.Models;
using AutoMapper;

namespace AttendingEngine.App_Start
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Student, StudentDto>();
            Mapper.CreateMap<StudentDto, Student>().ForMember(c => c.Id, opt => opt.Ignore()); ;
            Mapper.CreateMap<Class, ClassDto>();
            Mapper.CreateMap<ClassDto, Class>().ForMember(c => c.Id, opt => opt.Ignore()); ;
            Mapper.CreateMap<Course, StudentCourseDto>();
            Mapper.CreateMap<StudentCourseDto, Course>().ForMember(c => c.Id, opt => opt.Ignore()); ;
            Mapper.CreateMap<Teacher, TeacherDto>();
            Mapper.CreateMap<TeacherDto, Teacher>().ForMember(c => c.Id, opt => opt.Ignore()); ;
            Mapper.CreateMap<Lesson, LessonDto>();
            Mapper.CreateMap<LessonDto, Lesson>().ForMember(c => c.Id, opt => opt.Ignore()); ;


        }
    }
}