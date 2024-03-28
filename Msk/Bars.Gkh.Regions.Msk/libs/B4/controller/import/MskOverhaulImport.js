Ext.define('B4.controller.import.MskOverhaulImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.MskOverhaulPanel'],

    mainView: 'import.MskOverhaulPanel',
    mainViewSelector: 'mskoverhaulimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'mskoverhaulimportpanel',
            importId: 'Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport.MskOverhaulImport',
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