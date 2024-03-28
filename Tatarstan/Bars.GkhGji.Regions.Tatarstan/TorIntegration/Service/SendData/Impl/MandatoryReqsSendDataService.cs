namespace Bars.GkhGji.Regions.Tatarstan.TorIntegration.Service.SendData.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    // TODO : Расскоментировать после реализации 
    /*using Bars.GisIntegration.Tor.Enums;
    using Bars.GisIntegration.Tor.GraphQl;
    using Bars.GisIntegration.Tor.Service.SendData.Impl;*/
    using Bars.Gkh.Entities.Base;

    using MandatoryReqs = Entities.Dict.MandatoryReqs;

        /*public class MandatoryReqsSendDataService : BaseSendDataService<MandatoryReqs>
    {
        public MandatoryReqsSendDataService()
        {
            this.TypeObject = TypeObject.MandatoryReq;
        }

        public override IDataResult PrepareData(BaseParams baseParams)
        {
            var mandatoryReqsIds = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];

            if (mandatoryReqsIds.Length > 0)
            {
                var mandatoryReqsDomain = this.Container.ResolveDomain<MandatoryReqs>();
                var npaDomain = this.Container.ResolveDomain<Entities.Dict.MandatoryReqsNormativeDoc>();

                using (this.Container.Using(mandatoryReqsDomain, npaDomain))
                {
	                var npa = npaDomain.GetAll()
		                .Where(w => w.Npa.TorId != null)
		                .Select(x => new { x.Npa.TorId, x.MandatoryReqs.Id })
		                .AsEnumerable()
		                .GroupBy(x => x.Id)
		                .ToDictionary(x => x.Key, x => x.Select(s => (Guid)s.TorId).ToArray());

					foreach (var id in mandatoryReqsIds)
                    {
						var mandatory = mandatoryReqsDomain.Get(id);
	                    this.SendObject = mandatory;
						this.TypeRequest = this.SendObject.TorId == null ? TypeRequest.Initialization : TypeRequest.Correction;

                        if (this.TypeRequest == TypeRequest.Initialization)
                        {
                            var createMandatoryReqs = new MutationQueryBuilder()
                                .WithCreateMandatoryReqs(
                                    new MandatoryReqsQueryBuilder().WithId(),
                                    new MandatoryReqsCreateInput()
                                    {
                                        MandratoryReqContent = this.SendObject.MandratoryReqContent,
                                        StartDateMandatory = this.SendObject.StartDateMandatory,
                                        EndDateMandatory = this.SendObject.EndDateMandatory,
                                        Npa = npa.ContainsKey(this.SendObject.Id) ? npa[this.SendObject.Id] : new Guid[0],
                                    }).Build(Formatting.Indented, 2);

                            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("CreateMandatoryReqs", createMandatoryReqs, mandatory));
                        }
                        else
                        {
                            var updateMandatoryReqs = new MutationQueryBuilder()
                                .WithUpdateMandatoryReqs(
                                    new MandatoryReqsQueryBuilder().WithId(),
                                    new MandatoryReqsUpdateInput()
                                    {
                                        Id = this.SendObject.TorId,
                                        MandratoryReqContent = this.SendObject.MandratoryReqContent,
                                        StartDateMandatory = this.SendObject.StartDateMandatory,
                                        EndDateMandatory = this.SendObject.EndDateMandatory,
                                        Npa = npa.ContainsKey(this.SendObject.Id) ? npa[this.SendObject.Id] : new Guid[0],
                                    }).Build(Formatting.Indented, 2);

                            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("UpdateMandatoryReqs", updateMandatoryReqs, mandatory));
                        }
                    }

                    return this.SendRequest();
                }
            }

            return new BaseDataResult { Success = false, Message = "Нет данных для отправки" };
        }
    }*/
}
