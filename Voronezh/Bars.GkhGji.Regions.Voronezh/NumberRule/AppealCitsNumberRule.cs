namespace Bars.GkhGji.Regions.Voronezh.NumberRule
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило проставления номера обращения (Воронеж)
    /// </summary>
    public class AppealCitsNumberRule : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        /// <unheritfoc />
        public void SetNumber(IEntity entity)
        {
            var appeal = entity as AppealCits;

            if(appeal == null)
                return;

            var kindStatementGjiDomain = this.Container.ResolveDomain<KindStatementGji>();
            var appealCitsDomain = this.Container.ResolveDomain<AppealCits>();
            try
            {
                if (string.IsNullOrEmpty(appeal.NumberGji))
                {
                    var query = appealCitsDomain.GetAll()
                        .Where(x => x.NumberGji != null)
                        .Select(x => x.NumberGji);

                    appeal.NumberGji = query.Any()
                        ? (query.AsEnumerable().Select(x => x.ToLong()).Max() + 1).ToStr()
                        : "1";
                }

                //при обновлении будет пусто - затирается на клиентской части при смене вида обращения
                if (string.IsNullOrEmpty(appeal.DocumentNumber))
                {
                    var postfix = appeal.KindStatement?.Postfix ?? string.Empty;

                    var kindStatements = kindStatementGjiDomain.GetAll()
                        .Where(x => x.Postfix == postfix)
                        .Select(x => x.Id)
                        .ToList();

                    var query = appealCitsDomain.GetAll()
                        .Where(x => kindStatements.Contains(x.KindStatement.Id) || (postfix.IsEmpty() && x.KindStatement == null))
                        .Where(x => x.Id != appeal.Id)
                        .Where(x => x.Year == appeal.Year)
                        .Select(x => x.Number);

                    appeal.Number = query.Any()
                        ? (query.AsEnumerable().Select(x => x.ToLong()).Max() + 1).ToStr()
                        : "1";

                    appeal.DocumentNumber = appeal.Number + postfix;
                }
            }
            finally
            {
                this.Container.Release(appealCitsDomain);
                this.Container.Release(kindStatementGjiDomain);
            }
        }
    }
}