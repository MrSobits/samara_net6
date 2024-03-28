namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System.IO;
    using B4.Utils;
    using Castle.Windsor;
    using Mapping;
    using Wcf.Contracts.PersonalAccount;

    public class PersonalAccountPaymentInfoInSpbDeserializer : DefaultImportExportSerializer<PersonalAccountPaymentInfoIn>
    {
        public override string Code
        {
            get { return "dbf-spb"; }
        }

        public PersonalAccountPaymentInfoInSpbDeserializer(IWindsorContainer container) : base(container)
        {
        }

        public override ImportResult<PersonalAccountPaymentInfoIn> Deserialize(Stream data, IImportMap format, string fileName = null, DynamicDictionary extraParams = null)
        {
            var result = base.Deserialize(data, format, fileName, extraParams);



            return result;
        }
    }
}