Ext.define('B4.controller.dict.ProtocolMKDSource', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ProtocolMKDSource'],
    stores: ['dict.ProtocolMKDSource'],
    views: ['dict.protocolmkdsource.Grid'],

    mainView: 'dict.protocolmkdsource.Grid',
    mainViewSelector: 'protocolmkdsourcegrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolmkdsourcegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'protocolmkdsourcegrid',
            permissionPrefix: 'Gkh.Dictionaries.ProtocolMKDSource'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'protocolmkdsourcegridAspect',
            storeName: 'dict.ProtocolMKDSource',
            modelName: 'dict.ProtocolMKDSource',
            gridSelector: 'protocolmkdsourcegrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('protocolmkdsourcegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProtocolMKDSource').load();
    }
});