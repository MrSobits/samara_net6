Ext.define('B4.controller.dict.StopReason', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.StopReason'],
    stores: ['dict.StopReason'],
    views: ['dict.stopreason.Grid'],
    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'stopReasonGrid',
            permissionPrefix: 'Gkh.Dictionaries.StopReason'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'stopReasonGridWindowAspect',
            gridSelector: 'stopReasonGrid',
            storeName: 'dict.StopReason',
            modelName: 'dict.StopReason'
        }
    ],

    mainView: 'dict.stopreason.Grid',
    mainViewSelector: 'stopReasonGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'stopReasonGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('stopReasonGrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});