namespace Bars.GkhGji.NumberRule
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class AppealCitsNumberRuleTat : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            var appeal = entity as AppealCits;

            if(appeal == null)
                return;

            if (string.IsNullOrEmpty(appeal.Number))
            {
                var query =
                    Container.Resolve<IDomainService<AppealCits>>().GetAll()
                        .Where(x => x.Number != null)
                        .Select(x => x.Number);

                appeal.Number = query.Any() ? (query.AsEnumerable().Select(x => x.ToLong()).Max() + 1).ToStr() : "1";
            }

            if (string.IsNullOrEmpty(appeal.DocumentNumber) && appeal.Id > 0)
            {
                var code = Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll()
                                         .Where(x => x.AppealCits.Id == appeal.Id)
                                         .Select(x => x.RealityObject.Municipality.Code)
                                         .FirstOrDefault();

                appeal.DocumentNumber = string.Format("{0}-{1}", code, appeal.Number);
            }
        }
    }
}