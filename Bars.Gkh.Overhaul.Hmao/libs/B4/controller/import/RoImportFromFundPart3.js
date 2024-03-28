Ext.define('B4.controller.import.RoImportFromFundPart3', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.realityobj.RoImportFromFundPart3Panel'],

    mainView: 'import.realityobj.RoImportFromFundPart3Panel',
    mainViewSelector: 'roimportfromfundpart3panel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'roimportfromfundpart3panel',
        importId: 'Bars.Gkh.Overhaul.Hmao.Import.FundImportPart3',
        maxFileSize: 10485760
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});