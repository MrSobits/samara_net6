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
        'B4.aspects.StateContextMenu',
        'B4.enums.CourtPracticeState'
    ],

    models: ['courtpractice.CourtPractice',
        'courtpractice.CourtPracticeFile',
        'DocumentGji',
        'courtpractice.CourtPracticeInspector',
        'courtpractice.CourtPracticeRealityObject'
    ],
    stores: ['courtpractice.CourtPractice',
        'courtpractice.CourtPracticeFile',
        'DocumentGji',
        'courtpractice.CourtPracticeInspector',
        'courtpractice.CourtPracticeRealityObject'
    ],
    views: [
        'courtpractice.EditWindow',
        'courtpractice.Grid',
        'courtpractice.InspectorGrid',
        'courtpractice.InspectorEditWindow',
        'courtpractice.RealityObjectGrid',
        'courtpractice.FileGrid',
        'courtpractice.FileEditWindow'
    ],
    mainView: 'courtpractice.Grid',
    mainViewSelector: 'courtpracticeGrid',
    globalAppeal: null,
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
                actions['#courtpracticeEditWindow #btnPrescr'] = { 'click': { fn: this.goToDocument, scope: this } };
                actions['#courtpracticeEditWindow #btnLicRequest'] = { 'click': { fn: this.goToLicRequest, scope: this } };
                actions['#courtpracticeEditWindow #cbCourtPracticeState'] = { 'change': { fn: this.onChangecbCourtPracticeState, scope: this } }; 
                actions['#courtpracticeEditWindow #sfTypeFactViolation'] = { 'change': { fn: this.onChangesfTypeFactViolation, scope: this } }; 
                //
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            goToLicRequest: function () {
                debugger;
                var me = this,
                    portal = me.controller.getController('PortalController'),
                    controllerEditName,
                    record = me.getForm().getRecord(),
                    params = {};
                controllerEditName = 'B4.controller.MKDLicRequest';
                params.recId = record.get('MKDLicRequest');
                if (controllerEditName) {
                    portal.loadController(controllerEditName, params);
                }
            },
            goToDocument: function (btn) {
                var me = this,
                    portal = me.controller.getController('PortalController'),
                    controllerEditName,
                    params = {},
                    model,
                    record = me.getForm().getRecord(),
                    documentGji,
                    inspection,
                    defaultParams;
                debugger;
                documentGji = record.get('DocumentGji');
                inspectionId = record.get('InspId');
                model = me.controller.getModel('InspectionGji');
                var typeBase = record.get('TypeBase');
                var typeDocumentGji = record.get('TypeDocumentGji');
                controllerEditName = me.getControllerName(typeBase);
                params = new model({ Id: inspectionId });

                // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                if (documentGji) {
                    defaultParams = me.getDefaultParams(typeDocumentGji);
                    params.defaultController = defaultParams.controllerName;
                    params.defaultParams = {
                        inspectionId: inspectionId,
                        documentId: documentGji,
                        title: defaultParams.docName
                    };
                }
                else if (inspection) {
                    // Если нет документа но ест ьпроверка то тогда открываем просто проверку
                    inspection = record.get('InspectionGji');
                    model = me.getModel('InspectionGji');

                    controllerEditName = me.getControllerName(typeBase);
                    params = new model({ Id: inspection.Id });
                }

                if (controllerEditName) {
                    portal.loadController(controllerEditName, params);
                }

            },

            getControllerName: function (typeBase) {
                switch (typeBase) {
                    //Инспекционная проверка
                    case 10:
                        return 'B4.controller.baseinscheck.Navigation';
                    //Обращение граждан                  
                    case 20:
                        return 'B4.controller.basestatement.Navigation';
                    //Плановая проверка юр лиц                
                    case 30:
                        return 'B4.controller.basejurperson.Navigation';
                    //Распоряжение руководства               
                    case 40:
                        return 'B4.controller.basedisphead.Navigation';
                    //Требование прокуратуры                 
                    case 50:
                        return 'B4.controller.baseprosclaim.Navigation';
                    //Постановление прокуратуры                  
                    case 60:
                        return 'B4.controller.resolpros.Navigation';
                    //Проверка деятельности ТСЖ                   
                    case 70:
                        return 'B4.controller.baseactivitytsj.Navigation';
                    //Отопительный сезон                    
                    case 80:
                        return 'B4.controller.baseheatseason.Navigation';
                    //Административное дело
                    case 90:
                        return '';
                    //Протокол МВД
                    case 100:
                        return 'B4.controller.protocolmvd.Navigation';
                    //Проверка по плану мероприятий
                    case 110:
                        return 'B4.controller.baseplanaction.Navigation';
                    //Протокол МЖК
                    case 120:
                        return 'B4.controller.protocolmhc.Navigation';
                    case 140:
                        return 'B4.controller.protocol197.Navigation';
                    //Без основания                     
                    case 150:
                        return 'B4.controller.basedefault.Navigation';
                }

                return '';
            },

            getDefaultParams: function (typeDocument) {
                var result = {};
                debugger;
                switch (typeDocument) {
                    //Распоряжение
                    case 10:
                        {
                            result.controllerName = 'B4.controller.Disposal';
                            result.docName = B4.DisposalTextValues.getSubjectiveCase();
                        }
                        break;
                    //Предписание
                    case 50:
                        {
                            result.controllerName = 'B4.controller.Prescription';
                            result.docName = 'Предписание';
                        }
                        break;
                     //Постановление
                    case 70:
                        {
                            result.controllerName = 'B4.controller.Resolution';
                            result.docName = 'Постановление';
                        }
                        break;

                    //Постановление
                    case 140:
                        {
                            result.controllerName = 'B4.controller.Resolution';
                            result.docName = 'Постановление';
                        }
                        break;
                }

                return result;
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
                    sfDocumentGji = form.down('#sfDocumentGji'),
                    sfMKDLicRequest = form.down('#sfMKDLicRequest');
                if (newValue.Code == '01' || newValue.Code == '04' || newValue.Code == '05') {
                    sfDocumentGji.show();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
                }
                else if (newValue.Code == '11')
                {
                    sfMKDLicRequest.show();
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                }
                else {
                    sfDocumentGji.setValue(null);
                    sfDocumentGji.hide();
                    sfMKDLicRequest.setValue(null);
                    sfMKDLicRequest.hide();
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
                    cbDisputeInstance = form.down('#cbDisputeInstance');
                if (newValue == true) {
                    cbDisputeInstance.show();
                }
                else {
                    cbDisputeInstance.setValue(null);
                    cbDisputeInstance.hide();
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    courtpracticeId = record.getId();
                    //кнопка перехода
                    if (record.get('DocumentGji')) {
                        var btnPres = form.down('#btnPrescr');
                        btnPres.setDisabled(false);
                    }
                    else {
                        var btnPres = form.down('#btnPrescr');
                        btnPres.setDisabled(true);
                    }
                    //кнопка перехода 2
                    if (record.get('MKDLicRequest')) {
                        var btnLicRequest = form.down('#btnLicRequest');
                        btnLicRequest.setDisabled(false);
                    }
                    else {
                        var btnLicRequest = form.down('#btnLicRequest');
                        btnLicRequest.setDisabled(true);
                    }
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
                        //var answgrid = form.down('incomingdocumentanswergrid'),
                        //    answstore = answgrid.getStore();
                        //answstore.filter('docId', incomingdocumentId);
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
        debugger;
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
    }

    
});