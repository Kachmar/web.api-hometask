using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Models;
using Services.Validators;

namespace Services
{
    public class HomeTaskService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<HomeTask> _homeTaskRepository;

        public HomeTaskService()
        {

        }

        public HomeTaskService(IRepository<Course> courseRepository, IRepository<HomeTask> homeTaskRepository)
        {
            _courseRepository = courseRepository;
            _homeTaskRepository = homeTaskRepository;
        }

        public virtual ValidationResponse<HomeTask> CreateHomeTask(HomeTask homeTask)
        {
            var response = ValidateHomeTask(homeTask);
            if (response.HasErrors)
            {
                return response;
            }
            var all = _homeTaskRepository.GetAll();

            if (all.Any(p => p.Title == homeTask.Title))
            {
                return new ValidationResponse<HomeTask>("title", $"HomeTask with title '{homeTask.Title}' already exists.");
            }
            var course = _courseRepository.GetById(homeTask.CourseId);
            homeTask.Course = course;
            var createdHomeTask = _homeTaskRepository.Create(homeTask);
            return new ValidationResponse<HomeTask>(createdHomeTask);
        }

        public virtual HomeTask GetHomeTaskById(int id)
        {
            return _homeTaskRepository.GetById(id);
        }

        public virtual ValidationResponse UpdateHomeTask(HomeTask homeTask)
        {
            var response = ValidateHomeTask(homeTask);
            if (response.HasErrors)
            {
                return response;
            }
           
            _homeTaskRepository.Update(homeTask);
            return new ValidationResponse();
        }

        public virtual void DeleteHomeTask(int homeTaskId)
        {
            var homeTask = _homeTaskRepository.GetById(homeTaskId);
            if (homeTask == null)
            {
                throw new ArgumentException($"Cannot find homeTask with id '{homeTaskId}'");
            }
            _homeTaskRepository.Remove(homeTaskId);
        }

        public virtual List<HomeTask> GetAllHomeTasks()
        {
            return _homeTaskRepository.GetAll();
        }

        private ValidationResponse<HomeTask> ValidateHomeTask(HomeTask homeTask)
        {
            if (homeTask == null)
            {
                return new ValidationResponse<HomeTask>("homeTask", "HomeTask cannot be null");
            }

            return new ValidationResponse<HomeTask>(homeTask);
        }
    }
}
