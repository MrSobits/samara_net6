Ext.define('B4.controller.dict.LegalReason', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.LegalReason'],
    stores: ['dict.LegalReason'],

    views: ['dict.legalreason.Grid'],

    mainView: 'dict.legalreason.Grid',
    mainViewSelector: 'legalReasonGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'legalReasonGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'legalReasonGrid',
            permissionPrefix: 'GkhGji.Dict.LegalReason'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'legalReasonGridAspect',
            storeName: 'dict.LegalReason',
            modelName: 'dict.LegalReason',
            gridSelector: 'legalReasonGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('legalReasonGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.LegalReason').load();
    }
});