Ext.define('B4.controller.PlanReductionExpense', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.planreductionexpense.State',
        
        'B4.enums.KindServiceDi'
    ],

    models:
    [
        'PlanReductionExpense',
        'planreductionexpense.Works'
    ],
    stores:
    [
        'PlanReductionExpense',
        'service.BaseServiceSelect',
        'service.BaseServiceSelected',
        'planreductionexpense.Works'
    ],
    
    views:
    [
        'planreductionexpense.Grid',
        'planreductionexpense.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],
    
    editWindowSelector: '#planReductionExpenseEditWindow',
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'planreductionexpense.Grid',
    mainViewSelector: '#planReductionExpenseGrid',

    aspects: [
    {
        xtype: 'planreductionexpensestateperm',
        name: 'planReductionExpensePermissionAspect'
    },
    {
        xtype: 'gkhinlinegridaspect',
        name: 'planReductionExpenseWorksGridAspect',
        storeName: 'planreductionexpense.Works',
        modelName: 'planreductionexpense.Works',
        gridSelector: '#planReductionExpenseWorksGrid',
        saveButtonSelector: '#planReductionExpenseWorksGrid #planReductionExpenseWorksSaveButton',
        listeners: {
            beforesave: function (asp, store) {
                store.each(function (rec) {
                    if (!rec.get('Id')) {
                        rec.set('PlanReductionExpense', asp.controller.params.planReductionExpenseId);
                    }
                });
                return true;
            }
        }
    },
    {
        xtype: 'gkhgridmultiselectwindowaspect',
        name: 'planReductionExpenseAspect',
        gridSelector: '#planReductionExpenseGrid',
        storeName: 'PlanReductionExpense',
        modelName: 'PlanReductionExpense',
        editFormSelector: '#planReductionExpenseEditWindow',
        editWindowView: 'planreductionexpense.EditWindow',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#planReductionExpenseMultiSelectWindow',
        storeSelect: 'service.BaseServiceSelect',
        storeSelected: 'service.BaseServiceSelected',
        titleSelectWindow: 'Выбор услуг',
        titleGridSelect: 'Услуги',
        titleGridSelected: 'Выбранные услуги',
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
            { header: 'Вид', xtype: 'gridcolumn', dataIndex: 'KindServiceDi', flex: 1, renderer: function (val) { return B4.enums.KindServiceDi.displayRenderer(val); } }
        ],
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
        ],
        onBeforeLoad: function (store, operation) {
            if (this.controller.params.disclosureInfoRealityObjId > 0) {
                operation.params.disclosureInfoRealityObjId = this.controller.params.disclosureInfoRealityObjId;
            }
        },

        listeners: {
            getdata: function (asp, records) {
                var recordIds = [];
                records.each(function (rec) {
                    recordIds.push(rec.get('Id'));
                });

                if (recordIds[0] > 0) {
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddBaseService', 'PlanReductionExpense', {
                        objectIds: recordIds,
                        disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
                else {
                    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать услуги');
                    return false;
                }
                return true;
            },
            aftersetformdata: function (asp, record) {
                asp.controller.setCurrentId(record);
            }
        }
    }],
    
    init: function () {
        this.getStore('PlanReductionExpense').on('beforeload', this.onBeforeLoad, this, 'PlanReductionExpense');
        
        this.getStore('planreductionexpense.Works').on('beforeload', this.onBeforeLoad, this, 'PlanReductionExpenseWorks');
        
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('PlanReductionExpense').load();
        
        if (this.params) {
            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('planReductionExpensePermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation, type) {
        if (type == 'PlanReductionExpense') {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
        
        if (type == 'PlanReductionExpenseWorks') {
            operation.params.planReductionExpenseId = this.params.planReductionExpenseId;
        }
    },
    
    setCurrentId: function (rec) {
        var id = this.params.planReductionExpenseId = rec.getId();
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        var storePlanReductionExpenseWorks = this.getStore('planreductionexpense.Works');

        storePlanReductionExpenseWorks.removeAll();
        
        if (id > 0) {
            editWindow.down('#planReductionExpenseWorksGrid').setDisabled(false);
            storePlanReductionExpenseWorks.load();
        } else {
            editWindow.down('#planReductionExpenseWorksGrid').setDisabled(true);
        }
    }
});