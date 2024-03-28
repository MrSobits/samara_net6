Ext.define('B4.controller.AgentPIRDutyImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: {
         context: 'B4.mixins.Context'
    },

    views: [
        'AgentPIRDutyImportPanel'
    ],

    mainView: 'AgentPIRDutyImportPanel',
    mainViewSelector: 'agentpirdutyimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'agentpirdutyimportpanel',
            importId: 'Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports.AgentPIRDutyImport',
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