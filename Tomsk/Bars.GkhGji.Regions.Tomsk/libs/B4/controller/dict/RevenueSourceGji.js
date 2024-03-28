Ext.define('B4.controller.dict.RevenueSourceGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow', 'B4.aspects.permission.dict.HeatSeasonPeriod'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RevenueSourceGji'],
    stores: ['dict.RevenueSourceGji'],
    views: ['dict.revenuesourcegji.EditWindow', 'dict.revenuesourcegji.Grid'],

    mainView: 'dict.revenuesourcegji.Grid',
    mainViewSelector: 'revenueSourceGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'revenueSourceGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'heatseasonperiodperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'revenueSourceGjiGridWindowAspect',
            gridSelector: 'revenueSourceGjiGrid',
            editFormSelector: '#revenueSourceGjiEditWindow',
            storeName: 'dict.RevenueSourceGji',
            modelName: 'dict.RevenueSourceGji',
            editWindowView: 'dict.revenuesourcegji.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('revenueSourceGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RevenueSourceGji').load();
    }
});