Ext.define('B4.controller.import.OwnersImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.OwnersImportPanel'],

    mainView: 'import.OwnersImportPanel',
    mainViewSelector: 'ownersimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'ownersimportpanel',
        importId: 'Bars.Gkh.RegOperator.Imports.Account.OwnersImport',
        maxFileSize: 52428800
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});