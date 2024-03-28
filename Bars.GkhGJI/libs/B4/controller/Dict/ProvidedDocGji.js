Ext.define('B4.controller.dict.ProvidedDocGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ProvidedDocGji'],
    stores: ['dict.ProvidedDocGji'],

    views: ['dict.provideddocgji.Grid'],

    mainView: 'dict.provideddocgji.Grid',
    mainViewSelector: 'providedDocGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'providedDocGjiGrid'
        }
    ],

    aspects: [ 
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'providedDocGjiGrid',
            permissionPrefix: 'GkhGji.Dict.ProvidedDoc'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'providedDocGjiGridAspect',
            storeName: 'dict.ProvidedDocGji',
            modelName: 'dict.ProvidedDocGji',
            gridSelector: 'providedDocGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('providedDocGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProvidedDocGji').load();
    }
});