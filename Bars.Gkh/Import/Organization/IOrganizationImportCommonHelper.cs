namespace Bars.Gkh.Import.Organization
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;

    using NHibernate;

    public interface IOrganizationImportCommonHelper
    {
        void Init();
        List<Contragent> GetContragentListByMixedKey(string mixedKey);

        MunicipalityProxy GetMunicipalityProxy(string name);

        long? GetOrganizationFormIdByName(string name);

        void SaveData(IStatelessSession session);

        IDataResult CreateOrSetContragent(Record record);

        void UpdateRealtyObjectManOrg(long roId, DateTime contractStartDate, string manOrgName, string manInn, string startControlDate, string typeContract);

        List<ContragentProxy> GetContragents ();
    }
}