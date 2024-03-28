Ext.define('B4.controller.builder.SroInfo', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['builder.SroInfo'],
    stores: ['builder.SroInfo'],
    views: [
        'builder.SroInfoEditWindow',
        'builder.SroInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'builderSroInfoGridWindowAspect',
            gridSelector: '#builderSroInfoGrid',
            editFormSelector: '#builderSroInfoEditWindow',
            storeName: 'builder.SroInfo',
            modelName: 'builder.SroInfo',
            editWindowView: 'builder.SroInfoEditWindow',
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
    mainView: 'builder.SroInfoGrid',
    mainViewSelector: '#builderSroInfoGrid',

    init: function () {
        this.getStore('builder.SroInfo').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.SroInfo').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});