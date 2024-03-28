Ext.define('B4.controller.import.RealityObjectExaminationImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.realityobj.RoExaminationImportPanel'],

    mainView: 'import.realityobj.RoExaminationImportPanel',
    mainViewSelector: 'roexaminationimportpanel',
    
    aspects: [
    {
        xtype: 'gkhimportaspect',
            viewSelector: 'roexaminationimportpanel',
            importId: 'Bars.Gkh.Regions.Tyumen.Import.RealityObjectExaminationImport.RoImport',
            maxFileSize: 52428800,
            getUserParams: function () {
            var me = this;
        }
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});