Ext.define('B4.controller.AgentPIRDebtorCreditImport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhImportAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'AgentPIRDebtorCreditImportPanel'
    ],

    mainView: 'AgentPIRDebtorCreditImportPanel',
    mainViewSelector: 'agentpirdebtorcreditimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'agentpirdebtorcreditimportpanel',
            importId: 'Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports.AgentPIRDebtorCreditImport',
            initComponent: function () {
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