namespace Bars.Gkh.Import.RoImport
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    using NHibernate;

    public interface IStructElementImport
    {
        string AddToSaveList(RealityObject realityObject, List<StructuralElementRecord> structElements);
        void SaveData(IStatelessSession session);
    }
}