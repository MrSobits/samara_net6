Ext.define('B4.controller.dict.FLDocType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.FLDocType'],
    stores: ['dict.FLDocType'],

    views: ['dict.fldoctype.Grid'],

    mainView: 'dict.fldoctype.Grid',
    mainViewSelector: 'fldoctypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'fldoctypegrid'
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
            name: 'fldoctypeGridAspect',
            storeName: 'dict.FLDocType',
            modelName: 'dict.FLDocType',
            gridSelector: 'fldoctypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('fldoctypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.FLDocType').load();
    }
});