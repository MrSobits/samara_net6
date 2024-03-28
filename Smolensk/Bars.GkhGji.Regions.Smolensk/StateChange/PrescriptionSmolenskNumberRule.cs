using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;
using Bars.GkhGji.StateChange;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class PrescriptionSmolenskNumberRule : BaseDocSmolenskNumberRule
    {
        public override string Id { get { return "gji_smolensk_prescription_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера предписания Смоленска"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания в соответствии с правилами Смоленска"; } }

        protected override void InternalValidate(DocumentGji document, ValidateResult result)
        {
            var documentInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

            try
            {
                if (!documentInspectorDomain.GetAll().Any(x => x.DocumentGji.Id == document.Id))
                {
                    result.Message = string.Format("Номер не может быть присвоен, потому что у предписания нет инспектора");
                    result.Success = false;

                    return;
                }

                if (!document.DocumentDate.HasValue)
                {
                    result.Message = string.Format("Номер не может быть присвоен, потому что у предписания не сохранена дата");
                    result.Success = false;

                    return;
                }

                result.Success = true;
            }
            finally
            {
                Container.Release(documentInspectorDomain);
            }
        }

        protected override void Action(DocumentGji document)
        {
            var prescriptionDomain = Container.ResolveDomain<DocumentGji>();
            var documentInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

            try
            {
                var inspector = documentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id).Select(x => x.Inspector).First();

                var docQuery = documentInspectorDomain.GetAll()
                    .Where(x => x.Inspector.Id == inspector.Id);

                var maxNumber = prescriptionDomain.GetAll()
                    .Where(x => docQuery.Any(y => y.DocumentGji.Id == x.Id))
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    //.Where(x => x.DocumentSubNum == inspector.Id)
                    .SafeMax(x => x.DocumentNum).ToInt();

                document.DocumentNum = maxNumber + 1;
                document.DocumentNumber = "{0}/{1}".FormatUsing(document.DocumentNum.ToStr().PadLeft(5, '0'), inspector.Code);

            }
            finally
            {
                Container.Release(prescriptionDomain);
                Container.Release(documentInspectorDomain);
            }
        }
    }
}