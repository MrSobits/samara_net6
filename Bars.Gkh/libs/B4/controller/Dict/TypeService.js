Ext.define('B4.controller.dict.TypeService', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeServiceGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeService'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeServiceGridAspect',
            storeName: 'dict.TypeService',
            modelName: 'dict.TypeService',
            gridSelector: 'typeServiceGrid'
        }
    ],

    models: ['dict.TypeService'],
    stores: ['dict.TypeService'],
    views: ['dict.typeservice.Grid'],

    mainView: 'dict.typeservice.Grid',
    mainViewSelector: 'typeServiceGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeServiceGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeServiceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeService').load();
    }
});