namespace Bars.Gkh.Report.TechPassportSections
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.Gkh.PassportProvider;

    using Castle.Windsor;

    using B4;
    
    using Entities;

    public abstract class BaseTechPassportSectionReport : ITechPassportSectionReport
    {
        public IWindsorContainer Container { get; set; }

        protected Dictionary<string, List<string>> CellCodesByComponentCodes;

        protected Dictionary<string, Dictionary<string, string>> TechPassportValues;

        protected IPassportProvider PassportProvider;

        protected ReportParams ReportParams;

        protected long RealtyObjectId;

        protected bool DataProvided;

        protected BaseTechPassportSectionReport()
        {
            CellCodesByComponentCodes = new Dictionary<string, List<string>>();
        }
        
        public void PrepareSection(IPassportProvider iPassportProvider, ReportParams reportParams, long realtyObjectId, Dictionary<string, Dictionary<string, string>> techPassportData)
        {
            DataProvided = techPassportData != null;

            this.PassportProvider = iPassportProvider;
            this.TechPassportValues = techPassportData;
            this.ReportParams = reportParams;
            this.RealtyObjectId = realtyObjectId;

            BeforePrepareComponentIds();
            PrepareComponentIds();
            
            BeforeGetData();
            GetData();
            AfterGetData();
            
            BeforePlaceData();
            PlaceData();
            AfterPlaceData();
        }

        protected virtual void BeforePrepareComponentIds(){}

        protected abstract void PrepareComponentIds();

        protected virtual void BeforeGetData(){}

        protected virtual void GetData()
        {
            if (DataProvided) return;
            
            var componentCodes = CellCodesByComponentCodes.Keys;
            // получаем значения по заданным компонентам
            TechPassportValues = Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == RealtyObjectId && componentCodes.Contains(x.FormCode))
                    .Select(x => new
                        {
                            x.FormCode,
                            x.CellCode,
                            x.Value
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.FormCode)
                    .ToDictionary(x => x.Key,
                                x => x.ToDictionary(y => y.CellCode, y => PassportProvider.GetTextForCellValue(x.Key, y.CellCode, y.Value)));
        }

        protected virtual void AfterGetData(){}

        protected virtual void BeforePlaceData(){}

        protected virtual void PlaceData()
        {
            foreach (var component in CellCodesByComponentCodes)
            {
                PlaceComponentData(component.Key);
            }
        }

        protected void PlaceComponentData(string componentId)
        {
            if (!TechPassportValues.ContainsKey(componentId)) return;

            var component = CellCodesByComponentCodes[componentId];

            foreach (var cellCode in component)
            {
                if (TechPassportValues[componentId].ContainsKey(cellCode))
                {
                    ReportParams.SimpleReportParams[string.Format("{0}:{1}", componentId, cellCode)] =
                        TechPassportValues[componentId][cellCode];
                }
            }
        }

        protected virtual void AfterPlaceData(){}

        protected void GenerateCellCodes(string sectionId, int rowStart, int rowEnd, int colStart = 1, int colEnd = 1)
        {
            var cellCodes = new List<string>();

            for (int row = rowStart; row <= rowEnd; ++row)
            {
                for (int col = colStart; col <= colEnd; ++col)
                {
                    cellCodes.Add( row + ":" + col);
                }
            }

            CellCodesByComponentCodes[sectionId] = cellCodes;
        }
    }
}