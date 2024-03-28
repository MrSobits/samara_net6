Ext.define('B4.controller.import.RoImportFromFundPart5', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.realityobj.RoImportFromFundPart5Panel'],

    mainView: 'import.realityobj.RoImportFromFundPart5Panel',
    mainViewSelector: 'roimportfromfundpart5panel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'roimportfromfundpart5panel',
        importId: 'Bars.Gkh.Overhaul.Hmao.Import.FundImportPart5',
        maxFileSize: 10485760
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});