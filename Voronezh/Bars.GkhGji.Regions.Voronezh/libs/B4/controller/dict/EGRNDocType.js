Ext.define('B4.controller.dict.EGRNDocType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.EGRNDocType'],
    stores: ['dict.EGRNDocType'],

    views: ['dict.egrnapplicanttype.Grid'],

    mainView: 'dict.egrndoctype.Grid',
    mainViewSelector: 'egrndoctypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'egrndoctypegrid'
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
            name: 'egrndoctypeGridAspect',
            storeName: 'dict.EGRNDocType',
            modelName: 'dict.EGRNDocType',
            gridSelector: 'egrndoctypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('egrndoctypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.EGRNDocType').load();
    }
});