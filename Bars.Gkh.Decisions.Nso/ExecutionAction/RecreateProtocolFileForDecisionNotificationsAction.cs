namespace Bars.Gkh.Decisions.Nso.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;

    using Castle.Windsor;

    public class RecreateProtocolFileForDecisionNotificationsAction : BaseExecutionAction
    {
        private readonly IFileService fileService;

        private readonly IDomainService<DecisionNotification> notificationService;

        public RecreateProtocolFileForDecisionNotificationsAction(
            IWindsorContainer container,
            IDomainService<DecisionNotification> notificationService,
            IFileService fileService)
        {
            this.notificationService = notificationService;
            this.fileService = fileService;
        }

        /// <summary>
        ///     Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        ///     Описание действия
        /// </summary>
        public override string Description
            => "Пересоздает файл протокола для уведомления о решении собственников в случае, если уведомление и решение ссылаются на один и тот же файл";

        /// <summary>
        ///     Название для отображения
        /// </summary>
        public override string Name => "Пересоздание файла протокола для уведомления о решении собственников";

        private BaseDataResult Execute()
        {
            this.Container.InTransaction(
                () =>
                {
                    var notifications = this.notificationService.GetAll()
                        .Where(
                            x =>
                                x.Protocol.File != null && x.ProtocolFile != null
                                    && x.Protocol.File.Id == x.ProtocolFile.Id);
                    foreach (var notification in notifications)
                    {
                        notification.ProtocolFile = this.fileService.ReCreateFile(notification.ProtocolFile);
                        this.notificationService.Update(notification);
                    }
                });

            return new BaseDataResult();
        }
    }
}