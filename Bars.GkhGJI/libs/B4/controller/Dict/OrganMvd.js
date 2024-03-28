Ext.define('B4.controller.dict.OrganMvd', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.OrganMvd'],
    stores: ['dict.OrganMvd'],
    views: ['dict.OrganMvdGrid'],

    mainView: 'dict.OrganMvdGrid',
    mainViewSelector: 'organmvdgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'organmvdgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
         {
             xtype: 'inlinegridpermissionaspect',
             gridSelector: 'organmvdgrid',
             permissionPrefix: 'GkhGji.Dict.OrganMvd'
         },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'organMvdGridAspect',
            storeName: 'dict.OrganMvd',
            modelName: 'dict.OrganMvd',
            gridSelector: 'organmvdgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('organmvdgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.OrganMvd').load();
    }
});