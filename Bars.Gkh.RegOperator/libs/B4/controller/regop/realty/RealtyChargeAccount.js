Ext.define('B4.controller.regop.realty.RealtyChargeAccount', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.store.regop.realty.RealtyChargeAccountOperation',
        'B4.aspects.FormPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'regop.realty.RealtyChargeAccountPanel',
        'regop.realty.RealtyChargeAccountOperationDetailsWindow'
    ],

    models: [
        'RealityObjectChargeAccountOperation',
        'regop.realty.RealtyObjectChargeAccountProxy'
    ],

    stores: [
        'regop.realty.RealtyChargeAccountOperation'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realtychargeaccpanel'
        },
        {
            ref: 'window',
            selector: 'rchaopdetailswin'
        },
        {
            ref: 'operationGrid',
            selector: 'realtychargeaccpanel realtychargeaccopgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            },
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.AccountNum_View', applyTo: '#tfAccountNum', selector: 'realtychargeaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.DateOpen_View', applyTo: '#dfDateOpen', selector: 'realtychargeaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.DateClose_View', applyTo: '#dfDateClose', selector: 'realtychargeaccpanel' }
            ]
        },
        {
            xtype: 'formpanel',
            modelName: 'regop.realty.RealtyObjectChargeAccountProxy',
            formPanelSelector: 'realtychargeaccpanel form',
            objectId: function() {
                var me = this;
                return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            },
            onBeforeLoadRecord: function(asp) {
                asp.controller.mask('Загрузка', asp.controller.getMainView());
                return true;
            },
            afterLoadRecord: function(asp, rec) {
                var me = this,
                    store = asp.controller.getOperationGrid().getStore();

                me.controller.setContextValue(me.controller.getMainComponent(), 'accId', rec.get('Id'));

                store.load({
                    callback: function (records, operation, success) {
                        me.controller.unmask();
                    }
                });
            },
            onLoadRecordFailure: function (asp, message) {
                Ext.Msg.alert('Ошибка!', message);
                asp.controller.unmask();
            },
            name: 'formpanel'
        }
    ],

    init: function() {
        var me = this;
        me.control({
            'realtychargeaccpanel realtychargeaccopgrid': {
                'rowaction': me.showOperationDetails,
                'store.beforeload': {
                    fn: function (store, operation) {
                        operation.params.accId = me.getContextValue(me.getMainView(), 'accId');
                    }, scope: me
                }
            },
            'rchaopdetailswin b4grid[name=accountsummarygrid] b4editcolumn': {
                'click': me.redirectTo
            },
            'realtychargeaccpanel': {
                'updateme': me.updatePanel,
                'updatebalance': me.updateBalance
            }
        });
        me.callParent(arguments);
    },

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realtychargeaccpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    },

    showOperationDetails: function(grid, action, record) {
        if (action === 'edit') {
            var me = this,
                win = me.getWindow();

            if (!win) {
                win = Ext.widget('rchaopdetailswin', {
                    constrain: true,
                    closeAction: 'destroy',
                    renderTo: B4.getBody().getActiveTab().getEl()
                });

                win.down('[name="accountsummarygrid"]').getStore().on('beforeload', function(st) {
                    Ext.apply(st.getProxy().extraParams, {
                        operationId: me.getContextValue(me.getMainComponent(), 'operationId')
                    });
                });
            }

            me.setContextValue(me.getMainComponent(), 'operationId', record.get('Id'));

            win.down('[name="accountsummarygrid"]').getStore().load();

            win.show();
        }
    },

    redirectTo: function() {
        var me = this,
            record = Array.prototype.slice.call(arguments, 5, 6)[0];
        me.application.redirectTo(Ext.String.format('personal_acc_details/{0}', record.get('AccountId')));
    },

    updatePanel: function() {
        var me = this,
            asp = me.getAspect('formpanel');

        asp.controller = me;
        asp.loadRecord();
    },

    updateBalance: function() {
        var me = this,
            id = me.getContextValue(me.getMainView(), 'realityObjectId'),
            asp = me.getAspect('formpanel');

        asp.controller = me;

        me.mask();

        B4.Ajax.request({
            url: B4.Url.action('UpdateAccount', 'RealityObjectAccountUpdater'),
            timeout: 9999999,
            params: {
                id: id
            }
        }).next(function() {
            me.unmask();
            asp.loadRecord();
        }).error(function() {
            me.unmask();
        });
    }

});