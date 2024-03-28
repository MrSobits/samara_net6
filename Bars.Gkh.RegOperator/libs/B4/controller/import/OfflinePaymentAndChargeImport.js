Ext.define('B4.controller.import.OfflinePaymentAndChargeImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.OfflinePaymentAndChargeImport'],

    mainView: 'import.OfflinePaymentAndChargeImport',
    mainViewSelector: 'offlinepaymentandchargeimport',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'offlinepaymentandchargeimport',
            importId: 'Bars.Gkh.RegOperator.Imports.OfflinePaymentAndChargeImport',
            maxFileSize: 52428800
        }
    ],

    index: function() {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});