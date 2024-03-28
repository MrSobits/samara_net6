Ext.define('B4.controller.import.PerformedWork', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],
    mixins: { context: 'B4.mixins.Context' },
    views: ['import.PerformedWorkPanel'],
    mainView: 'import.PerformedWorkPanel',
    mainViewSelector: 'performedworkimportpanel',
    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'performedworkimportpanel',
            importId: 'Bars.GkhCr.Import.PerformedWorkImport'
        }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});