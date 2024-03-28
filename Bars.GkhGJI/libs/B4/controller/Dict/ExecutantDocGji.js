Ext.define('B4.controller.dict.ExecutantDocGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ExecutantDocGji'],
    stores: ['dict.ExecutantDocGji'],

    views: ['dict.executantdocgji.Grid'],

    mainView: 'dict.executantdocgji.Grid',
    mainViewSelector: 'executantDocGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'executantDocGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'executantDocGjiGrid',
            permissionPrefix: 'GkhGji.Dict.ExecutantDoc'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'executantDocGjiGridAspect',
            storeName: 'dict.ExecutantDocGji',
            modelName: 'dict.ExecutantDocGji',
            gridSelector: 'executantDocGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('executantDocGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ExecutantDocGji').load();
    }
});