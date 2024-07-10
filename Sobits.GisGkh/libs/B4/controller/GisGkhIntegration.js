Ext.define('B4.controller.GisGkhIntegration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
    ],

    models: [
        'gisGkh.TaskGridModel',
        'gisGkh.DictGridModel',
        'gisGkh.DisposalForGisGkhModel',
        'gisGkh.ResolutionForGisGkhModel',
        'gisGkh.ROForGisGkhExportModel',
        'gisGkh.MOForGisGkhExportModel',
        'gisGkh.ProgramForGisGkhExportModel',
        'gisGkh.ContragentForGisGkhExportModel',
        'gisGkh.RoomGridModel',
        'gisGkh.PremisesGridModel',
        'gisGkh.DownloadGridModel',
        'gisGkh.ProgramImportModel',
        'gisGkh.ObjectCrImportModel',
        'gisGkh.BuildContractImportModel',
        'gisGkh.PerfWorkActImportModel',
        'dict.PlanJurPersonGji'
    ],

    views: [
        //'gisGkh.PrepareDataResultWindow',
        //'gisGkh.ProtocolWindow',
        //'gisGkh.Panel',
        'gisGkh.TaskGrid',
        'gisGkh.DictGrid',
        'gisGkh.DictItemGrid',
        'gisGkh.EditWindow',
        'gisGkh.DictWindow',
        'gisGkh.Panel',
        'gisGkh.SignWindow',
        'gisGkh.MassSignWindow',
        'gisGkh.RoomMatchingPanel',
        'gisGkh.RoomGrid',
        'gisGkh.PremisesGrid',
        'gisGkh.ROGrid',
        'gisGkh.DownloadGrid'
    ],

    stores: [
        'gisGkh.TaskGridStore',
        'gisGkh.TaskSignStore',
        'gisGkh.DictGridStore',
        'gisGkh.DictItemStore',
        'gisGkh.DisposalForGisGkhStore',
        'gisGkh.ResolutionForGisGkhStore',
        'gisGkh.ROForGisGkhExportStore',
        'gisGkh.MOForGisGkhExportStore',
        'gisGkh.ProgramForGisGkhExportStore',
        'gisGkh.ContragentForGisGkhExportStore',
        'gisGkh.RoomGridStore',
        'gisGkh.PremisesGridStore',
        'gisGkh.DownloadGridStore',
        'gisGkh.ProgramImportStore',
        'gisGkh.ObjectCrImportStore',
        'gisGkh.BuildContractImportStore',
        'gisGkh.BuildContractForActImportStore',
        'gisGkh.PerfWorkActImportStore',
        'dict.PlanJurPersonGji'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    //refs: [
    //    { ref: 'mainView', selector: 'gisgkhtaskgrid' }
    //],
    refs: [
        { ref: 'mainView', selector: 'gisgkhintegrationpanel' }
    ],
    //mainView: 'gisGkh.TaskGrid',
    //mainViewSelector: 'gisgkhtaskgrid',
    mainView: 'gisGkh.Panel',
    mainViewSelector: 'gisgkhintegrationpanel',
    roomId: null,
    premisesId: null,
    roId: null,
    roomMatched: true,
    premisesMatched: true,
    programId: null,
    objCrId: null,
    buildContrId: null,

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'gisGkhTaskGridAspect',
            gridSelector: 'gisgkhtaskgrid',
            editFormSelector: '#gisGkhEditWindow',
            storeName: 'gisGkh.TaskGridStore',
            modelName: 'gisGkh.TaskGridModel',
            editWindowView: 'gisGkh.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['gisgkhtaskgrid actioncolumn[action="signRequest"]'] = { 'click': { fn: this.opensignwindow, scope: this } };
                delete actions[this.editFormSelector + 'b4savebutton'];
                actions[this.editFormSelector + ' b4savebutton'] = { 'click': { fn: this.saveRequest, scope: this } };
                actions['#gisGkhEditWindow #dfTypeRequest'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions['gisgkhtaskgrid #btnSignRequests'] = { 'click': { fn: this.openMassSignWindow, scope: this } };

                actions['#gisGkhEditWindow #dfPlanForContract'] = { 'change': { fn: this.onChangeProgram, scope: this } };
                actions['#gisGkhEditWindow #dfObjectCr'] = { 'beforeload': { fn: this.onBeforeLoadObjectCr, scope: this } };
                actions['#gisGkhEditWindow [name=ObjectCr]'] = { 'change': { fn: this.onChangeObjectCr, scope: this } };
                actions['#gisGkhEditWindow #dfBuildContract'] = { 'beforeload': { fn: this.onBeforeLoadBuildContract, scope: this } };
                actions['#gisGkhEditWindow #dfBuildContractForAct'] = { 'beforeload': { fn: this.onBeforeLoadBuildContractForAct, scope: this } };
                actions['#gisGkhEditWindow [name=BuildContractForAct]'] = { 'change': { fn: this.onChangeBuildContractForAct, scope: this } };
                actions['#gisGkhEditWindow #dfPerfWorkAct'] = { 'beforeload': { fn: this.onBeforeLoadPerfWorkAct, scope: this } };
            },

            onChangeProgram: function (field, newValue) {
                programId = newValue.Id;
            },
            onChangeObjectCr: function (field, newValue) {
                
                objCrId = newValue.Id;
            },
            onChangeBuildContractForAct: function (field, newValue) {
                
                buildContrId = newValue.Id;
            },

            onBeforeLoadObjectCr: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.programId = programId;
            },
            onBeforeLoadBuildContract: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.objCrId = objCrId;
            },
            onBeforeLoadBuildContractForAct: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.objCrId = objCrId;
            },
            onBeforeLoadPerfWorkAct: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.buildContrId = buildContrId;
            },

            onChangeType: function (field, newValue) {
                var form = this.getForm(),
                    dfRealityObject = form.down('#dfRealityObject'),
                    dfMunicipality = form.down('#dfMunicipality'),
                    dfPlanType = form.down('#dfPlanType'),
                    dfProgram = form.down('#dfProgram'),
                    dfContragent = form.down('#dfContragent'),
                    dfPlan = form.down('#dfPlan'),
                    dfPlanForContract = form.down('#dfPlanForContract'),
                    dfBuildContract = form.down('#dfBuildContract'),
                    dfBuildContractForAct = form.down('#dfBuildContractForAct'),
                    dfObjectCr = form.down('#dfObjectCr'),
                    dfPerfWorkAct = form.down('#dfPerfWorkAct'),
                    dfDateFrom = form.down('#dfDateFrom'),
                    dfDateTo = form.down('#dfDateTo'),
                    dfListGroup = form.down('#dfListGroup'),
                    dfRegNumber = form.down('#dfRegNumber'),
                    dfChargePeriod = form.down('#dfChargePeriod'),
                    cbRewrite = form.down('#cbRewrite'),
                    nfPage = form.down('#nfPage'),
                    dfDataProviderNsiItem = form.down('#dfDataProviderNsiItem'),
                    //dfStartDate = form.down('#dfStartDate'),
                    //dfEndDate = form.down('#dfEndDate'),
                    saveBtn = form.down('b4savebutton'),
                    dfExamination = form.down('#dfExamination'),
                    dfExaminationPlan = form.down('#dfExaminationPlan'),
                    dfResolution = form.down('#dfResolution');
                if (newValue == B4.enums.GisGkhTypeRequest.exportHouseData ||
                    newValue == B4.enums.GisGkhTypeRequest.exportAccountData ||
                    newValue == B4.enums.GisGkhTypeRequest.importAccountData) {
                    dfRealityObject.show();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportBriefApartmentHouse) {
                    dfRealityObject.show();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportRegionalProgram) {
                    dfRealityObject.hide();
                    dfMunicipality.show();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportRegionalProgramWork) {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.show();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportPlan) {
                    dfRealityObject.hide();
                    dfPlanType.show();
                    dfMunicipality.show();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportPlanWork ||
                    newValue == B4.enums.GisGkhTypeRequest.importPlanWork) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.show();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportOrgRegistry ||
                    newValue == B4.enums.GisGkhTypeRequest.exportLicense) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.show();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportDecisionsFormingFund ||
                    newValue == B4.enums.GisGkhTypeRequest.importDecisionsFormingFund) {
                    dfRealityObject.show();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importPlan) {
                    dfRealityObject.hide();
                    dfPlanType.show();
                    dfMunicipality.show();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.show();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportExaminations ||
                    newValue == B4.enums.GisGkhTypeRequest.importExaminations ||
                    newValue == B4.enums.GisGkhTypeRequest.exportDecreesAndDocumentsData ||
                    newValue == B4.enums.GisGkhTypeRequest.exportAppeal ||
                    newValue == B4.enums.GisGkhTypeRequest.exportAppealCR ||
                    newValue == B4.enums.GisGkhTypeRequest.exportDebtRequests) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.show();
                    dfDateTo.show();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importCurrentExaminations) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.show();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importDecreesAndDocumentsData) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.show();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importRegionalProgram ||
                    newValue == B4.enums.GisGkhTypeRequest.importRegionalProgramWork) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportNsiItems) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.show();
                    dfRegNumber.show();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportNsiPagingItems) {
                    dfRealityObject.hide();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.show();
                    dfRegNumber.show();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.show();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importPaymentDocumentData) {
                    dfRealityObject.show();
                    dfPlanType.hide();
                    dfMunicipality.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.show();
                    cbRewrite.show();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportPaymentDocumentData) {
                    dfRealityObject.show();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.show();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.exportDataProviderNsiItem) {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.show();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importInspectionPlan) {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.show();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importBuildContract) {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.show();
                    dfObjectCr.show();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.show();
                    dfBuildContractForAct.hide();
                }
                else if (newValue == B4.enums.GisGkhTypeRequest.importPerfWorkAct) {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.show();
                    dfPerfWorkAct.show();
                    dfPlanForContract.show();
                    dfBuildContractForAct.show();
                }
                //else if (newValue == B4.enums.GisGkhTypeRequest.exportAppeal) {
                //    dfRealityObject.hide();
                //    dfMunicipality.hide();
                //    dfPlanType.hide();
                //    dfProgram.hide();
                //    dfContragent.hide();
                //    dfPlan.hide();
                //    dfDateFrom.hide();
                //    dfDateTo.hide();
                //    dfExamination.hide();
                //    dfExaminationPlan.hide();
                //    dfResolution.hide();
                //    dfListGroup.hide();
                //    dfRegNumber.hide();
                //    dfChargePeriod.hide();
                //    cbRewrite.hide();
                //    nfPage.hide();
                //    dfDataProviderNsiItem.hide();
                //    dfStartDate.show();
                //    dfEndDate.show();
                //}
                else {
                    dfRealityObject.hide();
                    dfMunicipality.hide();
                    dfPlanType.hide();
                    dfProgram.hide();
                    dfContragent.hide();
                    dfPlan.hide();
                    dfDateFrom.hide();
                    dfDateTo.hide();
                    dfExamination.hide();
                    dfExaminationPlan.hide();
                    dfResolution.hide();
                    dfListGroup.hide();
                    dfRegNumber.hide();
                    dfChargePeriod.hide();
                    cbRewrite.hide();
                    nfPage.hide();
                    dfDataProviderNsiItem.hide();
                    dfBuildContract.hide();
                    dfObjectCr.hide();
                    dfPerfWorkAct.hide();
                    dfPlanForContract.hide();
                    dfBuildContractForAct.hide();
                    //dfStartDate.hide();
                    //dfEndDate.hide();
                }
                if (newValue != B4.enums.GisGkhTypeRequest.exportHouseData &&
                    newValue != B4.enums.GisGkhTypeRequest.exportAccountData &&
                    newValue != B4.enums.GisGkhTypeRequest.exportBriefApartmentHouse &&
                    newValue != B4.enums.GisGkhTypeRequest.exportNsiList &&
                    newValue != B4.enums.GisGkhTypeRequest.exportNsiRaoList &&
                    newValue != B4.enums.GisGkhTypeRequest.exportNsiItems &&
                    newValue != B4.enums.GisGkhTypeRequest.exportNsiPagingItems &&
                    newValue != B4.enums.GisGkhTypeRequest.exportRegionalProgram &&
                    newValue != B4.enums.GisGkhTypeRequest.exportRegionalProgramWork &&
                    newValue != B4.enums.GisGkhTypeRequest.exportPlan &&
                    newValue != B4.enums.GisGkhTypeRequest.exportPlanWork &&
                    newValue != B4.enums.GisGkhTypeRequest.exportOrgRegistry &&
                    newValue != B4.enums.GisGkhTypeRequest.importAccountData &&
                    newValue != B4.enums.GisGkhTypeRequest.exportDecisionsFormingFund &&
                    newValue != B4.enums.GisGkhTypeRequest.importDecisionsFormingFund &&
                    newValue != B4.enums.GisGkhTypeRequest.importPlan &&
                    newValue != B4.enums.GisGkhTypeRequest.importPlanWork &&
                    newValue != B4.enums.GisGkhTypeRequest.exportPaymentDocumentData &&
                    newValue != B4.enums.GisGkhTypeRequest.importPaymentDocumentData &&
                    newValue != B4.enums.GisGkhTypeRequest.exportDataProviderNsiItem &&
                    newValue != B4.enums.GisGkhTypeRequest.importRegionalProgram &&
                    newValue != B4.enums.GisGkhTypeRequest.importRegionalProgramWork &&
                    newValue != B4.enums.GisGkhTypeRequest.exportExaminations &&
                    newValue != B4.enums.GisGkhTypeRequest.importCurrentExaminations &&
                    newValue != B4.enums.GisGkhTypeRequest.exportInspectionPlans &&
                    newValue != B4.enums.GisGkhTypeRequest.importInspectionPlan &&
                    newValue != B4.enums.GisGkhTypeRequest.exportDecreesAndDocumentsData &&
                    newValue != B4.enums.GisGkhTypeRequest.importDecreesAndDocumentsData &&
                    newValue != B4.enums.GisGkhTypeRequest.exportLicense &&
                    newValue != B4.enums.GisGkhTypeRequest.exportAppeal &&
                    newValue != B4.enums.GisGkhTypeRequest.exportAppealCR &&
                    newValue != B4.enums.GisGkhTypeRequest.importBuildContract &&
                    newValue != B4.enums.GisGkhTypeRequest.importPerfWorkAct &&
                    newValue != B4.enums.GisGkhTypeRequest.exportDebtRequests) {
                    saveBtn.hide();
                }
                else {
                    saveBtn.show();
                }
            },

            saveRequest: function () {
                var rec, from = this.getForm();
                var me = this;
                if (this.fireEvent('beforesaverequest', this) !== false) {
                    from.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(from.getRecord());
                    var reqParams;
                    if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportHouseData ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportAccountData ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importAccountData ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importDecisionsFormingFund ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportDecisionsFormingFund) {
                        reqParams = [from.down('#dfRealityObject').getValue()];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportBriefApartmentHouse) {
                        reqParams = [from.down('#dfRealityObject').getValue()];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportRegionalProgram) {
                        reqParams = [from.down('#dfMunicipality').getValue()];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportRegionalProgramWork) {
                        reqParams = [from.down('#dfProgram').getValue()];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportPlan) {
                        reqParams = [
                            from.down('#dfPlanType').getValue(),
                            from.down('#dfMunicipality').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportExaminations ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importExaminations ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportDecreesAndDocumentsData ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportAppeal ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportAppealCR ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportDebtRequests) {
                        reqParams = [
                            from.down('#dfDateFrom').getValue(),
                            from.down('#dfDateTo').getValue(),
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importCurrentExaminations) {
                        reqParams = from.down('#dfExamination').getValue();
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportInspectionPlans) {
                        reqParams = [
                            null,
                            null
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importInspectionPlan) {
                        reqParams = from.down('#dfExaminationPlan').getValue();
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importDecreesAndDocumentsData) {
                        reqParams = from.down('#dfResolution').getValue();
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportOrgRegistry ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportLicense) {
                        reqParams = [
                            from.down('#dfContragent').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportDataProviderNsiItem) {
                        reqParams = [
                            from.down('#dfDataProviderNsiItem').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importPlan) {
                        reqParams = [
                            from.down('#dfPlan').getValue(),
                            from.down('#dfMunicipality').getValue(),
                            from.down('#dfPlanType').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportPlanWork ||
                        rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importPlanWork) {
                        reqParams = [
                            from.down('#dfPlan').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportNsiItems) {
                        reqParams = [
                            from.down('#dfListGroup').getValue(),
                            from.down('#dfRegNumber').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportNsiPagingItems) {
                        reqParams = [
                            from.down('#dfListGroup').getValue(),
                            from.down('#dfRegNumber').getValue(),
                            from.down('#nfPage').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importPaymentDocumentData) {
                        reqParams = [
                            from.down('#dfChargePeriod').getValue(),
                            from.down('#dfRealityObject').getValue(),
                            from.down('#cbRewrite').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportPaymentDocumentData) {
                        reqParams = [
                            from.down('#dfRealityObject').getValue(),
                            from.down('#dfChargePeriod').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importBuildContract) {
                        reqParams = [
                            from.down('#dfPlanForContract').getValue(),
                            from.down('#dfBuildContract').getValue(),
                            from.down('#dfObjectCr').getValue()
                        ];
                    }
                    else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.importPerfWorkAct) {
                        reqParams = [
                            from.down('#dfPlanForContract').getValue(),
                            from.down('#dfBuildContractForAct').getValue(),
                            from.down('#dfPerfWorkAct').getValue(),
                            from.down('#dfObjectCr').getValue()

                        ];
                    }
                    //else if (rec.data.TypeRequest == B4.enums.GisGkhTypeRequest.exportAppeal) {
                    //    reqParams = [
                    //        from.down('#dfStartDate').getValue(),
                    //        from.down('#dfEndDate').getValue()
                    //    ];
                    //}
                    B4.Ajax.request({
                        url: B4.Url.action('SaveRequest', 'GisGkhExecute'),
                        params: {
                            reqId: rec.getId(),
                            reqType: rec.data.TypeRequest,
                            reqParams: reqParams
                        },
                        timeout: 9999999
                    }).next(function (response) {

                        var data = Ext.decode(response.responseText);
                        from.close();
                        B4.QuickMsg.msg('Сохранение', data.data, 'success');
                        //me.unmask();
                        return true;
                    }).error(function (resp) {
                        Ext.Msg.alert('Ошибка', resp.message);
                        //me.unmask();
                    });
                    me.getGrid().getStore().load();
                }
            },

            opensignwindow: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
                var me = this;
                signwindow = Ext.create('B4.view.gisGkh.SignWindow');
                signwindow.rec = rec;
                signwindow.signer = new XadesSigner();
                signwindow.signer.getCertificates(2, 'My', me.fillCertificatesCombo, function (e) {
                    Ext.Msg.alert('Ошибка получения списка сертификатов!', 'Не удалось получить сертификаты' + '<br>' + (e.message || e));
                }, me);
                signwindow.show();
            },

            openMassSignWindow: function (btn) {
                var me = this;
                signwindow = Ext.create('B4.view.gisGkh.MassSignWindow');
                signwindow.signer = new XadesSigner();
                signwindow.signer.getCertificates(2, 'My', me.fillCertificatesCombo, function (e) {
                    Ext.Msg.alert('Ошибка получения списка сертификатов!', 'Не удалось получить сертификаты' + '<br>' + (e.message || e));
                }, me);
                signwindow.show();
            },

            fillCertificatesCombo: function (certificates) {
                var me = this,
                    certCombo = signwindow.down('#dfCert'),
                    certComboStore = certCombo.getStore();

                certCombo.clearValue();
                certComboStore.removeAll();
                Ext.each(certificates, function (cert) {
                    var certificateRec = Ext.create('B4.model.gisGkh.Certificate', {
                        SubjectName: cert.subjectName,
                        Certificate: cert,
                        Thumbprint: cert.thumbprint
                    });

                    certComboStore.add(certificateRec);
                });
            },

            // прототип ES6
            //findCert: function () {
            //    return new Promise(function (resolve, reject) {
            //        cadesplugin.async_spawn(function* () {
            //            var MyStoreExists = true;
            //            try {
            //                var oStore = yield cadesplugin.CreateObjectAsync("CAdESCOM.Store");
            //                if (!oStore) {
            //                    alert("Create store failed");
            //                    return;
            //                }

            //                yield oStore.Open(cadesplugin.CAPICOM_CURRENT_USER_STORE,
            //                    cadesplugin.CAPICOM_MY_STORE,
            //                    cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
            //            }
            //            catch (ex) {
            //                MyStoreExists = false;
            //            }
            //            var certCnt;
            //            var certs;
            //            if (MyStoreExists) {
            //                try {
            //                    certs = yield oStore.Certificates;
            //                    certCnt = yield certs.Count;
            //                }
            //                catch (ex) {
            //                    alert("Ошибка при получении Certificates или Count: " + cadesplugin.getLastError(ex));
            //                    return;
            //                }
            //                var certNames = [];
            //                for (var i = 1; i <= certCnt; i++) {
            //                    var cert;
            //                    try {
            //                        cert = yield certs.Item(i);
            //                    }
            //                    catch (ex) {
            //                        alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(ex));
            //                        return;
            //                    }

            //                    try {
            //                        var SubjectName = new String(yield cert.SubjectName);
            //                    }
            //                    catch (ex) {
            //                        alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(ex));
            //                    }
            //                    try {
            //                        var Thumbprint = new String(yield cert.Thumbprint);
            //                    }
            //                    catch (ex) {
            //                        alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(ex));
            //                    }
            //                    certNames.push({ cert: cert, subjectName: SubjectName, thumbprint: Thumbprint });

            //                }
            //                yield oStore.Close();
            //            }

            //            //В версии плагина 2.0.13292+ есть возможность получить сертификаты из 
            //            //закрытых ключей и не установленных в хранилище
            //            try {
            //                yield oStore.Open(cadesplugin.CADESCOM_CONTAINER_STORE);
            //                certs = yield oStore.Certificates;
            //                certCnt = yield certs.Count;
            //                for (var i = 1; i <= certCnt; i++) {
            //                    var cert = yield certs.Item(i);
            //                    //Проверяем не добавляли ли мы такой сертификат уже?
            //                    var found = false;
            //                    for (var j = 0; j < certNames.length; j++) {
            //                        if ((yield certNames[j].Thumbprint) === (yield cert.Thumbprint)) {
            //                            found = true;
            //                            break;
            //                        }
            //                    }
            //                    if (found)
            //                        continue;

            //                    try {
            //                        var SubjectName = new String(yield cert.SubjectName);
            //                    }
            //                    catch (ex) {
            //                        alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(ex));
            //                    }
            //                    try {
            //                        var Thumbprint = new String(yield cert.Thumbprint);
            //                    }
            //                    catch (ex) {
            //                        alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(ex));
            //                    }
            //                    certNames.push({ cert: cert, subjectName: SubjectName, thumbprint: Thumbprint });
            //                }
            //                yield oStore.Close();

            //            }
            //            catch (ex) {
            //            }
            //            resolve(certNames);

            //        }, resolve, reject);
            //    });
            //},

            findCert: function () {
                return new Promise(function (resolve, reject) {
                    cadesplugin.async_spawn(
                        /*#__PURE__*/
                        regeneratorRuntime.mark(function _callee() {
                            var MyStoreExists, oStore, certCnt, certs, certNames, i, cert, SubjectName, Thumbprint, found, j;
                            return regeneratorRuntime.wrap(function _callee$(_context) {
                                while (1) {
                                    switch (_context.prev = _context.next) {
                                        case 0:
                                            MyStoreExists = true;
                                            _context.prev = 1;
                                            _context.next = 4;
                                            return cadesplugin.CreateObjectAsync("CAdESCOM.Store");

                                        case 4:
                                            oStore = _context.sent;

                                            if (oStore) {
                                                _context.next = 8;
                                                break;
                                            }

                                            alert("Create store failed");
                                            return _context.abrupt("return");

                                        case 8:
                                            _context.next = 10;
                                            return oStore.Open(cadesplugin.CAPICOM_CURRENT_USER_STORE, cadesplugin.CAPICOM_MY_STORE, cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                                        case 10:
                                            _context.next = 15;
                                            break;

                                        case 12:
                                            _context.prev = 12;
                                            _context.t0 = _context["catch"](1);
                                            MyStoreExists = false;

                                        case 15:
                                            if (!MyStoreExists) {
                                                _context.next = 70;
                                                break;
                                            }

                                            _context.prev = 16;
                                            _context.next = 19;
                                            return oStore.Certificates;

                                        case 19:
                                            certs = _context.sent;
                                            _context.next = 22;
                                            return certs.Count;

                                        case 22:
                                            certCnt = _context.sent;
                                            _context.next = 29;
                                            break;

                                        case 25:
                                            _context.prev = 25;
                                            _context.t1 = _context["catch"](16);
                                            alert("Ошибка при получении Certificates или Count: " + cadesplugin.getLastError(_context.t1));
                                            return _context.abrupt("return");

                                        case 29:
                                            certNames = [];
                                            i = 1;

                                        case 31:
                                            if (!(i <= certCnt)) {
                                                _context.next = 68;
                                                break;
                                            }

                                            _context.prev = 32;
                                            _context.next = 35;
                                            return certs.Item(i);

                                        case 35:
                                            cert = _context.sent;
                                            _context.next = 42;
                                            break;

                                        case 38:
                                            _context.prev = 38;
                                            _context.t2 = _context["catch"](32);
                                            alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(_context.t2));
                                            return _context.abrupt("return");

                                        case 42:
                                            _context.prev = 42;
                                            _context.t3 = String;
                                            _context.next = 46;
                                            return cert.SubjectName;

                                        case 46:
                                            _context.t4 = _context.sent;
                                            SubjectName = new _context.t3(_context.t4);
                                            _context.next = 53;
                                            break;

                                        case 50:
                                            _context.prev = 50;
                                            _context.t5 = _context["catch"](42);
                                            alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(_context.t5));

                                        case 53:
                                            _context.prev = 53;
                                            _context.t6 = String;
                                            _context.next = 57;
                                            return cert.Thumbprint;

                                        case 57:
                                            _context.t7 = _context.sent;
                                            Thumbprint = new _context.t6(_context.t7);
                                            _context.next = 64;
                                            break;

                                        case 61:
                                            _context.prev = 61;
                                            _context.t8 = _context["catch"](53);
                                            alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(_context.t8));

                                        case 64:
                                            certNames.push({
                                                cert: cert,
                                                subjectName: SubjectName,
                                                thumbprint: Thumbprint
                                            });

                                        case 65:
                                            i++;
                                            _context.next = 31;
                                            break;

                                        case 68:
                                            _context.next = 70;
                                            return oStore.Close();

                                        case 70:
                                            _context.prev = 70;
                                            _context.next = 73;
                                            return oStore.Open(cadesplugin.CADESCOM_CONTAINER_STORE);

                                        case 73:
                                            _context.next = 75;
                                            return oStore.Certificates;

                                        case 75:
                                            certs = _context.sent;
                                            _context.next = 78;
                                            return certs.Count;

                                        case 78:
                                            certCnt = _context.sent;
                                            i = 1;

                                        case 80:
                                            if (!(i <= certCnt)) {
                                                _context.next = 127;
                                                break;
                                            }

                                            _context.next = 83;
                                            return certs.Item(i);

                                        case 83:
                                            cert = _context.sent;
                                            //Проверяем не добавляли ли мы такой сертификат уже?
                                            found = false;
                                            j = 0;

                                        case 86:
                                            if (!(j < certNames.length)) {
                                                _context.next = 99;
                                                break;
                                            }

                                            _context.next = 89;
                                            return certNames[j].Thumbprint;

                                        case 89:
                                            _context.t9 = _context.sent;
                                            _context.next = 92;
                                            return cert.Thumbprint;

                                        case 92:
                                            _context.t10 = _context.sent;

                                            if (!(_context.t9 === _context.t10)) {
                                                _context.next = 96;
                                                break;
                                            }

                                            found = true;
                                            return _context.abrupt("break", 99);

                                        case 96:
                                            j++;
                                            _context.next = 86;
                                            break;

                                        case 99:
                                            if (!found) {
                                                _context.next = 101;
                                                break;
                                            }

                                            return _context.abrupt("continue", 124);

                                        case 101:
                                            _context.prev = 101;
                                            _context.t11 = String;
                                            _context.next = 105;
                                            return cert.SubjectName;

                                        case 105:
                                            _context.t12 = _context.sent;
                                            SubjectName = new _context.t11(_context.t12);
                                            _context.next = 112;
                                            break;

                                        case 109:
                                            _context.prev = 109;
                                            _context.t13 = _context["catch"](101);
                                            alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(_context.t13));

                                        case 112:
                                            _context.prev = 112;
                                            _context.t14 = String;
                                            _context.next = 116;
                                            return cert.Thumbprint;

                                        case 116:
                                            _context.t15 = _context.sent;
                                            Thumbprint = new _context.t14(_context.t15);
                                            _context.next = 123;
                                            break;

                                        case 120:
                                            _context.prev = 120;
                                            _context.t16 = _context["catch"](112);
                                            alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(_context.t16));

                                        case 123:
                                            certNames.push({
                                                cert: cert,
                                                subjectName: SubjectName,
                                                thumbprint: Thumbprint
                                            });

                                        case 124:
                                            i++;
                                            _context.next = 80;
                                            break;

                                        case 127:
                                            _context.next = 129;
                                            return oStore.Close();

                                        case 129:
                                            _context.next = 133;
                                            break;

                                        case 131:
                                            _context.prev = 131;
                                            _context.t17 = _context["catch"](70);

                                        case 133:
                                            resolve(certNames);

                                        case 134:
                                        case "end":
                                            return _context.stop();
                                    }
                                }
                            }, _callee, null, [[1, 12], [16, 25], [32, 38], [42, 50], [53, 61], [70, 131], [101, 109], [112, 120]]);
                        }), resolve, reject);
                });
            },


            getCertificates: function (win) {
                var finder = this.findCert();
                finder.then(function (certNames) {
                    return certNames;
                }).then(function (val) {
                    var field = win.down('#dfCert');
                    var newStore = Ext.create('Ext.data.Store', {
                        fields: ['cert', 'subjectName', 'thumbprint'],
                        data: val
                    });
                    field.bindStore(newStore);
                    field.getStore().load();
                    return val;
                });
            },
            //listeners: {
            //    aftersetformdata: function (asp, record, form) {
            //        // запрещаем выбор начисления для привязки, если платёж уже сквитирован
            //        if (record.data.Reconcile == 10) {
            //            var dfCalculation = form.down('#dfCalculation');
            //            var dfCalculation = form.down('#dfCalculation');
            //            dfCalculation.setDisabled(true);
            //        }
            //    }
            //}
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gisGkhDictGridAspect',
            gridSelector: 'gisgkhdictgrid',
            editFormSelector: '#gisGkhDictWindow',
            storeName: 'gisGkh.DictGridStore',
            modelName: 'gisGkh.DictGridModel',
            editWindowView: 'gisGkh.DictWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {

            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var dictitemgrid = form.down('gisgkhdictitemgrid'),
                        dictitemstore = dictitemgrid.getStore();
                    dictitemstore.filter('NsiListId', record.getId());
                }
            }
        },
    ],

    init: function () {
        var me = this;
        me.control({
            //'gisgkhdictgrid #btnNewRequest': { click: { fn: this.opendict, scope: this } },
            'gisgkhdictgrid #btnGetDictionaries': { click: { fn: this.getDictionaries, scope: this } },
            'gisgkhtaskgrid #btnCheckAnswers': { click: { fn: this.checkAnswers, scope: this } },
            //'gisgkhtaskgrid #btnSignRequests': { click: { fn: this.openMassSignWindow, scope: this } },
            //'gisgkhtaskgrid actioncolumn[action="signRequest"]': { click: { fn: this.opensignwindow, grid: EventSource, scope: this } },
            'gisgkhtaskgrid actioncolumn[action="sendRequest"]': { click: { fn: this.sendRequest, scope: this } },
            'gisGkhSignWindow': { createsignature: { fn: this.onCreateSignature, scope: this } },
            'gisGkhMassSignWindow': { createsignature: { fn: this.onMassCreateSignature, scope: this } },
            'gisgkhrogrid': { selectionchange: { fn: this.onSelectRO, scope: this } },
            'gisgkhroomgrid': { selectionchange: { fn: this.onSelectRoom, scope: this } },
            'gisgkhpremisesgrid': { selectionchange: { fn: this.onSelectPremises, scope: this } },
            //'gisgkhroomgrid': { deselect: { fn: this.onDeSelectRoom, scope: this } },
            //'gisgkhpremisesgrid': { deselect: { fn: this.onDeSelectPremises, scope: this } }
            'roommatchingpanel #btnMatchRoom': { click: { fn: this.onMatchRoom, scope: this } },
            'roommatchingpanel #btnUnMatchRoom': { click: { fn: this.onUnMatchRoom, scope: this } },
            'gisgkhdownloadgrid #btnDownload': { click: { fn: this.onDownload, scope: this } }
        });
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            //view = me.getMainView() || Ext.widget('gisgkhtaskgrid');
            view = me.getMainView() || Ext.widget('gisgkhintegrationpanel');

        me.bindContext(view);
        me.application.deployView(view);
        view.down('gisgkhtaskgrid').getStore().load();
        view.down('gisgkhdictgrid').getStore().load();
        view.down('gisgkhrogrid').getStore().load();
    },

    onMatchRoom: function () {
        var result = B4.Ajax.request(B4.Url.action('MatchRoom', 'GisGkhExecute', {
            roomId: this.roomId,
            premisesId: this.premisesId
        }
        ))
            .next(function (response) {
                //me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
                Ext.ComponentQuery.query('gisgkhroomgrid')[0].getStore().load();
                Ext.ComponentQuery.query('gisgkhpremisesgrid')[0].getStore().load();
                var roGrid = Ext.ComponentQuery.query('gisgkhrogrid')[0];
                var store = roGrid.getStore();
                var rowIndex = store.getById(this.roId).index;
                store.load({
                    callback: function () {
                        roGrid.getView().select(rowIndex);
                    }
                })
                return true;
            }, this).error(function (response) {
                //
                //me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
            });
    },

    onUnMatchRoom: function () {
        var result = B4.Ajax.request(B4.Url.action('UnMatchRoom', 'GisGkhExecute', {
            roomId: this.roomId,
            //premisesId: this.premisesId
        }
        ))
            .next(function (response) {
                //me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
                Ext.ComponentQuery.query('gisgkhroomgrid')[0].getStore().load();
                Ext.ComponentQuery.query('gisgkhpremisesgrid')[0].getStore().load();
                var roGrid = Ext.ComponentQuery.query('gisgkhrogrid')[0];
                var store = roGrid.getStore();
                var rowIndex = store.getById(this.roId).index;
                store.load({
                    callback: function () {
                        roGrid.getView().select(rowIndex);
                    }
                })
                return true;
            }, this).error(function (response) {
                //
                //me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
            });
    },

    onDeSelectPremises: function () {
        this.permisesId = null;
        var btn = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnMatchRoom');
        btn.disabled = true;
    },

    onDeSelectRoom: function () {
        this.roomId = null;
        var btnMatchRoom = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnMatchRoom');
        var btnUnMatchRoom = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnUnMatchRoom');
        btnMatchRoom.disabled = true;
        btnUnMatchRoom.disabled = true;
    },

    onSelectPremises: function (model, records) {
        var btn = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnMatchRoom');
        if (records.length == 1) {
            this.premisesMatched = records[0].get('Matched');
            this.premisesId = records[0].getId();
            if (!this.premisesMatched && !this.roomMatched) {
                //if (this.roomId != null) {
                btn.setDisabled(false);
                // }
            }
            else {
                btn.setDisabled(true);
            }
        }
        else if (records.length == 0) {
            this.premisesId = null;
            this.premisesMatched = true;
            btn.setDisabled(true);
        }
    },

    onSelectRoom: function (model, records) {
        var btnMatchRoom = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnMatchRoom');
        var btnUnMatchRoom = Ext.ComponentQuery.query('roommatchingpanel')[0].down('#btnUnMatchRoom');
        if (records.length == 1) {
            var guid = records[0].get('GisGkhPremisesGUID');
            if (guid == null || guid == "") {
                this.roomMatched = false;
                btnUnMatchRoom.setDisabled(true);
            }
            else {
                btnUnMatchRoom.setDisabled(false);
            }
            this.roomId = records[0].getId();
            if (!this.premisesMatched && !this.roomMatched) {
                btnMatchRoom.setDisabled(false);
            }
            else {
                btnMatchRoom.setDisabled(true);
            }
            //if (this.premisesId != null) {
            //    btn.setDisabled(false);
            //}
        }
        else if (records.length == 0) {
            this.roomId = null;
            this.roomMatched = true;
            btnMatchRoom.setDisabled(true);
            btnUnMatchRoom.setDisabled(true);
        }
    },

    onSelectRO: function (model, records) {
        this.matchedList = null;
        
        matchingPanel = Ext.ComponentQuery.query('roommatchingpanel')[0];
        roomStore = matchingPanel.down('gisgkhroomgrid').getStore();
        roomStore.clearFilter(true);
        premisesStore = matchingPanel.down('gisgkhpremisesgrid').getStore();
        premisesStore.clearFilter(true);
        if (records.length == 1) {
            roomStore.filter('RealityObjectId', records[0].getId());

            premisesStore.filter('RealityObjectId', records[0].getId());
            this.roId = records[0].getId();
        }
        else if (records.length == 0) {
            this.roId = null;
            roomStore.removeAll();
            premisesStore.removeAll();
        }
    },

    onMassCreateSignature: function (win) {
        var me = this;
        certCombo = win.down('#dfCert');
        reqIdsToSign = win.down('#dfRequests').getValue();
        if (!certCombo.value) {
            Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            return;
        }
        //me.fireEvent('signingStart');
        me.mask('Подписание и отправка запроса в ГИС ЖКХ', this.getMainComponent());
        me.signNextRequest(win, reqIdsToSign);
    },

    signNextRequest: function (win, reqIdsToSign, previousSigningResult) {
        
        var me = this;
        if (previousSigningResult && previousSigningResult.success === false) {
            var errorMessage = (previousSigningResult.message || 'Ошибка при подписывании запроса')
                + '<br><br>'
                //+ 'Наименование запроса: ' + previousSigningResult.reqName
                //+ '<br><br>'
                + 'Идентификатор запроса: ' + previousSigningResult.reqId;
            win.close();
            me.unmask();
            Ext.Msg.alert('Ошибка!', errorMessage);
            me.fireEvent('signingComplete');
            return;
        }
        if (!reqIdsToSign || reqIdsToSign.length === 0) {
            //me.fireEvent('signingComplete');
            win.close();
            me.unmask();
            B4.QuickMsg.msg('Сообщение', 'Успешно подписано', 'success');
            //Ext.Msg.alert('Сообщение', data.data);
            me.getMainView().down('gisgkhtaskgrid').getStore().load();
            return;
        }
        
        var nextRecordIdToSign = reqIdsToSign[0];
        reqIdsToSign = reqIdsToSign.slice(1);
        me.signRequest(nextRecordIdToSign, reqIdsToSign, win);
    },

    signRequest: function (requestId, reqIdsToSign, win) {
        
        var me = this,
            signingResult = {
                success: true,
                message: '',
                reqId: requestId
                //reqName: reqName
            };

        me.fireEvent('packageSigning', {
            id: requestId,
            //name: reqName
        });

        //me.mask('Подписание и отправка запроса в ГИС ЖКХ', this.getMainComponent());
        var certField = win.down('#dfCert'),
            val = certField.getValue(),
            cert,
            certBase64,
            sign;
        //if (!certField.value) {
        //    Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
        //    return;
        //}
        B4.Ajax.request({
            url: B4.Url.action('GetXml', 'GisGkhExecute'),
            params: {
                reqId: requestId
            },
            timeout: 9999999
        }).next(function (response) {
            var notSignedData = Ext.decode(response.responseText).data;
            
            //if (!notSignedData || notSignedData.length === 0) {
            //    Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            //}
            if (!notSignedData || notSignedData.length === 0) {
                signingResult.success = false;
                signingResult.message = 'Не получены неподписанные данные с сервера';
                me.signNextRequest(win, reqIdsToSign, signingResult);
            }
            win.signer.setCertificate(val);
            win.signer.signXml(notSignedData, function (xml) {
                
                B4.Ajax.request({
                    url: B4.Url.action('SaveAndSendRequest', 'GisGkhExecute'),
                    params: {
                        reqId: requestId,
                        signedData: encodeURI(xml)
                    },
                    timeout: 9999999
                })
                    .next(function (response) {
                        var data = Ext.decode(response.responseText);
                        me.signNextRequest(win, reqIdsToSign, signingResult);
                        //win.close();
                        //B4.QuickMsg.msg('Сообщение', data.data, 'success');
                        //Ext.Msg.alert('Сообщение', data.data);
                        //me.unmask();
                        //return true;
                    })
                    .error(function (e) {
                        //Ext.Msg.alert('Ошибка', resp.message);
                        //me.unmask();
                        signingResult.success = false;
                        signingResult.message = 'Не удалось сохранить и отправить подписанные запросы' + '<br>' + (e.message || e);
                        me.signNextRequest(win, reqIdsToSign, signingResult);
                    });
            }, function (e) {
                signingResult.success = false;
                signingResult.message = 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e);
                me.signNextRequest(win, reqIdsToSign, signingResult);
                //Ext.Msg.alert('Ошибка!', 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e));
            }, me);
        })
            .error(function (e) {
                signingResult.success = false;
                signingResult.message = 'Не получены неподписанные данные с сервера' + '<br>' + (e.message || e);
                me.signNextRequest(win, reqIdsToSign, signingResult);
                //Ext.Msg.alert('Ошибка', resp.message);
                //me.unmask();
            });
    },

    onCreateSignature: function (win) {
        
        var me = this;
        certCombo = win.down('#dfCert');
        record = win.rec;
        //if (record.get('RequestState') != 10) {
        //    Ext.Msg.alert('Внимание', 'Подписать можно только сформированный неподписанный пакет');
        //    return false;
        //}
        if (!certCombo.value) {
            Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            return false;
        }
        me.mask('Подписание и отправка запроса в ГИС ЖКХ', this.getMainComponent());
        me.signNextRequest(win, [record.getId()]);
        //me.signRequest(record.getId(), [], win)
        //me.unmask();
    },

    sendRequest: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if ((rec.get('RequestState') != 20) && (rec.get('RequestState') != 10)) {
            Ext.Msg.alert('Внимание', 'Отправить можно только сформированный или подписанный пакет');
            return false;
        }
        me.mask('Отправка запроса в ГИС ЖКХ', this.getMainComponent());

        var result = B4.Ajax.request({
            url: B4.Url.action('SendRequest', 'GisGkhExecute'),
            params: {
                reqId: rec.getId()
            },
            timeout: 9999999
        })
            .next(function (response) {
                var data = Ext.decode(response.responseText);
                Ext.Msg.alert('Сообщение', data.data);

                me.getStore().load();
                me.unmask();

                return true;
            })
            .error(function (resp) {
                Ext.Msg.alert('Ошибка', resp.message);
                me.unmask();
            });
        return true;
    },

    getDictionaries: function (record) {
        var me = this;

        if (1 == 2) {
            Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
        }
        else {
            me.mask('Обмен данными с ГИС ЖКХ');
            
            var result = B4.Ajax.request(B4.Url.action('GetDictionaries', 'GisGkhExecute'
            )).next(function (response) {
                var data = Ext.decode(response.responseText);
                Ext.Msg.alert('Сообщение', data.data);

                me.getStore().load();
                me.unmask();

                return true;
            })
                .error(function (resp) {
                    Ext.Msg.alert('Ошибка', resp.message);
                    me.unmask();
                });

        }
    },

    checkAnswers: function (record) {
        var me = this;
        if (1 == 2) {
            Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
        }
        else {
            me.mask('Обмен данными с ГИС ЖКХ');
            var result = B4.Ajax.request(B4.Url.action('CheckAnswers', 'GisGkhExecute'
            )).next(function (response) {
                var data = Ext.decode(response.responseText);
                Ext.Msg.alert('Сообщение', data.data);
                me.unmask();
                return true;
            })
                .error(function (resp) {
                    Ext.Msg.alert('Ошибка', resp.message);
                    me.unmask();
                });

        }
    },

    onDownload: function () {
        var me = this;

        me.mask('Загрузка фалов из ГИС ЖКХ');
        var result = B4.Ajax.request(B4.Url.action('DownloadFiles', 'GisGkhExecute'
        )).next(function (response) {
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);
            me.unmask();
            return true;
        })
            .error(function (resp) {
                Ext.Msg.alert('Ошибка', resp.message);
                me.unmask();
            });

    },

    openrequestseditwindow: function () {
        taskInfo = Ext.create('B4.view.gisGkh.EditWindow');
        taskInfo.show();
    },
});