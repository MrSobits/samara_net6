namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;

    public class MKDLicRequestAnswerInterceptor : EmptyDomainInterceptor<MKDLicRequestAnswer>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<MKDLicRequestAnswer> service, MKDLicRequestAnswer entity)
        {
            var answerAttachDomain = Container.ResolveDomain<MKDLicRequestAnswerAttachment>();

            try
            {
                var attachIds = answerAttachDomain.GetAll().Where(x => x.MKDLicRequestAnswer.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in attachIds)
                {
                    answerAttachDomain.Delete(value);
                }

                return Success();
            }
            finally
            {
                Container.Release(answerAttachDomain);
            }
        }
    }
}
