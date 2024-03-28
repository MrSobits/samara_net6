namespace Bars.B4.Modules.Analytics.Reports.Interceptors
{
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Utils;

    // TODO: времянка. после обновления B4 до версии 2 выпилить
    public class PrintFormCategoryInterceptor : EmptyDomainInterceptor<PrintFormCategory>
    {
        public IDomainService<PrintForm> PrintFormDomain { get; set; }

        public override IDataResult BeforeDeleteAction(
            IDomainService<PrintFormCategory> service,
            PrintFormCategory entity)
        {
            if (PrintFormDomain.GetAll().Any(x => x.Category.Id == entity.Id))
            {
                return
                    Failure(
                        "Удаление категории {0} невозможно, так как имеются связанные с ней печатные формы".FormatUsing(
                            entity.Name));
            }

            return Success();
        }
    }
}