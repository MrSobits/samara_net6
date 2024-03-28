Ext.define('B4.controller.dict.Period', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Period'
    ],

    models: ['dict.Period'],
    stores: ['dict.Period'],
    views: ['dict.period.Grid', 'dict.period.EditWindow'],

    mainView: 'dict.period.Grid',
    mainViewSelector: 'periodGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'periodGrid'
        }
    ],

    aspects: [
        {
            xtype: 'perioddictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'periodGridWindowAspect',
            gridSelector: 'periodGrid',
            editFormSelector: '#periodEditWindow',
            storeName: 'dict.Period',
            modelName: 'dict.Period',
            editWindowView: 'dict.period.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('periodGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Period').load();
    }
});