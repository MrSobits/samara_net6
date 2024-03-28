Ext.define('B4.controller.dict.SSTUTransferOrg', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SSTUTransferOrg'],
    stores: ['dict.SSTUTransferOrg'],

    views: ['dict.sstutransferorg.Grid'],

    mainView: 'dict.sstutransferorg.Grid',
    mainViewSelector: 'sSTUTransferOrgGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'sSTUTransferOrgGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'sSTUTransferOrgGrid',
            permissionPrefix: 'GkhGji.Dict.SSTUTransferOrg'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'sSTUTransferOrgGridAspect',
            storeName: 'dict.SSTUTransferOrg',
            modelName: 'dict.SSTUTransferOrg',
            gridSelector: 'sSTUTransferOrgGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('sSTUTransferOrgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SSTUTransferOrg').load();
    }
});