Ext.define('B4.controller.dict.TypeProject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeProjectGrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeProject'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeProjectGridAspect',
            storeName: 'dict.TypeProject',
            modelName: 'dict.TypeProject',
            gridSelector: 'typeProjectGrid'
        }
    ],

    models: ['dict.TypeProject'],
    stores: ['dict.TypeProject'],
    views: ['dict.typeproject.Grid'],

    mainView: 'dict.typeproject.Grid',
    mainViewSelector: 'typeProjectGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'typeProjectGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeProjectGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeProject').load();
    }
});