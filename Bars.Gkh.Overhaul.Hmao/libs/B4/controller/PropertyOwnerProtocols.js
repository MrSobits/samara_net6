Ext.define('B4.controller.PropertyOwnerProtocols', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.ButtonDataExport',
    ],

    appealOrderId: null,
  
    models: [
        'PropertyOwnerProtocols',
        'PropertyOwnerProtocolsAnnex',
        'dict.Inspector',
        'PropertyOwnerProtocolsDecision'
    ],
    stores: [
        'PropertyOwnerProtocols',
        'dict.InspectorForSelect',
        'PropertyOwnerProtocolsAnnex',
        'dict.InspectorForSelected',
        'PropertyOwnerProtocolsDecision'

    ],
    views: [
        'protocolmkd.Grid',
        'protocolmkd.EditWindow',
        'protocolmkd.DecisionGrid',
        'protocolmkd.DecisionEditWindow',
        'longtermprobject.propertyownerprotocols.FileGrid',
        'longtermprobject.propertyownerprotocols.FileEditWindow'
    ],
    aspects: [

        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocolmkdgridButtonExportAspect',
            gridSelector: 'protocolmkdgrid',
            buttonSelector: 'protocolmkdgrid #btnExport',
            controllerName: 'PropertyOwnerProtocolsOperations',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'protocolmkdgridAspect',
            gridSelector: 'protocolmkdgrid',
            editFormSelector: '#protocolmkdEditWindow',
            storeName: 'PropertyOwnerProtocols',
            modelName: 'PropertyOwnerProtocols',
            editWindowView: 'protocolmkd.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },   
            otherActions: function (actions) {             
                actions['protocolmkdgrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['protocolmkdgrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['protocolmkdgrid #sfEmployer'] = { 'change': { fn: this.onChangesfEmployer, scope: this } };
                actions['protocolmkdgrid #dfDateRegStart'] = { 'change': { fn: this.onChangeDateRegStart, scope: this } };
                actions['protocolmkdgrid #dfDateRegEnd'] = { 'change': { fn: this.onChangeDateRegEnd, scope: this } };
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
            },
            onChangeDateRegStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateRegStart = newValue;
                }
            },
            onChangeDateRegEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateRegEnd = newValue;
                }
            },
            onChangesfEmployer: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    if (newValue) {
                        this.controller.params.sfEmployer = newValue.Id;
                    }
                   else {
                        this.controller.params.sfEmployer = 0;
                    }
                }
            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    var roaddress = record.get('RealityObject').Address;
                    var title = 'Протокол ОСС в МКД по адресу ' + roaddress;
                    form.setTitle(title);
                    var grid = form.down('protocolmkddecisiongrid'),
                        store = grid.getStore();
                    grid.setDisabled(false);
                    store.filter('protocolId', record.getId());
                    store.load();
                    if (record.get('Id')) {
                        var filerid = form.down('propertyownerprotocolsfilegrid'),
                            filestore = filerid.getStore();
                        filestore.on('beforeload',
                            function (store, operation) {
                                operation.params.protocolId = record.get('Id');
                            },
                            me);
                        filestore.load(); 
                        asp.controller.protocolId = record.get('Id');
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('GetInfo', 'PropertyOwnerProtocolInspector', { documentId: record.get('Id') }),
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
                }
              
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'ownerprotocolsfilegridAspect',
            gridSelector: 'propertyownerprotocolsfilegrid',
            editFormSelector: '#propertyownerprotocolsFileEditWindow',
            storeName: 'PropertyOwnerProtocolsAnnex',
            modelName: 'PropertyOwnerProtocolsAnnex',
            editWindowView: 'longtermprobject.propertyownerprotocols.FileEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Protocol', asp.controller.protocolId);
                    }
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
            name: 'propertyownerprotocolsMultiSelectWindowAspect',
            fieldSelector: '#protocolmkdEditWindow #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolmkdInspectorSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'PropertyOwnerProtocolInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.protocolId
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
            xtype: 'grideditwindowaspect',
            name: 'protocolmkdDecisionGridAspect',
            gridSelector: 'protocolmkddecisiongrid',
            editFormSelector: '#protocolmkddecisionEditWindow',
            storeName: 'PropertyOwnerProtocolsDecision',
            modelName: 'PropertyOwnerProtocolsDecision',
            editWindowView: 'protocolmkd.DecisionEditWindow'
        }      
    ],

    mainView: 'protocolmkd.Grid',
    mainViewSelector: 'protocolmkdgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolmkdgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('PropertyOwnerProtocols').on('beforeload', this.onBeforeLoadDoc, this);
        this.getStore('PropertyOwnerProtocols').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },


    onBeforeLoad: function (store, operation) {
        operation.params.appealOrderId = this.appealOrderId;
    },

    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.dateRegStart = this.params.dateRegStart;
            operation.params.dateRegEnd = this.params.dateRegEnd;
            operation.params.sfEmployer = this.params.sfEmployer;
        }
    },

    index: function () {
        this.params = {};
        var view = this.getMainView() || Ext.widget('protocolmkdgrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.dateRegStart = view.down('#dfDateRegStart').getValue();
        this.params.dateRegEnd = view.down('#dfDateRegEnd').getValue();
        this.params.sfEmployer = view.down('#sfEmployer').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('PropertyOwnerProtocols').load();
    }

});