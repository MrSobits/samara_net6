namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using NHibernate;

    public interface IProgramCrImportService
    {
        void SaveDpkrLink(ILogImport logImport, Dictionary<TypeWorkCr, List<int>> typeWorkYears);

        void DeleteDpkrLink(ISession session, IEnumerable<long> typeWorkIds);

        void DeleteDpkrLink(IStatelessSession session, IEnumerable<long> typeWorkIds);

        void DeleteTypeWorkCrVersionStage1(IStatelessSession session, IEnumerable<long> typeWorkIds);
    }
}
