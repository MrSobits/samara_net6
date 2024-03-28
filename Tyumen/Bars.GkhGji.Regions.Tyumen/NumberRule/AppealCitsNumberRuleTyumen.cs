using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using System.Linq;

namespace Bars.GkhGji.Regions.Tyumen.NumberRule
{
    public class AppealCitsNumberRuleTyumen : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            var appeal = entity as AppealCits;

            if (appeal == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(appeal.Number))
            {
                // Сначала делаем запрос, чтобы проверить потом, что в нем существуют записи. Если записей нет, то Max() падает.
                var query = this.Container.Resolve<IDomainService<AppealCits>>()
                        .GetAll()
                        .Where(x => x.Number != null && x.Year == appeal.Year)
                        .Select(x => x.Number);

                appeal.Number = query.Any() ? (query.AsEnumerable().Select(x => x.ToInt()).Max() + 1).ToStr() : "1";
            }

            if (string.IsNullOrEmpty(appeal.DocumentNumber))
            {
                appeal.DocumentNumber = string.Format("{0}-ж/{1}", appeal.Number, appeal.DateFrom.Value.Year);
            }
        }
    }
}