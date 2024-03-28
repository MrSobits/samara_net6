namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

    using Castle.Windsor;

    public class PersonalAccountPaymentInfoInDeserializer : DefaultImportExportSerializer<PersonalAccountPaymentInfoIn>
    {
        public PersonalAccountPaymentInfoInDeserializer(IWindsorContainer container)
            : base(container)
        {
        }

        public IDomainService<PaymentAgent> PaymentAgentDomain { get; set; }

        public override ImportResult<PersonalAccountPaymentInfoIn> Deserialize(Stream data, IImportMap format,
            string fileName = null, DynamicDictionary extraParams = null)
        {
            ImportResult<PersonalAccountPaymentInfoIn> result = null;

            if (format.Format.ToLowerInvariant() == "xml")
            {
                var deserialized = DeserializeXml(data);
                result = new ImportResult<PersonalAccountPaymentInfoIn>
                {
                    FileDate = _fileDate,
                    FileNumber = _fileNumber,
                    Rows = deserialized
                };
            }

            if (format.ProviderCode == "fs_gorod")
            {
                result = new FsGorodPaymentInfoSerializer(Container).Deserialize(data, format, fileName, extraParams);

                var agentId = result.GeneralData["AgentId"].ToStr();
                result.GeneralData["AgentName"] = PaymentAgentDomain.GetAll()
                    .Where(x => x.Code == agentId)
                    .Select(x => x.Contragent.Name)
                    .FirstOrDefault();
            }

            #region dbf

            if (format.Format.ToLowerInvariant() == "dbf")
            {
                if (fileName == null || fileName.Length < 3)
                {
                    throw new ArgumentException("Название файла не соответствует формату");
                }

                var first3Characters = fileName.Length > 2 ? fileName.Substring(0, 3) : null;

                var providers = PaymentAgentDomain.GetAll()
                    .WhereIf(!first3Characters.IsEmpty(),
                        x => x.PenaltyContractId == first3Characters || x.SumContractId == first3Characters)
                    .Where(x => x.PenaltyContractId != null)
                    .Where(x => x.SumContractId != null)
                    .Select(x => new
                    {
                        x.Code,
                        x.Contragent.Name,
                        x.PenaltyContractId,
                        x.SumContractId
                    })
                    .ToList();

                if (!providers.Any())
                {
                    throw new ArgumentException("Не удалось получить платежного агента");
                }

                if (providers.Count > 1)
                {
                    throw new ArgumentException(
                        "По коду {0} найдено более одного платежного агента".FormatUsing(first3Characters));
                }

                var provider = providers.First();

                result = base.Deserialize(data, format, fileName);

                result.GeneralData = new DynamicDictionary
                {
                    { "AgentId", provider.Code },
                    { "AgentName", provider.Name }
                };

                if (first3Characters == provider.PenaltyContractId)
                {
                    foreach (var item in result.Rows)
                    {
                        item.Value.PenaltyPaid = item.Value.TargetPaid;
                        item.Value.TargetPaid = 0m;
                        item.Value.SumPaid = 0m;
                    }
                }
                else
                {
                    foreach (var item in result.Rows)
                    {
                        item.Value.SumPaid = item.Value.TargetPaid;
                        item.Value.TargetPaid = 0m;
                        item.Value.PenaltyPaid = 0m;
                    }
                }
            }

            #endregion

            #region json

            if (format.Format.ToLowerInvariant() == "json")
            {
                result = new JsonPaymentInfoSerializer(Container).Deserialize(data, format, fileName, extraParams);
            }

            #endregion

            return result;
        }

        #region xml

        private IEnumerable<ImportRow<PersonalAccountPaymentInfoIn>> DeserializeXml(Stream data)
        {
            Imports.File file;
            try
            {
                using (var reader = new StreamReader(data, Encoding.GetEncoding(1251)))
                {
                    var ser = new XmlSerializer(typeof (Imports.File));
                    file = (Imports.File) ser.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Выбранный файл не соответствует формату выгрузки"));
            }

            if (file == null)
            {
                throw new Exception(string.Format("Выбранный файл не соответствует формату выгрузки"));
            }

            _fileNumber = file.FileNumber;

            DateTime.TryParseExact(file.FileDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _fileDate);

            if (!file.PersonalAccounts.Any())
            {
                throw new Exception(string.Format("Отсутствуют данные для загрузки"));
            }

            var payments = new List<ImportRow<PersonalAccountPaymentInfoIn>>();

            foreach (
                var personalAccount in file.PersonalAccounts.Where(x => x.Payments != null && x.Payments.Length > 0))
            {
                DateTime date;

                var record = new ImportRow<PersonalAccountPaymentInfoIn>();

                payments.Add(record);

                if (
                    !DateTime.TryParseExact(personalAccount.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out date))
                {
                    AddError(personalAccount, record,
                        "Неверный формат даты: '" + personalAccount.Date + "'. Дата ожидается в формате 'гггг-мм-дд'");
                    continue;
                }

                var newPayment = new PersonalAccountPaymentInfoIn
                {
                    OwnerType = personalAccount.Type.To<PersonalAccountPaymentInfoIn.AccountType>(),
                    AccountNumber = personalAccount.Number,
                    Name = personalAccount.Name,
                    Surname = personalAccount.Surname,
                    Patronymic = personalAccount.Patronymic,
                    Inn = personalAccount.Inn,
                    Kpp = personalAccount.Kpp,
                    PaymentDate = date,
                    AccountIsClosed = false,
                    DatePeriod = date,
                    Reason = personalAccount.Reason
                };

                record.Value = newPayment;

                var culture = CultureInfo.CreateSpecificCulture("ru-RU");

                foreach (var payment in personalAccount.Payments)
                {
                    decimal sum;

                    if (!decimal.TryParse((payment.Sum ?? "").Replace('.', ','), NumberStyles.Number, culture, out sum))
                    {
                        AddError(personalAccount, record,
                            string.Format("Не удалось распознать сумму: '{0}'", payment.Sum));
                        continue;
                    }

                    switch ((payment.Target ?? "").Trim())
                    {
                        case "01":
                            newPayment.TargetPaid = sum;
                            break;
                        case "02":
                            newPayment.PenaltyPaid = sum;
                            break;
                        default:
                            AddError(personalAccount, record,
                                string.Format("Не удалось распознать назначение платежа: '{0}'", payment.Target));
                            continue;
                    }
                }
            }

            return payments;
        }

        private static void AddError(PersonalAccount key, ImportRow<PersonalAccountPaymentInfoIn> record, string message)
        {
            var title =
                "type = '{0}', number = '{1}' surname = '{2}' name = '{3}' patronymic = '{4}' inn = '{5}' kpp = '{6}' date = '{7}'"
                    .FormatUsing(
                        key.Type,
                        key.Number,
                        key.Surname,
                        key.Name,
                        key.Patronymic,
                        key.Inn,
                        key.Kpp,
                        key.Date);

            record.Warning = message;
            record.Title = title;
        }

        #endregion

        private DateTime _fileDate;
        private string _fileNumber;
    }
}