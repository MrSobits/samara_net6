namespace Bars.Gkh.Import.RoImport
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    using NHibernate;

    public interface ITechPassportDataImport
    {
        string AddToSaveList(RealityObject realityObject, Dictionary<string, string> data);

        void SaveData(IStatelessSession session);
    }
}