Ext.define('B4.controller.dict.ProtocolMKDIniciator', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ProtocolMKDIniciator'],
    stores: ['dict.ProtocolMKDIniciator'],
    views: ['dict.protocolmkdiniciator.Grid'],

    mainView: 'dict.protocolmkdiniciator.Grid',
    mainViewSelector: 'protocolmkdiniciatorgrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolmkdiniciatorgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'protocolmkdiniciatorgrid',
            permissionPrefix: 'Gkh.Dictionaries.ProtocolMKDIniciator'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'protocolmkdiniciatorgridAspect',
            storeName: 'dict.ProtocolMKDIniciator',
            modelName: 'dict.ProtocolMKDIniciator',
            gridSelector: 'protocolmkdiniciatorgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('protocolmkdiniciatorgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProtocolMKDIniciator').load();
    }
});