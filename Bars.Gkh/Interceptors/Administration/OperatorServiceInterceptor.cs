namespace Bars.Gkh.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.RegOperator.Administration;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    //Заглушка потомучт оот этого класса могли наследоватся в других регионах
    public class OperatorServiceInterceptor : OperatorServiceInterceptor<Operator>
    {
    }

   

    /// <summary>
    /// Generic класс Оператора чтобы легче наследоватся в регионных модулях. Напрмиер НСО
    /// </summary>
    public class OperatorServiceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : Operator
    {
        private readonly List<PasswordMasksConfig> passMasks = new List<PasswordMasksConfig>();

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            if (service.GetAll().Any(x => x.User.Login == entity.Login))
            {
                return Failure("Пользователь с таким логином уже существует");
            }

            var userManager = Container.Resolve<IGkhUserManager>();
            var appContext = Container.Resolve<IAppContext>();

            try
            {
                var password = entity.Password;

                if (!appContext.IsDebug)
                {
                    if (string.IsNullOrEmpty(entity.Password))
                    {
                        return Failure("Введите пароль");
                    }


                    if (password.Length < 6)
                    {
                        return Failure("Ограничение пароля: не менее 6 знаков");
                    }
                }

                var roles = new List<Role>();
                if (entity.Role != null)
                {
                    roles.Add(entity.Role);
                }

                entity.User = userManager.SaveCustomUser(0, entity.Name, entity.Login, password, entity.Email, roles);
                return Success();
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(appContext);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (service.GetAll().Any(x => x.User.Login == entity.Login && x.Id != entity.Id))
            {
                return Failure("Пользователь с таким логином уже существует");
            }
            var passCongig = this.Container.GetGkhConfig<AdministrationConfig>().UserPasswordConfig;

            var userManager = Container.Resolve<IGkhUserManager>();
            var appContext = Container.Resolve<IAppContext>();

            Operator operatorChanger = userManager.GetActiveOperator();
            if (operatorChanger.User != null)
            {
                User userChanger = operatorChanger.User;
                var changerRoles = userChanger.Roles;
                if (changerRoles != null && changerRoles.Count == 1)
                {
                    Role changerRole = changerRoles.FirstOrDefault().Role;
                    if (changerRole != null)
                    {
                        if (!changerRole.Name.ToLower().Contains("админ") && changerRole.Name.ToLower() != "esia")
                        {
                            return Failure("У вас нет прав на редактирование пользователя");
                        }

                    }
                    else
                    {
                        return Failure("У вас нет прав на редактирование пользователя");
                    }


                }
                else
                {
                    return Failure("У вас нет прав на редактирование пользователя");
                }

            }
            else
            {
                return Failure("У вас нет прав на редактирование пользователя");
            }


            try //проверка пароля на соответствие конфигу
            {
                var newPass = entity.NewPassword;
                if (newPass != null && newPass.Length>0 /*!appContext.IsDebug*/)
                {
                    // Смена пароля
                    if (passCongig.PasswordDifficultySwitcher == Enums.YesNo.Yes)
                    {
                        int passwordLength = passCongig.MinimalLength;
                        if (passwordLength>0 && newPass.Length < passwordLength)
                        {
                            return Failure("Новый пароль не соответствует длинне, указанной в настройках");
                        }
                        for (int i = 0; i < newPass.Length; i++)
                        {
                            string ch = newPass.Substring(i, 1);
                            bool isCorrect = ValidatePasswordChar(ch, passCongig.PasswordMasks);
                            if (!isCorrect)
                            {
                                return Failure("Не соответствие допустимым символам пароля, указанным в настройках");
                            }
                        }

                    }
                    else if (!string.IsNullOrEmpty(entity.Password) && !string.IsNullOrEmpty(newPass))
                    {
                        if (newPass.Length < 6)
                        {
                            return Failure("Ограничение пароля: не менее 6 знаков");
                        }
                    }
                }

                var name = string.IsNullOrEmpty(entity.Name) ? entity.User.Name : entity.Name;
                var login = string.IsNullOrEmpty(entity.Login) ? entity.User.Login : entity.Login;
                var email = string.IsNullOrEmpty(entity.Email) ? entity.User.Email : entity.Email;

                var roles = new List<Role>();
                if (entity.Role != null)
                {
                    roles.Add(entity.Role);
                }
                else if (entity.User.Roles != null)
                {
                    foreach (var userRole in entity.User.Roles)
                    {
                        roles.Add(userRole.Role);
                    }
                }

                entity.User = userManager.SaveCustomUser(entity.User.Id, name, login, newPass, email, roles);
                return Success();
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(appContext);
            }
        }

        public bool ValidatePasswordChar(string str, List<PasswordMasksConfig> listMasks)
        {
            foreach (PasswordMasksConfig mask in listMasks)
            {
                string m = mask.Mask;
                bool isCorrect = false;
                isCorrect = Regex.IsMatch(str, m);
                if (isCorrect)
                {
                    return isCorrect;
                }
            }
            return false;
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            //var userProfileParamService = Container.Resolve<IDomainService<UserProfileParam>>();
            var operContragentService = Container.Resolve<IDomainService<OperatorContragent>>();
            var operInspectorService = Container.Resolve<IDomainService<OperatorInspector>>();
            var operMunicipalityService = Container.Resolve<IDomainService<OperatorMunicipality>>();
            var repLogImport = Container.Resolve<IDomainService<LogImport>>();

            try
            {
                //var userProfileParamList = userProfileParamService.GetAll().Where(x => x.User.Id == entity.User.Id).Select(x => x.Id).ToList();
                //foreach (var value in userProfileParamList)
                //{
                //    userProfileParamService.Delete(value);
                //}

                operContragentService.GetAll().Where(x => x.Operator.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => operContragentService.Delete(x));

                operInspectorService.GetAll().Where(x => x.Operator.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => operInspectorService.Delete(x));

                operMunicipalityService.GetAll().Where(x => x.Operator.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => operMunicipalityService.Delete(x));

                repLogImport.GetAll().Where(x => x.Operator.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => repLogImport.Delete(x));

                return Success();
            }
            finally
            {
                //Container.Release(userProfileParamService);
                Container.Release(operContragentService);
                Container.Release(operInspectorService);
                Container.Release(operMunicipalityService);
                Container.Release(repLogImport);
            }
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            // так удаляет. пробовал удалять через IUserService.DeleteUser - облом!
            var userService = Container.Resolve<IDomainService<User>>();

            try
            {
                var userList = userService.GetAll().Where(x => x.Id == entity.User.Id).Select(x => x.Id).ToList();
                foreach (var value in userList)
                {
                    userService.Delete(value);
                }

                return Success();
            }
            finally
            {
                Container.Release(userService);
            }

        }
    }
}
