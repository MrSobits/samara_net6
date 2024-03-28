namespace Bars.Gkh.Authentification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.RoleFilterRestriction;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    public class UserManager : IGkhUserManager
    {
        private User user;

        public IWindsorContainer Container { get; set; }

        public IQueryable<UserInfo> GetList()
        {
            var service = this.Container.Resolve<IDomainService<User>>();

            try
            {
                var data = service.GetAll().Select(x => new UserInfo { Id = x.Id, Name = x.Name, Login = x.Login });

                return data;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public User SaveCustomUser(long id, string name, string login, string password, string email, IList<Role> listRoles)
        {
            var repUser = this.Container.Resolve<IDomainService<User>>();
            var repRole = this.Container.Resolve<IDomainService<Role>>();
            var repRolePer = this.Container.Resolve<IDomainService<RolePermission>>();
            var repUserRole = this.Container.Resolve<IDomainService<UserRole>>();
            var userService = this.Container.Resolve<IUserService>();
            var userPasswordService = this.Container.Resolve<IDomainService<UserPassword>>();

            //если нет списка ролей, создать
            if (listRoles == null)
                listRoles = new List<Role>();

            try
            {
                User savedUser;

                //по умолчанию даем роль esia
                if (!listRoles.Any())
                {                  
                    // пока в виде костыля. Требуется разобраться что не так с ЕСИА роль esia
                    // var roleAdmin = repRole.GetAll().FirstOrDefault(x => x.Name == "Администратор");
                    var esiaRole = repRole.GetAll().FirstOrDefault(x => x.Name == "esia");
                    if (esiaRole == null)
                    {
                        esiaRole = repRole.GetAll().FirstOrDefault(x => x.Name == "Администратор");
                        if (esiaRole == null)
                        {
                            esiaRole = new Role { Name = "esia" };

                            var permission = new RolePermission { PermissionId = "B4.Security.Role", Role = esiaRole };
                            repRolePer.Save(permission);

                            permission = new RolePermission { PermissionId = "B4.Security.AccessRights", Role = esiaRole };
                            repRolePer.Save(permission);

                            repRole.Save(esiaRole);
                        }
                    }
                    listRoles.Add(esiaRole);
                }

                if (id == 0)
                {
                    savedUser = new User
                    {
                        Name = name,
                        Login = login,
                        Email = email
                    };

                    userService.CreateUser(savedUser, MD5.GetHashString64(password, HashType.MD5B2));
                }
                else
                {
                    savedUser = repUser.Get(id);
                    savedUser.Name = name;
                    savedUser.Login = login;
                    savedUser.Email = email;

                    //Пароль меняем только если пришедший пароль непустой
                    if (!string.IsNullOrEmpty(password))
                    {
                        var userPassword = userPasswordService.GetAll().FirstOrDefault(x => x.User == savedUser);

                        if (userPassword == null || userPassword.Password != password)
                        {
                            //Если пришедший пароль неравен тому паролю который в зашифрованном виде
                            //то тогда необходимо получить новый Хэш
                            var newPassword = MD5.GetHashString64(password, HashType.MD5B2);
                            if (userPassword == null || userPassword.Password != password)
                            {
                                userService.ChangePassword(savedUser, newPassword);
                            }
                        }
                    }

                    repUser.Update(savedUser);

                    //удаляем все текущие роли и заполняем теми ролями которые в listRoles
                    var currRoles = repUserRole.GetAll().Where(x => x.User.Id == savedUser.Id).Select(x => x.Id).ToList();
                    foreach (var currRole in currRoles)
                    {
                        repUserRole.Delete(currRole);
                    }
                }

                foreach (var role in listRoles)
                {
                    var userRole = new UserRole { Role = role, User = savedUser };
                    repUserRole.Save(userRole);
                }

                return savedUser;
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

        public void SaveUser(long id, string name, string login, string password)
        {
            var service = this.Container.Resolve<IDomainService<UserRole>>();

            try
            {
                var listRoles = new List<Role>();

                if (id > 0)
                {
                    listRoles = service.GetAll()
                        .Where(x => x.User.Id == id)
                        .Select(x => x.Role)
                        .ToList();
                }

                this.SaveCustomUser(id, name, login, password, string.Empty, listRoles);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public Operator GetActiveOperator()
        {
            var user = this.Container.Resolve<IUserIdentity>();
            var operatorService = this.Container.Resolve<IRepository<Operator>>();

            try
            {
                if (user is AnonymousUserIdentity)
                {
                    return null;
                }

                var oper = operatorService.GetAll().FirstOrDefault(x => x.User.Id == user.UserId);

                return oper;
            }
            finally
            {
                this.Container.Release(user);
                this.Container.Release(operatorService);
            }
        }

        public User GetActiveUser()
        {
            if (this.user.IsNotNull())
            {
                return this.user;
            }

            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var userService = this.Container.Resolve<IDomainService<User>>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();

            try
            {
                if (userIdentity is AnonymousUserIdentity)
                {
                    return null;
                }

                var flush = session.FlushMode;
                session.FlushMode = FlushMode.Never;
                this.user = userService.GetAll()
                    .FetchMany(x => x.Roles)
                    .FirstOrDefault(x => x.Id == userIdentity.UserId);
                session.Evict(this.user);
                session.FlushMode = flush;
            }
            finally
            {
                this.Container.Release(userIdentity);
                this.Container.Release(userService);
                this.Container.Release(sessionProvider);
            }

            return this.user;
        }

        /// <summary>
        /// Получет список муниципальных образований текущего оператора
        /// </summary>
        public List<Municipality> GetActiveOperatorMunicipalities()
        {
            var service = this.Container.Resolve<IRepository<OperatorMunicipality>>();
            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return new List<Municipality>();
                }

                return service.GetAll().Where(x => x.Operator.Id == user.Id).Select(x => x.Municipality).ToList();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Метод для определения есть ли у текущего оператора Контрагенты
        /// </summary>
        /// <returns></returns>
        public bool HasOperatorInspector()
        {
            var service = this.Container.Resolve<IRepository<OperatorInspector>>();

            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return false;
                }

                return service.GetAll().Any(x => x.Operator.Id == user.Id);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Метод для определения есть ли у текущего оператора Контрагенты
        /// </summary>
        /// <returns></returns>
        public bool HasOperatorContragent()
        {
            var srvice = this.Container.Resolve<IRepository<OperatorContragent>>();
            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return false;
                }

                return srvice.GetAll().Any(x => x.Operator.Id == user.Id);
            }
            finally
            {
                this.Container.Release(srvice);
            }
        }

        /// <summary>
        /// Метод для определения есть ли у текущего оператора Муниципальные образования
        /// </summary>
        /// <returns></returns>
        public bool HasOperatorMunicipalities()
        {
            var service = this.Container.Resolve<IRepository<OperatorMunicipality>>();

            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return false;
                }

                return service.GetAll().Any(x => x.Operator.Id == user.Id);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public List<long> GetContragentIds()
        {
            var user = this.Container.Resolve<IUserIdentity>();
            var opContrService = this.Container.Resolve<IRepository<OperatorContragent>>();
            try
            {
                if (user is AnonymousUserIdentity)
                {
                    return new List<long>();
                }

                return opContrService.GetAll()
                    .Where(x => x.Operator.User.Id == user.UserId)
                    .Select(x => x.Contragent.Id)
                    .ToList();
            }
            finally
            {
                this.Container.Release(user);
                this.Container.Release(opContrService);
            }
        }

        public List<long> GetMunicipalityIds()
        {
            var user = this.Container.Resolve<IUserIdentity>();
            var opMuService = this.Container.Resolve<IRepository<OperatorMunicipality>>();
            try
            {
                if (user is AnonymousUserIdentity)
                {
                    return new List<long>();
                }

                return opMuService.GetAll()
                    .Where(x => x.Operator.User.Id == user.UserId)
                    .Select(x => x.Municipality.Id)
                    .ToList();
            }
            finally
            {
                this.Container.Release(opMuService);
            }
        }

        public List<long> GetInspectorIds()
        {
            var user = this.Container.Resolve<IUserIdentity>();
            var opInspService = this.Container.Resolve<IRepository<OperatorInspector>>();
            var operatorService = this.Container.Resolve<IRepository<Operator>>();
            var sobscrService = this.Container.Resolve<IRepository<InspectorSubscription>>();

            try
            {
                if (user is AnonymousUserIdentity)
                {
                    return new List<long>();
                }

                List<long> listSubscribers = new List<long>();
                try
                {

                    var thisOperator = operatorService.GetAll()
                        .FirstOrDefault(x => x.User.Id == user.UserId);

                    if (thisOperator != null && thisOperator.Inspector != null)
                    {
                        listSubscribers.Add(thisOperator.Inspector.Id);
                        listSubscribers.AddRange(sobscrService.GetAll()
                            .Where(x => x.SignedInspector == thisOperator.Inspector).Select(x => x.Inspector.Id).Distinct().ToList());
                    }
                }
                catch (Exception e)
                {

                }

                listSubscribers.AddRange(opInspService.GetAll()
                    .Where(x => x.Operator.User.Id == user.UserId)
                    .Select(x => x.Inspector.Id)
                    .ToList());

                return listSubscribers.Distinct().ToList();
            }
            finally
            {
                this.Container.Release(user);
                this.Container.Release(opInspService);
                this.Container.Release(operatorService);
                this.Container.Release(sobscrService);
            }
        
        }

        public IList<Role> GetNoServiceFilterRoles()
        {
            var roles = new List<Role>();
            var r = this.Container.ResolveAll<INoServiceFilterRole>().Select(x => x.RoleName).ToList();

            this.Container.UsingForResolvedAll<INoServiceFilterRole>(
                (container, roleNameServices) =>
                    roles.AddRange(this.GetNoServiceFilterRoles(roleNameServices.Select(x => x.RoleName.ToLower()).ToList)));

            return roles;
        }

        public IList<Role> GetNoServiceFilterRoles(Func<IList<string>> roleNames)
        {
            var roles = new List<Role>();

            this.Container.UsingForResolved<IDomainService<Role>>(
                (container, roleDomain) => roleDomain.GetAll()
                    .AsEnumerable()
                    .Where(x => roleNames().Contains(x.Name.ToLower()))
                    .ForEach(roles.Add));

            return roles;
        }

        /// <summary>
        /// Получение/создание базового оператора для входа через ЕСИА
        /// </summary>
        /// <returns></returns>
        public Operator GetBaseOperatorForEsia()
        {
            var operatorRepo = this.Container.ResolveRepository<Operator>();
            var roleDomain = this.Container.ResolveDomain<Role>();
            var userService = this.Container.Resolve<IUserService>();
            var userDomain = this.Container.ResolveDomain<User>();

            try
            {
                var baseOperatorForEsia =
                    operatorRepo.GetAll().FirstOrDefault(x => x.User.Roles.Any(y => y.Role.Name == "esia"));

                if (baseOperatorForEsia == null)
                {
                    var esiaRole = roleDomain.GetAll().FirstOrDefault(x => x.Name == "esia");
                    if (esiaRole == null)
                    {
                        esiaRole = new Role { Name = "esia" };
                        roleDomain.Save(esiaRole);
                    }

                    var rolesList = new List<Role> { esiaRole };

                    var esiaUser = this.SaveCustomUser(0, "esia", "esia", "esia", string.Empty, rolesList);

                    baseOperatorForEsia = new Operator
                    {
                        User = esiaUser,
                        IsActive = true
                    };

                    operatorRepo.Save(baseOperatorForEsia);
                }

                return baseOperatorForEsia;
            }
            finally
            {
                this.Container.Release(operatorRepo);
                this.Container.Release(roleDomain);
                this.Container.Release(userService);
                this.Container.Release(userDomain);
            }
        }

        /// <summary>
        /// Получет список инспекторов текущего оператора
        /// </summary>
        public List<Inspector> GetActiveOperatorInspectors()
        {
            var service = this.Container.Resolve<IRepository<OperatorInspector>>();
            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return new List<Inspector>();
                }

                return service.GetAll().Where(x => x.Operator.Id == user.Id).Select(x => x.Inspector).ToList();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получает список контрагентов текущего оператора
        /// </summary>
        public List<Contragent> GetActiveOperatorContragents()
        {
            var service = this.Container.Resolve<IRepository<OperatorContragent>>();
            try
            {
                var user = this.GetActiveOperator();

                if (user == null)
                {
                    return new List<Contragent>();
                }

                return service.GetAll().Where(x => x.Operator.Id == user.Id).Select(x => x.Contragent).ToList();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <inheritdoc />
        public IList<Role> GetActiveOperatorRoles()
        {
            var userRolesService = this.Container.Resolve<IDomainService<UserRole>>();
            using (this.Container.Using(userRolesService))
            {
                var userId = this.GetActiveUser()?.Id ?? 0;

                return userRolesService.GetAll()
                    .Where(x => x.User.Id == userId)
                    .Select(x => x.Role)
                    .ToList();
            }
        }
    }
}