namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class QualificationDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            /*var loadParam = GetLoadParam(baseParams);

            var currPeriod = this.Container.Resolve<IDomainService<Period>>()
                                      .GetAll().FirstOrDefault(x => x.DateStart < DateTime.Now && (!x.DateEnd.HasValue || x.DateEnd > DateTime.Now));
            var typeWorkSumDict = Container.Resolve<IDomainService<TypeWorkCr>>()
                         .GetAll()
                         .Where(x => x.FinanceSource.TypeFinance == TypeFinance.FederalLaw)
                         .GroupBy(x => x.ObjectCr.Id)
                         .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

            var qualMembers = this.Container.Resolve<IDomainService<QualificationMember>>().GetAll().Where(x => x.IsPrimary && x.Period == currPeriod).Select(x => x.Id).ToList();
            var qualMemberCount = this.Container.Resolve<IDomainService<QualificationMember>>().GetAll().Where(x => x.Period == currPeriod).Select(x => x.Id).Count();


            var voiceMembersDict = Container.Resolve<IDomainService<VoiceMember>>().GetAll()
                             .Where(x =>
                                 x.Qualification.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                                 && x.Qualification.ObjectCr.ProgramCr.Period.Id == currPeriod.Id
                                 && x.TypeAcceptQualification == TypeAcceptQualification.Yes)
                                 .WhereIf(qualMembers.Count > 0, x => qualMembers.Contains(x.QualificationMember.Id))
                             .GroupBy(x => x.Qualification.Id).ToDictionary(x => x.Key, y => y.Count());

            var qualifications = Container.Resolve<IDomainService<Qualification>>()
                         .GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active && x.ObjectCr.ProgramCr.Period.Id == currPeriod.Id)
                          .GroupBy(x => x.ObjectCr.Id)
                          .ToDictionary(x => x.Key, y => y.Where(z => voiceMembersDict.ContainsKey(z.Id)).Select(x => new
                          {
                              yesCount = voiceMembersDict[x.Id],
                              ContragentName = x.Builder.Contragent.Name,
                              Rating = string.Format("{0} из {1}", voiceMembersDict[x.Id], qualMemberCount)
                          }).OrderByDescending(x => x.yesCount).FirstOrDefault());

            return Container.Resolve<IDomainService<ObjectCr>>()
                      .GetAll()
                      .Where(x => x.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active && x.ProgramCr.Period.Id == currPeriod.Id)
                      .Select(x => new
                      {
                          x.Id,
                          ProgrammName = x.ProgramCr.Name,
                          MunicipalityName = x.RealityObject.Municipality.Name,
                          x.RealityObject.Address
                      })
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.MunicipalityName)
                        .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                        .ToList()
                        .AsQueryable()
                        .Select(x => new
                        {
                            x.Id,
                            x.ProgrammName,
                            x.MunicipalityName,
                            x.Address,
                            BuilderName = qualifications.ContainsKey(x.Id) && qualifications[x.Id] != null ? qualifications[x.Id].ContragentName : null,
                            Sum = typeWorkSumDict.ContainsKey(x.Id) ? typeWorkSumDict[x.Id] : 0,
                            Rating = qualifications.ContainsKey(x.Id) && qualifications[x.Id] != null ? qualifications[x.Id].Rating : string.Format("0 из {0}", qualMemberCount)
                        }).Filter(loadParam, Container).ToList();*/

            var loadParams = baseParams.GetLoadParam();

            var data = Container.Resolve<IDomainService<ViewQualification>>().GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.ProgrammName,
                        x.MunicipalityName,
                        x.Address,
                        x.BuilderName,
                        x.Sum,
                        Rating = x.Rating ?? "0 из " + x.QualMemberCount
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .ToList();

            return data;
        }
    }
}