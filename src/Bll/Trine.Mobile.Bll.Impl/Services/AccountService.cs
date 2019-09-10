using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sogetrel.Sinapse.Framework.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Messages;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;
using Xamarin.Essentials;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        public AccountService(IMapper mapper, Dal.Swagger.IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<string> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var credentials = new UserCredentialsModel(email, password);
            Token token;
            UserModel user;

            try
            {
                token = await _gatewayRepository.ApiAccountsLoginPostAsync(_mapper.Map<UserCredentials>(credentials));
                if (token == null || string.IsNullOrEmpty(token.UserId))
                    throw new BusinessException(ErrorMessages.unknownUserBodyText);

                var entity = await _gatewayRepository.ApiAccountsUsersByIdGetAsync(token.UserId);
                user = _mapper.Map<UserModel>(entity);

                if (user == null || string.IsNullOrEmpty(user.Id))
                    throw new BusinessException(ErrorMessages.unknownUserBodyText);

                var tokenModel = _mapper.Map<TokenModel>(token);
                AppSettings.AccessToken = tokenModel;
                AppSettings.CurrentUser = user;
                //await BlobCache.UserAccount.InsertObject(CacheKeys._CurrentUser, user);
                await SecureStorage.SetAsync(CacheKeys._CurrentUser, JsonConvert.SerializeObject(user));

                return tokenModel.UserId;
            }
            catch (ApiException apiExc)
            {
                if (apiExc.StatusCode == 401)
                    throw new BusinessException("Cet utilisateur n'existe pas. Veuillez vous inscrire via la page précédente.");
                else
                    throw new TechnicalException(ErrorMessages.serverErrorText + apiExc.StatusCode);
            }
        }

        public async Task<bool> DoesUserExist(RegisterUserModel model)
        {
            try
            {
                // Verifying if user exists
                var result = await _gatewayRepository.ApiAccountsUsersExistsGetAsync(model.Email);
                return (result != null);
            }
            catch // Il est normal d'avoir une exception ici (code 404 comme quoi l'user n'existe pas)
            {
                return false;
            }
        }

        public async Task<string> RegisterUser(RegisterUserModel model)
        {
            Token token = null;

            try
            {
                // Verifying if user exists
                var result = await _gatewayRepository.ApiAccountsUsersExistsGetAsync(model.Email);
                if (result != null)
                    throw new BusinessException(ErrorMessages.userAlreadyExists);
            }
            catch (ApiException apiexc) // Il est normal d'avoir une exception ici (code 404 comme quoi l'user n'existe pas)
            {
                if (apiexc.StatusCode != 404)
                    throw new TechnicalException(ErrorMessages.serverErrorText + apiexc.StatusCode);
            }

            try
            {
                // Registers user
                var result = await _gatewayRepository.ApiAccountsUsersRegisterPostAsync(_mapper.Map<RegisterUserRequest>(model));
                token = result.Token;

                _logger.LogTrace("New user subscription");

                //TODO : trop de calls, a optimiser
                var user = _mapper.Map<UserModel>(await _gatewayRepository.ApiAccountsUsersByIdGetAsync(token.UserId));
                if (user == null || string.IsNullOrEmpty(user.Id))
                    throw new BusinessException(ErrorMessages.unknownUserBodyText);

                AppSettings.AccessToken = _mapper.Map<TokenModel>(token);
                AppSettings.CurrentUser = user;
                //await BlobCache.UserAccount.InsertObject(CacheKeys._CurrentUser, user);
                await SecureStorage.SetAsync(CacheKeys._CurrentUser, JsonConvert.SerializeObject(user));

                return token.UserId;
            }
            catch (ApiException apiExc)
            {
                throw new TechnicalException(ErrorMessages.serverErrorText + apiExc.StatusCode);
            }
            catch (HttpRequestException httpExc)
            {
                throw new TechnicalException(ErrorMessages.serverErrorText + httpExc.Message);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> RegisterCompany(RegisterCompanyModel model)
        {
            try
            {
                string json = "";

                try
                {
                    // Verifying if company exists
                    var result = await _gatewayRepository.ApiAccountsCompaniesExistsGetAsync(model.Siret);
                    if (result != null)
                        throw new BusinessException(ErrorMessages.companyAlreadyExists);
                }
                catch (ApiException apiexc)
                {
                    if (apiexc.StatusCode != 204)
                        throw apiexc;
                }

                // Updates company
                json = await _gatewayRepository.ApiAccountsCompaniesRegisterPostAsync(_mapper.Map<RegisterCompanyRequest>(model));
                string id = JsonConvert.DeserializeObject<string>(json);
                return id;
            }
            catch (ApiException apiExc)
            {
                throw new TechnicalException(ErrorMessages.serverErrorText + apiExc.StatusCode);
            }
            catch
            {
                throw;
            }
        }

        public async Task RecoverPasswordAsync(PasswordUpdateModel model)
        {
            try
            {
                await _gatewayRepository.ApiAccountsUsersPasswordPostAsync(_mapper.Map<PasswordUpdate>(model));
            }
            catch (ApiException apiExc)
            {
                if (apiExc.StatusCode == 404)
                    throw new BusinessException(ErrorMessages.uknownUserHeaderText);
                else
                    throw new TechnicalException(ErrorMessages.serverErrorText + apiExc.StatusCode);
            }
            catch
            {
                throw;
            }
        }
    }
}
