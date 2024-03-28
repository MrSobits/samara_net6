Ext.define('B4.controller.dict.ProtocolMKDState', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ProtocolMKDState'],
    stores: ['dict.ProtocolMKDState'],
    views: ['dict.protocolmkdstate.Grid'],

    mainView: 'dict.protocolmkdstate.Grid',
    mainViewSelector: 'protocolmkdstategrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolmkdstategrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'protocolmkdstategrid',
            permissionPrefix: 'Gkh.Dictionaries.ProtocolMKDState'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'protocolmkdstategridAspect',
            storeName: 'dict.ProtocolMKDState',
            modelName: 'dict.ProtocolMKDState',
            gridSelector: 'protocolmkdstategrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('protocolmkdstategrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProtocolMKDState').load();
    }
});