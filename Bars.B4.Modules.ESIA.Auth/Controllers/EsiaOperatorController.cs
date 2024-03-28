namespace Bars.B4.Modules.ESIA.Auth.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Bars.Gkh.Helpers;
    using Bars.Gkh.Utils;
    using Bars.B4.Utils;
    using DataAccess;
    using Gkh.Entities;
    using Gkh.Entities.Administration.Operator;
    using Security;
    using Gkh.Authentification;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Application;

    public class EsiaOperatorController : BaseController
    {
        public IGkhUserManager UserManager { get; set; }

        public ActionResult AppendToEsia(BaseParams baseParams)
        {
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();
            var userRoleDomain = this.Container.ResolveDomain<UserRole>();
            var contragentOperatorDomain = this.Container.ResolveDomain<OperatorContragent>();
            var authorizationService = this.Container.Resolve<IAuthenticationService>(new { this.Request });
            var operatorDomain = this.Container.ResolveDomain<Operator>();

            try
            {
                var login = baseParams.Params.GetAs<string>("login");
                var password = baseParams.Params.GetAs<string>("password");

                var userId = userIdentity.UserId;

                var esiaopertorId = Convert.ToInt64(Request.Cookies["esiaoperatorId"].Value);
                var orgOgrn = Request.Cookies["esiaOrgOgrn"];

                if (orgOgrn == null)
                {
                    return JsFailure("Не удалось определить ОГРН организации пользователя ЕСИА. Попробуйте повторить процедуру входа");
                }

                var esiaOperator = esiaOperatorDomain.Get(esiaopertorId);
                if (esiaOperator == null)
                    return JsFailure("Не удалось получить данные ЕСИА");

                var authenticationResult = authorizationService.Authenticate(login, password);
                if (!authenticationResult.Success)
                {
                    return this.JsFailure("Неверный логин или пароль");
                }

                //Ищем связку контрагент-оператор по ОГРН и идентификатору юзера
                //var oper = contragentOperatorDomain.GetAll()
                //    .FirstOrDefault(x => x.Contragent.Ogrn == esiaOperator.OrgOgrn
                //        && x.Operator.User.Id == authenticationResult.UserId);

                //Отключаем проверку на огрн для юзеров без организаций
                bool hasContragent = false;
                var userOperator = operatorDomain.GetAll()
                    .FirstOrDefault(x => x.User.Id == authenticationResult.UserId);

                var contragentOperator = contragentOperatorDomain.GetAll()
                     .FirstOrDefault(x => x.Operator.User.Id == authenticationResult.UserId);

                if (contragentOperator != null)
                {
                    hasContragent = true;
                }

                var oper = contragentOperatorDomain.GetAll()
                 .FirstOrDefault(x => x.Contragent.Ogrn == esiaOperator.OrgOgrn
                     && x.Operator.User.Id == authenticationResult.UserId);

                //Operator operat = oper.Operator;

                //var currentUser = UserManager.GetActiveOperator();
                //var currentRole = currentUser.User.Roles.FirstOrDefault();

                //UserRole role = oper.Operator.User.Roles.FirstOrDefault();

                if (oper == null && hasContragent)
                {
                    return this.JsFailure("Данные организации указанного пользователя и данные организации пользователя в ЕСИА не совпадают");
                }

                esiaOperator.Operator = userOperator;
                esiaOperatorDomain.Save(esiaOperator);

                //Обновляем имя оператора
                var operatorObj = esiaOperator.Operator;
                operatorObj.Name = new[] { esiaOperator.LastName, esiaOperator.FirstName, esiaOperator.MiddleName }
                    .AggregateWithSeparator(" ");

                operatorDomain.Update(operatorObj);

                //26521

                //var userData = authenticationResult.UserData;
                //userData.AsDynamic.UserId = authenticationResult.UserId;
                //userData.AsDynamic.UserName = authenticationResult.UserName;

                ////var userData = authenticationResult.UserName;

                //var json = JsonNetConvert.SerializeObject(Container, userData);

                //var ticket = new FormsAuthenticationTicket(
                //    2,
                //    userId.ToString(CultureInfo.InvariantCulture),
                //    DateTime.Now,
                //    DateTime.Now.AddHours(12),
                //    true,
                //    json);

                //var encTicket = FormsAuthentication.Encrypt(ticket);

                //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                //{
                //    Expires = ticket.Expiration
                //};

                //Response.Cookies.Add(authCookie);

                //return RedirectToAction("index", "home");
                return this.JsSuccess();
            }
            catch (Exception e)
            {
                EmailSender.Instance.TrySendIfLogEnabled(e.Message, e.InnerException + "\r\n" + e.StackTrace);
                return this.JsFailure("Не удалось привязать оператора");
            }
            finally
            {
                this.Container.Release(userIdentity);
                this.Container.Release(esiaOperatorDomain);
                this.Container.Release(contragentOperatorDomain);
                this.Container.Release(authorizationService);
                this.Container.Release(operatorDomain);
            }
        }

        public ActionResult CreateNewEsiaOperator(BaseParams baseParams)
        {
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();
            var userRoleDomain = this.Container.ResolveDomain<UserRole>();
            var userDomain = this.Container.ResolveDomain<User>();
            var contragentOperatorDomain = this.Container.ResolveDomain<OperatorContragent>();
            var authorizationService = this.Container.Resolve<IAuthenticationService>(new { this.Request });
            var operatorRepo = this.Container.Resolve<IRepository<Operator>>();
            var contragentDomain = ApplicationContext.Current.Container.Resolve<IRepository<Contragent>>();
            var operatorContragentDomain = ApplicationContext.Current.Container.Resolve<IRepository<OperatorContragent>>();
            var repUserRole = this.Container.Resolve<IDomainService<UserRole>>();
            var repRole = this.Container.Resolve<IDomainService<Role>>();
            try
            {
                var esiaopertorId = Convert.ToInt64(Request.Cookies["esiaoperatorId"].Value);
                var orgOgrn = Request.Cookies["esiaOrgOgrn"];

                if (orgOgrn == null)
                {
                    return JsFailure("Не удалось определить ОГРН организации пользователя ЕСИА. Попробуйте повторить процедуру входа");
                }

                var esiaOperator = esiaOperatorDomain.Get(esiaopertorId);
                if (esiaOperator == null)
                    return JsFailure("Не удалось получить данные ЕСИА");
                var contragent = contragentDomain.GetAll().Where(x => x.Ogrn == orgOgrn.Value).FirstOrDefault();
         //       esiaOperator.Operator = operatorContragentDomain.GetAll().Where(x => x.Contragent == contragent).Select(x => x.Operator).FirstOrDefault();
                 var user = SaveCustomUser(GetName(esiaOperator), GetLogin(esiaOperator), esiaOperator.OrgOgrn, esiaOperator.PersonEmail);
                var sameOperator = operatorContragentDomain.GetAll().Where(y => y.Contragent == contragent).Select(y => y.Operator).FirstOrDefault();
                if (sameOperator == null)
                {
                    var role = repRole.GetAll().FirstOrDefault(x => x.Name == "Управляющие организации");
                    if (role == null)
                    {
                        role = new Role { Name = "Управляющие организации" };


                        repRole.Save(role);

                        var customUserRole = new UserRole
                        {
                            Role = role,
                            User = user
                        };
                        repUserRole.Save(customUserRole);
                    }
                    else
                    {

                        var customUserRole = new UserRole
                        {
                            Role = role,
                            User = user
                        };
                        repUserRole.Save(customUserRole);
                    }

                }
                else
                {
                    var customRole = repUserRole.GetAll()
                        .Where(x => x.User == sameOperator.User && !x.Role.Name.ToLower().Contains("админ")).FirstOrDefault().Role;

                    if (customRole != null)
                    {
                        var customUserRole = new UserRole
                        {
                            Role = customRole,
                            User = user
                        };
                        repUserRole.Save(customUserRole);
                    }
                    else
                    {
                        var role = repRole.GetAll().FirstOrDefault(x => x.Name == "Управляющие организации");
                        if (role == null)
                        {
                            role = new Role { Name = "Управляющие организации" };


                            repRole.Save(role);

                            var customUserRole = new UserRole
                            {
                                Role = role,
                                User = user
                            };
                            repUserRole.Save(customUserRole);
                        }
                        else
                        {

                            var customUserRole = new UserRole
                            {
                                Role = role,
                                User = user
                            };
                            repUserRole.Save(customUserRole);
                        }
                    }
                }
                var currentUserOperator = operatorRepo.GetAll()
                    .Where(x => x.User == user).FirstOrDefault();
                if (currentUserOperator != null)
                {
                    currentUserOperator.Contragent = contragent;
                    currentUserOperator.Name = GetName(esiaOperator);
                    currentUserOperator.IsActive = true;                    
                    operatorRepo.Update(currentUserOperator);
                    esiaOperator.Operator = currentUserOperator;
                    esiaOperatorDomain.Save(esiaOperator);
                    var operatorContragent = operatorContragentDomain.GetAll()
                        .Where(x => x.Operator == currentUserOperator).FirstOrDefault();
                    if (operatorContragent != null)
                    {
                        operatorContragent.Contragent = contragent;
                        operatorContragentDomain.Update(operatorContragent);
                    }
                    else
                    {
                        operatorContragentDomain.Save(new OperatorContragent
                        {
                            Operator = esiaOperator.Operator,
                            Contragent = contragent
                        });
                    }

                    try
                    {
                        var emailsenders = ApplicationContext.Current.Container.ResolveAll<IESIAEMailSender>();
                        foreach (var emailsender in emailsenders)
                        {
                            //Высылаем письмо
                            emailsender.SendEmail(contragent.Name, contragent.Ogrn, GetName(esiaOperator), GetLogin(esiaOperator), esiaOperator.OrgOgrn);
                        }
                    }
                    catch
                    {

                    }
                    var authenticationResult = authorizationService.Authenticate(user.Login, esiaOperator.OrgOgrn);
                    //Выполняем логин
                    //    this.Login(context, esiaOperator);
                    var data = new { usrlogin = user.Login, usrpass = esiaOperator.OrgOgrn };

                    return this.JsSuccess(data);
                }
                else
                {
                    var newOperator = new Operator
                    {
                        TypeWorkplace = Gkh.Enums.TypeWorkplace.BackOffice,
                        ContragentType = Gkh.Enums.ContragentType.NotSet,
                        Contragent = contragent,
                        User = user,
                        Name = GetName(esiaOperator),
                        IsActive = true
                    };
                    operatorRepo.Save(newOperator);
                    esiaOperator.Operator = newOperator;
                    esiaOperatorDomain.Save(esiaOperator);



                    operatorContragentDomain.Save(new OperatorContragent
                    {
                        Operator = esiaOperator.Operator,
                        Contragent = contragent
                    });
                    try
                    {
                        var emailsenders = ApplicationContext.Current.Container.ResolveAll<IESIAEMailSender>();
                        foreach (var emailsender in emailsenders)
                        {
                            //Высылаем письмо
                            emailsender.SendEmail(contragent.Name, contragent.Ogrn, GetName(esiaOperator), GetLogin(esiaOperator), esiaOperator.OrgOgrn);
                        }
                    }
                    catch 
                    {

                    }
                    var authenticationResult = authorizationService.Authenticate(user.Login, esiaOperator.OrgOgrn);
                    //Выполняем логин
                    //    this.Login(context, esiaOperator);
                    var data = new { usrlogin = user.Login, usrpass = esiaOperator.OrgOgrn };

                    return this.JsSuccess(data);
                }

              
            }
            catch (Exception e)
            {
                EmailSender.Instance.TrySendIfLogEnabled(e.Message, e.InnerException + "\r\n" + e.StackTrace);
                return this.JsFailure(e.Message);
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
           
            var repRolePer = this.Container.Resolve<IDomainService<RolePermission>>();
           
            var userService = this.Container.Resolve<IUserService>();
            var userPasswordService = this.Container.Resolve<IDomainService<UserPassword>>();
            var existingUser = repUser.GetAll()
                .Where(x => x.Login == login).FirstOrDefault();
            if (existingUser != null)
            {
                return existingUser;
            }
            try
            {
                //создаем пользователя
                var user = new User
                {
                    Name = name,
                    Login = login,
                    Email = email
                };
                string passwordHash = MD5.GetHashString64(password, HashType.MD5B2);
                userService.CreateUser(user, passwordHash);
                repUser.Save(user);




                return user;
            }
            catch (Exception e)
            {
                return null;
            }
          
        }

     

    }
}