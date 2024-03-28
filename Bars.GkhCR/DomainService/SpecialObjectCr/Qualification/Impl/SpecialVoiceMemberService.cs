namespace Bars.GkhCr.DomainService
{
    using Bars.B4.DomainService.BaseParams;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    public class SpecialVoiceMemberService : ISpecialVoiceMemberService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SpecialQualification> ServiceQualification { get; set; }

        public IDomainService<QualificationMember> ServiceMember { get; set; }

        public IDomainService<SpecialVoiceMember> Service { get; set; }

        public IDataResult SaveVoiceMembers(BaseParams baseParams)
        {
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var records = baseParams.Params.Read<SaveParam<SpecialVoiceMember>>()
                        .Execute((container => Converter.ToSaveParam<SpecialVoiceMember>(container, true)));

                    var removedIds = Converter.ToLongArray(baseParams.Params, "removed");

                    removedIds.ForEach(x => this.Service.Delete(x));

                    foreach (var saveRecord in records)
                    {
                        var rec = saveRecord.AsObject();

                        if (rec.Id > 0)
                        {
                            this.Service.Update(rec);
                        }
                        else
                        {
                            this.Service.Save(rec);
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