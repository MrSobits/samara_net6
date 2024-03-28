Ext.define('B4.controller.dict.Work', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.Work'],
    stores: ['dict.Work'],
    views: [
        'dict.work.EditWindow',
        'dict.work.Grid'
    ],
    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'workGrid',
            permissionPrefix: 'Gkh.Dictionaries.Work'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'workGridWindowAspect',
            gridSelector: 'workGrid',
            editFormSelector: '#workEditWindow',
            storeName: 'dict.Work',
            modelName: 'dict.Work',
            editWindowView: 'dict.work.EditWindow'
        }
    ],

    mainView: 'dict.work.Grid',
    mainViewSelector: 'workGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'workGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('workGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Work').load();
    }
});