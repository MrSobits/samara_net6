Ext.define('B4.controller.import.DebtorClaimWorkImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: {
         context: 'B4.mixins.Context'
    },

    views: [
        'import.DebtorClaimWorkImportPanel'
    ],

    mainView: 'import.DebtorClaimWorkImportPanel',
    mainViewSelector: 'debtorclaimworkimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'debtorclaimworkimportpanel',
            importId: 'Bars.Gkh.RegOperator.Imports.DebtorClaimWork.DebtorClaimWorkImport',
            initComponent: function() {
                var me = this;
                Ext.apply(me,
                    {
                        maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
                    });

                me.callParent(arguments);
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});