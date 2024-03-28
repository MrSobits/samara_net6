namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;

    public interface ITatarstanDisposalService
    {
        IDataResult GetDependenciesString(BaseParams baseParams);

        List<TatarstanDisposalBeforeDeleteItem> GetDependenciesItems(long disposalId);
    }
}
