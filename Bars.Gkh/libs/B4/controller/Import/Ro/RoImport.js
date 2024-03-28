Ext.define('B4.controller.Import.Ro.RoImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.Ro.Panel'],

    mainView: 'Import.Ro.Panel',
    mainViewSelector: 'roimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'roimportpanel',
        importId: 'Bars.Gkh.Import.RoImport.RoImport',
        maxFileSize: 52428800
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});