namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Представление для <see cref="VoiceMember"/>
    /// </summary>
    public class VoiceMemberViewModel : BaseViewModel<VoiceMember>
    {
        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<VoiceMember> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var qualificationId = baseParams.Params.GetAs<long?>("qualificationId") ?? loadParams.Filter.GetAs<long>("qualificationId");
            var objectCrId = baseParams.Params.GetAs<long?>("objectCrId") ?? loadParams.Filter.GetAs<long>("objectCrId");

            var periodId = this.Container.Resolve<IDomainService<Entities.ObjectCr>>().GetAll()
                         .Where(x => x.Id == objectCrId)
                         .Select(x => x.ProgramCr.Period.Id)
                         .FirstOrDefault();

            var qualMembersDict = this.Container.Resolve<IDomainService<QualificationMember>>().GetAll()
                    .Where(x => x.Period.Id == periodId)
                    .ToDictionary(x => x.Id, y => y.Name);

            var data = domainService.GetAll()
                .Where(x => x.Qualification.Id == qualificationId)
                .Where(x => x.QualificationMember.Period.Id == periodId)
                .Select(x => new
                    {
                        x.Id,
                        x.TypeAcceptQualification,
                        QualificationMember = x.QualificationMember.Id,
                        MemberName = x.QualificationMember.Name,
                        x.DocumentDate,
                        x.Reason
                    })
                .AsEnumerable()
                .ToDictionary(x => x.QualificationMember);

            var dataList = new List<object>();

            // Догоняем полученные дома уже имеющимися в базе данными
            foreach (var qualMember in qualMembersDict)
            {
                if (data.ContainsKey(qualMember.Key))
                {
                    var obj = data[qualMember.Key];

                    dataList.Add(new
                        {
                            obj.Id,
                            obj.TypeAcceptQualification,
                            obj.MemberName,
                            obj.DocumentDate,
                            obj.Reason,
                            Qualification = qualificationId,
                            obj.QualificationMember
                        });
                }
                else
                {
                    dataList.Add(new
                        {
                            TypeAcceptQualification = TypeAcceptQualification.NotDefined,
                            MemberName = qualMember.Value,
                            DocumentDate = (DateTime?)null,
                            Reason = (string)null,
                            Qualification = qualificationId,
                            QualificationMember = qualMember.Key
                        });
                }
            }

            return new ListDataResult(dataList.ToList(), dataList.Count);
        }
    }
}

