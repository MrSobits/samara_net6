using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Saha.StateChange
{
    public class DisposalValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name 
        {
            get { return string.Format("Проверка возможности формирования номера {0} (Cаха)", DisposalText.GenetiveCase.ToLower()); } 
        }

        public override string Description 
        {
            get { return string.Format("Данное правило проверяет формирование номера {0} в соответствии с правилами (Cаха)", DisposalText.GenetiveCase.ToLower()); }
        }

        protected override void Action(DocumentGji document)
        {
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();

            try
            {
                var maxNum = disposalDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum) ?? 0;

                document.DocumentNum = maxNum + 1;

                document.DocumentNumber = "01-10-{0}-{1}".FormatUsing(document.DocumentNum.ToStr().PadLeft(2, '0'), DateTime.Now.Year % 100);

            }
            finally
            {
                Container.Release(disposalDomain);
            }
        }
    }
}