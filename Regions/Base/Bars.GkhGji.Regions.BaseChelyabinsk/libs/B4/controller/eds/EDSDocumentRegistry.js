Ext.define('B4.controller.eds.EDSDocumentRegistry', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiNestedDigitalSignatureGridAspect',
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['eds.EDSDocumentRegistry'],
    stores: ['eds.EDSDocumentRegistry'],

    views: ['eds.DocumentSignGrid'],

    mainView: 'eds.DocumentSignGrid',
    mainViewSelector: 'esddocumentsigngrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'esddocumentsigngrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjinesteddigitalsignaturegridaspect',
            gridSelector: 'esddocumentsigngrid',
            controllerName: 'EDSDocumentSignature',
            name: 'edsDocumentSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'esddocumentsigngridAspect',
            storeName: 'eds.EDSDocumentRegistry',
            modelName: 'eds.EDSDocumentRegistry',
            gridSelector: 'esddocumentsigngrid'
        }
    ],

    init: function () {
        this.control({
            'esddocumentsigngrid': { itemclick: { fn: this.itemClick, scope: this } },
        });

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('esddocumentsigngrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('eds.EDSDocumentRegistry').load();
    },

    itemClick: function (grid, rec) {
        this.aspectCollection.items
            .filter(function (item) { return item.name == 'edsDocumentSignatureAspect' })
            .forEach(function (item) {
                debugger;
                item.controllerName = rec.get('SignController')
            });
    }
});