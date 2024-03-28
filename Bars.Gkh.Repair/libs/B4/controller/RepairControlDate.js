Ext.define('B4.controller.RepairControlDate', {
    extend: 'B4.base.Controller',
    requires:
    [
       'B4.aspects.GridEditWindow',
       'B4.aspects.GkhGridMultiSelectWindow',
       'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['dict.RepairProgram', 'RepairControlDate'],
    stores: ['dict.RepairProgram', 'RepairControlDate',
         'dict.WorkKindCurrentRepairForSelect', 'dict.WorkKindCurrentRepairForSelected'],
    
    views: ['repaircontroldate.Grid',
            'repaircontroldate.EditWindow',
             'repaircontroldate.WorkGrid',
             'repaircontroldate.WorkEditWindow',
             'SelectWindow.MultiSelectWindow'],

    mainView: 'repaircontroldate.Grid',
    mainViewSelector: 'repairControlDateGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'repairControlDateGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRepair.RepairControlDate.Create', applyTo: 'b4addbutton', selector: '#repairControlDateWorkGrid' },
                { name: 'GkhRepair.RepairControlDate.Edit', applyTo: 'b4savebutton', selector: '#repairControlDateWorkEditWindow' },
                {
                    name: 'GkhRepair.RepairControlDate.Delete', applyTo: 'b4deletecolumn', selector: '#repairControlDateWorkGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'GkhRepair.RepairControlDate.Field.Date', applyTo: '#dfDate', selector: '#repairControlDateWorkEditWindow' }
                
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'repairControlDateGridWindowAspect',
            gridSelector: 'repairControlDateGrid',
            editFormSelector: '#repairControlDateEditWindow',
            storeName: 'dict.RepairProgram',
            modelName: 'dict.RepairProgram',
            editWindowView: 'repaircontroldate.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.repairPogramId = record.getId();
                    asp.controller.getStore('RepairControlDate').load();
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'repairControlDateWorkAspect',
            gridSelector: 'repaircontroldateworkgrid',
            storeName: 'RepairControlDate',
            modelName: 'RepairControlDate',
            editFormSelector: '#repairControlDateWorkEditWindow',
            editWindowView: 'repaircontroldate.WorkEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#repairControlDateWorkMultiSelectWindow',
            storeSelect: 'dict.WorkKindCurrentRepairForSelect',
            storeSelected: 'dict.WorkKindCurrentRepairForSelected',
            titleSelectWindow: 'Выбор видов работ',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    if (recordIds[0] > 0) {
                        B4.Ajax.request(B4.Url.action('AddWorks', 'RepairControlDate', {
                            objectIds: recordIds,
                            repairPogramId: asp.controller.repairPogramId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды работ');
                        return false;
                    }
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.controlDateId = record.getId();
                }
            }
        }
    ],

    init: function () {
        this.getStore('RepairControlDate').on('beforeload', this.onBeforeLoad, this, 'Rec');
        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.repairPogramId = this.repairPogramId;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('repairControlDateGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RepairProgram').load();
    }
});