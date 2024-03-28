//namespace Bars.GkhCr.Report
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using B4;
//    using B4.Modules.Reports;
//    using B4.Utils;
//    using Entities;
//    using Gkh.Entities;
//    using Gkh.Modules.RegOperator.Entities.RegOperator;
//    using Gkh.Report;
//    using Gkh.Utils;
//    using GkhCr.Properties;
//    using Stimulsoft.Report;

//    public class OverhaulProposalReport : GkhBaseStimulReport
//    {
//        #region .ctor

//        /// <summary>
//        /// .ctor
//        /// </summary>
//        public OverhaulProposalReport()
//            : base(new ReportTemplateBinary(Properties.Resources.OverhaulProposalReport))
//        {
//        }

//        #endregion .ctor

//        #region Private fields

//        private long _overhaulProposalId;

//        #endregion Private fields

//        #region Protected properties

//        /// <summary>
//        /// Код шаблона (файла)
//        /// </summary>
//        protected override string CodeTemplate { get; set; }

//        #endregion Protected properties

//        #region Public properties

//        /// <summary>
//        /// Наименование отчета
//        /// </summary>
//        public override string Name
//        {
//            get { return "Предложение по капремонту"; }
//        }

//        /// <summary>
//        /// Описание отчета
//        /// </summary>
//        public override string Description
//        {
//            get { return "Предложение по капремонту"; }
//        }

//        /// <summary>
//        /// Идентификатор отчета
//        /// </summary>
//        public override string Id
//        {
//            get { return "OverhaulProposalReport"; }
//        }

//        /// <summary>
//        /// Код формы, на которой находится кнопка печати
//        /// </summary>
//        public override string CodeForm
//        {
//            get { return "OverhaulProposal"; }
//        }

//        /// <summary>Формат печатной формы</summary>
//        public override StiExportFormat ExportFormat
//        {
//            get { return StiExportFormat.Word2007; }
//            set { }
//        }

//        #endregion Public properties

//        #region Public methods

//        /// <summary>
//        /// Подготовить параметры отчета
//        /// </summary>
//        /// <param name="reportParams"></param>
//        public override void PrepareReport(ReportParams reportParams)
//        {           

//            try
//            {
//                Report["overhaulProposal"] = _overhaulProposalId;              
//            }
//            finally
//            {

//            }

          
//        }

//        /// <summary>
//        /// Установить пользовательские параметры
//        /// </summary>
//        /// <param name="userParamsValues"></param>
//        public override void SetUserParams(UserParamsValues userParamsValues)
//        {
//            _overhaulProposalId = userParamsValues.GetValue<long>("Id");
//        }

//        /// <summary>
//        /// Получить список шаблонов
//        /// </summary>
//        /// <returns></returns>
//        public override List<TemplateInfo> GetTemplateInfo()
//        {
//            return new List<TemplateInfo>
//            {
//                new TemplateInfo
//                {
//                    Name = "Предложение по капремонту",
//                    Description = "Предложение по капремонту",
//                    Code = "OverhaulProposalReport",
//                    Template = Resources.OverhaulProposalReport
//                }
//            };
//        }

//        #endregion Public methods
//    }
//}
namespace Bars.GkhCr.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities;
    using Gkh.Entities;
    using Gkh.Modules.RegOperator.Entities.RegOperator;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhCr.Properties;

    /// <summary>
    /// Печатка договора подряда
    /// </summary>
    public class OverhaulProposalReport : GkhBaseStimulReport
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OverhaulProposalReport() : base(new ReportTemplateBinary(Resources.OverhaulProposalReport))
        {
        }
        
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "OverhaulProposal"; }
        }

        /// <summary>
        /// Код отчета
        /// </summary>
        public override string CodeForm
        {
            get { return "OverhaulProposal"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Предложение по капремонту"; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return "Предложение по капремонту"; }
        }

        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор договора подряда
        /// </summary>
        protected long OverhaulProposalId;

        /// <summary>
        /// Установить пользовательские параметры отчета
        /// </summary>
        /// <param name="userParamsValues">Параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            OverhaulProposalId = userParamsValues.GetValue<long>("OverhaulProposalId");
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Получить информацию о шаблонах
        /// </summary>
        /// <returns>Информация о шаблонах</returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "OverhaulProposal",
                    Description = "Предложение о проведении капремонта",
                    Name = "OverhaulProposal",
                    Template = Resources.OverhaulProposalReport
                }
            };
        }

        /// <summary>
        /// Подготовить отчет
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var opDomain = Container.Resolve<IDomainService<OverhaulProposal>>();

            try
            {
                this.ReportParams["overhaulProposal"] = OverhaulProposalId;
                if (OverhaulProposalId > 0)
                {
                    RealityObject gro = opDomain.Get(OverhaulProposalId).ObjectCr.RealityObject;
                    this.ReportParams["address"] = gro.Municipality.Name + ", " + gro.Address;
                }
            }
            finally
            {
                Container.Release(opDomain);
            }


        }
    }
}
