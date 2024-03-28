namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Gkh.Entities;
    using Mapping;
    using NHibernate.Linq;
    using Wcf.Contracts.PersonalAccount;
    using Castle.Windsor;

    public class PersonalAccountPaymentInfoInAltDeserializer :
        DefaultImportExportSerializer<PersonalAccountPaymentInfoIn>
    {
        public override string Code
        {
            get { return "dbf2"; }
        }

        protected IWindsorContainer Container;

        public PersonalAccountPaymentInfoInAltDeserializer(IWindsorContainer container)
            : base(container)
        {
            Container = container;
        }

        public override ImportResult<PersonalAccountPaymentInfoIn> Deserialize(
            Stream data,
            IImportMap format,
            string fileName = null,
            DynamicDictionary extraParams = null)
        {
            ValidateFileName(fileName);
            ImportResult<PersonalAccountPaymentInfoIn> result = base.Deserialize(data, format, fileName);

            var agent = FindPaymentAgent(fileName);

            TransformRows(result.Rows, agent);
            ApplyGeneralData(result, agent);

            return result;
        }

        protected void ApplyGeneralData(ImportResult<PersonalAccountPaymentInfoIn> result, PaymentAgent agent)
        {
            result.GeneralData = new DynamicDictionary
            {
                {"AgentId", agent.Code},
                {"AgentName", agent.Contragent.Name}
            };
        }

        protected void ValidateFileName(string fileName)
        {
            const string pattern = @"^[1-9]\d{9}RP\d{6}$";
            if (fileName == null || Regex.IsMatch(fileName, pattern))
            {
                throw new ArgumentException("Название файла не соответствует формату (минимум 10 символов)");
            }
        }

        protected PaymentAgent FindPaymentAgent(string fileName)
        {
            var paymentAgentsDomain = Container.ResolveDomain<PaymentAgent>();
            using (Container.Using(paymentAgentsDomain))
            {
                var code = fileName.Substring(0, 10);
                var agents = paymentAgentsDomain.GetAll().Where(x => x.Code == code).Fetch(x => x.Contragent).ToList();

                if (agents.Count > 1)
                {
                    throw new ArgumentException("По коду {0} найдено более одного платежного агента".FormatUsing(code));
                }

                if (!agents.Any())
                {
                    throw new ArgumentException("Не удалось получить платежного агента");
                }

                return agents.First();
            }
        }

        protected void TransformRows(IEnumerable<ImportRow<PersonalAccountPaymentInfoIn>> rows, PaymentAgent agent)
        {
            foreach (var importRow in rows)
            {
                importRow.Value.DatePeriod = DateTime.ParseExact("01" + importRow.Value.Period, "ddMMyy",
                    CultureInfo.InvariantCulture);
                if (importRow.Value.AccountNumber.IsNotEmpty())
                {
                    importRow.Value.AccountNumber = importRow.Value.AccountNumber.TrimStart(new[] {'0'});
                }
                if (importRow.Value.ExternalAccountNumber.IsNotEmpty())
                {
                    importRow.Value.ExternalAccountNumber = importRow.Value.ExternalAccountNumber.TrimStart(new[] {'0'});
                }
            }
        }
    }
}
