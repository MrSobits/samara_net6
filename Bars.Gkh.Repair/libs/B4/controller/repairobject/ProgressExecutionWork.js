Ext.define('B4.controller.repairobject.ProgressExecutionWork', {
/*
* Контроллер раздела ход выполнения работы объекта текущего ремонта
*/
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.repairobject.ProgressExecutionWork'
    ],

    models: ['repairobject.RepairWork'],
    stores: ['repairobject.ProgressExecutionWork'],
    views: [
        'repairobject.progressexecution.Grid',
        'repairobject.progressexecution.EditWindow'
    ],

    mainView: 'repairobject.progressexecution.Grid',
    mainViewSelector: '#progressExecutionRepairWorkGrid',
    mixins: { mask: 'B4.mixins.MaskBody' },

    aspects: [
        {
            xtype: 'progressexecutionrepairworkperm',
            name: 'progressExecutionRepairWorkPerm',
            editFormAspectName: 'progressExecutionRepairWorkGridAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'progressExecutionRepairWorkGridAspect',
            storeName: 'repairobject.ProgressExecutionWork',
            modelName: 'repairobject.RepairWork',
            gridSelector: '#progressExecutionRepairWorkGrid',
            editFormSelector: 'progressexecutionworkeditwin',
            editWindowView: 'repairobject.progressexecution.EditWindow',

            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.ObjectCr = this.controller.params.get('Id');
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.workId = record.get('Work').Id;
                }
            }
        }
    ],

    init: function () {
        this.getStore('repairobject.ProgressExecutionWork').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('repairobject.ProgressExecutionWork').load();
        if (this.params) {
            this.getAspect('progressExecutionRepairWorkPerm').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.repairObjectId = this.params.get('Id');
        }
    }
});