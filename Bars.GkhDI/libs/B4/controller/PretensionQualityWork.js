Ext.define('B4.controller.PretensionQualityWork', {
    extend: 'B4.base.Controller',
    views: ['pretensionqualitywork.EditPanel'],

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

    mainView: 'pretensionqualitywork.EditPanel',
    mainViewSelector: 'pretensionqualityworkeditpanel',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'pretensionQualityWorkPermissionAspect',
            permissions: [
                { name: 'GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_Edit', applyTo: 'b4savebutton', selector: 'pretensionqualityworkeditpanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'pretensionQualityWorkEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: 'pretensionqualityworkeditpanel'
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('pretensionQualityWorkEditPanelAspect').setData(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('pretensionQualityWorkPermissionAspect').setPermissionsByRecord(me.params);
        }
    }
});
