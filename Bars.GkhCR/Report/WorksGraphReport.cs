namespace Bars.GkhCr.Report
{
	using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
	using Bars.GkhCr.Entities;

    using Castle.Windsor;

	/// <summary>
	/// Отчет Графики работ
	/// </summary>
	public class WorksGraphReport : BasePrintForm
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

        private long programmCrId;

        private List<string> workCodes = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13",
                                                            "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "999", "29",
                                                            "141", "142", "143", "144", "31", "145", "88", "100", "101", "102" };

		/// <summary>
		/// Конструктор
		/// </summary>
        public WorksGraphReport()
            : base(new ReportTemplateBinary(Properties.Resources.WorksGraphReport))
        {
        }

        #region свойства

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get
            {
                return "Графики работ";
            }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Desciption
        {
            get
            {
                return "Графики работ";
            }
        }

		/// <summary>
		/// Наименование группы
		/// </summary>
        public override string GroupName
        {
            get
            {
                return "Капитальный ремонт";
            }
        }

		/// <summary>
		/// Клиентский контроллер
		/// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.WorksGraph";
            }
        }

		/// <summary>
		/// Ограничение прав доступа
		/// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.WorksGraph";
            }
        }

        #endregion

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
        public override void SetUserParams(BaseParams baseParams)
        {
			this.programmCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToLong() : 0;
        }

		/// <summary>
		/// Генератор отчета
		/// </summary>
        public override string ReportGenerator { get; set; }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            if (this.programmCrId > 0)
            {
                var programmCrName = this.Container.Resolve<IDomainService<ProgramCr>>().Get(this.programmCrId).Name;
                reportParams.SimpleReportParams["programmCr"] = programmCrName;
            }
            var worksByFinSrcByObjectCrByMunicipality = this.Container.Resolve<IDomainService<TypeWorkCr>>()
                    .GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programmCrId 
                                && this.workCodes.Contains(x.Work.Code))
                    .Select(x => new
                    {
                        ObjectCr = x.ObjectCr.Id,
                        GjiNum = x.ObjectCr.GjiNum,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        x.ObjectCr.RealityObject.Address,
                        FinanceSource = x.FinanceSource != null ? x.FinanceSource.Name : string.Empty,
                        DateStart = x.DateStartWork != null
                                ? x.DateStartWork.Value.ToShortDateString()
                                : "-",
                        DateEnd = x.DateEndWork != null
                                ? x.DateEndWork.Value.ToShortDateString()
                                : "-",
                        Work = x.Work.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Municipality)
                    .ToDictionary(x => x.Key,x => x.GroupBy(y => y.ObjectCr)
                                                .ToDictionary(z => z.Key,z => z.GroupBy(q => q.FinanceSource)
                                                                                .ToDictionary(w => w.Key, w => w.ToList())));
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            foreach (var municipality in worksByFinSrcByObjectCrByMunicipality.OrderBy(x => x.Key))
            {
                foreach (var objectCr in municipality.Value)
                {
                    foreach (var finSource in objectCr.Value)
                    {
                        section.ДобавитьСтроку();
                        section["municipality"] = municipality.Key;
                        section["finsource"] = finSource.Key;
                        var address = string.Empty;
                        var gjiNum = string.Empty;
                        foreach (var work in finSource.Value)
                        {
                            if (address == string.Empty)
                            {
                                address = work.Address;
                            }

                            if (gjiNum == string.Empty)
                            {
                                gjiNum = work.GjiNum;
                            }                            

                            section["workStart" + work.Work] = work.DateStart;
                            section["workEnd" + work.Work] = work.DateEnd;
                        }

                        section["address"] = address;
                        section["GjiNum"] = gjiNum;
                    }
                }
            }
        }
    }
}