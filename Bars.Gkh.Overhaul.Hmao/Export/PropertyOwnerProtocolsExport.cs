namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    public class PropertyOwnerProtocolsExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var PropertyOwnerProtocolsDomain = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
            var PropertyOwnerProtocolInspectorDomain = Container.Resolve<IDomainService<PropertyOwnerProtocolInspector>>();
            var PropertyOwnerProtocolsDecisionDomain = Container.Resolve<IDomainService<PropertyOwnerProtocolsDecision>>();

            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var dateRegStart2 = baseParams.Params.GetAs("dateRegStart", new DateTime());
            var dateRegEnd2 = baseParams.Params.GetAs("dateRegEnd", new DateTime());
            var sfEmployer = baseParams.Params.GetAs<long>("sfEmployer");
            List<long> employerProtList = new List<long>();
            if (sfEmployer > 0)
            {
                employerProtList = PropertyOwnerProtocolInspectorDomain.GetAll()
                    .Where(x => x.Inspector.Id == sfEmployer)
                    .Select(x => x.PropertyOwnerProtocols.Id).Distinct().ToList();
            }
            var protList = PropertyOwnerProtocolsDomain.GetAll()
                 .Where(x => x.DocumentDate >= dateStart2 && x.DocumentDate <= dateEnd2)
                 .Where(x => x.RegistrationDate >= dateRegStart2 && x.RegistrationDate <= dateRegEnd2)
                 .WhereIf(sfEmployer > 0 || employerProtList.Count > 0, x => employerProtList.Contains(x.Id))
                 .Select(x => x.Id).Distinct().ToList();

            var decisionDict = PropertyOwnerProtocolsDecisionDomain.GetAll()
                .Where(x => protList.Contains(x.Protocol.Id))
                .Select(x => new
                {
                    x.Protocol.Id,
                    x.Decision.Name
                }).AsEnumerable().GroupBy(x => x.Id)
                 .ToDictionary(x => x.Key, y => y.AggregateWithSeparator(x => x.Name, "; ")); ;


            var data = PropertyOwnerProtocolsDomain.GetAll()
                 .Where(x => x.DocumentDate >= dateStart2 && x.DocumentDate <= dateEnd2)
                 .Where(x => x.RegistrationDate >= dateRegStart2 && x.RegistrationDate <= dateRegEnd2)
                 .WhereIf(sfEmployer > 0 || employerProtList.Count > 0, x => employerProtList.Contains(x.Id))
                 .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    ProtocolTypeId = decisionDict.ContainsKey(x.Id) ? decisionDict[x.Id] : "",//x.ProtocolTypeId.Name,
                        RealityObject = x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name,
                    ProtocolMKDState = x.ProtocolMKDState != null ? x.ProtocolMKDState.Name : "",
                    ProtocolMKDSource = x.ProtocolMKDSource != null ? x.ProtocolMKDSource.Name : "",
                    ProtocolMKDIniciator = x.ProtocolMKDIniciator != null ? x.ProtocolMKDIniciator.Name : "",
                    x.RegistrationDate,
                    x.Description,
                    x.VoteForm,
                    x.DocumentFile,
                    x.RegistrationNumber,
                    Inspector = x.Inspector != null ? x.Inspector.Fio : "",
                    x.DocumentNumber,
                    x.DocumentDate
                })
                .AsQueryable()
                .Filter(loadParams, Container)
                .ToList();
            return data;
        }
    }
}