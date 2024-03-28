Ext.define('B4.controller.Import.Operator', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.OperatorPanel'],

    mainView: 'Import.OperatorPanel',
    mainViewSelector: 'operatorimportpanel',

    aspects: [
    {
        xtype: 'gkhimportaspect',
        viewSelector: 'operatorimportpanel',
        importId: 'Bars.Gkh.Import.ImportOperator'
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});