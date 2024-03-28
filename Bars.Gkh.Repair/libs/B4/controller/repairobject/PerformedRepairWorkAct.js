Ext.define('B4.controller.repairobject.PerformedRepairWorkAct', {
/*
* Контроллер раздела акты выполненных работ
*/
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['repairobject.RepairWork'],
    stores: ['repairobject.PerformedRepairWorkAct'],
    views: [
        'repairobject.performedrepairworkact.Grid',
        'repairobject.performedrepairworkact.EditWindow'
    ],

    mainView: 'repairobject.performedrepairworkact.Grid',
    mainViewSelector: '#performedRepairWorkActGrid',
    mixins: { mask: 'B4.mixins.MaskBody' },

    aspects: [
         {
             xtype: 'gkhpermissionaspect',
             permissions: [
                { name: 'GkhRepair.RepairObject.PerformedRepairWorkAct.Edit', applyTo: 'b4addbutton', selector: '#performedRepairWorkActGrid' }
             ]
         },
        {
            xtype: 'grideditwindowaspect',
            name: 'performedRepairWorkActGridAspect',
            storeName: 'repairobject.PerformedRepairWorkAct',
            modelName: 'repairobject.PerformedRepairWorkAct',
            gridSelector: '#performedRepairWorkActGrid',
            editFormSelector: '#performedRepairWorkActEditWindow',
            editWindowView: 'repairobject.performedrepairworkact.EditWindow',

            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.ObjectAddress = this.controller.params.get('Address');
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var store = form.down('b4selectfield').getStore();
                    store.clearFilter(true);
                    store.on('beforeload', function (store, operation) {
                        operation.params = operation.params || {};
                        operation.params.repairObjectId = asp.controller.params.get('Id');
                    });
                }
            }
        }
    ],

    init: function () {
        this.getStore('repairobject.PerformedRepairWorkAct').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('repairobject.PerformedRepairWorkAct').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.repairObjectId = this.params.get('Id');
        }
    }
});