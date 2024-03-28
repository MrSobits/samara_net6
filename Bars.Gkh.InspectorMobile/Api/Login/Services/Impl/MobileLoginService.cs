namespace Bars.Gkh.InspectorMobile.Api.Login.Services.Impl
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.ESIA.Auth.Dto;
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.BaseApiIntegration.Services.Impl;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.ConfigSections;
    using Bars.Gkh.InspectorMobile.Entities;

    using Castle.Core.Internal;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для авторизации пользователя мобильного приложения
    /// </summary>
    public class MobileLoginService : ApiLoginService, IMobileLoginService
    {
        private readonly IGkhConfigProvider gkhConfigProvider;
        private readonly IDomainService<MpRole> mpRoleDomainService;
        private readonly IDomainService<UserRole> userRoleDomainService;
        private readonly IDomainService<OperatorContragent> operatorContragentDomainService;
        private readonly IDomainService<EsiaOperator> esiaOperatorDomain;
        private readonly IDomainService<Contragent> contragentDomain;

        public MobileLoginService(
            IRepository<Operator> operatorRepository,
            IGkhConfigProvider gkhConfigProvider,
            IDomainService<MpRole> mpRoleDomainService,
            IDomainService<UserRole> userRoleDomainService,
            IDomainService<OperatorContragent> operatorContragentDomainService,
            IDomainService<EsiaOperator> esiaOperatorDomain,
            IDomainService<Contragent> contragentDomain)
            : base(operatorRepository)
        {
            this.gkhConfigProvider = gkhConfigProvider;
            this.mpRoleDomainService = mpRoleDomainService;
            this.userRoleDomainService = userRoleDomainService;
            this.operatorContragentDomainService = operatorContragentDomainService;
            this.esiaOperatorDomain = esiaOperatorDomain;
            this.contragentDomain = contragentDomain;
        }

        /// <inheritdoc />
        public override ValidateResult AuthorizeUser(long userId)
        {
            var baseResult = base.AuthorizeUser(userId);

            if (!baseResult.Success)
            {
                return baseResult;
            }

            var operatorContragents = this.operatorContragentDomainService.GetAll()
                .Where(x => x.Operator.Id == this.UserOperator.Id)
                .Select(x => new { x.Contragent.Id, x.Contragent.ShortName })
                .ToList();

            if (operatorContragents.Any())
            {
                var contragentInfo = operatorContragents.Count > 1
                    ? $"к организациям {string.Join(", ", operatorContragents.Select(x => x.ShortName))}, у которых"
                    : $"к организации {operatorContragents.Select(x => x.ShortName).First()}, у которой";
                var errorMsg =
                    $"Учетная запись пользователя привязана {contragentInfo} нет прав на работу в мобильном приложении";

                var permittedContragent = this.gkhConfigProvider.Get<InspectorMobileConfig>().Organization;
                if (permittedContragent == null)
                {
                    return ValidateResult.No(errorMsg);
                }

                var isUserContragentPermitted = operatorContragents.Select(x => x.Id).Contains(permittedContragent.Id);
                if (!isUserContragentPermitted)
                {
                    return ValidateResult.No(errorMsg);
                }
            }

            if (!this.UserOperator.MobileApplicationAccessEnabled)
            {
                return ValidateResult.No($"Для пользователя {this.UserOperator.User.Login} отсутствуют права доступа к работе в мобильном приложении");
            }

            var userRole = this.userRoleDomainService.GetAll().SingleOrDefault(x => x.User.Id == userId);
            var isPermittedUserRole = userRole != null &&
                this.mpRoleDomainService.GetAll().Any(x => x.Role == userRole.Role);

            if (!isPermittedUserRole)
            {
                return ValidateResult.No($"Роль пользователя {this.UserOperator.User.Login} не соответствует ролевой модели мобильного приложения");
            }

            return ValidateResult.Yes();
        }

        /// <inheritdoc />
        public async Task<ValidateResult> AuthorizeEsiaUser(UserInfoDto userInfo)
        {
            var organizationsOgrns = userInfo.Organizations.Select(y => y.Ogrn);
            var esiaOperators = this.esiaOperatorDomain.GetAll()
                .Where(x => x.UserId == userInfo.Id &&
                    organizationsOgrns.Contains(x.OrgOgrn));

            if (!esiaOperators.Any())
            {
                return ValidateResult.No($"Пользователю {userInfo.Name} необходимо предварительно выполнить вход в ГИС МЖФ РТ под учетной записью ЕСИА");
            }

            var esiaOperatorOgrns = esiaOperators.Select(x => x.OrgOgrn);
            var permittedContragentDto = this.gkhConfigProvider.Get<InspectorMobileConfig>().Organization;

            if (permittedContragentDto == null)
            {
                return ValidateResult.No("Учетная запись пользователя привязана к организации, у которой нет прав на работу в мобильном приложении");
            }

            var permittedContragent = await this.contragentDomain.GetAll().SingleAsync(x => x.Id == permittedContragentDto.Id);

            var permittedEsiaOrganizationOgrn = esiaOperatorOgrns
                .SingleOrDefault(x => x == permittedContragent.Ogrn);

            if (permittedEsiaOrganizationOgrn.IsNullOrEmpty())
            {
                return ValidateResult.No("Учетная запись пользователя привязана к организации, у которой нет прав на работу в мобильном приложении");
            }

            userInfo.SelectedOrganizationId = userInfo.Organizations
                .SingleOrDefault(x => x.Ogrn == permittedEsiaOrganizationOgrn)?
                .Id;

            if (userInfo.SelectedOrganizationId.IsNullOrEmpty())
            {
                return ValidateResult.No("Учетная запись пользователя привязана к организации, у которой нет прав на работу в мобильном приложении");
            }

            return this.AuthorizeUser(esiaOperators.Single(x => x.OrgOgrn == permittedEsiaOrganizationOgrn).Operator.User.Id);
        }
    }
}