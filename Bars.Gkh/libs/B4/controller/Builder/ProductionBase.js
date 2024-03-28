Ext.define('B4.controller.builder.ProductionBase', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['builder.ProductionBase'],
    stores: ['builder.ProductionBase'],
    views: [
        'builder.ProductionBaseEditWindow',
        'builder.ProductionBaseGrid'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'builderProductionBaseGridWindowAspect',
            gridSelector: '#builderProductionBaseGrid',
            editFormSelector: '#builderProductionBaseEditWindow',
            storeName: 'builder.ProductionBase',
            modelName: 'builder.ProductionBase',
            editWindowView: 'builder.ProductionBaseEditWindow',
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
    mainView: 'builder.ProductionBaseGrid',
    mainViewSelector: '#builderProductionBaseGrid',

    init: function () {
        this.getStore('builder.ProductionBase').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.ProductionBase').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});