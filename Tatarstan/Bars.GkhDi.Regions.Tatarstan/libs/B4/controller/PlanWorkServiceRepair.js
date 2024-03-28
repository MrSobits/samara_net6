Ext.define('B4.controller.PlanWorkServiceRepair', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.planworkservicerepair.State',
        'B4.enums.KindServiceDi'
    ],

    models: [
        'PlanWorkServiceRepair',
        'planworkservicerepair.Works',
        'service.WorkRepairTechServ'
    ],
    stores: [
        'PlanWorkServiceRepair',
        'service.ServiceSelect',
        'service.ServiceSelected',
        'planworkservicerepair.Works',
        'service.WorkRepairTechService'
    ],

    views: [
        'planworkservicerepair.Grid',
        'planworkservicerepair.EditWindow',
        'planworkservicerepair.WorksEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    editWindowSelector: '#planWorkServiceRepairEditWindow',
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'planworkservicerepair.Grid',
    mainViewSelector: '#planWorkServiceRepairGrid',

    aspects: [
        {
            xtype: 'planworkservicerepairstateperm',
            name: 'planWorkServiceRepairPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            editFormSelector: '#planWorkServiceRepairWorksEditWindow',
            name: 'planWorkServiceRepairWorksGridAspect',
            storeName: 'planworkservicerepair.Works',
            modelName: 'planworkservicerepair.Works',
            gridSelector: '#planWorkServiceRepairWorksGrid',
            editWindowView: 'planworkservicerepair.WorksEditWindow',
            otherActions: function(actions) {
                actions[this.gridSelector + ' #planWorkServiceRepairWorksReloadButton'] = { 'click': { fn: this.reloadButtonClick, scope: this } };
            },
            reloadButtonClick: function() {
                var asp = this;
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('ReloadWorkRepairList', 'PlanWorkServiceRepair', {
                    planWorkServiceRepairId: asp.controller.params.planWorkServiceRepairId
                })).next(function() {
                    Ext.Msg.alert('Успех!', 'Работы успешно перезаполнены');
                    asp.updateGrid();
                    asp.controller.unmask();
                }).error(function() {
                    Ext.Msg.alert('Ошибка!', 'Не удалось перезаполнить работы');
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'planWorkServiceRepairAspect',
            gridSelector: '#planWorkServiceRepairGrid',
            storeName: 'PlanWorkServiceRepair',
            modelName: 'PlanWorkServiceRepair',
            editFormSelector: '#planWorkServiceRepairEditWindow',
            editWindowView: 'planworkservicerepair.EditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#planWorkServiceRepairMultiSelectWindow',
            storeSelect: 'service.ServiceSelect',
            storeSelected: 'service.ServiceSelected',
            titleSelectWindow: 'Выбор услуг',
            titleGridSelect: 'Услуги',
            titleGridSelected: 'Выбранные услуги',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Вид', xtype: 'gridcolumn', dataIndex: 'KindServiceDi', flex: 1, renderer: function(val) { return B4.enums.KindServiceDi.displayRenderer(val); } },
                { header: 'Поставщик', xtype: 'gridcolumn', dataIndex: 'ProviderName', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            onBeforeLoad: function(store, operation) {
                operation.params.kindTemplateService = 30;
                operation.params.isTemplateService = true;
                operation.params.disclosureInfoRealityObjId = this.controller.params.disclosureInfoRealityObjId;
            },

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];
                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddTemplateService', 'PlanWorkServiceRepair'),
                            method: 'POST',
                            params: {
                                objectIds: recordIds,
                                disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                            }
                        }).next(function() {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать услуги');
                        return false;
                    }
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    asp.controller.setCurrentId(record);
                }
            }
        }
    ],

    init: function() {
        this.getStore('PlanWorkServiceRepair').on('beforeload', this.onBeforeLoad, this, 'PlanWorkServiceRepair');
        this.getStore('planworkservicerepair.Works').on('beforeload', this.onBeforeLoad, this, 'PlanWorkServiceRepairWorks');
        this.getStore('service.WorkRepairTechService').on('beforeload', this.onWorkRepairTechServBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('PlanWorkServiceRepair').load();
        if (this.params) {
            var me = this;
            me.params.getId = function() { return me.params.disclosureInfoId; };
            this.getAspect('planWorkServiceRepairPermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function(store, operation, type) {
        if (type == 'PlanWorkServiceRepair') {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }

        if (type == 'PlanWorkServiceRepairWorks') {
            operation.params.planWorkServiceRepairId = this.params.planWorkServiceRepairId;
        }
    },
    
    onWorkRepairTechServBeforeLoad: function (store, operation) {
        operation.params = {};
        operation.params.baseServiceId = this.params.baseServiceId;
        operation.params.showOnlyGroups = true;
    },

    setCurrentId: function(rec) {
        var id = this.params.planWorkServiceRepairId = rec.getId();
        this.params.baseServiceId = rec.get('BaseService').Id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        var storePlanWorkServiceRepairWorks = this.getStore('planworkservicerepair.Works');
        var storeWorkRepairTechServ = this.getStore('service.WorkRepairTechService');

        storePlanWorkServiceRepairWorks.removeAll();
        storeWorkRepairTechServ.removeAll();

        if (id > 0) {
            editWindow.down('#planWorkServiceRepairWorksGrid').setDisabled(false);
            editWindow.down('#planWorkServiceRepairServicesGrid').setDisabled(false);
            storePlanWorkServiceRepairWorks.load();
            storeWorkRepairTechServ.load();
        } else {
            editWindow.down('#planWorkServiceRepairWorksGrid').setDisabled(true);
            editWindow.down('#planWorkServiceRepairServicesGrid').setDisabled(true);
        }
    }
});