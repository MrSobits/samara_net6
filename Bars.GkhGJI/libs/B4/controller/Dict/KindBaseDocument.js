Ext.define('B4.controller.dict.KindBaseDocument', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['KindBaseDocument'],
    stores: ['dict.KindBaseDocument'],

    views: ['dict.kindbasedocument.Grid'],

    mainView: 'dict.kindbasedocument.Grid',
    mainViewSelector: 'kindbasedocumentgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindbasedocumentgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindbasedocumentgrid',
            permissionPrefix: 'GkhGji.Dict.KindBaseDocument'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.KindBaseDocument',
            modelName: 'KindBaseDocument',
            gridSelector: 'kindbasedocumentgrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('kindbasedocumentgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});