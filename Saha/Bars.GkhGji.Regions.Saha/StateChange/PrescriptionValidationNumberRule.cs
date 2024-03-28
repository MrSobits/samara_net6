using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Saha.StateChange
{
    public class PrescriptionValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_prescription_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера предписания (Cаха)"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания в соответствии с правилами (Cаха)"; } }

        protected override void Action(DocumentGji document)
        {
            var prescriptionDomain = Container.ResolveDomain<Prescription>();

            try
            {
                var maxNum = prescriptionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum) ?? 0;

                document.DocumentNum = maxNum + 1;

                document.DocumentNumber = "08-01-{0}-{1}".FormatUsing(document.DocumentNum.ToStr().PadLeft(2, '0'), DateTime.Now.Year % 100);

            }
            finally
            {
                Container.Release(prescriptionDomain);
            }
        }
    }
}