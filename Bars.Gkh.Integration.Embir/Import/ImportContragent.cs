using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Import;
using Bars.Gkh.Integration.DataFetcher;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.RegOperator.Domain;
using Castle.Core.Internal;
using Castle.Windsor;
using Newtonsoft.Json.Linq;

namespace Bars.Gkh.Integration.Embir.Import
{
    using System.Reflection;
    using Gkh.Import.Impl;

    public class ImportContragentEmbir : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportEmbir"; }
        }

        public override string Name
        {
            get { return "Импорт контрагентов с ЕМБИР"; }
        }

        public override string PossibleFileExtensions
        {
            get { return string.Empty; }
        }

        public override string PermissionName
        {
            get { return "Import.Embir.View"; }
        }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public IDomainService<Contragent> ContragentDomain{ get; set; }

        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }

        public IDomainService<ContragentBankCreditOrg> ContragentBankDomain { get; set; }

        public IDomainService<Position> PositionDomain { get; set; }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var importHelper = new ImportIntegrationHelper(Container);
            var httpQueryBuilder = importHelper.GetHttpQueryBuilder();
            httpQueryBuilder.AddParameter("type", "SubjectGkh");

            var webClientFetcher = new WebClientFetcher();

            var data = webClientFetcher.GetData(httpQueryBuilder);

            dynamic[] listSubjectGkh = Enumerable.ToArray(data ?? new List<JObject>());

            var contragentByInn = ContragentDomain
                .GetAll()
                .Where(x => x.Inn != null && x.Inn != "")
                .AsEnumerable()
                .GroupBy(x => x.Inn.Trim())
                .ToDictionary(x => x.Key, y => y.First());

            var contragentContracts = ContragentContactDomain.GetAll()
                .Where(x => x.Position.Code == "1")
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            var contragentBanks = ContragentBankDomain.GetAll()
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            var leaderPosition = PositionDomain.GetAll().FirstOrDefault(x => x.Code == "1");

            if (leaderPosition == null)
            {

                leaderPosition = new Position
                {
                    Code = "1",
                    Name = "Руководитель"
                };

                PositionDomain.Save(leaderPosition);
            }

            var listContragentToSave = new List<Contragent>();
            var listContactToSave = new List<ContragentContact>();
            var listBankToSave = new List<ContragentBankCreditOrg>();
            foreach (var dynSubjectGkh in listSubjectGkh)
            {
                var subjectGkh = (JObject)(dynSubjectGkh);

                var inn = subjectGkh["SubjectCode"].ToStr().Trim();
                var fioLeader = subjectGkh["FIOLeader"].ToStr().Trim();
                var calcAccount = subjectGkh["CalcAccount"].ToStr().Trim();

                Contragent contragent = contragentByInn.ContainsKey(inn) ? contragentByInn[inn] : new Contragent();

                var subjectName = subjectGkh["SubjectName"].ToStr();
                if (!subjectName.IsEmpty())
                {
                    if (contragent.Name.IsEmpty())
                    {
                        contragent.Name = subjectName;
                        LogImport.Info("Информация", "Добавлена запись в реестр 'Контрагенты'. ИНН:{0}. Наименование: {1}".FormatUsing(inn, subjectName));
                    }
                }

                var subjectAbbr = subjectGkh["SubjectAbbr"].ToStr();
                if (!subjectAbbr.IsEmpty())
                {
                    if (contragent.ShortName.IsEmpty() || (contragent.ShortName == contragent.Name &&  contragent.ShortName != subjectAbbr))
                    {
                        contragent.ShortName = subjectAbbr;
                        LogImport.Info("Информация",
                            "К записи контрагента добавлено сокращенное наименование. ИНН: {0}. Сокр. наименование: {1}"
                                .FormatUsing(inn, subjectAbbr));
                    }
                    else
                    {
                        LogImport.Info("Информация", "У контрагента уже заполнено поле 'Cокращенное наименование'. ИНН:{0}. Значение: {1}. Значение из ЕМБИР: {2}"
                             .FormatUsing(inn, contragent.ShortName, subjectAbbr));
                    }
                }


                var kpp = subjectGkh["KPP"].ToStr();
                if (!subjectAbbr.IsEmpty())
                {
                    if (contragent.Kpp.IsEmpty())
                    {
                        contragent.Kpp = kpp;
                        LogImport.Info("Информация",
                            "К записи контрагента добавлено КПП. ИНН: {0}. КПП: {1}".FormatUsing(inn, kpp));
                    }
                    else
                    {
                        LogImport.Info("Информация", "У контрагента уже заполнено поле 'КПП'. ИНН: {0}. Значение: {1}. Значение из ЕМБИР: {2}"
                             .FormatUsing(inn, contragent.Kpp, kpp));
                    }
                }

                if (string.IsNullOrEmpty(contragent.Inn))
                {
                    contragent.Inn = !string.IsNullOrEmpty(inn) ? inn : string.Empty;
                }

                contragent.ContragentState = ContragentState.Active;

                if (!fioLeader.IsEmpty())
                {
                    var contact = new ContragentContact();
                    var person = new FullNameParser().Parse(fioLeader.ToStr());

                    if (person != null)
                    {
                        contact.Contragent = contragent;
                        contact.Surname = person.LastName;
                        contact.Name = person.FirstName;
                        contact.Patronymic = person.SecondName;
                        contact.Position = leaderPosition;
                        contact.FullName = fioLeader;
                        if (contragentContracts.ContainsKey(contragent.Id))
                        {
                            var existContact = contragentContracts[contragent.Id].FirstOrDefault(
                                x => (x.FullName ?? "").ToUpper() == fioLeader.ToUpper());

                            contact = existContact ?? contact;
                        }
                        else
                        {
                            LogImport.Info("Информация", "Контрагенту добавлен новый руководитель. ИНН: {0}. ФИО: {1} {2} {3}".FormatUsing(inn, contact.Surname, contact.Name, contact.Patronymic));
                        }

                        var phone = subjectGkh["Phones"].ToStr();
                        if (!phone.IsEmpty())
                        {
                            if (contact.Phone.IsEmpty())
                            {
                                contact.Phone = phone;
                                LogImport.Info("Информация",
                                    "У руководителя контрагента добавлена запись в поле 'Телефон'. ИНН: {0}. Телефон: {1}"
                                        .FormatUsing(inn, phone));
                            }
                            else
                            {
                                LogImport.Info("Информация", "У руководителя контрагента уже есть запись в поле 'Телефон'. ИНН: {0}. Значение: {1}. Значение из ЕМБИР: {2}"
                                     .FormatUsing(inn, contact.Phone, phone));
                            }
                        }

                        listContactToSave.Add(contact);
                    }
                }

                if (!calcAccount.IsEmpty())
                {
                    var bank = new ContragentBankCreditOrg();
                    if (contragentBanks.ContainsKey(contragent.Id))
                    {
                        var existBank =
                            contragentBanks[contragent.Id].FirstOrDefault(x => x.SettlementAccount == calcAccount);

                        bank = existBank ?? bank;
                    }
                    else
                    {
                        LogImport.Info("Информация",
                            "Контрагенту добавлен новый расчетный счет. ИНН: {0}. Расчетный счет: {1}".FormatUsing(inn,
                                calcAccount));
                    }

                    bank.Name = "";
                    bank.SettlementAccount = calcAccount;
                    bank.Contragent = contragent;

                    var bik = subjectGkh["BIK"].ToStr();
                    if (!bik.IsEmpty())
                    {
                        if (bank.Bik.IsEmpty())
                        {
                            bank.Bik = bik;
                            LogImport.Info("Информация",
                                "Расчетному счету контрагента добавлена запись в поле 'БИК'. ИНН: {0}. Расчетный счет: {1}. БИК: {2}"
                                    .FormatUsing(inn, calcAccount, bik));
                        }
                        else
                        {
                            LogImport.Info("Информация",
                                "У расчетного счета контрагента уже есть запись в поле 'БИК'. ИНН: {0}. Расчетный счет: {1} Значение: {2}. Значение из ЕМБИР: {3}"
                                    .FormatUsing(inn, calcAccount, bank.Bik, bik));
                        }
                    }

                    listBankToSave.Add(bank);
                }

                listContragentToSave.Add(contragent);
                LogImport.CountAddedRows++;
            }

            TransactionHelper.InsertInManyTransactions(Container, listContragentToSave, 10000, false, true);
            TransactionHelper.InsertInManyTransactions(Container, listContactToSave, 10000, false, true);
            TransactionHelper.InsertInManyTransactions(Container, listBankToSave, 10000, false, true);

            LogImport.SetFileName(Name);
            LogImport.ImportKey = this.Key;
            LogManager.AddLog(LogImport);
            LogManager.FileNameWithoutExtention = Name;
            LogManager.Save();

            return new ImportResult();
        }
    }
}