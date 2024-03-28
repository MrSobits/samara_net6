using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Saha.StateChange
{
    public class ActSurveyValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_actsurvey_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта обследования (Cаха)"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования в соответствии с правилами (Cаха)"; } }

        protected override void Action(DocumentGji document)
        {
            var actSurveyDomain = Container.ResolveDomain<ActSurvey>();
            var actCheckDomain = Container.ResolveDomain<ActCheck>();

            try
            {
                var maxActViewNum = actCheckDomain.GetAll()
                    .Where(x => x.TypeActCheck == TypeActCheckGji.ActView)
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum) ?? 0;

                var maxActSurveyNum = actSurveyDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    .SafeMax(x => x.DocumentNum) ?? 0;

                document.DocumentNum = Math.Max(maxActViewNum, maxActSurveyNum) + 1;
                document.DocumentNumber = "08-10-{0}-{1}".FormatUsing(document.DocumentNum.ToStr().PadLeft(2, '0'), DateTime.Now.Year % 100);

            }
            finally
            {
                Container.Release(actSurveyDomain);
            }
        }
    }
}