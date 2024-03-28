Ext.define('B4.controller.import.MskCeoStateServiceImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.MskCeoStateServicePanel'],

    mainView: 'import.MskCeoStateServicePanel',
    mainViewSelector: 'mskceostateserviceimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'mskceostateserviceimportpanel',
            importId: 'Bars.Gkh.Regions.Msk.Import.MskCeoStateForServiceImport',
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