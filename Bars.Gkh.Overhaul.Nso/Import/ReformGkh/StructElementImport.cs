namespace Bars.Gkh.Overhaul.Nso.Import.ReformGkh
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import.RoImport;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Core;
    using Gkh.Entities.CommonEstateObject;
    using NHibernate;

    public class StructElementImport : IStructElementImport, IInitializable
    {
        public IRepository<StructuralElement> StructElementmRepository { get; set; }

        private Dictionary<string, StructuralElement> StructElementsDict;

        private List<RealityObjectStructuralElement> realityObjectStructuralElementToCreate = new List<RealityObjectStructuralElement>();

        private string log;

        public void Initialize()
        {
            this.StructElementsDict = this.StructElementmRepository.GetAll()
                .Where(x => x.Code != null)
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, x => x.First());
        }

        public string AddToSaveList(RealityObject realityObject, List<StructuralElementRecord> structElements)
        {
            this.log = string.Empty;

            structElements.ForEach(x => this.SaveRobjStructElem(realityObject, x));

            if (!string.IsNullOrWhiteSpace(this.log))
            {
                this.log = string.Format("В системе не найдены КЭ с кодами: {0}. Соответствующие КЭ из файла не загружены", this.log.Trim(',').Trim());
            }

            return this.log;
        }

        private void SaveRobjStructElem(RealityObject realityObject, StructuralElementRecord structuralElementRecord)
        {
            if (!this.StructElementsDict.ContainsKey(structuralElementRecord.Code))
            {
                this.log += ", " + structuralElementRecord.Code;
                return;
            }

            var structElement = this.StructElementsDict[structuralElementRecord.Code];

            var roStructEl = new RealityObjectStructuralElement
            {
                RealityObject = realityObject,
                StructuralElement = structElement,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            int lastOverhaulYear;
            if (structuralElementRecord.LastOverhaulYear != null && int.TryParse(structuralElementRecord.LastOverhaulYear, out lastOverhaulYear))
            {
                roStructEl.LastOverhaulYear = lastOverhaulYear;
            }

            decimal volume;
            if (structuralElementRecord.Volume != null && this.decimalTryParse(structuralElementRecord.Volume, out volume))
            {
                roStructEl.Volume = volume;
            }

            decimal wearout;
            if (structuralElementRecord.Wearout != null && this.decimalTryParse(structuralElementRecord.Wearout, out wearout))
            {
                roStructEl.Wearout = wearout;
            }

            this.realityObjectStructuralElementToCreate.Add(roStructEl);
        }

        public void SaveData(IStatelessSession session)
        {
            this.realityObjectStructuralElementToCreate.ForEach(x => session.Insert(x));
        }

        private bool decimalTryParse(string str, out decimal value)
        {
            var result = Decimal.TryParse(
                str.Replace(
                    CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator),
                out value);
            return result;
        }
    }
}