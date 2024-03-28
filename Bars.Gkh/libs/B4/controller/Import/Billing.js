Ext.define('B4.controller.Import.Billing', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.BillingPanel'],

    mainView: 'Import.BillingPanel',
    mainViewSelector: 'billingimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'billingimportpanel',
        importId: 'Bars.Gkh.Import.ImportBilling',
        maxFileSize: 10485760
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});