Ext.define('B4.controller.FinPerfomanceCommunalService', {
    extend: 'B4.base.Controller',
    views: ['financialperformance.CommunalServiceEditPanel'],

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['DisclosureInfoRealityObj', 'RealityObject'],

    mainView: 'financialperformance.CommunalServiceEditPanel',
    mainViewSelector: 'finperfcomserviceeditpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'finPerfomanceComServiceEditPanelPermAspect',
            permissions: [
                { name: 'GkhDi.DisinfoRealObj.FinancialPerformance.CommunalService_Edit', applyTo: 'b4savebutton', selector: 'finperfcomserviceeditpanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'finPerfomanceComServiceEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: 'finperfcomserviceeditpanel'
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('finPerfomanceComServiceEditPanelAspect').setData(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('finPerfomanceComServiceEditPanelPermAspect').setPermissionsByRecord(me.params);
        }
    }
});