Ext.define('B4.controller.dict.GroupWorkTo', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    models: ['dict.GroupWorkTo'],
    stores: ['dict.GroupWorkTo'],
    views: ['dict.groupworkto.Grid'],

    mainView: 'dict.groupworkto.Grid',
    mainViewSelector: 'groupWorkToGrid',

    refs: [{
        ref: 'mainView',
        selector: 'groupWorkToGrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'groupWorkToGrid',
            permissionPrefix: 'GkhDi.Dict.GroupWorkTo'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'groupWorkToGridAspect',
            storeName: 'dict.GroupWorkTo',
            modelName: 'dict.GroupWorkTo',
            gridSelector: 'groupWorkToGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('groupWorkToGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.GroupWorkTo').load();
    }
});