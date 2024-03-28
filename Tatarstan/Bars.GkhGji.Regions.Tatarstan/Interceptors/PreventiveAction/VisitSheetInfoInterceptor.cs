using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction
{
    internal class VisitSheetInfoInterceptor : EmptyDomainInterceptor<VisitSheetInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<VisitSheetInfo> service, VisitSheetInfo entity)
        {
            if (entity.Info.IsEmpty())
            {
                return this.Failure("Необходимо заполнить поле Сведения");
            }

            if (entity.Comment.IsEmpty())
            {
                return this.Failure("Необходимо заполнить поле Комментарий");
            }

            return this.Success();
        }
    }
}