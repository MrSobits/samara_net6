Ext.define('B4.controller.dict.DocumentCode', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.DocumentCode'],
    stores: ['dict.DocumentCode'],

    views: ['dict.documentcode.Grid'],

    mainView: 'dict.documentcode.Grid',
    mainViewSelector: 'documentcodegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'documentcodegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'documentcodegrid',
            permissionPrefix: 'GkhGji.Dict.DocumentCode'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'documentCodeGridInlineAspect',
            storeName: 'dict.DocumentCode',
            modelName: 'dict.DocumentCode',
            gridSelector: 'documentcodegrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('documentcodegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.DocumentCode').load();
    }
});