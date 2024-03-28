Ext.define('B4.controller.FinancialPerformance', {
    extend: 'B4.base.Controller',
    views: ['financialperformance.EditPanel'],

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['DisclosureInfoRealityObj', 'RealityObject'],

    mainView: 'financialperformance.EditPanel',
    mainViewSelector: 'financialperformanceEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'finPerfomanceEditPanelPermAspect',
            permissions: [
                { name: 'GkhDi.DisinfoRealObj.FinancialPerformance.FinancialPanel_Edit', applyTo: 'b4savebutton', selector: 'financialperformanceEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'finPerfomanceEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: 'financialperformanceEditPanel'
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('finPerfomanceEditPanelAspect').setData(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('finPerfomanceEditPanelPermAspect').setPermissionsByRecord(me.params);
        }
    }
});