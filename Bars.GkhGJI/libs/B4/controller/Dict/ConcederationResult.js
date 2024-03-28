Ext.define('B4.controller.dict.ConcederationResult', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.ConcederationResult'],
    stores: ['dict.ConcederationResult'],

    views: ['dict.concederationresult.Grid'],

    mainView: 'dict.concederationresult.Grid',
    mainViewSelector: 'concederationresultgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'concederationresultgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'concederationresultgridAspect',
            storeName: 'dict.ConcederationResult',
            modelName: 'dict.ConcederationResult',
            gridSelector: 'concederationresultgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('concederationresultgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ConcederationResult').load();
    }
});