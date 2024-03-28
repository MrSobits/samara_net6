namespace Bars.Gkh.RegOperator.Imports.VTB24
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Gkh.Entities;
    using Domain.ImportExport;
    using Domain.ImportExport.Mapping;
    using Domain.ImportExport.Serializers;
    using Dto.Origin;
    using Wcf.Contracts.PersonalAccount;
    using Castle.Windsor;

    public class VtbMessageSerializer : IImportExportSerializer<PersonalAccountPaymentInfoIn>
    {
        private readonly IWindsorContainer _container;
        private const string AgentIdKey = "AgentId";
        private const string AgentId = "040813827";
        private const string AgentNameKey = "AgentName";
        public const string OverrideFileInfo = "OverrideFileInfo";

        public VtbMessageSerializer(IWindsorContainer container)
        {
            _container = container;
        }

        public string Code { get { return "vtb24origin"; } }

        public ImportResult<PersonalAccountPaymentInfoIn> Deserialize(Stream data, IImportMap format, string fileName = null, DynamicDictionary extraParams = null)
        {
            var result = VtbMessageTransformations.Transform(data, fileName);

            // для совместимости
            result.GeneralData[AgentIdKey] = AgentId;

            var paymentAgentDomain = _container.ResolveDomain<PaymentAgent>();
            using (_container.Using(paymentAgentDomain))
            {
                result.GeneralData[AgentNameKey] = paymentAgentDomain.GetAll()
                    .Where(x => x.Code == AgentId)
                    .Select(x => x.Contragent.Name)
                    .FirstOrDefault();
            }

            result.GeneralData[OverrideFileInfo] = true;

            // Заменяем номер файла имя_файла-идентификатор_сообщения_ВТБ
            result.FileNumber = String.Format("{0}-{1}", result.FileNumber,
                result.GeneralData.GetAs<long>(VtbMessageTransformations.VtbMessageId)
                    .ToString(CultureInfo.InvariantCulture));

            return result;
        }

        public Stream Serialize(List<PersonalAccountPaymentInfoIn> data, IImportMap format)
        {
            throw new System.NotImplementedException();
        }
    }
}
