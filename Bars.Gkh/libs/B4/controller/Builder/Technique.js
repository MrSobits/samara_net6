Ext.define('B4.controller.builder.Technique', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['builder.Technique'],
    stores: ['builder.Technique'],
    views: [
        'builder.TechniqueEditWindow',
        'builder.TechniqueGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'builderTechniqueGridWindowAspect',
            gridSelector: '#builderTechniqueGrid',
            editFormSelector: '#builderTechniqueEditWindow',
            storeName: 'builder.Technique',
            modelName: 'builder.Technique',
            editWindowView: 'builder.TechniqueEditWindow',
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
    mainView: 'builder.TechniqueGrid',
    mainViewSelector: '#builderTechniqueGrid',

    init: function () {
        this.getStore('builder.Technique').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.Technique').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});