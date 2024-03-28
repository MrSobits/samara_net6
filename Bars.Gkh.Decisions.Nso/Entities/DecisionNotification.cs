namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    using B4.Modules.States;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    public class DecisionNotification : BaseImportableEntity, IStatefulEntity
    {
        public virtual RealityObjectDecisionProtocol Protocol { get; set; }

        public virtual string Number { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual FileInfo Document { get; set; }

        public virtual FileInfo ProtocolFile { get; set; }

        public virtual string AccountNum { get; set; }

        public virtual DateTime OpenDate { get; set; }

        public virtual DateTime CloseDate { get; set; }

        public virtual FileInfo BankDoc { get; set; }

        public virtual string IncomeNum { get; set; }

        public virtual DateTime RegistrationDate { get; set; }

        public virtual bool OriginalIncome { get; set; }

        public virtual bool CopyIncome { get; set; }

        public virtual bool CopyProtocolIncome { get; set; }

        public virtual State State { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }
    }
}