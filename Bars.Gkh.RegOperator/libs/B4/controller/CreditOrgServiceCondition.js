Ext.define('B4.controller.CreditOrgServiceCondition', {
    //confContribId: null,
    //confContribRecordId: null,
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhGridPermissionAspect'
    ],

    models: [
        'CreditOrgServiceCondition',
        'CreditOrg'
    ],

    stores: [
        'CreditOrgServiceCondition',
        'CreditOrg'
    ],

    views: [
        'creditorgservicecondition.EditWindow',
        'creditorgservicecondition.Grid'
    ],

    refs: [
        {
            ref: 'AddEditWindow',
            selector: 'creditOrgServiceCondEditWindow'
        },
        {
            ref: 'mainView',
            selector: 'creditOrgServiceCondGrid'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.CreditorgServiceCondition.Edit', applyTo: 'b4savebutton', selector: 'creditOrgServiceCondEditWindow' }
            ]
        },
        {
            xtype: 'gkhgridpermissionaspect',
            permissionPrefix: 'Gkh.Orgs.CreditorgServiceCondition',
            gridSelector: 'creditOrgServiceCondGrid'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'creditOrgServiceCondGridEditWindowAspect',
            gridSelector: 'creditOrgServiceCondGrid',
            editFormSelector: 'creditOrgServiceCondEditWindow',
            storeName: 'CreditOrgServiceCondition',
            modelName: 'CreditOrgServiceCondition',
            editWindowView: 'creditorgservicecondition.EditWindow'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('creditOrgServiceCondGrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('CreditOrgServiceCondition').load();
    }
});