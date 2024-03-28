Ext.define('B4.controller.dict.EGRNObjectType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.EGRNObjectType'],
    stores: ['dict.EGRNObjectType'],

    views: ['dict.egrnobjecttype.Grid'],

    mainView: 'dict.egrnobjecttype.Grid',
    mainViewSelector: 'egrnobjecttypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'egrnobjecttypegrid'
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
            name: 'egrnobjecttypeGridAspect',
            storeName: 'dict.EGRNObjectType',
            modelName: 'dict.EGRNObjectType',
            gridSelector: 'egrnobjecttypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('egrnobjecttypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.EGRNObjectType').load();
    }
});