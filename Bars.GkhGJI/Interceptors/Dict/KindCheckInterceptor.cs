namespace Bars.GkhGji.Interceptors
{
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class KindCheckInterceptor : EmptyDomainInterceptor<KindCheckGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KindCheckGji> service, KindCheckGji entity)
        {
            var dependDisposal =
                Container.Resolve<IDomainService<Disposal>>().GetAll()
                         .Where(x => x.KindCheck.Id == entity.Id && x.DocumentNumber != null)
                         .Select(x => new { x.DocumentNumber, x.DocumentDate })
                         .OrderBy(x => x.DocumentNumber)
                         .Take(10)
                         .ToList();

            if (dependDisposal.Any())
            {
                var strBuilder = new StringBuilder("Данный вид проверки имеет зависимые документы:");

                foreach (var item in dependDisposal)
                {
                    strBuilder.Append("<br>");
                    strBuilder.AppendFormat("№ {0} от {1}",
                        item.DocumentNumber,
                        item.DocumentDate.HasValue
                            ? item.DocumentDate.Value.ToShortDateString()
                            : "");
                }

                strBuilder.Append("<br>Удаление отменено");

                return this.Failure(strBuilder.ToString());
            }

            //удаляем виды проверки типа обследования
            var dsTypeSurveyKindCheck = Container.Resolve<IDomainService<TypeSurveyKindInspGji>>();

            var dependTypeSurvey = dsTypeSurveyKindCheck.GetAll()
                                            .Where(x => x.KindCheck.Id == entity.Id)
                                            .Select(x => x.Id)
                                            .ToList();

            foreach (var item in dependTypeSurvey)
            {
                dsTypeSurveyKindCheck.Delete(item);
            }

            return this.Success();
        }
    }
}