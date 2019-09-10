using AutoMapper;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Extensions;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Dal;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IImageAttachmentStorageRepository _imageAttachmentStorageRepository;

        public UserService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger, IImageAttachmentStorageRepository imageAttachmentStorageRepository) : base(mapper, gatewayRepository, logger)
        {
            _imageAttachmentStorageRepository = imageAttachmentStorageRepository;
        }

        public async Task DeleteUser(string id)
        {
            try
            {
                await _gatewayRepository.ApiUsersDeleteAsync(id);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var users = await _gatewayRepository.ApiUsersGetAsync();
                var mappedUsers = _mapper.Map<List<UserModel>>(users);
                return mappedUsers;
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<List<UserModel>> SearchUsers(string email = null, string firstname = null, string lastname = null, string companyName = null)
        {
            try
            {
                var users = await _gatewayRepository.ApiUsersSearchGetAsync(email, firstname, lastname, companyName);
                return _mapper.Map<List<UserModel>>(users);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            try
            {
                await _gatewayRepository.ApiUsersByIdPutAsync(user.Id, _mapper.Map<User>(user));
                return user;
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<UserModel> UploadProfilePicture(Stream stream, UserModel user)
        {
            try
            {
                // Image Loading
                var image = Image.Load<Rgba32>(stream);
                _logger.LogTrace("Image attachment loaded");

                var imageEncoder = JpegFormat.Instance;
                var bytes = image.Resize(512).GetBytes(imageEncoder);

                _logger.LogTrace("Image attachment rendered");

                // Image upload to Azure
                var fileExtension = imageEncoder.FileExtensions.FirstOrDefault();
                var uri = await _imageAttachmentStorageRepository.UploadToStorage(bytes, $"{Guid.NewGuid()}.{fileExtension}", imageEncoder.DefaultMimeType);
                _logger.LogTrace("Image attachment uploaded to Azure");

                // Updating the user with the new profile pic uri
                user.ProfilePicUrl = uri.AbsoluteUri;
                return await UpdateUser(user);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}
