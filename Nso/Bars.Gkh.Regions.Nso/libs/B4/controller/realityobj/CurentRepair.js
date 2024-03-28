Ext.define('B4.controller.realityobj.CurentRepair', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.realityobj.CurentRepair'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
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
        'SelectWindow.MultiSelectWindow',
        'realityobj.CurentRepairEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjcurentrepairgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'curentRepairPerm',
            permissions: [
                { name: 'Gkh.RealityObject.Register.CurentRepair.Create', applyTo: 'b4addbutton', selector: 'realityobjcurentrepairgrid' },
                { name: 'Gkh.RealityObject.Register.CurentRepair.Edit', applyTo: 'b4savebutton', selector: 'realityobjcurrrepaireditwin' },
                { name: 'Gkh.RealityObject.Register.CurentRepair.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjcurentrepairgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
            },
            {
                xtype: 'gkhgridmultiselectwindowaspect',
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
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                    { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                ],
                columnsGridSelected: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                    { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false }
                ],
                editFormSelector: 'realityobjcurrrepaireditwin',
                editWindowView: 'realityobj.CurentRepairEditWindow',
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
                        
                        me.controller.mask('Сохранение', me.getGrid());
                        store.sync({
                            callback: function () {
                                me.controller.unmask();
                                store.load();
                            },

                            failure: function (result) {
                                me.controller.unmask();
                                if (result && result.exceptions[0] && result.exceptions[0].response) {
                                    Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                                }
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