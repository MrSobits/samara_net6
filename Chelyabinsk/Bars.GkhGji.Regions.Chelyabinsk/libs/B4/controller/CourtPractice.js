Ext.define('B4.controller.CourtPractice', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.store.realityobj.RealityObjectForSelect',
        'B4.store.realityobj.RealityObjectForSelected',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.enums.CourtPracticeState'
    ],

    models: ['courtpractice.CourtPractice',
        'courtpractice.CourtPracticeFile',
        'courtpractice.CourtPracticeInspector',
        'B4.model.DocumentGji',
        'courtpractice.DisputeHistory',
        'courtpractice.CourtPracticeRealityObject'
    ],
    stores: ['courtpractice.CourtPractice',
        'courtpractice.CourtPracticeFile',
        'courtpractice.CourtPracticeInspector',
        'courtpractice.CourtPracticeRealityObject',
        'B4.store.courtpractice.DocsForSelect',
        'courtpractice.DisputeHistory',
        'resolution.AppealDecision',
        'B4.store.courtpractice.CourtPracticePrescription'
    ],
    views: [
        'courtpractice.EditWindow',
        'courtpractice.Grid',
        'courtpractice.DisputeHistoryGrid',
        'courtpractice.DisputeHistoryEditWindow',
        'courtpractice.InspectorGrid',
        'courtpractice.InspectorEditWindow',
        'courtpractice.RealityObjectGrid',
        'courtpractice.FileGrid',
        'courtpractice.FileEditWindow'
    ],
    mainView: 'courtpractice.Grid',
    mainViewSelector: 'courtpracticeGrid',
    globalAppeal: null,
    courtpracticeGlobId:null,
    refs: [
        {
            ref: 'mainView',
            selector: 'courtpracticeGrid'
        },
        {
            ref: 'courtpracticeEditWindow',
            selector: 'courtpracticeeditwindow'
        }
    ],
    courtpracticeId: null,

    aspects: [
        //{
        //    xtype: 'gkhbuttonprintaspect',
        //    name: 'incomingDocumentPrintAspect',
        //    buttonSelector: '#incomingdocumentEditWindow #btnPrint',
        //    codeForm: 'IncomingDocument',
        //    getUserParams: function () {
        //        var param = { Id: incomingdocumentId };
        //        this.params.userParams = Ext.JSON.encode(param);
        //    }
        //},
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'courtpracticeGridStateTransferAspect',
            gridSelector: 'courtpracticeGrid',
            stateType: 'courtpractice',
            menuSelector: 'courtpracticeGridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'courtpracticeButtonExportAspect',
            gridSelector: 'courtpracticeGrid',
            buttonSelector: 'courtpracticeGrid #btnExport',
            controllerName: 'CourtPracticeOperations',
            actionName: 'Export'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'courtpracticeStateButtonAspect',
            stateButtonSelector: '#courtpracticeEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var model = this.controller.getModel('courtpractice.CourtPractice');
                    model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('courtpracticeGridAspect').setFormData(rec);
                        },
                        scope: this
                    })


                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'courtpracticeGridAspect',
            gridSelector: 'courtpracticeGrid',
            editFormSelector: '#courtpracticeEditWindow',
            storeName: 'courtpractice.CourtPractice',
            modelName: 'courtpractice.CourtPractice',
            editWindowView: 'courtpractice.EditWindow',
            otherActions: function (actions) {
                              
                actions['courtpracticeGrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['courtpracticeGrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#courtpracticeEditWindow #cbInLaw'] = { 'change': { fn: this.onChangecbInLaw, scope: this } };    
                actions['#courtpracticeEditWindow #cbCourtCosts'] = { 'change': { fn: this.onChangecbCourtCosts, scope: this } };    
                actions['#courtpracticeEditWindow #cbInterimMeasures'] = { 'change': { fn: this.onChangecbcbInterimMeasures, scope: this } }; 
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                actions['#courtpracticeEditWindow #cbDispute'] = { 'change': { fn: this.onChangecbcbDispute, scope: this } }; 
                actions['#courtpracticeEditWindow #cbCourtPracticeState'] = { 'change': { fn: this.onChangecbCourtPracticeState, scope: this } }; 
                actions['#courtpracticeEditWindow #sfTypeFactViolation'] = { 'change': { fn: this.onChangesfTypeFactViolation, scope: this } }; 
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            onChangecbInLaw: function (field, newValue) {
                var form = this.getForm(),
                cbInLawDate = form.down('#cbInLawDate');
                if (newValue == true)
                {
                    cbInLawDate.show();                 
                }
                else
                {
                    cbInLawDate.hide();
                }
            },
            onChangesfTypeFactViolation: function (field, newValue) {

                var form = this.getForm(),
                    sfDocumentGji = form.down('#sfDocumentGji');
                var sfMKDLicRequest = form.down('#sfMKDLicRequest');
                var sfAppealCitsDecision = form.down('#sfAppealCitsDecision');
                var sfAppealCitsDecision2 = form.down('#sfAppealCitsDecision2');
                var sfAppealCitsDefinition = form.down('#sfAppealCitsDefinition');
                var sfResolutionDefinition = form.down('#sfResolutionDefinition');
                admonition = form.down('#admon');

                if (newValue.Code == '01' || newValue.Code == '04' || newValue.Code == '05') {
                    sfDocumentGji.show();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    admonition.setValue(null);
                    admonition.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else if (newValue.Code == '11') {
                    sfMKDLicRequest.show();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    admonition.setValue(null);
                    admonition.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else if (newValue.Code == '17') {
                    admonition.show();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else if (newValue.Code == '03') {
                    sfAppealCitsDecision.show();
                    admonition.setValue(null);
                    admonition.hide();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else if (newValue.Code == '031') {
                    sfAppealCitsDecision2.show();
                    admonition.setValue(null);
                    admonition.hide();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else if (newValue.Code == '032') {
                    sfAppealCitsDefinition.show();
                    admonition.setValue(null);
                    admonition.hide();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
                else {
                    admonition.setValue(null);
                    admonition.hide();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    sfAppealCitsDecision.setValue(null);
                    sfAppealCitsDecision.hide();
                    sfAppealCitsDecision2.setValue(null);
                    sfAppealCitsDecision2.hide();
                    sfAppealCitsDefinition.setValue(null);
                    sfAppealCitsDefinition.hide();
                    sfResolutionDefinition.setValue(null);
                    sfResolutionDefinition.hide();
                }
            },
            onChangeCheckbox: function (field, newValue) {
                var pasams = this.controller.params;
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('courtpractice.CourtPractice').load();
            },
            onChangecbCourtPracticeState: function (field, newValue) {
                var form = this.getForm(),
                    tfPausedComment = form.down('#tfPausedComment');
                if (newValue == B4.enums.CourtPracticeState.Paused) {
                    tfPausedComment.show();
                }
                else
                {
                    tfPausedComment.hide();
                    tfPausedComment.setValue(null);
                }
            },
            onChangecbCourtCosts: function (field, newValue) {
                var form = this.getForm(),
                    nfCourtCostsPlan = form.down('#nfCourtCostsPlan'),
                    nfCourtCostsFact = form.down('#nfCourtCostsFact');
                if (newValue == true) {
                    nfCourtCostsPlan.show();
                    nfCourtCostsFact.show();
                }
                else {
                    nfCourtCostsPlan.hide();
                    nfCourtCostsFact.hide();
                }
            },
            onChangecbcbInterimMeasures: function (field, newValue) {
                var form = this.getForm(),
                    cbInterimMeasuresDate = form.down('#cbInterimMeasuresDate');
                if (newValue == true) {
                    cbInterimMeasuresDate.show();
                }
                else {
                    cbInterimMeasuresDate.hide();
                }
            },
            onChangecbcbDispute: function (field, newValue) {
                
                var form = this.getForm(),
                    cbDisputeInstance = form.down('#cbDisputeInstance'),
                    cbDisputeResult = form.down('#cbDisputeResult'),
                    cbSaveHistory = form.down('#cbSaveHistory');
                if (newValue == true) {
                    cbDisputeInstance.show();
                    cbDisputeResult.show();
                    cbSaveHistory.show();
                }
                else {
                    cbDisputeInstance.setValue(null);
                    cbDisputeInstance.hide();
                    cbDisputeResult.hide();
                    cbSaveHistory.hide();
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    courtpracticeId = record.getId();
                    asp.controller.courtpracticeGlobId = record.getId();
                    var fieldDocs = form.down('#sfDocumentGji');
                    //пытаемся проставить прочитано для инспектора и/или руководителя управления
                    if (courtpracticeId != 0) {
                        asp.controller.getAspect('courtpracticeStateButtonAspect').setStateData(courtpracticeId, record.get('State'));
                        var rogrid = form.down('courtpracticeRealObjGrid'),
                            rostore = rogrid.getStore();
                        rogrid.setDisabled(false);
                        rostore.filter('courtpracticeId', courtpracticeId);
                        var inspgrid = form.down('courtpracticeinspectorgrid'),
                            inspstore = inspgrid.getStore();
                        inspgrid.setDisabled(false);
                        inspstore.filter('courtpracticeId', courtpracticeId);
                        var filegrid = form.down('courtpracticefilegrid'),
                            filestore = filegrid.getStore();
                        filegrid.setDisabled(false);
                        filestore.filter('courtpracticeId', courtpracticeId);

                        var histgrid = form.down('courtpracticeDisputeHistoryGrid'),
                            histstore = histgrid.getStore();
                        histgrid.setDisabled(false);
                        histstore.filter('courtpracticeId', courtpracticeId);

                        //var answgrid = form.down('incomingdocumentanswergrid'),
                        //    answstore = answgrid.getStore();
                        //answstore.filter('docId', incomingdocumentId);

                       
                    }    
                    if (courtpracticeId > 0) {

                        asp.controller.mask('Загрузка', form);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('GetInfo', 'CourtPracticeOperations'),
                            params: {
                                courtpracticeId: courtpracticeId
                            }
                        }).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            fieldDocs.updateDisplayedText(obj.docNames);
                            fieldDocs.setValue(obj.docIds);                        

                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        fieldDocs.updateDisplayedText(null);
                        fieldDocs.setValue(null);                       
                    }

                }
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'courtpracticeHistoryGridAspect',
            gridSelector: 'courtpracticeDisputeHistoryGrid',
            editFormSelector: '#cpDisputeHistoryEditwindow',
            storeName: 'courtpractice.DisputeHistory',
            modelName: 'courtpractice.DisputeHistory',
            editWindowView: 'courtpractice.DisputeHistoryEditWindow',
            otherActions: function (actions) {                
                actions['#cpDisputeHistoryEditwindow #cbInLaw'] = { 'change': { fn: this.onChangecbInLaw, scope: this } };
                actions['#cpDisputeHistoryEditwindow #cbCourtCosts'] = { 'change': { fn: this.onChangecbCourtCosts, scope: this } };
                actions['#cpDisputeHistoryEditwindow #cbInterimMeasures'] = { 'change': { fn: this.onChangecbcbInterimMeasures, scope: this } };
                actions['#cpDisputeHistoryEditwindow #cbDispute'] = { 'change': { fn: this.onChangecbcbDispute, scope: this } };
                actions['#cpDisputeHistoryEditwindow #cbCourtPracticeState'] = { 'change': { fn: this.onChangecbCourtPracticeState, scope: this } };
                actions['#cpDisputeHistoryEditwindow #sfTypeFactViolation'] = { 'change': { fn: this.onChangesfTypeFactViolation, scope: this } };
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            onChangecbInLaw: function (field, newValue) {
                var form = this.getForm(),
                    cbInLawDate = form.down('#cbInLawDate');
                if (newValue == true) {
                    cbInLawDate.show();
                }
                else {
                    cbInLawDate.hide();
                }
            },
            onChangesfTypeFactViolation: function (field, newValue) {
                
                var form = this.getForm(),
                    sfDocumentGji = form.down('#sfDocumentGji');
                  var sfMKDLicRequest = form.down('#sfMKDLicRequest');
                admonition = form.down('#admon');
                
                if (newValue.Code == '01' || newValue.Code == '04' || newValue.Code == '05') {
                    sfDocumentGji.show();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                    admonition.setValue(null);
                    admonition.hide();
                }
                else if (newValue.Code == '11') {
                    sfMKDLicRequest.show();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    admonition.setValue(null);
                    admonition.hide();
                }
                else if (newValue.Code == '17') {
                    admonition.show();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                }
                else {
                    admonition.setValue(null);
                    admonition.hide();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                }
            },
            onChangecbCourtPracticeState: function (field, newValue) {
                var form = this.getForm(),
                    tfPausedComment = form.down('#tfPausedComment');
                if (newValue == B4.enums.CourtPracticeState.Paused) {
                    tfPausedComment.show();
                }
                else {
                    tfPausedComment.hide();
                    tfPausedComment.setValue(null);
                }
            },
            onChangecbCourtCosts: function (field, newValue) {
                var form = this.getForm(),
                    nfCourtCostsPlan = form.down('#nfCourtCostsPlan'),
                    nfCourtCostsFact = form.down('#nfCourtCostsFact');
                if (newValue == true) {
                    nfCourtCostsPlan.show();
                    nfCourtCostsFact.show();
                }
                else {
                    nfCourtCostsPlan.hide();
                    nfCourtCostsFact.hide();
                }
            },
            onChangecbcbInterimMeasures: function (field, newValue) {
                var form = this.getForm(),
                    cbInterimMeasuresDate = form.down('#cbInterimMeasuresDate');
                if (newValue == true) {
                    cbInterimMeasuresDate.show();
                }
                else {
                    cbInterimMeasuresDate.hide();
                }
            },
            onChangecbcbDispute: function (field, newValue) {
                
                var form = this.getForm(),
                    cbDisputeInstance = form.down('#cbDisputeInstance'),
                    cbDisputeResult = form.down('#cbDisputeResult');
                if (newValue == true) {
                    cbDisputeInstance.show();
                    cbDisputeResult.show();
                }
                else {
                    cbDisputeInstance.setValue(null);
                    cbDisputeInstance.hide();
                    cbDisputeResult.hide();
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    
                  
                    var fieldDocs = form.down('#sfDocumentGji');
                    //пытаемся проставить прочитано для инспектора и/или руководителя управления
                   
                    if (asp.controller.courtpracticeGlobId > 0) {

                        asp.controller.mask('Загрузка', form);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('GetInfo', 'CourtPracticeOperations'),
                            params: {
                                courtpracticeId: asp.controller.courtpracticeGlobId
                            }
                        }).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            fieldDocs.updateDisplayedText(obj.docNames);
                            fieldDocs.setValue(obj.docIds);

                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        fieldDocs.updateDisplayedText(null);
                        fieldDocs.setValue(null);
                    }

                }
            }
        },
        {
            /*
           аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
           по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
           По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
           и сохраняем инспекторов через серверный метод /Operator/AddInspectors
           */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'courtpracticePrescriptionMultiSelectWindowAspect',
            fieldSelector: '#courtpracticeEditWindow #sfDocumentGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#courtpracticePrescriptionSelectWindow',
            storeSelect: 'courtpractice.DocsForSelect',
            storeSelected: 'courtpractice.CourtPracticePrescription',
            textProperty: 'DocumentNumber',
            columnsGridSelect: [
                {
                    text: 'Тип документа', dataIndex: 'TypeDocumentGji', flex: 1,
                    renderer: function (val) {
                        return B4.enums.TypeDocumentGji.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeDocumentGji.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    text: 'Номер документа', dataIndex: 'DocumentNumber', flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата документа', dataIndex: 'DocumentDate', flex: 0.5, filter: { xtype: 'datefield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор документов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['courtpracticeId'] = courtpracticeId;
            } ,
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddDocs', 'CourtPracticeOperations'),
                        params: {
                            objectIds: Ext.encode(recordIds),
                            courtpracticeId: courtpracticeId
                        }
                    }).next(function () {
                        Ext.Msg.alert('Сохранение!', 'Документы сохранены успешно');
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'courtpracticeInspectorGridAspect',
            gridSelector: 'courtpracticeinspectorgrid',
            editFormSelector: '#courtpracticeInspectorEditWindow',
            storeName: 'courtpractice.CourtPracticeInspector',
            modelName: 'courtpractice.CourtPracticeInspector',
            editWindowView: 'courtpractice.InspectorEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('CourtPractice', courtpracticeId);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'courtpracticeFileGridAspect',
            gridSelector: 'courtpracticefilegrid',
            editFormSelector: '#courtpracticeFileEditWindow',
            storeName: 'courtpractice.CourtPracticeFile',
            modelName: 'courtpractice.CourtPracticeFile',
            editWindowView: 'courtpractice.FileEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('CourtPractice', courtpracticeId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'courtpracticeRealityObjectAspect',
            gridSelector: 'courtpracticeRealObjGrid',
            storeName: 'courtpractice.CourtPracticeRealityObject',
            modelName: 'courtpractice.CourtPracticeRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#courtpracticeRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.courtpracticeId = this.controller.courtpracticeId;
            },

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddCourtPracticeRealityObjects', 'CourtPracticeOperations', {
                            objectIds: recordIds,
                            courtpracticeId: courtpracticeId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        
    ],

    mainView: 'courtpractice.Grid',
    mainViewSelector: 'courtpracticeGrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('courtpracticeGrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.showCloseAppeals = view.down('#cbShowCloseAppeals').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('courtpractice.CourtPractice').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('courtpractice.CourtPractice').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var grid = this.getMainView();        
        if (this.params && this.params.recId > 0) {
            var model = this.getModel('courtpractice.CourtPractice');
            this.getAspect('courtpracticeGridAspect').editRecord(new model({ Id: this.params.recId }));
            this.params.recId = 0;
        }
    },

    onBeforeLoadDoc: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});