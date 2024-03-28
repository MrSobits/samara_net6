namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Домен-сервис для <see cref="PaymentDocumentTemplate"/>
    /// </summary>
    public class PaymentDocumentTemplateDomainService : BaseDomainService<PaymentDocumentTemplate>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var reports = new List<PaymentDocumentTemplate>();
            this.InTransaction(() =>
            {
                var result = base.Save(baseParams);
                reports = result.Data as List<PaymentDocumentTemplate>;
                if (reports != null && reports.Count == 1)
                {
                    this.SaveTemplate(baseParams, reports.First());
                }
            });
            return new BaseDataResult(reports.Select(x => new { x.Id, x.Period, x.TemplateCode }));
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var reports = new List<PaymentDocumentTemplate>();
            this.InTransaction(() =>
            {
                var result = base.Update(baseParams);
                reports = result.Data as List<PaymentDocumentTemplate>;
                if (reports != null && reports.Count == 1)
                {
                    this.SaveTemplate(baseParams, reports.First());
                }
            });
            return new BaseDataResult(reports.Select(x => new { x.Id, x.Period, x.TemplateCode }));
        }

        private void SaveTemplate(BaseParams baseParams, PaymentDocumentTemplate report)
        {
            if (baseParams.Files.Any())
            {
                var fileData = baseParams.Files["TemplateFile"];
                report.Template = fileData.Data;
            }
            else
            {
                report.Template = Convert.FromBase64String(baseParams.Params.GetAs<string>("Template"));
            }

            this.UpdateInternal(report);
        }
    }
}