namespace Bars.Gkh.Import
{
    using System;
    using System.Reflection;
    using B4.Utils;
    using GkhExcel;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using Entities;
    using Enums.Import;
    using Castle.Windsor;
    using B4.DataAccess;
    using B4.Modules.Security;

    using Bars.B4.IoC;

    using Enums;
    using Impl;

    public class ImportOperator : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportOperator"; }
        }

        public override string Name
        {
            get { return "Импорт операторов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.Operator.View"; }
        }

        public IWindsorContainer Container { get; set; }

        #endregion Properties

        public ILogImport LogImport { get; set; }
        public ILogImportManager LogImportManager { get; set; }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var operators = new List<Operator>();
            var operatorMunicipalities = new Dictionary<string, Municipality>();

            var contragentService = Container.Resolve<IDomainService<Contragent>>();
            var roleService = Container.Resolve<IDomainService<Role>>();
            var municipalityService = Container.Resolve<IDomainService<Municipality>>();
            var operatorService = Container.Resolve<IDomainService<Operator>>();
            var operatorMunicipalityService = Container.Resolve<IDomainService<OperatorMunicipality>>();
            var operatorContragentService = Container.Resolve<IDomainService<OperatorContragent>>();

            var file = baseParams.Files["FileImport"];

            //получение сущностей
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var data = excel.GetRows(0, 0);
                    var header = data[0].Select(x => x.Value.ToLower()).ToArray();
                    var dict = new Dictionary<string, string>();

                    for (int i = 1; i < data.Count(); i++)
                    {
                        var row = data[i];
                        if (string.IsNullOrEmpty(row[0].Value))
                        {
                            continue;
                        }
                        dict.Clear();
                        for (int j = 0; j < row.Length; j++)
                        {
                            dict.Add(header[j], row[j].Value);
                        }

                        var login = dict.ContainsKey("login") ? dict["login"] : null;
                        var password = dict.ContainsKey("password") ? dict["password"] : "";
                        var role = dict.ContainsKey("role") ? dict["role"] : null;
                        var mu = dict["mu"];
                        var innOrg = dict["inn_org"];
                        var name = dict.ContainsKey("fio") ? dict["fio"] : null;
                        var email = dict["email"];
                        var phone = dict["phone"];

                        if (login.IsEmpty())
                        {
                            LogImport.Error("Оператор не добавлен", string.Format("Не заполнено обязательное поле \"Логин\" в строке {0}", i + 1));
                            continue;
                        }

                        if (role.IsEmpty())
                        {
                            LogImport.Error("Оператор не добавлен", string.Format("Не заполнено обязательное поле \"Роль\" в строке {0}", i + 1));
                            continue;
                        }

                        if (name.IsEmpty())
                        {
                            LogImport.Error("Оператор не добавлен", string.Format("Не заполнено обязательное поле \"Имя\" в строке {0}", i + 1));
                            continue;
                        }

                        var Mo = mu;
                        if (mu != null) mu = mu.ToLower().Replace(" ", "");

                        Contragent contragent = contragentService
                            .GetAll()
                            .FirstOrDefault(x => x.Inn == innOrg);

                        if (contragent == null && !innOrg.IsEmpty())
                        {
                            LogImport.Warn("", string.Format("Не найдена организация с ИНН \"{2}\" для оператора с логином \"{0}\" в строке {1}", login, i + 1, innOrg));
                        }

                        var newOperator = new Operator
                        {
                            Name = name,
                            Login = login,
                            Password = password,
                            Email = email,
                            Phone = phone,
                            Contragent = contragent,
                            Role = roleService
                                .GetAll()
                                .FirstOrDefault(x => x.Name.ToLower() == role.ToLower()),

                            TypeWorkplace = TypeWorkplace.FrontOffice,
                            IsActive = true
                        };

                        if (newOperator.Role == null)
                        {
                            LogImport.Error("Оператор не добавлен", string.Format("Не найдена роль \"{2}\" для оператора с логином \"{0}\" в строке {1}", login, i + 1, role));
                            continue;
                        }

                        operators.Add(newOperator);

                        //МО
                        var municipality = municipalityService
                            .GetAll()
                            .FirstOrDefault(x => x.Name.ToLower().Replace(" ", "") == mu.ToLower().Replace(" ", ""));

                        if (municipality == null && !mu.IsEmpty())
                        {
                            LogImport.Warn("", string.Format("Не найдено муниципальное образование \"{2}\" для оператора с логином \"{0}\" в строке {1}", login, i + 1, Mo));
                        }

                        if (municipality != null && !operatorMunicipalities.ContainsKey(newOperator.Login) &&
                            !operatorMunicipalityService.GetAll().Any(x => x.Operator.User.Login == newOperator.Login && x.Municipality.Id == municipality.Id))
                        {
                            operatorMunicipalities.Add(newOperator.Login, municipality);
                        }
                    }
                }
            }

            //сохранение сущностей
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var oper in operators)
                    {
                        var existOper = operatorService.GetAll().FirstOrDefault(x => x.User.Login == oper.Login);
                        if (existOper == null)
                        {
                            operatorService.Save(oper);

                            LogImport.Info("Оператор добавлен", string.Format("логин \"{0}\"", oper.Login));

                            if (operatorMunicipalities.ContainsKey(oper.Login))
                            {
                                var operMo = new OperatorMunicipality
                                {
                                    Operator = oper,
                                    Municipality = operatorMunicipalities[oper.Login]
                                };
                                operatorMunicipalityService.Save(operMo);
                            }

                            if (oper.Contragent != null)
                            {
                                var operContr = new OperatorContragent
                                {
                                    Operator = oper,
                                    Contragent = oper.Contragent
                                };
                                operatorContragentService.Save(operContr);
                            }

                            LogImport.CountAddedRows++;
                        }
                        else
                        {
                            LogImport.Warn("Оператор не добавлен", string.Format("Оператор с таким логином \"{0}\" уже существует", oper.Login));
                        }
                    }

                    transaction.Commit();

                    //логи
                    LogImport.SetFileName(file.FileName);
                    LogImport.ImportKey = CodeImport;

                    LogImportManager.FileNameWithoutExtention = file.FileName;
                    LogImportManager.Add(file, LogImport);
                    LogImportManager.Save();

                }
                catch (Exception exc)
                {
                    transaction.Rollback();

                    Container.Release(contragentService);
                    Container.Release(roleService);
                    Container.Release(municipalityService);
                    Container.Release(operatorService);
                    Container.Release(operatorMunicipalityService);
                    Container.Release(operatorContragentService);

                    throw;
                }
            }

            var statusImport = LogImport.CountError > 0 ? StatusImport.CompletedWithError :
                        LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning :
                        StatusImport.CompletedWithoutError;

            return new ImportResult(statusImport, string.Format("Импортировано {0} записей", LogImport.CountAddedRows), string.Empty, LogImportManager.LogFileId);
        }
    }
}