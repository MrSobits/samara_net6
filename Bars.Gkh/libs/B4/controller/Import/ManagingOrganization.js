Ext.define('B4.controller.Import.ManagingOrganization', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],
    mixins: { context: 'B4.mixins.Context' },
    views: ['Import.ManagingOrganizationPanel'],
    mainView: 'Import.ManagingOrganizationPanel',
    mainViewSelector: 'managingorganizationimportpanel',
    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'managingorganizationimportpanel',
            importId: 'Bars.Gkh.Import.ManagingOrganizationImport'
        }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});