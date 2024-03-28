Ext.define('B4.controller.dict.GISGMPPayerStatus', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.GISGMPPayerStatus'],
    stores: ['dict.GISGMPPayerStatus'],

    views: ['dict.gisgmppayerstatus.Grid'],

    mainView: 'dict.gisgmppayerstatus.Grid',
    mainViewSelector: 'gisgmppayerstatusgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'gisgmppayerstatusgrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'inlinegridpermissionaspect',
        //    gridSelector: 'regionCodeGrid',
        //    permissionPrefix: 'GkhGji.Dict.RegionCode'
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'gisgmppayerstatusGridAspect',
            storeName: 'dict.GISGMPPayerStatus',
            modelName: 'dict.GISGMPPayerStatus',
            gridSelector: 'gisgmppayerstatusgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('gisgmppayerstatusgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.GISGMPPayerStatus').load();
    }
});