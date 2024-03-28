namespace Bars.B4.Modules.ESIA.Auth.Services.Impl
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.Auth.Dto;
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис OAuth авторизации
    /// </summary>
    public class OAuthLoginService : IOAuthLoginService
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер логирования
        /// </summary>
        public ILogManager LogManager { get; set; }

        /// <summary>
        /// Сервис интеграции с приложением авторизации
        /// </summary>
        public IAuthAppIntegrationService AuthAppIntegrationService { get; set; }

        /// <inheritdoc />
        public IDataResult<UserInfoDto> GetUserInfo(string code)
        {
            try
            {
                var contragentDomain = this.Container.ResolveDomain<Contragent>();

                using (this.Container.Using(contragentDomain))
                {
                    if (code == null)
                    {
                        return this.Error<UserInfoDto>("Не передан код авторизации. " +
                            "Возможно, доступ к запрашиваемым данным не был предоставлен");
                    }

                    var esiaOAuthToken = this.AuthAppIntegrationService.GetOAuthToken(code);

                    var personInfo = this.AuthAppIntegrationService.GetPersonInfo(esiaOAuthToken);

                    var userInfo = new UserInfoDto
                    {
                        Id = personInfo.Id,
                        Name = personInfo.Name,
                        FirstName = personInfo.FirstName,
                        LastName = personInfo.LastName,
                        MiddleName = personInfo.MiddleName,
                        Gender = personInfo.Gender,
                        BirthDate = personInfo.BirthDate?.ToString()
                    };

                    if (!personInfo.Trusted)
                    {
                        return this.Error("Учетная запись не подтверждена", userInfo);
                    }

                    userInfo.Organizations = this.AuthAppIntegrationService.GetPersonOrganizationsInfo(esiaOAuthToken)
                        .Select(x => new OrganizationInfoDto
                        {
                            Id = x.Id,
                            ShortName = x.ShortName,
                            FullName = x.FullName,
                            Ogrn = x.Ogrn,
                            IsActive = x.IsActive
                        })
                        .ToList();

                    if (!userInfo.Organizations.Any())
                    {
                        return this.Error("В ЕСИА не привязана организация", userInfo);
                    }

                    if (userInfo.Organizations.Count > 1)
                    {
                        return this.Error("У пользователя зарегистрировано несколько организаций. " +
                            "Пожалуйста, выберите организацию, от имени которой вы будете работать в Cистеме",
                            userInfo);
                    }

                    userInfo.SelectedOrganizationId = userInfo.Organizations.First().Id;

                    var contragentMatching = this.MatchOrganizationWithContragent(userInfo);

                    if (!contragentMatching.Success)
                    {
                        return this.Error(contragentMatching.Message, userInfo);
                    }

                    var loginResult = this.PerformLoginActions(userInfo);
                    if (!loginResult.Success)
                    {
                        return this.Error<UserInfoDto>(loginResult.Message);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message == "Token start time has not come")
                {
                    return this.Error<UserInfoDto>("Ошибка логина через OAuth. Возможно, причина в отставании времени на сервере приложения от времени ЕСИА");
                }

                this.LogManager.Error("Ошибка логина через OAuth", e);
                return this.Error<UserInfoDto>("Ошибка отправки запроса к ЕСИА");
            }

            return new GenericDataResult<UserInfoDto>(message: "Пользователь успешно авторизован");
        }

        /// <inheritdoc />
        public IDataResult MatchOrganizationWithContragent(UserInfoDto userInfo)
        {
            var contragentDomain = this.Container.ResolveDomain<Contragent>();
            var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();

            using (this.Container.Using(contragentDomain, operatorContragentDomain, esiaOperatorDomain))
            {
                var selectedOrganization = userInfo.SelectedOrganization;

                var contragentList = esiaOperatorDomain.GetAll()
                    .Join(operatorContragentDomain.GetAll(),
                        x => x.Operator.Id,
                        y => y.Operator.Id,
                        (x, y) => new
                        {
                            esiaOperator = x,
                            operatorContragent = y.Contragent
                        })
                    .Where(x => x.esiaOperator.UserId == userInfo.Id &&
                        x.esiaOperator.OrgOgrn == selectedOrganization.Ogrn &&
                        x.operatorContragent.Ogrn == selectedOrganization.Ogrn)
                    .Select(x => new ContragentInfoDto
                    {
                        Id = x.operatorContragent.Id,
                        ShortName = x.operatorContragent.ShortName
                    })
                    .ToList();

                if (contragentList.Count > 1)
                {
                    return new BaseDataResult(false,
                        $"При определении привязки организации \"{selectedOrganization.FullName}\" произошла ошибка. " +
                        "Пожалуйста, обратитесь к оператору Системы");
                }

                if (contragentList.Count == 0)
                {
                    contragentList = contragentDomain.GetAll()
                        .Where(x => x.Ogrn == selectedOrganization.Ogrn)
                        .Select(x => new ContragentInfoDto
                        {
                            Id = x.Id,
                            ShortName = x.ShortName
                        })
                        .ToList();
                }

                if (contragentList.Count == 1)
                {
                    userInfo.SelectedContragentId = contragentList.First().Id;
                    return new BaseDataResult();
                }

                var errorMessage = string.Empty;

                if (contragentList.Count > 1)
                {
                    errorMessage = $"Для организации \"{selectedOrganization.FullName}\" " +
                        $"по ОГРН \"{selectedOrganization.Ogrn}\" найдено сразу несколько контрагентов. " +
                        "Пожалуйста, выберите тот, к которому относится ваша учетная запись";
                }
                else
                {
                    errorMessage = $"Для организации \"{selectedOrganization.FullName}\" доступ запрещен. " +
                        "Рекомендуем обратиться к оператору Системы";
                }

                userInfo.MatchedContragents = contragentList;

                return new BaseDataResult(false, errorMessage);
            }
        }

        /// <inheritdoc />
        public IDataResult PerformLoginActions(UserInfoDto userInfo)
        {
            try
            {
                var contragentDomain = this.Container.ResolveDomain<Contragent>();
                var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
                var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();

                var userManager = this.Container.Resolve<IGkhUserManager>();

                using (this.Container.Using(contragentDomain,
                    operatorContragentDomain, esiaOperatorDomain, userManager))
                {
                    var context = HttpContext.Current;

                    var selectedOrganization = userInfo.SelectedOrganization;

                    var existsEsiaOperator = esiaOperatorDomain.GetAll()
                        .FirstOrDefault(x => x.UserId == userInfo.Id &&
                            x.OrgOgrn == selectedOrganization.Ogrn);

                    if (existsEsiaOperator != null &&
                        existsEsiaOperator.IsActive != selectedOrganization.IsActive)
                    {
                        existsEsiaOperator.IsActive = selectedOrganization.IsActive;
                        esiaOperatorDomain.Update(existsEsiaOperator);
                    }

                    if (!selectedOrganization.IsActive)
                    {
                        return new BaseDataResult(false, "Учетная запись ЕСИА заблокирована");
                    }

                    if (existsEsiaOperator != null)
                    {
                        this.AddLoginInfoToCookies(context, existsEsiaOperator, userInfo);
                        return new BaseDataResult();
                    }

                    var contragent = contragentDomain.FirstOrDefault(x => x.Id == userInfo.SelectedContragentId);
                    if (contragent != null)
                    {
                        var esiaOperator = new EsiaOperator
                        {
                            Operator = userManager.GetBaseOperatorForEsia(),
                            UserName = userInfo.Name,
                            LastName = userInfo.LastName,
                            FirstName = userInfo.FirstName,
                            MiddleName = userInfo.MiddleName,
                            Gender = userInfo.Gender,
                            BirthDate = userInfo.BirthDate,
                            UserId = userInfo.Id,
                            OrgName = selectedOrganization.FullName,
                            OrgShortName = selectedOrganization.ShortName,
                            OrgOgrn = selectedOrganization.Ogrn,
                            IsActive = true
                        };

                        esiaOperatorDomain.Save(esiaOperator);

                        this.AddLoginInfoToCookies(context, esiaOperator, userInfo);
                        return new BaseDataResult();
                    }

                    return BaseDataResult.Error("Не удалось найти указанного контрагента");
                }
            }
            catch (Exception exception)
            {
                var logManager = this.Container.Resolve<ILogManager>();
                logManager.Error("Непредвиденная ошибка", exception);

                return BaseDataResult.Error("Непредвиденная ошибка");
            }
        }

        /// <inheritdoc />
        public IDataResult LinkEsiaAccount(BaseParams baseParams, HttpRequestBase request)
        {
            try
            {
                var userIdentity = this.Container.Resolve<IUserIdentity>();
                var gkhUserManager = this.Container.Resolve<IGkhUserManager>();
                var authorizationService = this.Container.Resolve<IAuthenticationService>();

                var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();
                var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
                var operatorDomain = this.Container.ResolveDomain<Operator>();
                var userRoleDomain = this.Container.ResolveDomain<UserRole>();
    
                using (this.Container.Using(userIdentity, gkhUserManager, authorizationService,
                        esiaOperatorDomain, operatorContragentDomain, operatorDomain, userRoleDomain))
                {
                    var login = baseParams.Params.GetAs<string>("login");
                    var password = baseParams.Params.GetAs<string>("password");
                    var userId = userIdentity.UserId;
                    var orgOgrn = request.Cookies["esiaOrgOgrn"];
                    var selectedContragentId = request.Cookies["selectedContragentId"];

                    if (orgOgrn == null || selectedContragentId == null)
                    {
                        return BaseDataResult.Error("При получении данных возникла ошибка. Попробуйте повторить процедуру входа");
                    }

                    var authenticationResult = authorizationService.Authenticate(login, password);

                    if (!authenticationResult.Success)
                    {
                        return BaseDataResult.Error("Неверный логин или пароль");
                    }

                    var baseEsiaOperator = gkhUserManager.GetBaseOperatorForEsia();

                    //Ищем нелинкованного оператора ЕСИА по идентификатору юзера и ОГРН выбранной организации
                    var esiaOperator = esiaOperatorDomain.GetAll()
                        .FirstOrDefault(x =>
                            x.Operator.User.Id == userId &&
                            x.Operator.Id == baseEsiaOperator.Id &&
                            x.OrgOgrn == orgOgrn.Value);

                    if (esiaOperator == null)
                    {
                        return BaseDataResult.Error("Не удалось получить данные ЕСИА");
                    }

                    //Ищем связку контрагент-оператор по ОГРН и идентификатору юзера
                    var operatorContragent = operatorContragentDomain.GetAll()
                        .FirstOrDefault(x => x.Operator.User.Id == authenticationResult.UserId);

                    var contragentErrorMessage = string.Empty;

                    if (operatorContragent == null)
                    {
                        contragentErrorMessage = "не привязана организация";
                    }
                    else if (operatorContragent.Contragent.Id.ToString() != selectedContragentId.Value)
                    {
                        contragentErrorMessage = $"привязана организация \"{operatorContragent.Contragent.ShortName}\"";
                    }

                    if (contragentErrorMessage.IsNotEmpty())
                    {
                        return BaseDataResult.Error($"К учетной записи {contragentErrorMessage}, доступ ограничен");
                    }

                    esiaOperator.Operator = operatorContragent.Operator;
                    esiaOperatorDomain.Save(esiaOperator);

                    return new BaseDataResult();
                }
            }
            catch (Exception)
            {
                return BaseDataResult.Error("Не удалось привязать оператора");
            }
        }

        /// <summary>
        /// Выгрузить информацию о результатах логина в куки
        /// </summary>
        private void AddLoginInfoToCookies(HttpContext context, EsiaOperator esiaOperator, UserInfoDto userInfo)
        {
            var userId = esiaOperator.Operator.User.Id;
            var userName = esiaOperator.UserName;

            var account = new DynamicDictionary
            {
                AsDynamic =
                {
                    UserId = userId,
                    UserName = userName,
                    EsiaAuth = true
                }
            };

            var json = JsonNetConvert.SerializeObject(this.Container, account);
            var auth = new FormsAuthenticationTicket(2,
                userId.ToString(CultureInfo.InvariantCulture),
                DateTime.Now,
                DateTime.Now.AddHours(12),
                true,
                json);

            var ticket = FormsAuthentication.Encrypt(auth);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket)
            {
                Expires = auth.Expiration,
                HttpOnly = true
            };

            context.Response.Cookies.Add(cookie);

            context.Response.Cookies.Add(new HttpCookie("esiaOrgOgrn")
            {
                Value = esiaOperator.OrgOgrn,
                Expires = DateTime.Now.AddMinutes(5),
                HttpOnly = true
            });

            context.Response.Cookies.Add(new HttpCookie("selectedContragentId")
            {
                Value = userInfo.SelectedContragentId.ToString(),
                Expires = DateTime.Now.AddMinutes(5),
                HttpOnly = true
            });
        }

        /// <summary>
        /// Вернуть результат с ошибкой
        /// </summary>
        private IDataResult<T> Error<T>(string message, T userInfo = null) where T: class
        {
            return new GenericDataResult<T>
            {
                Success = false,
                Message = message,
                Data = userInfo
            };
        }
    }
}