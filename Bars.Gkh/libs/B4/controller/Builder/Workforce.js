Ext.define('B4.controller.builder.Workforce', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhGridPermissionAspect',
        'B4.aspects.permission.GkhEditPermissionAspect'
    ],

    models: ['builder.Workforce'],
    stores: ['builder.Workforce'],
    views: [
        'builder.WorkforceEditWindow',
        'builder.WorkforceGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'builderWorkforceGridWindowAspect',
            gridSelector: '#builderWorkforceGrid',
            editFormSelector: '#builderWorkforceEditWindow',
            storeName: 'builder.Workforce',
            modelName: 'builder.Workforce',
            editWindowView: 'builder.WorkforceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.Builder = this.controller.params.get('Id');
                    }
                }
            }
        }
    ],

    params: null,
    mainView: 'builder.WorkforceGrid',
    mainViewSelector: '#builderWorkforceGrid',

    init: function () {
        this.getStore('builder.Workforce').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.Workforce').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});