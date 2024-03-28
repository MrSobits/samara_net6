namespace Bars.B4.Modules.ESIA.Auth.Actions
{
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.OAuth20;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.Operator;
    using Bars.Gkh.Helpers;
    using Castle.Windsor;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    /// <summary>
    /// Действие по логину через OAuth
    /// </summary>
    public class EsiaOauthLogin : IEsiaOauthLoginAction
    {
        public IWindsorContainer Container { get; set; }


        /// <summary>
        /// Выполнение логина
        /// </summary>
        public IDataResult PerformLogin(EsiaUserInfo userInfo, HttpContext context)
        {
            var contragentDomain = ApplicationContext.Current.Container.Resolve<IRepository<Contragent>>();
            var operatorContragentDomain = ApplicationContext.Current.Container.Resolve<IRepository<OperatorContragent>>();
            var operatorDomain = ApplicationContext.Current.Container.Resolve<IRepository<Operator>>();
            var userManager = ApplicationContext.Current.Container.Resolve<IGkhUserManager>();
            var esiaOperatorDomain = ApplicationContext.Current.Container.ResolveDomain<EsiaOperator>();
            var logManager = ApplicationContext.Current.Container.Resolve<ILogManager>();

            try
            {
                var keyPieces = userInfo.SelectedOrganizationKey.Split("###");
                if (keyPieces.Count() != 2)
                    return BaseDataResult.Error($"Не удалось разобрать ключ: {userInfo.SelectedOrganizationKey}");

                var esiaOperator = new EsiaOperator
                {
                    LastName = userInfo.LastName,
                    FirstName = userInfo.FirstName,
                    MiddleName = userInfo.MiddleName,
                    UserId = userInfo.Id,
                    FullName = keyPieces[0],
                    OrgOgrn = keyPieces[1],
                };

                if (esiaOperator.OrgOgrn.IsEmpty())
                    return BaseDataResult.Error("В ЕСИА не привязана организация");

                //---ищем запись об этом операторе---
                var existsEsiaOperator = esiaOperatorDomain.GetAll()
                    .Where(x => x.OrgOgrn == esiaOperator.OrgOgrn)
                    .FirstOrDefault(x => x.UserId == esiaOperator.UserId);

                // 1 случай, данные учетки ЕСИА уже есть в ЖКХ - тупо логинимся
                if (existsEsiaOperator != null)
                {
                    if (existsEsiaOperator.Operator.IsActive)
                    {
                        Login(context, existsEsiaOperator);
                        return new BaseDataResult();
                    }
                    else
                    {
                        return BaseDataResult.Error("Пользователь заблокирован");
                    }
                }

                var contragentByOgrn = contragentDomain.GetAll().FirstOrDefault(x => x.Ogrn == esiaOperator.OrgOgrn);
                if (contragentByOgrn != null)
                {
                    esiaOperator.Operator = userManager.GetBaseOperatorForEsia();
                    esiaOperatorDomain.Save(esiaOperator);

                    this.Login(context, esiaOperator);
                    return new BaseDataResult();
                }

                // 2 случай, под этой учеткой ЕСИА входят первый раз - сопоставляем контрагента                


                if (contragentByOgrn == null)
                {
                    return BaseDataResult.Error("В системе отсутствует контрагент с ОГРН "+ esiaOperator.OrgOgrn);
                    // 2.1 случай, не нашли в системе контрагента ни по ОГРН, ни по КПП - создаем нового
                    contragentByOgrn = new Contragent
                    {
                        Ogrn = esiaOperator.OrgOgrn,
                        IsSite = false,
                        Name = esiaOperator.FullName,
                        TypeEntrepreneurship = Gkh.Enums.TypeEntrepreneurship.NotSet,
                        ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet,
                        ContragentState = Gkh.Enums.ContragentState.Active
                    };
                    contragentDomain.Save(contragentByOgrn);
                    var user = SaveCustomUser(GetName(esiaOperator), GetLogin(esiaOperator), esiaOperator.OrgOgrn, esiaOperator.PersonEmail);
                    esiaOperator.Operator = operatorContragentDomain.GetAll().Where(x => x.Contragent.Id == contragentByOgrn.Id && x.Operator.IsActive).Select(x => x.Operator).FirstOrDefault();
                    if (esiaOperator.Operator == null)
                    {
                        esiaOperator.Operator = new Operator
                        {
                            TypeWorkplace = Gkh.Enums.TypeWorkplace.BackOffice,
                            ContragentType = Gkh.Enums.ContragentType.NotSet,
                            Contragent = contragentByOgrn,
                            User = user,
                            Name = GetName(esiaOperator),
                            IsActive = true
                        };
                        operatorDomain.Save(esiaOperator.Operator);
                    }
                    esiaOperatorDomain.Save(esiaOperator);

                    operatorContragentDomain.Save(new OperatorContragent
                    {
                        Operator = esiaOperator.Operator,
                        Contragent = contragentByOgrn
                    });
                    try
                    {
                        //var emailsenders = ApplicationContext.Current.Container.ResolveAll<IESIAEMailSender>();
                        //foreach (var emailsender in emailsenders)
                        //{
                        //    //Высылаем письмо
                        //    emailsender.SendEmail(contragentByOgrn.Name, contragentByOgrn.Ogrn, GetName(esiaOperator), GetLogin(esiaOperator), esiaOperator.OrgOgrn);
                        //}
                    }
                    catch { }
                }
                //Выполняем логин
                this.Login(context, esiaOperator);
                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                logManager.Error("Непредвиденная ошибка", exception);
             //   EmailSender.Instance.TrySendIfLogEnabled(exception.Message, exception.InnerException + "\r\n" + exception.StackTrace);
                return BaseDataResult.Error("Непредвиденная ошибка");
            }
            finally
            {
                ApplicationContext.Current.Container.Release(contragentDomain);
                ApplicationContext.Current.Container.Release(userManager);
                ApplicationContext.Current.Container.Release(esiaOperatorDomain);
            }
        }

        private string GetLogin(EsiaOperator esiaOperator)
        {
            return $"{esiaOperator.LastName} {esiaOperator.FirstName[0]}{esiaOperator.MiddleName[0]}_{esiaOperator.OrgOgrn}";
        }

        private string GetName(EsiaOperator esiaOperator)
        {
            return $"{esiaOperator.LastName} {esiaOperator.FirstName} {esiaOperator.MiddleName}";
        }

        public User SaveCustomUser(string name, string login, string password, string email)
        {
            var repUser = this.Container.Resolve<IDomainService<User>>();
            var repRole = this.Container.Resolve<IDomainService<Role>>();
            var repRolePer = this.Container.Resolve<IDomainService<RolePermission>>();
            var repUserRole = this.Container.Resolve<IDomainService<UserRole>>();
            var userService = this.Container.Resolve<IUserService>();
            var userPasswordService = this.Container.Resolve<IDomainService<UserPassword>>();

            try
            {
                //создаем пользователя
                var user = new User
                {
                    Name = name,
                    Login = login,
                    Email = email
                };
                userService.CreateUser(user, MD5.GetHashString64(password, HashType.MD5B2));

                //создаем роль
                var role = repRole.GetAll().FirstOrDefault(x => x.Name == "Управляющие организации");
                if (role == null)
                {
                    role = new Role { Name = "Управляющие организации" };

                    //var permission = new RolePermission { PermissionId = "B4.Security.Role", Role = esiaRole };
                    //repRolePer.Save(permission);

                    //permission = new RolePermission { PermissionId = "B4.Security.AccessRights", Role = esiaRole };
                    //repRolePer.Save(permission);

                    repRole.Save(role);
                }

                var userRole = new UserRole { Role = role, User = user };
                repUserRole.Save(userRole);

                return user;
            }
            finally
            {
                this.Container.Release(repUser);
                this.Container.Release(repRole);
                this.Container.Release(repRolePer);
                this.Container.Release(repUserRole);
                this.Container.Release(userService);
                this.Container.Release(userPasswordService);
            }
        }

        public void Login(HttpContext context, EsiaOperator esiaOperator)
        {
            var userId = esiaOperator.Operator.User.Id;
            var userName = esiaOperator.UserName;

            var account = new DynamicDictionary();
            account.AsDynamic.UserId = userId;
            account.AsDynamic.UserName = userName;
            account.AsDynamic.EsiaAuth = true;

            var json = JsonNetConvert.SerializeObject(ApplicationContext.Current.Container, account);
            var auth = new FormsAuthenticationTicket(2,
                userId.ToString(CultureInfo.InvariantCulture),
                DateTime.Now,
                DateTime.Now.AddHours(12),
                true,
                json);

            var ticket = FormsAuthentication.Encrypt(auth);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket)
            {
                Expires = auth.Expiration
            };

            context.Response.Cookies.Add(cookie);

            context.Response.Cookies.Add(new HttpCookie("esiaSuccess")
            {
                Value = "success",
                Expires = DateTime.Now.AddMinutes(5)
            });

            context.Response.Cookies.Add(new HttpCookie("esiaOrgOgrn")
            {
                Value = esiaOperator.OrgOgrn,
                Expires = DateTime.Now.AddMinutes(5)
            });

            context.Response.Cookies.Add(new HttpCookie("esiaoperatorId")
            {
                Value = esiaOperator.Id.ToString(),
                Expires = DateTime.Now.AddMinutes(5)
            });
        }
    }
}
