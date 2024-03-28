Ext.define('B4.controller.MKDLicRequest', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    models: ['mkdlicrequest.MKDLicRequest',
        'mkdlicrequest.MKDLicRequestFile',
        'mkdlicrequest.MKDLicRequestRealityObject',
        'dict.Inspector',
        'mkdlicrequest.MKDLicRequestQueryAnswer',
        'mkdlicrequest.MKDLicRequestQuery'
    ],
    stores: ['mkdlicrequest.MKDLicRequest',
        'mkdlicrequest.MKDLicRequestFile',
        'mkdlicrequest.MKDLicRequestRealityObject',
        'mkdlicrequest.MKDLicRequestQuery',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'mkdlicrequest.MKDLicRequestQueryAnswer'
    ],
    views: [
        'mkdlicrequest.EditWindow',
        'mkdlicrequest.Grid',
        'mkdlicrequest.FileGrid',
        'mkdlicrequest.FileEditWindow',
        'mkdlicrequest.RealityObjectGrid',
        'mkdlicrequest.MKDLicRequestQueryGrid',
        'mkdlicrequest.MKDLicRequestQueryAnswerGrid',
        'mkdlicrequest.MKDLicRequestQueryEditWindow',
        'mkdlicrequest.MKDLicRequestQueryAnswerEditWindow'
    ],
    mainView: 'mkdlicrequest.Grid',
    mainViewSelector: 'mkdLicRequestGrid',
    globalAppeal: null,
    courtId: 0,
    refs: [
        {
            ref: 'mainView',
            selector: 'mkdLicRequestGrid'
        },
        {
            ref: 'mkdlicrequestEditWindow',
            selector: 'mkdlicrequesteditwindow'
        }
    ],
    mkdlicrequest :null,
    mkdlicrequestId: null,
    mkdlicrequestQueryId: null,

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'mkdlicrequestqueryanswergrid',
            controllerName: 'MKDLicRequestQueryAnswer',
            name: 'MKDLicRequestQueryFileSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'mkdLicrequestquerygrid',
            controllerName: 'MKDLicRequestQuery',
            name: 'MKDLicRequestQueryAnswerFileSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'mkdlicrequestfilegrid',
            controllerName: 'MKDLicRequestFile',
            name: 'mkdLicRequestFileSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'mkdlicrequestButtonExportAspect',
            gridSelector: 'mkdLicRequestGrid',
            buttonSelector: 'mkdLicRequestGrid #btnExport',
            controllerName: 'GjiScript',
            actionName: 'MKDLicRequestExport'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'mkdlicrequestGridStateTransferAspect',
            gridSelector: 'mkdLicRequestGrid',
            stateType: 'mkdlicrequest',
            menuSelector: 'mkdlicrequestGridStateMenu'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'mkdlicrequestStateButtonAspect',
            stateButtonSelector: '#mkdlicrequestEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var model = this.controller.getModel('mkdlicrequest.MKDLicRequest');
                    model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('mkdlicrequestGridAspect').setFormData(rec);
                        },
                        scope: this
                    })


                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestGridAspect',
            gridSelector: 'mkdLicRequestGrid',
            editFormSelector: '#mkdlicrequestEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequest',
            modelName: 'mkdlicrequest.MKDLicRequest',
            editWindowView: 'mkdlicrequest.EditWindow',
            otherActions: function (actions) {
                              
                actions['mkdLicRequestGrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['mkdLicRequestGrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#mkdlicrequestEditWindow #cbObjection'] = { 'change': { fn: this.onChangecbObjection, scope: this } };   
                actions['mkdLicRequestGrid #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#mkdlicrequestEditWindow #btnCourtPractice'] = { 'click': { fn: this.goToCourtPractice, scope: this } };
                //actions['#courtpracticeEditWindow #cbInLaw'] = { 'change': { fn: this.onChangecbInLaw, scope: this } };    
                //actions['#courtpracticeEditWindow #cbCourtCosts'] = { 'change': { fn: this.onChangecbCourtCosts, scope: this } };    
                //actions['#courtpracticeEditWindow #cbInterimMeasures'] = { 'change': { fn: this.onChangecbcbInterimMeasures, scope: this } }; 
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                //
            },
            goToCourtPractice: function () {
                debugger;
                var me = this,
                    portal = me.controller.getController('PortalController'),
                    controllerEditName,
                    params = {};
                controllerEditName = 'B4.controller.CourtPractice';
                params.recId = me.controller.courtId;
                if (controllerEditName) {
                    portal.loadController(controllerEditName, params);
                }
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            onChangeRealityObject: function (field, newValue) {
                this.controller.params.realityObjectId = newValue && newValue.Id;
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            onChangecbObjection: function (field, newValue) {
                var form = this.getForm(),
                    cbObjectionResult = form.down('#cbObjectionResult');
                if (newValue == true) {
                    cbObjectionResult.setDisabled(false);
                }
                else {
                    cbObjectionResult.setDisabled(true);
                }
            },
            onChangeCheckbox: function (field, newValue) {
                var pasams = this.controller.params;
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    mkdlicrequestId = record.getId();
                    asp.controller.mkdlicrequestId = record.getId();
                    var btnGoTo = form.down('#btnCourtPractice');
                    //пытаемся проставить прочитано для инспектора и/или руководителя управления
                    if (mkdlicrequestId != 0) {
                        asp.controller.getAspect('mkdlicrequestStateButtonAspect').setStateData(mkdlicrequestId, record.get('State'));
                        var filegrid = form.down('mkdlicrequestfilegrid'),
                            fieldInspectors = form.down('#trigFInspectors'),
                            filestore = filegrid.getStore();
                        filegrid.setDisabled(false);
                        filestore.filter('mkdlicrequestId', mkdlicrequestId);
                        var rogrid = form.down('mkdlicrequestrogrid'),
                            rostore = rogrid.getStore();
                        rogrid.setDisabled(false);
                        rostore.filter('mkdlicrequestId', mkdlicrequestId);
                        var regquery = form.down('mkdLicrequestquerygrid'),
                            regqueryStor = regquery.getStore();
                        regquery.setDisabled(false);
                        regqueryStor.filter('MkdLicRequest', mkdlicrequestId);
                        fieldInspectors.setDisabled(false);
    
                    }
                    else
                    {       
                        btnGoTo.setDisabled(true);
                        var rogrid = form.down('mkdlicrequestrogrid'),
                            fieldInspectors = form.down('#trigFInspectors'),
                            rostore = rogrid.getStore();
                        rostore.clearData();
                        fieldInspectors.setDisabled(true);
                    }
                    if (record.get('Id')) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('GetInfo', 'MKDLicRequestInspector', { documentId: record.get('Id') }),
                            //для IE, чтобы не кэшировал GET запрос
                            cache: false
                        }).next(function (response) {
                            asp.controller.unmask();
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText),
                                fieldInspectors = form.down('#trigFInspectors');
                            fieldInspectors.updateDisplayedText(obj.inspectorNames);
                            fieldInspectors.setValue(obj.inspectorIds);
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    //получаем того кто на нас ссылается
                    if (record.get('Id')) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetDocInfo', 'CourtPracticeOperations', {
                            docId: record.get('Id'),
                            typeEntity: 'MKDLicRequest'
                        })).next(function (response) {
                            asp.controller.unmask();
                            debugger;
                            //десериализуем полученную строку             
                            var data = Ext.decode(response.responseText);
                        
                            if (data.data.courtId > 0) {

                                btnGoTo.setDisabled(false);
                            }
                            else {
                                btnGoTo.setDisabled(true);
                            }
                            asp.controller.courtId = data.data.courtId;
                        }).error(function () {
                            asp.controller.unmask();
                        });
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
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'mkdlicrequestMultiSelectWindowAspect',
            fieldSelector: '#mkdlicrequestEditWindow #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdlicrequestInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'MKDLicRequestInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.mkdlicrequestId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'mkdlicrequestrogridAspect',
            gridSelector: 'mkdlicrequestrogrid',
            storeName: 'mkdlicrequest.MKDLicRequestRealityObject',
            modelName: 'mkdlicrequest.MKDLicRequestRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdlicrequestroMultiSelectWindow',
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
                operation.params.mkdlicrequestId = this.controller.mkdlicrequestId;
            },

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddStatementRealityObjects', 'AppealCitsRealObject', {
                            objectIds: recordIds,
                            mkdlicrequestId: asp.controller.mkdlicrequestId
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
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestFileGridAspect',
            gridSelector: 'mkdlicrequestfilegrid',
            editFormSelector: '#mkdlicrequestFileEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequestFile',
            modelName: 'mkdlicrequest.MKDLicRequestFile',
            editWindowView: 'mkdlicrequest.FileEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        debugger;
                        record.set('MKDLicRequest', mkdlicrequestId);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestqueryGridWindowAspect',
            gridSelector: 'mkdLicrequestquerygrid',
            editFormSelector: '#mkdLicRequestQueryEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequestQuery',
            modelName: 'mkdlicrequest.MKDLicRequestQuery',
            editWindowView: 'mkdlicrequest.MKDLicRequestQueryEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    debugger;
                    if (!record.get('Id')) {
                        record.set('MKDLicRequest', mkdlicrequestId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    mkdlicrequestQueryId = record.getId();
                    var grid = form.down('mkdlicrequestqueryanswergrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.MkdLicRequest = mkdlicrequestQueryId;
                        },
                        me);
                    store.load();

                }
            }
        }, 
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestqueryAnswerGridWindowAspect',
            gridSelector: 'mkdlicrequestqueryanswergrid',
            editFormSelector: '#mkdLicRequestQueryAnswerEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequestQueryAnswer',
            modelName: 'mkdlicrequest.MKDLicRequestQueryAnswer',
            editWindowView: 'mkdlicrequest.MKDLicRequestQueryAnswerEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MKDLicRequestQuery', mkdlicrequestQueryId);
                    }
                }
            }
        },     
        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('mkdLicRequestGrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.realityObjectId = view.down('#sfRealityObject').getValue();
        this.params.showCloseAppeals = view.down('#cbShowCloseAppeals').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('mkdlicrequest.MKDLicRequest').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('mkdlicrequest.MKDLicRequest').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var grid = this.getMainView();        
        if (this.params && this.params.recId > 0) {
            var model = this.getModel('mkdlicrequest.MKDLicRequest');
            this.getAspect('mkdlicrequestGridAspect').editRecord(new model({ Id: this.params.recId }));
            this.params.recId = 0;
        }
    },

    onBeforeLoadDoc: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
            operation.params.dateStart = this.params.dateStart;
            operation.params.realityObjectId = this.params.realityObjectId;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});