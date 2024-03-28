namespace Bars.GkhGji.Regions.Nnovgorod.Report
{
    using System.Collections.Generic;
    using Gkh.Report;
    using Properties;

    public class DisposalGjiNotificationReport : GkhGji.Report.DisposalGjiNotificationReport
    {
        /// <summary>
        /// Информация о шаблоне
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_InstructionNotification",
                            Name = "DisposalNotification",
                            Description = "Уведомление о проверке из приказа",
                            Template = Resources.BlockGJI_InstructionNotification
                        }
                };
        }
        /// <summary>
        /// Названия генератора отчета
        /// </summary>
        public override string ReportGeneratorName
        {
            get { return "XlsIoGenerator"; }
        }
    }
}