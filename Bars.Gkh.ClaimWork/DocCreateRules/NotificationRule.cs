namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Enums;

    /// <summary>
    /// Правило для создания уведомления ПИР
    /// </summary>
    public class NotificationRule : BaseClaimWorkDocRule
    {
        public override string Id => "NotificationCreateRule";

        public override string Description => "Создание уведомления ПИР";

        public override string ActionUrl => "notification";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.Notification;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
             var notifDomain = this.Container.ResolveDomain<NotificationClw>();

            try
            {
                var notification = notifDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                if (notification == null)
                {
                    notification = new NotificationClw
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.Notification,
                        DocumentDate = DateTime.Now
                    };

                    notifDomain.Save(notification);
                }

                return new BaseDataResult(notification);
            }
            finally
            {
                this.Container.Release(notifDomain);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var notifDomain = this.Container.ResolveDomain<NotificationClw>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var claimWorkWithDoc = notifDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<NotificationClw>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    result.Add(new NotificationClw
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.Notification,
                        DocumentDate = DateTime.Now
                    });
                }

                return result;
            }
            finally
            {
                this.Container.Release(notifDomain);
            }
        }
    }
}