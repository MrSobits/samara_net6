using Bars.B4.DomainService.BaseParams;

namespace Bars.GkhCr.DomainService
{
    using B4;
    using B4.DataAccess;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;
    using Bars.B4.Utils;

    public class VoiceMemberService : IVoiceMemberService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Qualification> ServiceQualification { get; set; }

        public IDomainService<QualificationMember> ServiceMember { get; set; }

        public IDomainService<VoiceMember> Service { get; set; }


        public IDataResult SaveVoiceMembers(BaseParams baseParams)
        {
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var records =
                        baseParams.Params.Read<SaveParam<VoiceMember>>()
                                  .Execute((container => Converter.ToSaveParam<VoiceMember>(container, true)));

                    var removedIds = Converter.ToLongArray(baseParams.Params, "removed");

                    removedIds.ForEach(x => Service.Delete(x));

                    foreach (var saveRecord in records)
                    {
                        var rec = saveRecord.AsObject();

                        if (rec.Id > 0)
                        {
                            Service.Update(rec);
                        }
                        else
                        {
                            Service.Save(rec);
                        }
                    }

                    tr.Commit();
                    return new BaseDataResult {Success = true};
                }
                catch (ValidationException exc)
                {
                    tr.Rollback();
                    return new BaseDataResult {Success = false, Message = exc.Message};
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}