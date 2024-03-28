Ext.define('B4.controller.Import.organization.OrganizationImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.organization.Panel'],

    mainView: 'Import.organization.Panel',
    mainViewSelector: 'organizationimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'organizationimportpanel',
        importId: 'Bars.Gkh.Import.Organization.OrganizationImport',
        maxFileSize: 52428800,
        getUserParams: function () {
            var me = this;
            me.params.updatePeriodsManOrgs = me.controller.getMainComponent().down('checkbox[name=updatePeriodsManOrgs]').getValue();
        }
    }],
    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});