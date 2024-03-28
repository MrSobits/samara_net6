Ext.define('B4.controller.import.OwnerDecisionProtocolImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.OwnerDecisionProtocolImportPanel'],

    mainView: 'import.OwnerDecisionProtocolImportPanel',
    mainViewSelector: 'ownerdecisionprotocolimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'ownerdecisionprotocolimportpanel',
            buttonSelector: 'ownerdecisionprotocolimportpanel #button',
            importId: 'Bars.Gkh.RegOperator.Imports.DecisionProtocol.OwnerDecisionProtocolImport',
            maxFileSize: 52428800
        }
    ],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});