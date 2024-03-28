Ext.define('B4.controller.dict.KindStatementGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KindStatementGji'],
    stores: ['dict.KindStatementGji'],

    views: ['dict.kindstatementgji.Grid'],

    mainView: 'dict.kindstatementgji.Grid',
    mainViewSelector: 'kindStatementGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindStatementGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindStatementGjiGrid',
            permissionPrefix: 'GkhGji.Dict.KindStatement'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindStatementGjiGridAspect',
            storeName: 'dict.KindStatementGji',
            modelName: 'dict.KindStatementGji',
            gridSelector: 'kindStatementGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindStatementGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindStatementGji').load();
    }
});