Ext.define('B4.controller.DocumentRegister', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.enums.ClaimWorkDocumentType',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: [
        'claimwork.DocumentRegisterPanel'
    ],

    mainView: 'B4.view.claimwork.DocumentRegisterPanel',
    mainViewSelector: 'documentregisterpanel',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'documentregisterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.PaymentPlannedPeriodColumn_View',
                    selector: 'pretensiongrid',
                    applyBy: function (component, allowed) {
                        var me = this;
                        if (component) {
                            me.controller.isShowColumnPaymentPlannedPeriod = allowed;
                        }
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'documentregisterpanel b4combobox[name=TypeDocument]': { 'change': me.onChangeTypeDocument },
            'documentregisterpanel b4updatebutton': { 'click': me.onUpdateBtnClick }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('documentregisterpanel');

        view.ctxKey = 'documentregister';
        me.application.deployView(view);
    },

    deployTabs: function (controller, view) {
        var me = this,
            component = view.down('#cwcPaymentPlannedPeriod'),
            container = me.cmpQueryInContext('container[name=gridcontainer]')[0],
            viewSelector = Ext.String.format('#{0}', view.getItemId());
        if (container && !container.down(viewSelector)) {
            container.add(view);

            if (component) {
                component.setVisible(controller.isShowColumnPaymentPlannedPeriod);
            }
       }
    },

    deployViewKeys: {
        documentregister: 'deployTabs'
    },

    onChangeTypeDocument: function (cmp, newValue) {
        var container = cmp.up('panel').down('container[name=gridcontainer]');
        container.removeAll();

        if (newValue) {
            Ext.History.add(newValue);
        }
    },

    onUpdateBtnClick: function(btn) {
        var grid = btn.up('panel').down('grid');

        if (grid) {
            grid.getStore().load();
        }
    }
});