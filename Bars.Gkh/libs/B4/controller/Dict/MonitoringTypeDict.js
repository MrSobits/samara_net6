Ext.define('B4.controller.dict.MonitoringTypeDict', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.MonitoringTypeDict'],
    stores: ['dict.MonitoringTypeDict'],
    views: ['dict.capitalgroup.Grid'],

    mainView: 'dict.monitoringtypedict.Grid',
    mainViewSelector: 'monitoringtypedictGrid',

    refs: [{
        ref: 'mainView',
        selector: 'monitoringtypedictGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'monitoringtypedictGrid',
            permissionPrefix: 'Gkh.Dictionaries.MonitoringTypeDict'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'monitoringtypedictGridAspect',
            storeName: 'dict.MonitoringTypeDict',
            modelName: 'dict.MonitoringTypeDict',
            gridSelector: 'monitoringtypedictGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('monitoringtypedictGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.MonitoringTypeDict').load();
    }
});