Ext.define('B4.controller.Import.Appeal', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.AppealPanel'],

    mainView: 'Import.AppealPanel',
    mainViewSelector: 'appealimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'appealimportpanel',
        importId: 'Bars.GkhGji.Import.Appeal.ImportAppeal'
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});