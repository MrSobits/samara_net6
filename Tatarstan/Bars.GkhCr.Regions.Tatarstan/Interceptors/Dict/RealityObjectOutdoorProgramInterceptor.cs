namespace Bars.GkhCr.Regions.Tatarstan.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    using NHibernate.Util;

    public class RealityObjectOutdoorProgramInterceptor : EmptyDomainInterceptor<RealityObjectOutdoorProgram>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectOutdoorProgram> service, RealityObjectOutdoorProgram entity)
        {
            var changeJournalDomain = this.Container.ResolveDomain<RealityObjectOutdoorProgramChangeJournal>();

            using (this.Container.Using(changeJournalDomain))
            {
                // удаление записей журнала перед удалением программы
                changeJournalDomain.GetAll()
                    .Where(x => x.RealityObjectOutdoorProgram.Id == entity.Id)
                    .Select(x => x.Id)
                    .AsEnumerable()
                    .ForEach(x => changeJournalDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}
