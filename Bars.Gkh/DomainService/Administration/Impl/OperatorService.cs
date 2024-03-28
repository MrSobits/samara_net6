namespace Bars.Gkh.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.RegOperator.Administration;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Castle.Windsor;

    public class OperatorService : IOperatorService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetActiveOperatorId()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var opt = userManager.GetActiveOperator();
            return new BaseDataResult(new { Id = opt == null ? 0 : opt.Id });
        }

        public IDataResult GetProfile()
        {
            return new BaseDataResult(new { bg = "#fff" });
        }

        public IDataResult GetActiveOperator()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var opt = userManager.GetActiveOperator();
            if (opt != null)
            {
                var obj = Container.Resolve<IDomainService<Operator>>().Get(opt.Id);
                var userRole =
                    Container.Resolve<IDomainService<UserRole>>().FirstOrDefault(x => x.User.Id == obj.User.Id);
                //var userRole = obj.User.Roles.FirstOrDefault();

                return
                    new BaseDataResult(
                        new
                        {
                            obj.Id,
                            obj.User.Name,
                            obj.User.Login,
                            Password = string.Empty,
                            NewPassword = string.Empty,
                            Role = userRole != null ? userRole.Role : null,
                            obj.User.Email,
                            obj.TypeWorkplace,
                            obj.Phone,
                            obj.IsActive
                        });
            }

            return new BaseDataResult(null);
        }

        public IDataResult ChangeProfile(BaseParams baseParams)
        {
            try
            {
                // оператор у которого меняется профиль
                var oprtwhoseChangeProfile = baseParams.Params.GetAs<Operator>("record");

                var password = string.Empty;

                if (string.IsNullOrEmpty(oprtwhoseChangeProfile.Password) && !string.IsNullOrEmpty(oprtwhoseChangeProfile.NewPassword))
                {
                    return new BaseDataResult(false, "Заполните текущий пароль");
                }

                if (oprtwhoseChangeProfile.NewPassword != oprtwhoseChangeProfile.NewPasswordCommit)
                {
                    return new BaseDataResult(false, "Введенные пароли не совпадают");
                }

                if (!string.IsNullOrEmpty(oprtwhoseChangeProfile.Password))
                {
                    var user = this.Container.Resolve<IRepository<User>>().GetAll()
                        .FirstOrDefault(x => x.Login.Equals(oprtwhoseChangeProfile.Login));

                    if (user == null)
                    {
                        return new BaseDataResult(false, "Пользователя не существует");
                    }

                    // старый пароль придет в открытом виде, преобразуем в hash
                    var passMd5 = MD5.GetHashString64(oprtwhoseChangeProfile.Password, HashType.MD5B2);

                    var userPassService = Container.Resolve<IDomainService<UserPassword>>();
                    var userPass = userPassService.GetAll().FirstOrDefault(x => x.User == user && x.Password.Equals(passMd5));

                    if (userPass == null)
                    {
                        return new BaseDataResult(false, "Введен некорректный старый пароль");
                    }

                    var newPassword = oprtwhoseChangeProfile.NewPassword;
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        if (newPassword.Length < 6 || newPassword.Count(char.IsDigit) == 0 || newPassword.Count(char.IsLetter) == 0)
                        {
                            return new BaseDataResult(
                                false,
                                "Ограничение пароля: не менее 6 знаков, должен содержать цифры и буквы.");
                        }

                        // присваиваем новый пароль
                        password = newPassword;
                    }
                }

                var rep = Container.Resolve<IRepository<Operator>>();
                var oprtr = rep.Get(oprtwhoseChangeProfile.Id);
                var userManager = Container.Resolve<IGkhUserManager>();

                userManager.SaveCustomUser(
                    oprtr.User.Id,
                    oprtwhoseChangeProfile.Name,
                    oprtwhoseChangeProfile.Login,
                    password,
                    oprtwhoseChangeProfile.Email,
                    oprtwhoseChangeProfile.Role != null ? new List<Role>() { oprtwhoseChangeProfile.Role } : null);

                // сохраняем телефон в операторе
                oprtr.Phone = oprtwhoseChangeProfile.Phone;
                oprtr.ExportFormat = oprtwhoseChangeProfile.ExportFormat;
                rep.Update(oprtr);

                return new BaseDataResult(true, "Изменения сохранены успешно.");
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Message = exc.Message, Success = false };
            }
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var operatorId = baseParams.Params.GetAs<long>("operatorId");

                var inspectorNames = new StringBuilder();
                var inspectorIds = new StringBuilder();
                var contragentNames = new StringBuilder();
                var contragentIds = new StringBuilder();
                var municipalityNames = new StringBuilder();
                var municipalityIds = new StringBuilder();

                // Пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var serviceOperatorInspector = Container.Resolve<IDomainService<OperatorInspector>>();
                var serviceOperatorContragent = Container.Resolve<IDomainService<OperatorContragent>>();
                var serviceOperatorMunicipality = Container.Resolve<IDomainService<OperatorMunicipality>>();

                var dataInspectors = serviceOperatorInspector.GetAll()
                    .Where(x => x.Operator.Id == operatorId)
                    .Select(x => new
                        {
                            x.Inspector.Id,
                            x.Inspector.Fio
                        })
                    .ToArray();

                var dataManOrgs = serviceOperatorContragent.GetAll()
                  .Where(x => x.Operator.Id == operatorId)
                  .Select(x => new
                      {
                          x.Contragent.Id,
                          x.Contragent.Name
                      })
                  .ToArray();

                var dataMunicipalities = serviceOperatorMunicipality.GetAll()
                  .Where(x => x.Operator.Id == operatorId)
                  .Select(x => new
                      {
                          x.Municipality.Id,
                          x.Municipality.Name
                      })
                  .ToArray();

                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(item.Fio))
                    {
                        if (inspectorNames.Length > 0)
                            inspectorNames.Append(", ");

                        inspectorNames.Append(item.Fio);
                    }

                    if (item.Id > 0)
                    {
                        if (inspectorIds.Length > 0)
                            inspectorIds.Append(", ");

                        inspectorIds.Append(item.Id);
                    }
                }

                foreach (var item in dataManOrgs)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        if (contragentNames.Length > 0)
                            contragentNames.Append(", ");

                        contragentNames.Append(item.Name);
                    }

                    if (item.Id > 0)
                    {
                        if (contragentIds.Length > 0)
                            contragentIds.Append(", ");

                        contragentIds.Append(item.Id);
                    }
                }

                foreach (var item in dataMunicipalities)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        if (municipalityNames.Length > 0)
                            municipalityNames.Append(", ");

                        municipalityNames.Append(item.Name);
                    }

                    if (item.Id > 0)
                    {
                        if (municipalityIds.Length > 0)
                            municipalityIds.Append(", ");

                        municipalityIds.Append(item.Id);
                    }
                }

                Container.Release(serviceOperatorContragent);
                Container.Release(serviceOperatorInspector);
                Container.Release(serviceOperatorMunicipality);

                return new BaseDataResult(new
                    {
                        inspectorNames = inspectorNames.ToString(),
                        inspectorIds = inspectorIds.ToString(),
                        contragentNames = contragentNames.ToString(),
                        contragentIds = contragentIds.ToString(),
                        municipalityNames = municipalityNames.ToString(),
                        municipalityIds = municipalityIds.ToString()
                    });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
        }

        public IDataResult AddInspectors(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var operatorId = baseParams.Params.GetAs<long>("operatorId");
                    var oprt = Container.Resolve<IDomainService<Operator>>().Load(operatorId);

                    var inspectorIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var serviceInspector = Container.Resolve<IDomainService<Inspector>>();
                    var serviceOperatorInspectors = Container.Resolve<IDomainService<OperatorInspector>>();

                    // В этом словаре будут существующие инспектора
                    // key - идентификатор Инспектора
                    // value - идентификатор Инспектора оператора
                    var dictInspectors =
                        serviceOperatorInspectors.GetAll()
                            .Where(x => x.Operator.Id == operatorId)
                            .AsEnumerable()
                            .GroupBy(x => x.Inspector.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in inspectorIds)
                    {
                        if (dictInspectors.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictInspectors.Remove(id);
                            continue;
                        }

                        var newObj = new OperatorInspector
                            {
                                Operator = oprt,
                                Inspector = serviceInspector.Load(id)
                            };

                        serviceOperatorInspectors.Save(newObj);
                    }

                    // Если какието инспектора остались в dictInspectors то их удаляем
                    // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                    foreach (var keyValue in dictInspectors)
                    {
                        serviceOperatorInspectors.Delete(keyValue.Value);
                    }

                    Container.Release(serviceInspector);
                    Container.Release(serviceOperatorInspectors);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var operatorId = baseParams.Params.GetAs<long>("operatorId");
                    var municipalityIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
                    var serviceOperatorMunicipality = Container.Resolve<IDomainService<OperatorMunicipality>>();

                    var dictMunicipalities =
                        serviceOperatorMunicipality.GetAll()
                            .Where(x => x.Operator.Id == operatorId)
                            .AsEnumerable()
                            .GroupBy(x => x.Municipality.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    var oprt = Container.Resolve<IDomainService<Operator>>().Load(operatorId);

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in municipalityIds)
                    {
                        if (dictMunicipalities.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictMunicipalities.Remove(id);
                            continue;
                        }

                        var newObj = new OperatorMunicipality
                            {
                                Operator = oprt,
                                Municipality = serviceMunicipality.Load(id)
                            };

                        serviceOperatorMunicipality.Save(newObj);
                    }

                    foreach (var keyValue in dictMunicipalities)
                    {
                        serviceOperatorMunicipality.Delete(keyValue.Value);
                    }

                    Container.Release(serviceMunicipality);
                    Container.Release(serviceOperatorMunicipality);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult AddContragents(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var operatorId = baseParams.Params.GetAs<long>("operatorId");
                    var contragentIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var serviceContragent = Container.Resolve<IDomainService<Contragent>>();
                    var serviceOperatorCtrn = Container.Resolve<IDomainService<OperatorContragent>>();

                    var dictContragents =
                        serviceOperatorCtrn.GetAll()
                            .Where(x => x.Operator.Id == operatorId)
                            .AsEnumerable()
                            .GroupBy(x => x.Contragent.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    var oprt = Container.Resolve<IDomainService<Operator>>().Load(operatorId);
                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in contragentIds)
                    {
                        if (dictContragents.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictContragents.Remove(id);
                            continue;
                        }

                        var newObj = new OperatorContragent
                            {
                                Operator = oprt,
                                Contragent = serviceContragent.Load(id)
                            };

                        serviceOperatorCtrn.Save(newObj);
                    }

                    foreach (var keyValue in dictContragents)
                    {
                        serviceOperatorCtrn.Delete(keyValue.Value);
                    }

                    Container.Release(serviceContragent);
                    Container.Release(serviceOperatorCtrn);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult ListContragent(BaseParams baseParams)
        {
            var operatorId = baseParams.Params.GetAs<long>("operatorId");
            var service = Container.Resolve<IDomainService<OperatorContragent>>();

            var data = service.GetAll()
                .Where(x => x.Operator.Id == operatorId)
                .Select(x => new
                    {
                        x.Contragent.Id,
                        x.Contragent.Name,
                        x.Contragent.Inn,
                        Municipality = x.Contragent.Municipality.Name
                    });

            int totalCount = data.Count();
            var result = data.ToArray();

            Container.Release(service);

            return new ListDataResult(result, totalCount);
        }

        public IDataResult ListMunicipality(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<OperatorMunicipality>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(userManager, service))
            {
                var operatorId = baseParams.Params.GetAs<long?>("operatorId") ?? userManager.GetActiveOperator()?.Id ?? 0;

                var data = service.GetAll()
                    .Where(x => x.Operator.Id == operatorId)
                    .Select(x => new
                    {
                        x.Municipality.Id,
                        x.Municipality.Name
                    })
                    .ToArray();

                return new ListDataResult(data, data.Length);
            }
        }

        public IDataResult ListInspector(BaseParams baseParams)
        {
            var operatorId = baseParams.Params.GetAs<long>("operatorId");
            var service = Container.Resolve<IDomainService<OperatorInspector>>();

            var data = service.GetAll()
                .Where(x => x.Operator.Id == operatorId)
                .Select(x => new
                    {
                        x.Inspector.Id,
                        x.Inspector.Fio,
                        x.Inspector.Code
                    });

            int totalCount = data.Count();
            var result = data.ToArray();

            Container.Release(service);

            return new ListDataResult(result, totalCount);
        }

        public IDataResult GenerateNewPassword(BaseParams baseParams)
        {
            string password = "";
            string abc = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*()";
            var passCongig = this.Container.GetGkhConfig<AdministrationConfig>().UserPasswordConfig;
            int length = passCongig.PasswordDifficultySwitcher == Enums.YesNo.Yes ? passCongig.MinimalLength : 6;
            Random rnd = new Random();
            int lng = abc.Length;
            int count = 0;
            do
            {               
                string ch = abc[rnd.Next(lng)].ToString();
                if (passCongig.PasswordDifficultySwitcher == Enums.YesNo.Yes && passCongig.PasswordMasks.Count > 0)
                {
                    if (ValidatePasswordChar(ch, passCongig.PasswordMasks))
                    {
                        count++;
                        password += ch;
                    }
                }
                else
                {
                    count++;
                    password += ch;
                }

            }
            while (count < length);            
            
            //for (int i = 0; i < length; i++)
            //    password += abc[rnd.Next(lng)];

            return new BaseDataResult(
                new {password});
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
    }
}