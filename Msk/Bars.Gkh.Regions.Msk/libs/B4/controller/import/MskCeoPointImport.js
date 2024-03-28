Ext.define('B4.controller.import.MskCeoPointImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.MskCeoPointPanel'],

    mainView: 'import.MskCeoPointPanel',
    mainViewSelector: 'mskceopointimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'mskceopointimportpanel',
            importId: 'Bars.Gkh.Regions.Msk.Import.MskCeoPointImport',
            maxFileSize: 52428800
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});