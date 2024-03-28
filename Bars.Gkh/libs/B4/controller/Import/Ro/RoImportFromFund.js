Ext.define('B4.controller.Import.Ro.RoImportFromFund', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.Ro.RoImportFromFundPanel'],

    mainView: 'Import.Ro.RoImportFromFundPanel',
    mainViewSelector: 'roimportfromfundpanel',
    params: {},
    
    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'roimportfromfundpanel',
        importId: 'Bars.Gkh.Import.FundRealtyObjectsImport',
        maxFileSize: 5242880,
        getUserParams: function () {
            this.params.createObjectsOnImport = this.controller.getMainComponent().down('#chbCreateObjects').getValue();
            this.params.replaceDataOnImport = this.controller.getMainComponent().down('#chbReplaceData').getValue();
        }
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});