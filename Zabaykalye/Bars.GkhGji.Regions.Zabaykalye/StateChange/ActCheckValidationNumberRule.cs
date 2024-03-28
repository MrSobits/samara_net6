namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_zabaykalye_actcheck_validation_number"; } }

        public override string Name { get { return "Забайкалье - Проверка возможности формирования номера акта проверки"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Забайкалье - Данное правило проверяет формирование номера акта проверки в соответствии с правилами"; } }

        protected override void Action(DocumentGji document)
        {
            var actCheckDomain = Container.ResolveDomain<ActCheck>();
            var actSurveyDomain = Container.ResolveDomain<ActSurvey>();
            try
            {
                var num = 0;

                var actCheck = actCheckDomain.Get(document.Id);
                /*
                 * Для актов обследования своя нумерация 
                 * Для актов проверки своя
                 */
                var documentDate = document.DocumentDate.ToDateTime();
                if (actCheck.TypeActCheck == TypeActCheckGji.ActView)
                {
                    num = actCheckDomain.GetAll()
                        .Where(x => x.TypeActCheck == TypeActCheckGji.ActView && x.Id != document.Id)
                        .Select(x => new {x.DocumentNumber, x.DocumentDate})
                        .Where(x => x.DocumentNumber != null && x.DocumentDate.HasValue && x.DocumentDate.Value.Year == documentDate.Year)
                        .SafeMax(x => x.DocumentNumber.ToInt());
                }
                else
                {
                    num = actCheckDomain.GetAll()
                        .Where(x => x.TypeActCheck != TypeActCheckGji.ActView && x.Id != document.Id)
                        .Select(x => new {x.DocumentNumber, x.DocumentDate})
                        .Where(x => x.DocumentNumber != null && x.DocumentDate.HasValue && x.DocumentDate.Value.Year == documentDate.Year)
                        .SafeMax(x => x.DocumentNumber.ToInt());
                }
                
                document.DocumentNum = num+1;
                document.DocumentNumber = document.DocumentNum.ToString();

            }
            finally
            {
                Container.Release(actCheckDomain);
                Container.Release(actSurveyDomain);
            }

        }
    }
}