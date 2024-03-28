using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.Gkh.Decisions.Nso.Entities;
using Bars.Gkh.Domain;
using Castle.Windsor;

namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    public class DecisionStraightForwardService : IDecisionStraightForwardService
    {
        private readonly IFileService _fileService;
        private readonly IWindsorContainer Container;

        public DecisionStraightForwardService(IFileService fileService, IWindsorContainer container)
        {
            this._fileService = fileService;
            Container = container;
        }

        public IDataResult GetConfirm(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");
            var notificationDomain = Container.ResolveDomain<DecisionNotification>();
            var stateProvider = Container.Resolve<IStateProvider>();

            var protocolDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var protocol = protocolDomain.Get(protocolId);

            var notification = notificationDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocolId);

            if (notification == null)
            {
                notification = new DecisionNotification
                {
                    Protocol = protocol,
                    ProtocolFile = _fileService.ReCreateFile(protocol.File)
                };

                notificationDomain.Save(notification);

                stateProvider.SetDefaultState(notification);
            }

            if ((notification.ProtocolFile != null && protocol.File != null
                && notification.ProtocolFile.CheckSum != protocol.File.CheckSum
                && notification.ProtocolFile.FullName != protocol.File.FullName)
               ||
               (notification.ProtocolFile == null && protocol.File != null))
            {
                notification.ProtocolFile = _fileService.ReCreateFile(protocol.File);

                if (notification.Id == 0)
                {
                    notificationDomain.Save(notification);
                }
                else
                {
                    notificationDomain.Update(notification);
                }

            }
            else if (notification.ProtocolFile != null
                && protocol.File == null)
            {
                notification.ProtocolFile = null;

                if (notification.Id == 0)
                {
                    notificationDomain.Save(notification);
                }
                else
                {
                    notificationDomain.Update(notification);
                }
            }

            Container.Release(notificationDomain);
            Container.Release(stateProvider);
            Container.Release(protocolDomain);

            return new BaseDataResult(notification.Id);
        }
    }
}
