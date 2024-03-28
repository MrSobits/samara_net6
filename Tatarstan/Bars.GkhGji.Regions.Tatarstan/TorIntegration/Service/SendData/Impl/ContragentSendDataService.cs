namespace Bars.GkhGji.Regions.Tatarstan.TorIntegration.Service.SendData.Impl
{
    using System;

    using Bars.B4;
    // TODO : Расскоментировать после реализации GisIntegration
    /*using Bars.GisIntegration.Tor.Enums;
    using Bars.GisIntegration.Tor.GraphQl;
    using Bars.GisIntegration.Tor.Service.SendData.Impl;*/
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

	/*public class ContragentSendDataService : BaseSendDataService<Contragent>
    {
        public ContragentSendDataService()
        {
            this.TypeObject = TypeObject.Subject;
        }

        /// <inheritdoc />
        public override IDataResult GetData(BaseParams baseParams)
        {
            var getSubjectsQuery = new QueryQueryBuilder()
                .WithFindAllSubject(
                    new SubjectQueryBuilder()
                        .WithAllScalarFields()
                        .WithControlObjects(new ControlObjectQueryBuilder().WithAllScalarFields())
                        .WithCodeOktmo(new dicOKTMOQueryBuilder().WithAllScalarFields())
                        .WithCheckOutEgrsmsPs(new CheckOutEGRSMSPQueryBuilder().WithAllScalarFields())
                        .WithCheckOutEgriPs(new CheckOutEGRIPQueryBuilder().WithAllScalarFields())
                        .WithSubjectTypes(new dicSubjectTypeQueryBuilder().WithAllScalarFields())
                        .WithCodeEgruLs(new CheckOutEGRULQueryBuilder().WithAllScalarFields())
                        .WithCodeOksm(new dicOKSMQueryBuilder().WithAllScalarFields())
                        .WithMaterials(new MaterialsQueryBuilder().WithAllScalarFields())
                        .WithLicences(new LicenceQueryBuilder().WithAllScalarFields())
                        .WithControlType(new dicControlTypesQueryBuilder().WithAllScalarFields())
                        .WithCodeOkveDs(new ActivityQueryBuilder().WithAllScalarFields())
                        .WithRiskCategory(new RiskCatQueryBuilder().WithAllScalarFields())
                        .WithAssignedOfficer(new PersonQueryBuilder().WithAllScalarFields())
                        .WithControlItemPassports(new ControlItemPassportQueryBuilder().WithAllScalarFields())
                        .WithFilials(new SubjectQueryBuilder().WithAllScalarFields())
                        .WithHeadOrganization(new SubjectQueryBuilder().WithAllScalarFields()),
                    limit:10000)
                .Build(Formatting.Indented);
            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("findAllSubjects", getSubjectsQuery, null));
            this.TypeRequest = TypeRequest.Getting;
            return this.SendRequest();
        }
    }*/
}