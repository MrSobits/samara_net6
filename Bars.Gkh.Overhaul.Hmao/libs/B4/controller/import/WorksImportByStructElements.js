Ext.define('B4.controller.import.WorksImportByStructElements', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.realityobj.WorksImportByStructElementsPanel'],

    mainView: 'import.realityobj.WorksImportByStructElementsPanel',
    mainViewSelector: 'worksimportbystructelementspanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'worksimportbystructelementspanel',
        importId: 'Bars.Gkh.Overhaul.Hmao.Import.StructElemWorksImport',
        maxFileSize: 10485760
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});