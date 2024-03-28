Ext.define('B4.controller.dict.TypeCourtGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.TypeCourtGji'],
    stores: ['dict.TypeCourtGji'],

    views: ['dict.typecourtgji.Grid'],

    mainView: 'dict.typecourtgji.Grid',
    mainViewSelector: 'typeCourtGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typeCourtGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeCourtGjiGrid',
            permissionPrefix: 'GkhGji.Dict.TypeCourt'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeCourtGjiGridAspect',
            storeName: 'dict.TypeCourtGji',
            modelName: 'dict.TypeCourtGji',
            gridSelector: 'typeCourtGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeCourtGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeCourtGji').load();
    }
});