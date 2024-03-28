Ext.define('B4.controller.import.TpElevatorsImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.TpElevatorsImportPanel'],

    mainView: 'import.TpElevatorsImportPanel',
    mainViewSelector: 'elevatorsimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'elevatorsimportpanel',
            importId: 'Bars.GkhTp.Import.ElevatorsImport.ElevatorsImport',
            maxFileSize: 52428800
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});