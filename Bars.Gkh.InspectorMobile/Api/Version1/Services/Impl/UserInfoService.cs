namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.User;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для работы с данными о пользователе
    /// </summary>
    public class UserInfoService : IUserInfoService
    {
        private readonly IGkhUserManager gkhUserManager;
        private readonly IDomainService<Operator> operatorDomain;
        private readonly IDomainService<User> userDomain;
        private readonly IDomainService<UserRole> userRoleDomain;

        public UserInfoService(IGkhUserManager gkhUserManager, IDomainService<Operator> operatorDomain, IDomainService<User> userDomain, IDomainService<UserRole> userRoleDomain)
        {
            this.gkhUserManager = gkhUserManager;
            this.operatorDomain = operatorDomain;
            this.userDomain = userDomain;
            this.userRoleDomain = userRoleDomain;
        }

        /// <inheritdoc />
        public Task<UserInfoGet> GetAsync(DateTime? date)
        {
            var currOperator = this.gkhUserManager.GetActiveOperator();

            if (date.HasValue && currOperator.ObjectEditDate < date)
            {
                return Task.FromResult((UserInfoGet)null);
            }
            
            return Task.FromResult(new UserInfoGet
            {
                FullName = currOperator.User?.Name,
                Position = currOperator.Inspector?.Position,
                Mail = currOperator.User?.Email,
                Telephone = currOperator.Phone,
                Photo = currOperator.UserPhoto?.Id
            });
        }

        /// <inheritdoc />
        public async Task PutAsync(UserInfoUpdate model)
        {
            var currOperator = this.gkhUserManager.GetActiveOperator();
            currOperator.Phone = model.Telephone;

            if (model.Photo.HasValue)
            {
                currOperator.UserPhoto = new FileInfo { Id = model.Photo.Value };
            }

            var currUser = currOperator.User;
            currUser.Name = model.FullName;
            currUser.Email = model.Mail;

            if (currOperator.Role == null)
            {
                currOperator.Role = (await this.userRoleDomain.GetAll().FirstOrDefaultAsync(x => x.User == currUser))?.Role;
            }
            
            this.userDomain.Update(currUser);
            this.operatorDomain.Update(currOperator);
        }
    }
}