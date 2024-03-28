using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Entities;
using Bars.GkhGji.StateChange;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class DisposalSmolenskNumberRule : BaseDocSmolenskNumberRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_smolensk_disposal_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name
        {
            get { return string.Format("Проверка возможности формирования номера {0} Смоленска", DisposalText.GenetiveCase.ToLower()); }
        }

        public override string Description
        {
            get { return string.Format("Данное правило проверяет формирование номера {0} в соответствии с правилами Смоленска", DisposalText.GenetiveCase.ToLower()); }
        }

        protected override void Action(DocumentGji document)
        {
            var disposalDomain = Container.ResolveDomain<Disposal>();

            try
            {
	            var documentDate = document.DocumentDate.ToDateTime();
                var maxNumber = disposalDomain.GetAll()
					.Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == documentDate.Year)
                    .SafeMax(x => x.DocumentNum).ToInt();

                document.DocumentNum = maxNumber + 1;
                document.DocumentNumber = document.DocumentNum.ToStr().PadLeft(5, '0');

            }
            finally
            {
                Container.Release(disposalDomain);
            }
        }
    }
}