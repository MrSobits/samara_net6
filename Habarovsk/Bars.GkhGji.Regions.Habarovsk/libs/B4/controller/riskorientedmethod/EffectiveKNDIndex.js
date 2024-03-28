Ext.define('B4.controller.riskorientedmethod.EffectiveKNDIndex', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['riskorientedmethod.EffectiveKNDIndex'],
    stores: ['riskorientedmethod.EffectiveKNDIndex'],

    views: ['riskorientedmethod.EffectiveKNDIndexGrid'],

    mainView: 'riskorientedmethod.EffectiveKNDIndexGrid',
    mainViewSelector: 'effectivekndindexgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'effectivekndindexgrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'inlinegridpermissionaspect',
        //    gridSelector: 'regionCodeGrid',
        //    permissionPrefix: 'GkhGji.Dict.RegionCode'
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'effectiveKNDIndexAspect',
            storeName: 'riskorientedmethod.EffectiveKNDIndex',
            modelName: 'riskorientedmethod.EffectiveKNDIndex',
            gridSelector: 'effectivekndindexgrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('effectivekndindexgrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('riskorientedmethod.EffectiveKNDIndex').load();
    }
});