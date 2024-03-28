Ext.define('B4.controller.dict.KindProtocolTsj', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KindProtocolTsj'],
    stores: ['dict.KindProtocolTsj'],

    views: ['dict.kindprotocoltsj.Grid'],

    mainView: 'dict.kindprotocoltsj.Grid',
    mainViewSelector: 'kindProtocolTsjGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindProtocolTsjGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindProtocolTsjGrid',
            permissionPrefix: 'GkhGji.Dict.KindProtocolTsj'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindProtocolTsjGridAspect',
            storeName: 'dict.KindProtocolTsj',
            modelName: 'dict.KindProtocolTsj',
            gridSelector: 'kindProtocolTsjGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindProtocolTsjGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindProtocolTsj').load();
    }
});