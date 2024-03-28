namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class RealityObjectCouncillorsService : IRealityObjectCouncillorsService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult IsShowConcillors(BaseParams baseParams)
        {
            try
            {
                var realityObjId = baseParams.Params["realtyObjectId"].ToInt();

                var apartaments = true;
                var expression = true;

                // все договора с собственниками
                var manOrgContractOwnersIdsList = this.Container.Resolve<IDomainService<ManOrgContractOwners>>().GetAll().Select(x => x.Id).ToList();

                // ТСЖ/ЖСК
                var manOrgContractRelationIdsList = this.Container.Resolve<IDomainService<ManOrgContractRelation>>()
                    .GetAll()
                    .Where(x => x.Parent.ManagingOrganization.Contragent.ContragentState == ContragentState.Liquidated && x.TypeRelation == TypeContractRelation.TransferTsjUk)
                    .Select(x => x.Parent.Id)
                    .ToList();

                var manOrgBaseContractsIdsList = Container.Resolve<IDomainService<ManOrgBaseContract>>()
                             .GetAll()
                             .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                             .Select(x => x.Id)
                             .ToList();


                // Все договора с ТСЖ/ЖСК
                var manOrgContractJskTsjIdsList = this.Container.Resolve<IDomainService<ManOrgContractTransfer>>()
                    .GetAll()
                    .Where(x => x.ManOrgJskTsj.Contragent.ContragentState == ContragentState.Liquidated && x.ManOrgJskTsj.IsTransferredManagementTsj)
                    .Select(x => x.Id)
                    .ToList();

                // все связи домов с договорами
                IEnumerable<ManOrgContractRealityObject> manOrgContractRealityObjectList;
                var currentDate = DateTime.Now.Date;

                // Если есть договоры с собственниками
                if (manOrgContractOwnersIdsList.Any())
                {
                    manOrgContractRealityObjectList = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                                                          .GetAll()
                                                          .Where(x => x.RealityObject.Id == realityObjId
                                                              && x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK
                                                              && (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                                                              && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate))
                                                          .ToList()
                                                          .Where(x => manOrgContractOwnersIdsList.Contains(x.ManOrgContract.Id));
                    
                    apartaments = manOrgContractRealityObjectList.Any(x => x.RealityObject.NumberApartments > 4);
                    expression = manOrgContractRealityObjectList.Any();


                    if (expression && apartaments)
                    {
                        return new BaseDataResult(new { visible = true })
                                   {
                                       Success = true
                                   };
                    }
                }

                if (manOrgContractJskTsjIdsList.Any())
                {
                    manOrgContractRealityObjectList = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                                                          .GetAll()
                                                          .Where(x => x.RealityObject.Id == realityObjId
                                                              && x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK
                                                              && (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                                                              && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate))
                                                          .ToList()
                                                          .Where(x => manOrgContractJskTsjIdsList.Contains(x.ManOrgContract.Id));

                    apartaments = manOrgContractRealityObjectList.Any(x => x.RealityObject.NumberApartments > 4);
                    expression = manOrgContractRealityObjectList.Any();

                    if (expression && apartaments)
                    {
                        return new BaseDataResult(new { visible = true })
                        {
                            Success = true
                        };
                    }
                }

                if (manOrgBaseContractsIdsList.Any())
                {
                    manOrgContractRealityObjectList = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.RealityObject.Id == realityObjId
                        && (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate))
                     .ToList()
                     .Where(x => manOrgBaseContractsIdsList.Contains(x.ManOrgContract.Id));

                    apartaments = manOrgContractRealityObjectList.Any(x => x.RealityObject.NumberApartments > 4);
                    expression = manOrgContractRealityObjectList.Any();

                    if (expression && apartaments)
                    {
                        return new BaseDataResult(new { visible = true })
                        {
                            Success = true
                        };
                    }
                }

                if (manOrgContractRelationIdsList.Any())
                {
                    manOrgContractRealityObjectList = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                                                          .GetAll()
                                                          .Where(x => x.RealityObject.Id == realityObjId
                                                              && (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                                                              && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate))
                                                          .ToList()
                                                          .Where(x => manOrgContractRelationIdsList.Contains(x.ManOrgContract.Id));

                    apartaments = manOrgContractRealityObjectList.Any(x => x.RealityObject.NumberApartments > 4);
                    expression = manOrgContractRealityObjectList.Any();

                    if (expression && apartaments)
                    {
                        return new BaseDataResult(new { visible = true })
                        {
                            Success = true
                        };
                    }
                }

                return new BaseDataResult(new { visible = false, apartamentsOnly = expression && !apartaments }) { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
