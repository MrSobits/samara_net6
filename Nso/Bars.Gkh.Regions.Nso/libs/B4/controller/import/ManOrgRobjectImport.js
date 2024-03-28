Ext.define('B4.controller.import.ManOrgRobjectImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.ManOrgRobjectImportPanel'],

    mainView: 'import.ManOrgRobjectImportPanel',
    mainViewSelector: 'manorgrobjectimportpanel',
    
    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'manorgrobjectimportpanel',
        importId: 'Bars.Gkh.Regions.Nso.Import.ManOrgRobjectImport',
        maxFileSize: 52428800
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});