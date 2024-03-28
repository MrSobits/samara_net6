Ext.define('B4.controller.import.MskCeoStateImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.MskCeoStatePanel'],

    mainView: 'import.MskCeoStatePanel',
    mainViewSelector: 'mskceostateimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'mskceostateimportpanel',
            importId: 'Bars.Gkh.Regions.Msk.Import.MskCeoStateImport',
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