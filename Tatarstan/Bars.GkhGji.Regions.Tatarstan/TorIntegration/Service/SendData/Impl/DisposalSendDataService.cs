namespace Bars.GkhGji.Regions.Tatarstan.TorIntegration.Service.SendData.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    // TODO : Расскоментировать после реализации 
   /* using Bars.GisIntegration.Tor.Entities;
    using Bars.GisIntegration.Tor.Enums;
    using Bars.GisIntegration.Tor.GraphQl;
    using Bars.GisIntegration.Tor.Service.SendData.Impl;*/
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /*public class DisposalSendDataService : BaseSendDataService<TatarstanDisposal>
    {
        private Guid? ControlOrgGuid { get; set; }

        public DisposalSendDataService()
        {
            this.TypeObject = TypeObject.Disposal;
        }

        public override IDataResult PrepareData(BaseParams baseParams)
        {
            var taskDomain = this.Container.ResolveDomain<TorTask>();
            var disposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();
            var inspectionRealityObject = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var controlOrgRelationDomain = this.Container.ResolveDomain<ControlOrganizationControlTypeRelation>();
            var actCheckDomain = this.Container.ResolveDomain<ActCheck>();
            var documentRelationDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var controlListDomain = this.Container.ResolveDomain<ControlList>();
            var сonfigRefInformationKndTorDomain = this.Container.ResolveDomain<ConfigurationReferenceInformationKndTor>();

            try
            {
                var documentId = baseParams.Params.GetAsId();

                this.SendObject = disposalDomain.Get(documentId);
                this.TypeRequest = this.SendObject.TorId == null ? TypeRequest.Initialization : TypeRequest.Correction;

                this.ControlOrgGuid = this.SendObject.ControlType is null
                    ? null
                    : controlOrgRelationDomain.GetAll()
                        .Where(x => x.ControlType.Id == this.SendObject.ControlType.Id)
                        .Select(x => x.ControlOrganization.TorId)
                        .FirstOrDefault();

                var actCheckId = documentRelationDomain.GetAll()
                    .Where(x => x.Parent.Id == this.SendObject.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => x.Children.Id)
                    .FirstOrDefault();

                var act = actCheckDomain.Get(actCheckId);

                var realityObj = inspectionRealityObject.GetAll()
                    .Where(x => x.Inspection.Id == this.SendObject.Inspection.Id)
                    .AsEnumerable()
                    .Select(x => x.RealityObject)
                    .ToList();

                var controlList = controlListDomain.GetAll()
                    .FirstOrDefault(x => x.Disposal.Id == documentId);

                //сохранение guidа паспорта 
                var disposalTorId = this.SendObject.TorId;

                this.GetSubjectsRequest(this.SendObject.Inspection.Contragent, сonfigRefInformationKndTorDomain);
                this.GetControlCardsRequest(disposalDomain, сonfigRefInformationKndTorDomain, act, this.SendObject);

                var result = this.SendRequest(false);

                this.SendObject.ControlCardId = result.Success && this.ListSendRequest.Contains(this.ListRequests.LastOrDefault()?.Item2)
                    ? this.SendObject.TorId
                    : null;
                this.SendObject.TorId = disposalTorId;
                disposalDomain.Update(this.SendObject);

                this.GetObjectsRequest(realityObj);
                this.GetControlItemResultsRequest(this.SendObject);
                result = this.SendRequest(false);

                this.SendObject.ResultTorId = result.Success && this.ListSendRequest.Contains(this.ListRequests.LastOrDefault()?.Item2)
                    ? this.SendObject.TorId
                    : null;
                this.SendObject.TorId = disposalTorId;
                disposalDomain.Update(this.SendObject);

                if (!(controlList is null))
                {
                    this.GetControlListsRequest(controlList);
                }

                this.GetControlActsRequest(act, this.SendObject);

                result = this.SendRequest(false);

                this.GetControlItemPassportsRequest(this.SendObject, realityObj, сonfigRefInformationKndTorDomain);

                result = this.SendRequest();
                return result;
            }

            catch (Exception)
            {
                this.Task.TorTaskState = TorTaskState.NotComplete;
                taskDomain.Update(this.Task);
                return new BaseDataResult { Success = false };
            }
            finally
            {
                this.Container.Release(taskDomain);
                this.Container.Release(disposalDomain);
                this.Container.Release(inspectionRealityObject);
                this.Container.Release(controlOrgRelationDomain);
                this.Container.Release(actCheckDomain);
                this.Container.Release(documentRelationDomain);
                this.Container.Release(controlListDomain);
                this.Container.Release(сonfigRefInformationKndTorDomain);
            }
        }

        /// <summary>
        /// Запрос на отправку субъекта Subject
        /// </summary>
        private void GetSubjectsRequest(Contragent sendSubject, IDomainService<ConfigurationReferenceInformationKndTor> сonfigRefInformationKndTorDomain)
        {
            string requestName = null;
            string request = null;

            var subjectTypes = сonfigRefInformationKndTorDomain.GetAll()
                .Where(w => w.Type == Enums.DictTypes.SubjectType)
                .Select(s => new { s.Value, s.TorId })?
                .ToDictionary(d => d.Value, d => d.TorId);

            if (sendSubject.TorId != null)
            {
                var getSubject = new QueryQueryBuilder()
                    .WithGetSubject(new SubjectQueryBuilder().WithId(), (Guid) sendSubject.TorId)
                    .Build(Formatting.Indented, 2);

                if (this.TorIntegrationService.SendGetRequest("GetSubject", getSubject, CancellationToken.None) != null)
                {
                    requestName = "UpdateSubject";
                    request = new MutationQueryBuilder()
                        .WithUpdateSubject(
                            new SubjectQueryBuilder().WithId(),
                            new SubjectUpdateInput()
                            {
                                Id = sendSubject.TorId,
                                MainName = sendSubject.Name.Replace("\"", ""),
                                Address = sendSubject.JuridicalAddress,
                                NameOrg = sendSubject.ShortName.Replace("\"", ""),
                                SubjectTypesId = sendSubject.OrganizationForm.Code == "91"
                                ? subjectTypes.ContainsKey("ИП") ? subjectTypes["ИП"] : null
                                : subjectTypes.ContainsKey("ЮЛ") ? subjectTypes["ЮЛ"] : null
                            }).Build(Formatting.Indented, 2);
                }
            }

            if (sendSubject.TorId == null || request == null)
            {
                requestName = "CreateSubject";
                request = new MutationQueryBuilder()
                    .WithCreateSubject(
                        new SubjectQueryBuilder().WithId(),
                        new SubjectCreateInput()
                        {
                            MainName = sendSubject.Name.Replace("\"", ""),
                            Address = sendSubject.JuridicalAddress,
                            NameOrg = sendSubject.ShortName.Replace("\"", ""),
                            SubjectTypesId = sendSubject.OrganizationForm.Code == "91"
                                ? subjectTypes.ContainsKey("ИП") ? subjectTypes["ИП"] : null
                                : subjectTypes.ContainsKey("ЮЛ") ? subjectTypes["ЮЛ"] : null
                        }).Build(Formatting.Indented, 2);
            }

            if (request == null || requestName == null)
            {
                return;
            }

            this.AddToList(requestName, request, sendSubject);
        }

        /// <summary>
        /// Запрос на отправку карточки проверки ControlCard
        /// </summary>
        private void GetControlCardsRequest(IDomainService<TatarstanDisposal> disposalDomain, 
            IDomainService<ConfigurationReferenceInformationKndTor> сonfigRefInformationKndTorDomain, 
            ActCheck act, 
            TatarstanDisposal disposal)
        {

            IDomainService<DisposalSurveyPurpose> surveyPurposeDomain;
            IDomainService<ActCheckPeriod> actCheckPeriodDomain;
            IDomainService<BaseJurPerson> basejurpersonDomain;
            IDomainService<InspectionBaseContragent> inspectionContragentDomain;

            string requestName = null;
            string request = null;

            using (this.Container.Using(
                surveyPurposeDomain = this.Container.ResolveDomain<DisposalSurveyPurpose>(),
                basejurpersonDomain = this.Container.ResolveDomain<BaseJurPerson>(),
                actCheckPeriodDomain = this.Container.ResolveDomain<ActCheckPeriod>(),
                inspectionContragentDomain = this.Container.ResolveDomain<InspectionBaseContragent>()))
            {
                var surveyPurpose = surveyPurposeDomain.GetAll()
                    .Where(x => x.Disposal.Id == this.SendObject.Id)
                    .Select(x => x.SurveyPurpose.Name).ToList();

                var subjectDisposalPrevDate = disposalDomain.GetAll()
                    .Where(x => x.Inspection.Contragent.Id == this.SendObject.Inspection.Contragent.Id && x.DocumentDate < this.SendObject.DocumentDate)
                    .OrderByDescending(x => x.DocumentDate)
                    .Select(x => x.DocumentDate).FirstOrDefault();

                var baserJurPerson = basejurpersonDomain.Get(this.SendObject.Inspection.Id);

                var actCheckDate = actCheckPeriodDomain.GetAll()
                    .Where(x => act.Id == x.ActCheck.Id)
                    .OrderBy(x => x.DateCheck)
                    .Select(x => x.DateCheck)
                    .FirstOrDefault();

                var inspectionContragent = inspectionContragentDomain.GetAll()
                    .Where(x => x.InspectionGji.Id == disposal.Inspection.Id)
                    .Select(x => x.Contragent.Name).ToList();

                var knmForm = сonfigRefInformationKndTorDomain.GetAll()
                    .Where(w => w.Type == Enums.DictTypes.KnmForm)
                    .Select(s => new { s.Value, s.TorId })?
                    .ToDictionary(d => d.Value, d => d.TorId);

                var measureUnitTypes = сonfigRefInformationKndTorDomain.GetAll()
                    .Where(w => w.Type == Enums.DictTypes.MeasureUnitType)
                    .Select(s => new { s.Value, s.TorId })?
                    .ToDictionary(d => d.Value.ToLower(), d => d.TorId);

                //var notPlannedForm = new List<TypeCheck>() { TypeCheck.NotPlannedDocumentation, TypeCheck.NotPlannedExit, TypeCheck.NotPlannedExit };
                var plannedForm = new List<TypeCheck>() { TypeCheck.PlannedDocumentation, TypeCheck.PlannedExit, TypeCheck.PlannedExit };

                this.GetSurveyPeriod(disposal, measureUnitTypes, out var measureUnitType, out var controlDuration);

                if (disposal.ControlCardId != null)
                {
                    var getControlCard = new QueryQueryBuilder()
                        .WithGetControlCard(new ControlCardQueryBuilder().WithId(), (Guid) disposal.ControlCardId)
                        .Build(Formatting.Indented, 2);

                    if (this.TorIntegrationService.SendGetRequest("GetControlCard", getControlCard, CancellationToken.None) != null)
                    {
                        requestName = "UpdateControlCard";
                        request = new MutationQueryBuilder()
                            .WithUpdateControlCard(
                                new ControlCardQueryBuilder().WithId(),
                                new ControlCardUpdateInput()
                                {
                                    Id = disposal.ControlCardId,
                                    ProcDataAgreement = disposal.DocumentDateWithResultAgreement,
                                    ControlStartDate = disposal.DateStart,
                                    ControlDuration = controlDuration,
                                    ControlEndDate = disposal.DateEnd,
                                    ControlPurpose = string.Join(", ", surveyPurpose),
                                    NumberFgiserp = disposal.ErpRegistrationNumber,
                                    FgiserpRegData = disposal.ErpRegistrationDate,
                                    LastEndControlDate = subjectDisposalPrevDate,
                                    CheckControlRestrict = baserJurPerson?.Reason,
                                    InternalNumberFgiserp = disposal.ErpId,
                                    ControlFactStartDate = actCheckDate,
                                    JointControlOrganization = string.Join(", ", inspectionContragent),
                                    ControlFormId = plannedForm.Contains(disposal.KindCheck.Code)
                                        ? knmForm.ContainsKey("Плановая") ? knmForm["Плановая"] : null
                                        : knmForm.ContainsKey("Внеплановая") ? knmForm["Внеплановая"] : null,
                                    ControlBaseId = disposal.InspectionBase?.TorId,
                                    MeasureUnitTypeId = measureUnitType
                                }).Build(Formatting.Indented, 2);
                    }
                }

                if (disposal.ControlCardId == null || request == null)
                {
                    requestName = "CreateControlCard";
                    request = new MutationQueryBuilder()
                        .WithCreateControlCard(
                            new ControlCardQueryBuilder().WithId(),
                            new ControlCardCreateInput()
                            {
                                ProcDataAgreement = disposal.DocumentDateWithResultAgreement,
                                ControlStartDate = disposal.DateStart,
                                ControlDuration = controlDuration,
                                ControlEndDate = disposal.DateEnd,
                                ControlPurpose = string.Join(", ", surveyPurpose),
                                NumberFgiserp = disposal.ErpRegistrationNumber,
                                FgiserpRegData = disposal.ErpRegistrationDate,
                                LastEndControlDate = subjectDisposalPrevDate,
                                CheckControlRestrict = baserJurPerson?.Reason,
                                InternalNumberFgiserp = disposal.ErpId,
                                ControlFactStartDate = actCheckDate,
                                JointControlOrganization = string.Join(", ", inspectionContragent),
                                ControlFormId = plannedForm.Contains(disposal.KindCheck.Code)
                                    ? knmForm.ContainsKey("Плановая") ? knmForm["Плановая"] : null
                                    : knmForm.ContainsKey("Внеплановая") ? knmForm["Внеплановая"] : null,
                                ControlBaseId = disposal.InspectionBase?.TorId,
                                MeasureUnitTypeId = measureUnitType
                            }).Build(Formatting.Indented, 2);
                }


                if (request == null || requestName == null)
                {
                    return;
                }

                this.AddToList(requestName, request, disposal);
            }
        }

        /// <summary>
        /// Запрос на отправку объекта(дома) Object
        /// </summary>
        private void GetObjectsRequest(List<RealityObject> realityObjects)
        {
            IDomainService<RealityObject> realityObjectDomain;
            string requestName = null;
            string request = null;

            using (this.Container.Using(realityObjectDomain = this.Container.ResolveDomain<RealityObject>()))
            {
                var realityObj = realityObjectDomain.GetAll().Where(x => realityObjects.Contains(x))
                    .AsEnumerable()
                    .Select(x => new
                    {
                        RealityObject = x,
                        x.Id,
                        x.TorId,
                        MoTorId = x.Municipality.TorId,
                        x.Address,
                        x.FiasAddress.AddressGuid,
                        x.FiasAddress.PostCode,
                        x.FiasAddress.AddressName,
                        x.FiasAddress.HouseGuid
                    }).ToList();

                foreach (var realObj in realityObj)
                {
                    if (realObj.HouseGuid.HasValue)
                    {
                        this.CreateAddressIfNotExist((Guid) realObj.HouseGuid, realObj.Address, realObj.PostCode, realObj.AddressName);
                    }

                    if (!(realObj.TorId is null))
                    {
                        var getObject = new QueryQueryBuilder()
                            .WithGetControlObject(new ControlObjectQueryBuilder().WithId(), (Guid) realObj.TorId)
                            .Build(Formatting.Indented, 2);

                        if (this.TorIntegrationService.SendGetRequest("GetControlObject", getObject, CancellationToken.None) != null)
                        {

                            requestName = "UpdateControlObject";
                            request = new MutationQueryBuilder()
                                .WithUpdateControlObject(
                                    new ControlObjectQueryBuilder().WithId(),
                                    new ControlObjectUpdateInput()
                                    {
                                        Id = realObj.TorId,
                                        ControlObjectName = realObj.Address,
                                        AddressId = realObj.HouseGuid,
                                        SubjectId = this.SendObject.Inspection.Contragent.TorId,
                                        CodeOktmoId = realObj.MoTorId
                                    }).Build(Formatting.Indented, 2);
                        }
                    }

                    if (realObj.TorId is null || request is null)
                    {
                        requestName = "CreateControlObject";
                        request = new MutationQueryBuilder()
                            .WithCreateControlObject(
                                new ControlObjectQueryBuilder().WithId(),
                                new ControlObjectCreateInput()
                                {
                                    ControlObjectName = realObj.Address,
                                    AddressId = realObj.HouseGuid,
                                    SubjectId = this.SendObject.Inspection.Contragent.TorId,
                                    CodeOktmoId = realObj.MoTorId
                                }).Build(Formatting.Indented, 2);
                    }

                    this.AddToList(requestName, request, realObj.RealityObject);
                }
            }
        }

        /// <summary>
        /// Запрос на создание адресов
        /// </summary>
        private void CreateAddressIfNotExist(Guid houseGuid, string address, string addressName, string postCode)
        {
            var getAddress = new QueryQueryBuilder()
                .WithGetAddress(new AddressQueryBuilder().WithId(), houseGuid)
                .Build(Formatting.Indented, 2);

            if (this.TorIntegrationService.SendGetRequest("GetAddress", getAddress, CancellationToken.None) == null)
            {
                var createAddress = new MutationQueryBuilder()
                    .WithCreateAddress(
                        new AddressQueryBuilder().WithId(),
                        new AddressCreateInput()
                        {
                            Id = houseGuid,
                            CodeKladr = null,
                            CodeFias = houseGuid.ToString(),
                            Address = address ?? addressName,
                            PostIndex = postCode,
                            AddressFact = addressName
                        }).Build(Formatting.Indented, 2);

                this.AddToList("CreateAddress", createAddress, null);
            }
        }

        /// <summary>
        /// Запрос на создание ControlItemResult Результаты проверки
        /// </summary>
        private void GetControlItemResultsRequest(TatarstanDisposal disposal)
        {
            IDomainService<Resolution> resolutionDomain;
            string requestName = null;
            string request = null;

            using (this.Container.Using(resolutionDomain = this.Container.ResolveDomain<Resolution>()))
            {
                var resolution = resolutionDomain.GetAll()
                    .Where(x => x.Stage.Parent.Id == disposal.Stage.Id)
                    .Select(x => new
                    {
                        Amount = x.PenaltyAmount.Value,
                        Paided = x.Paided == YesNoNotSet.Yes,
                    }).ToList();

                if (disposal.ResultTorId != null)
                {
                    var getControlItemResult = new QueryQueryBuilder()
                        .WithGetControlItemResult(new ControlItemResultQueryBuilder().WithId(), (Guid) disposal.ResultTorId)
                        .Build(Formatting.Indented, 2);

                    if (this.TorIntegrationService.SendGetRequest("GetControlItemResult", getControlItemResult, CancellationToken.None) != null)
                    {
                        requestName = "UpdateControlItemResult";
                        request = new MutationQueryBuilder()
                            .WithUpdateControlItemResult(
                                new ControlItemResultQueryBuilder().WithId(),
                                new ControlItemResultUpdateInput()
                                {
                                    Id = disposal.ResultTorId,
                                    SumAdmFine = resolution.Sum(x => x.Amount),
                                    SumAdmFineStatus = resolution.TrueForAll(x => x.Paided),
                                    LinkedControlCardId = disposal.ControlCardId
                                }).Build(Formatting.Indented, 2);
                    }
                }

                if (disposal.ResultTorId == null || request == null)
                {
                    requestName = "CreateControlItemResult";
                    request = new MutationQueryBuilder()
                        .WithCreateControlItemResult(
                            new ControlItemResultQueryBuilder().WithId(),
                            new ControlItemResultCreateInput()
                            {
                                SumAdmFine = resolution.Sum(x => x.Amount),
                                SumAdmFineStatus = resolution.TrueForAll(x => x.Paided),
                                LinkedControlCardId = disposal.ControlCardId
                            }).Build(Formatting.Indented, 2);
                }

                if (request == null || requestName == null)
                {
                    return;
                }

                this.AddToList(requestName, request, disposal);
            }
        }

        /// <summary>
        /// Запрос на создание ControlList
        /// </summary>
        private void GetControlListsRequest(ControlList controlList)
        {
            string requestName = null;
            string request = null;

            if (controlList.TorId != null)
            {
                var getControlList = new QueryQueryBuilder()
                    .WithGetdocControlList(new docControlListQueryBuilder().WithId(), (Guid) controlList.TorId)
                    .Build(Formatting.Indented, 2);

                if (this.TorIntegrationService.SendGetRequest("GetdocControlList", getControlList, CancellationToken.None) != null)
                {
                    requestName = "UpdatedocControlList";
                    request = new MutationQueryBuilder()
                        .WithUpdatedocControlList(
                            new docControlListQueryBuilder().WithId(),
                            new docControlListUpdateInput()
                            {
                                Id = controlList.TorId,
                                ControlListStartDate = controlList.StartDate.Date,
                                ControlListEndDate = controlList.EndDate?.Date,
                                ControlItemResultId = this.SendObject.ResultTorId,
                                ControlTypeId = controlList.Disposal.ControlType?.TorId
                            }).Build(Formatting.Indented, 2);

                }
            }

            if (controlList.TorId == null || request == null)
            {
                requestName = "CreatedocControlList";
                request = new MutationQueryBuilder()
                    .WithCreatedocControlList(
                        new docControlListQueryBuilder().WithId(),
                        new docControlListCreateInput()
                        {
                            ControlListStartDate = controlList.StartDate.Date,
                            ControlListEndDate = controlList.EndDate?.Date,
                            ControlItemResultId = this.SendObject.ResultTorId,
                            ControlTypeId = controlList.Disposal.ControlType?.TorId
                        }).Build(Formatting.Indented, 2);
            }

            if (request == null || requestName == null)
            {
                return;
            }

            this.AddToList(requestName, request, controlList);
        }

        /// <summary>
        /// Запрос на создание ControlAct
        /// </summary>
        private void GetControlActsRequest(ActCheck act, TatarstanDisposal disposal)
        {
            string requestName = null;
            string request = null;

            if (!(act.DocumentPlaceFias?.HouseGuid is null))
            {
                this.CreateAddressIfNotExist((Guid) act.DocumentPlaceFias.HouseGuid,
                    null,
                    act.DocumentPlaceFias.PostCode,
                    act.DocumentPlaceFias.AddressName);
            }

            if (act.TorId != null)
            {
                var getControlAct = new QueryQueryBuilder()
                    .WithGetdocControlAct(new docControlActQueryBuilder().WithId(), (Guid) act.TorId)
                    .Build(Formatting.Indented, 2);

                if (this.TorIntegrationService.SendGetRequest("GetdocControlAct", getControlAct, CancellationToken.None) != null)
                {
                    requestName = "UpdatedocControlAct";
                    request = new MutationQueryBuilder()
                        .WithUpdatedocControlAct(
                            new docControlActQueryBuilder().WithId(),
                            new docControlActUpdateInput()
                            {
                                Id = act.TorId,
                                ControlActCreateDate = act.DocumentDate,
                                ControlActCreatePlaceId = act.DocumentPlaceFias?.HouseGuid,
                                ActLinkedtoControlCardId = disposal.ControlCardId
                            }).Build(Formatting.Indented, 2);
                }
            }

            if (act.TorId == null || request == null)
            {
                requestName = "CreatedocControlAct";
                request = new MutationQueryBuilder()
                    .WithCreatedocControlAct(
                        new docControlActQueryBuilder().WithId(),
                        new docControlActCreateInput()
                        {
                            ControlActCreateDate = act.DocumentDate,
                            ControlActCreatePlaceId = act.DocumentPlaceFias?.HouseGuid,
                            ActLinkedtoControlCardId = disposal.ControlCardId
                        }).Build(Formatting.Indented, 2);
            }

            if (request == null || requestName == null)
            {
                return;
            }

            this.AddToList(requestName, request, act);
        }

        /// <summary>
        /// Запрос на создание ControlItemPassport
        /// </summary>
        private void GetControlItemPassportsRequest(TatarstanDisposal disposal, 
            List<RealityObject> realityObj, 
            IDomainService<ConfigurationReferenceInformationKndTor> сonfigRefInformationKndTorDomain)
        {
            string requestName = null;
            string request = null;

            var CheckStatus = сonfigRefInformationKndTorDomain.GetAll()
                    .Where(w => w.Type == Enums.DictTypes.CheckStatus)
                    .Select(s => new { s.Value, s.TorId })?
                    .ToDictionary(d => d.Value, d => d.TorId);

            if (disposal.TorId != null)
            {
                var getControlItemPassport = new QueryQueryBuilder()
                    .WithGetControlItemPassport(new ControlItemPassportQueryBuilder().WithId(), (Guid) disposal.TorId)
                    .Build(Formatting.Indented, 2);

                if (this.TorIntegrationService.SendGetRequest("GetControlItemPassport", getControlItemPassport, CancellationToken.None) != null)
                {
                    requestName = "UpdateControlItemPassport";
                    request = new MutationQueryBuilder()
                        .WithUpdateControlItemPassport(
                            new ControlItemPassportQueryBuilder().WithId(),
                            new ControlItemPassportUpdateInput()
                            {
                                Id = disposal.TorId,
                                CreateDate = disposal.DocumentDate,
                                ControlCardId = disposal.ControlCardId,
                                ControlObjects = realityObj.Where(x => x.TorId.HasValue).Select(x => x.TorId).To<List<Guid>>(),
                                ControlOrganizationId = this.ControlOrgGuid,
                                ControlTypeId = disposal.ControlType?.TorId,
                                StatusKnmNameId = CheckStatus.ContainsKey("Актуальный") ? CheckStatus["Актуальный"] : null,
                                SubjectId = disposal.Inspection.Contragent.TorId
                            }).Build(Formatting.Indented, 2);
                }
            }

            if (disposal.TorId == null || request == null)
            {
                requestName = "CreateControlItemPassport";
                request = new MutationQueryBuilder()
                    .WithCreateControlItemPassport(
                        new ControlItemPassportQueryBuilder().WithId(),
                        new ControlItemPassportCreateInput()
                        {
                            CreateDate = disposal.DocumentDate,
                            ControlCardId = disposal.ControlCardId,
                            ControlObjects = realityObj.Where(x => x.TorId.HasValue).Select(x => x.TorId).To<List<Guid>>(),
                            ControlOrganizationId = this.ControlOrgGuid,
                            ControlTypeId = disposal.ControlType?.TorId,
                            StatusKnmNameId = CheckStatus.ContainsKey("Актуальный") ? CheckStatus["Актуальный"] : null,
                            SubjectId = disposal.Inspection.Contragent.TorId
                        }).Build(Formatting.Indented, 2);

            }

            if (request == null || requestName == null)
            {
                return;
            }

            this.AddToList(requestName, request, disposal);
        }

        private void AddToList(string requestName, string request, IUsedInTorIntegration subject)
        {
            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>(requestName, request, subject));
        }

        /// <summary>
        /// Получение периода обследования
        /// </summary>
        /// <param name="disposal">Распоряжение</param>
        /// <param name="measureUnitTypes">Справочник единиц измерения</param>
        /// <param name="measureUnitType">Идентификатор Тор единицы измерения периода обследования</param>
        /// <param name="controlDuration">Значение периода обследования</param>
        private void GetSurveyPeriod(TatarstanDisposal disposal, Dictionary<string, Guid?> measureUnitTypes, out Guid? measureUnitType, out int? controlDuration)
        {
            if (disposal.CountDays != null)
            {
                measureUnitType = measureUnitTypes.ContainsKey("день") ? measureUnitTypes["день"] : null;
                controlDuration = disposal.CountDays;
                return;
            }

            if (disposal.CountHours != null)
            {
                measureUnitType = measureUnitTypes.ContainsKey("час") ? measureUnitTypes["час"] : null;
                controlDuration = disposal.CountHours;
                return;
            }

            if (disposal.DateStart != null)
            {
                measureUnitType = measureUnitTypes.ContainsKey("день") ? measureUnitTypes["день"] : null;
                controlDuration = disposal.DateEnd == null
                    ? Math.Max((DateTime.Now - disposal.DateStart.GetValueOrDefault()).Days, 1)
                    : Math.Max((disposal.DateEnd.GetValueOrDefault() - disposal.DateStart.GetValueOrDefault()).Days, 1);
                return;
            }

            measureUnitType = null;
            controlDuration = null;
        }
    }*/
}