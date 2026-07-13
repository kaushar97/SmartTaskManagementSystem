using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Repository;

namespace TaskManagement.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskDTO> CreateTaskAsync(AddTaskRequestDTO addTaskRequestDTO)
        {
            var taskDomainModel = new TaskItem
            {
                Title = addTaskRequestDTO.Title,
                Description = addTaskRequestDTO.Description,
                Status = addTaskRequestDTO.Status,
                Priority = addTaskRequestDTO.Priority,
                DueDate = addTaskRequestDTO.DueDate,
                AssignedToId = addTaskRequestDTO.AssignedToId,
            };
            taskDomainModel = await _taskRepository.CreateTaskAsync(taskDomainModel);
            return new TaskDTO
            {
                PublicId = taskDomainModel.PublicId,
                Title = taskDomainModel.Title,
                Description = taskDomainModel.Description,
                Status = taskDomainModel.Status,
                Priority = taskDomainModel.Priority,
                DueDate = taskDomainModel.DueDate,
                CreatedDate = taskDomainModel.CreatedDate,
            };
        }

        public async Task<TaskDTO?> DeleteTaskAsync(int Id)
        {
            var taskDomainModel = await _taskRepository.DeleteTaskAsync(Id);
            if(taskDomainModel == null) { return null; }

            return new TaskDTO
            {
                PublicId = taskDomainModel.PublicId,
                Title = taskDomainModel.Title,
                Description = taskDomainModel.Description,
                Status = taskDomainModel.Status,
                Priority = taskDomainModel.Priority,
                DueDate = taskDomainModel.DueDate,
                CreatedDate = taskDomainModel.CreatedDate,
            };
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync()
        {
            var taskDomainModel = await _taskRepository.GetAllTasksAsync();
            var taskDTOs = taskDomainModel.Select(domain => new TaskDTO
            {
                Title = domain.Title,
                Description = domain.Description,
                Status = domain.Status,
                Priority = domain.Priority,
                DueDate = domain.DueDate,
                AssignedToPublicId = domain.AssignedTo.PublicId,
                CreatedDate = domain.CreatedDate,
            });
            return taskDTOs;
        }

        public async Task<TaskDTO?> GetTaskByIdAsync(Guid Id)
        {
            var taskDomainModel = await _taskRepository.GetTaskByIdAsync(Id);
            if (taskDomainModel == null) { return null; }
            return new TaskDTO
            {
                Title = taskDomainModel.Title,
                Description = taskDomainModel.Description,
                Status = taskDomainModel.Status,
                Priority = taskDomainModel.Priority,
                DueDate = taskDomainModel.DueDate,
                AssignedToPublicId = taskDomainModel.AssignedTo.PublicId,
                CreatedDate = taskDomainModel.CreatedDate,
            };
        }
        
        public async Task<TaskDTO?> UpdateTaskAsync(UpdateTaskRequestDTO updateTaskRequestDTO, Guid publicId, UserDTO userDetails)
        {
            var taskDomainModel = await _taskRepository.UpdateTaskAsync(updateTaskRequestDTO, publicId, userDetails.Id);
            if (taskDomainModel == null) { return null; }
            return new TaskDTO
            {
                PublicId = taskDomainModel.PublicId,
                Title = taskDomainModel.Title,
                Description = taskDomainModel.Description,
                Status = taskDomainModel.Status,
                Priority = taskDomainModel.Priority,
                DueDate = taskDomainModel.DueDate,
                AssignedToPublicId =  userDetails.PublicId,
                CreatedDate = taskDomainModel.CreatedDate,
            };
        }
    }
}
