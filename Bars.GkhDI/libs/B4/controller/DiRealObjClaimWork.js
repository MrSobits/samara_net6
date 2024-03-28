Ext.define('B4.controller.DiRealObjClaimWork', {
    extend: 'B4.base.Controller',
    views: ['direalobjclaimwork.EditPanel'],

    requires:
    [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models:
    [
        'DisclosureInfoRealityObj',
        'RealityObject'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    mainView: 'direalobjclaimwork.EditPanel',
    mainViewSelector: 'direalobjclaimworkeditpanel',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'diRealObjClaimWorkPermissionAspect',
            permissions: [
                { name: 'GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_Edit', applyTo: 'b4savebutton', selector: 'direalobjclaimworkeditpanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'diRealityObjClaimWorkEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: 'direalobjclaimworkeditpanel'
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('diRealityObjClaimWorkEditPanelAspect').setData(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('diRealObjClaimWorkPermissionAspect').setPermissionsByRecord(me.params);
        }
    }
});
