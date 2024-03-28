Ext.define('B4.controller.Import.Tarif', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.TarifPanel'],

    mainView: 'Import.TarifPanel',
    mainViewSelector: 'tarifimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
            viewSelector: 'tarifimportpanel',
            importId: 'Bars.GkhGji.Regions.Voronezh.Import.TarifImport'
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});