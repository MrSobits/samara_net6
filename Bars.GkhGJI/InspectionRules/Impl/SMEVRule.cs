namespace Bars.GkhGji.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа Распоряжения из основания проверки соискателя лицензии
    /// </summary>
    public class SMEVRule : ISMEVRule
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public string CodeRegion
        {
            get { return "All"; }
        }

        public string Id
        {
            get { return "BaseSMEVInspectionRule"; }
        }

        public string Description
        {
            get { return "Правило отправки в СМЭВ запросов"; }
        }

      

        public virtual void SendRequests(InspectionGji inspection)
        {
  
        }

        public virtual void GetResponce()
        {

        }
    }
}
