namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using AutoMapper;

    using Bars.B4.Application;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

    public class VtbMessageTransformations
    {
        private static readonly Regex FileNameValidatorRegex = new Regex(@"\d{1,12}");
        public const string VtbMessageId = "VtbMessageId";
        public const string VtbImportedSum = "VtbImportedSum";
        public const string VtbSender = "VtbSender";

        private static IList<PersonalAccountPaymentInfoIn> Tranform(IEnumerable<VtbOperation> operations)
        {
            var mapper = ApplicationContext.Current.Container.Resolve<IMapper>();
            return Enumerable.ToList(operations.Select(mapper.Map<PersonalAccountPaymentInfoIn>));
        }

        public static ImportResult<PersonalAccountPaymentInfoIn> Transform(Stream source, string fileName)
        {
            var s = new XmlSerializer(typeof(VtbMessage));
            var vtbMessage = (VtbMessage) s.Deserialize(source);

            if (!vtbMessage.IsValid())
            {
                throw new ArgumentException(
                    "Схема документа BTБ24 корректна, но содержимое не проходит внутренние проверки");
            }

            var result = new ImportResult<PersonalAccountPaymentInfoIn>
            {
                FileNumber = fileName,
                FileDate = vtbMessage.Date
            };

            // трансформируем и дополняем платежи
            var infoIns = Tranform(vtbMessage.Operations);

            result.Rows = (from v in infoIns select new ImportRow<PersonalAccountPaymentInfoIn>() {Value = v}).ToList();

            result.GeneralData[VtbMessageId] = vtbMessage.Id;
            result.GeneralData[VtbImportedSum] = vtbMessage.Total.Amount;
            result.GeneralData[VtbSender] = vtbMessage.Sender;

            return result;
        }


    }
}
