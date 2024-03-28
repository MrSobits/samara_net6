Ext.define('B4.controller.realityobj.CurentRepair', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.permission.realityobj.CurentRepair'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.CurentRepair'
    ],

    stores: [
        'realityobj.CurentRepair',
        'dict.WorkKindCurrentRepairForSelect',
        'dict.WorkKindCurrentRepairForSelected'
    ],

    views: [
        'realityobj.CurentRepairGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjcurentrepairgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'curentrepairperm',
            name: 'curentRepairPerm'
        },
        {
            //Аспект взаимодействия таблицы конструктивных элементов по дому с формой массового выбора конструктивных элементов
            //При добавлении открывается форма массового выбора конструктивных элементов. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе

            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'realityObjCurrentReapirAspect',
            gridSelector: 'realityobjcurentrepairgrid',
            storeName: 'realityobj.CurentRepair',
            modelName: 'realityobj.CurentRepair',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityObjCurrentRepairMultiSelectWindow',
            storeSelect: 'dict.WorkKindCurrentRepairForSelect',
            storeSelected: 'dict.WorkKindCurrentRepairForSelected',
            titleSelectWindow: 'Выбор видов работ',
            titleGridSelect: 'Виды работ для отбора',
            titleGridSelected: 'Выбранные виды работ',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (me, records) {
                    var store = me.controller.getStore(me.storeName);

                    records.each(function (rec) {
                        if (rec.get('Id')) {
                            var recordWorkKind = me.controller.getModel('realityobj.CurentRepair').create();
                            recordWorkKind.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                            recordWorkKind.set('WorkKind', rec.get('Id'));
                            recordWorkKind.set('WorkKindName', rec.get('Name'));

                            store.insert(0, recordWorkKind);
                        }
                    });
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('realityobj.CurentRepair').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjcurentrepairgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.CurentRepair').load();
        me.getAspect('curentRepairPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});