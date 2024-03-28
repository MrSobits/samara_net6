Ext.define('B4.controller.import.SuspenseAccount', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.SuspenseAccountPanel'],

    mainView: 'import.SuspenseAccountPanel',
    mainViewSelector: 'suspenseaccountpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'suspenseaccountpanel',
        importId: 'Bars.Gkh.RegOperator.Imports.SuspenseAccount',
        maxFileSize: 52428800
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});