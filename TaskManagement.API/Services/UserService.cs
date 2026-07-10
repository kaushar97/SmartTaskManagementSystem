using TaskManagement.API.Model.DTO;
using TaskManagement.API.Repository;


namespace TaskManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDTO?> GetByPublicIdAsync(Guid publicId)
        {
            var userDomainModel = await _userRepository.GetByPublicIdAsync(publicId);
            if (userDomainModel == null) { return null; };
            return new UserDTO
            {
                Id = userDomainModel.Id,
                PublicId = userDomainModel.PublicId,
                FirstName = userDomainModel.FirstName,
                LastName = userDomainModel.LastName,
                Email = userDomainModel.Email,
            };
        }
    }
}
