Ext.define('B4.controller.dict.PhysicalPersonDocType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.PhysicalPersonDocType'],
    stores: ['dict.PhysicalPersonDocType'],

    views: ['dict.physicalpersondoctype.Grid'],

    mainView: 'dict.physicalpersondoctype.Grid',
    mainViewSelector: 'physicalpersondoctypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'physicalpersondoctypegrid'
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
            name: 'physicalpersondoctypeGridAspect',
            storeName: 'dict.PhysicalPersonDocType',
            modelName: 'dict.PhysicalPersonDocType',
            gridSelector: 'physicalpersondoctypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('physicalpersondoctypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.PhysicalPersonDocType').load();
    }
});