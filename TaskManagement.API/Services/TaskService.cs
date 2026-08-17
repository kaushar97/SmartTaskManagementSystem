using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Model.Domain;
using TaskManagement.API.Model.DTO;
using TaskManagement.API.Repository;
using static TaskManagement.API.Model.Domain.Enum;

namespace TaskManagement.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public TaskService(ITaskRepository taskRepository, ICurrentUserService currentUserService, IUserService userService)
        {
            _taskRepository = taskRepository;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<TaskDTO?> CreateTaskAsync(AddTaskRequestDTO addTaskRequestDTO)
        {
            var identityId = _currentUserService.IdentityUserId;

            if(identityId == null)
            {
                return null;
            }

            var userCreatedDetails = await _userService.GetByIdentityUserIdAsync(identityId);
            var userAssignedDetails = await _userService.GetByPublicIdAsync(addTaskRequestDTO.AssignedToPublicId);

            if(userCreatedDetails == null || userAssignedDetails == null)
            {
                return null;
            }

            var taskDomainModel = new TaskItem
            {
                Title = addTaskRequestDTO.Title,
                Description = addTaskRequestDTO.Description,
                Status = addTaskRequestDTO.Status,
                Priority = addTaskRequestDTO.Priority,
                DueDate = addTaskRequestDTO.DueDate,
                AssignedToId = userAssignedDetails.Id,
                CreatedById = userCreatedDetails.Id,
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
                AssignedToPublicId = userAssignedDetails.PublicId,
                AssignedToName = $"{userAssignedDetails.FirstName} {userAssignedDetails.LastName}",
                CreatedByName = $"{userCreatedDetails.FirstName} {userCreatedDetails.LastName}"
            };
        }

        public async Task<TaskDTO?> DeleteTaskAsync(Guid PublicId)
        {
            var taskDomainModel = await _taskRepository.DeleteTaskAsync(PublicId);
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
                AssignedToPublicId = taskDomainModel.AssignedTo.PublicId,
                AssignedToName = $"{taskDomainModel.AssignedTo.FirstName} {taskDomainModel.AssignedTo.LastName}",
                CreatedByName = $"{taskDomainModel.CreatedBy.FirstName} {taskDomainModel.CreatedBy.LastName}"
            };
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync(string? filterOn = null, TskStatus? filterQuery = null, 
            TaskPriority? priority = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var taskDomainModel = await _taskRepository.GetAllTasksAsync(filterOn, filterQuery, priority, 
                sortBy, isAscending, pageNumber, pageSize);
            var taskDTOs = taskDomainModel.Select(domain => new TaskDTO
            {
                PublicId = domain.PublicId,
                Title = domain.Title,
                Description = domain.Description,
                Status = domain.Status,
                Priority = domain.Priority,
                DueDate = domain.DueDate,
                AssignedToPublicId = domain.AssignedTo.PublicId,
                CreatedDate = domain.CreatedDate,
                AssignedToName = $"{domain.AssignedTo.FirstName} {domain.AssignedTo.LastName}",
                CreatedByName = $"{domain.CreatedBy.FirstName} {domain.CreatedBy.LastName}"
            });
            return taskDTOs;
        }

        public async Task<TaskDTO?> GetTaskByIdAsync(Guid Id)
        {
            var taskDomainModel = await _taskRepository.GetTaskByIdAsync(Id);
            if (taskDomainModel == null) { return null; }
            return new TaskDTO
            {
                PublicId = taskDomainModel.PublicId,
                Title = taskDomainModel.Title,
                Description = taskDomainModel.Description,
                Status = taskDomainModel.Status,
                Priority = taskDomainModel.Priority,
                DueDate = taskDomainModel.DueDate,
                AssignedToPublicId = taskDomainModel.AssignedTo.PublicId,
                CreatedDate = taskDomainModel.CreatedDate,
                AssignedToName = $"{taskDomainModel.AssignedTo.FirstName} {taskDomainModel.AssignedTo.LastName}",
                CreatedByName = $"{taskDomainModel.CreatedBy.FirstName} {taskDomainModel.CreatedBy.LastName}"
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
                AssignedToName = $"{userDetails.FirstName} {userDetails.LastName}",
                CreatedByName = $"{taskDomainModel.CreatedBy.FirstName} {taskDomainModel.CreatedBy.LastName}"
            };
        }
    }
}
