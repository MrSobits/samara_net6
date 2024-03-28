Ext.define('B4.controller.regop.realty.RealtyPaymentAccount', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.FormPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow'
    ],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'regop.realty.RealtyPaymentAccountPanel',
        'regop.realty.RealtyPaymentAccountOperationEditWindow'
    ],
    
    models: [
        'RealityObjectPaymentAccountOperation',
        'regop.realty.RealtyObjectPaymentAccountProxy',
        'regop.realty.RealtyPaymentAccountOperationBySources'
    ],
    
    stores: [
        'regop.realty.RealtyPaymentAccountOperation',
        'regop.realty.RealtyPaymentAccountOperationBySources'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realtypaymentaccpanel'
        },
        {
            ref: 'debetGrid',
            selector: 'realtypaymentaccpanel realtypayatranfsergrid[type=debet]'
        },
        {
            ref: 'creditGrid',
            selector: 'realtypaymentaccpanel realtypayatranfsergrid[type=credit]'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            },
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.AccountNum_View', applyTo: '#tfAccountNum', selector: 'realtypaymentaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.DateOpen_View', applyTo: '#dfDateOpen', selector: 'realtypaymentaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.DateClose_View', applyTo: '#dfDateClose', selector: 'realtypaymentaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.Limit_View', applyTo: '#tfLimit', selector: 'realtypaymentaccpanel' },
                { name: 'Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.BankAccountNum_View', applyTo: '#tfBankAccountNum', selector: 'realtypaymentaccpanel'}
            ]
        },
        {
            /*
            Аспект взаимодействия "кредитного" грида с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'realtyPayAccOperEditWinAspect',
            gridSelector: 'realtypayatranfsergrid[type="credit"]',
            editFormSelector: 'realtypayaccoperationeditwin',
            storeName: 'regop.realty.RealtyPaymentAccountOperationBySources',
            modelName: 'regop.realty.RealtyPaymentAccountOperationBySources',
            editWindowView: 'regop.realty.RealtyPaymentAccountOperationEditWindow',
            editRecord: function (record) {
                var me = this,
                    view = me.controller.getMainView(),
                    id = record.getId(),
                    typePaymentOperation = record.get('OperationType'),
                    win = me.getForm(),
                    gridStore = win.down('realtypayaccopercreddetailsgrid').getStore();

                if (id > 0) {
                    me.controller.setContextValue(view, 'roPaymentAccOperId', id);
                }
                me.controller.setContextValue(view, 'typePaymentOperation', typePaymentOperation);

                if (gridStore) {
                    gridStore.on('beforeload', me.onBeforeLoadOperationbySourcesGrid, me);
                    gridStore.load();
                }
            },
            
            onBeforeLoadOperationbySourcesGrid: function (store, operation) {
                var me = this,
                    view = me.controller.getMainView();
                
                operation.params.roPaymentAccOperId = me.controller.getContextValue(view, 'roPaymentAccOperId');
                operation.params.typePaymentOperation = me.controller.getContextValue(view, 'typePaymentOperation');
            }
        },
        {
            xtype: 'formpanel',
            modelName: 'regop.realty.RealtyObjectPaymentAccountProxy',
            formPanelSelector: 'realtypaymentaccpanel form',
            objectId: function () {
                var me = this;
                return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            },
            afterLoadRecord: function (asp, rec) {
                var debStore = asp.controller.getDebetGrid().getStore(),
                    credStore = asp.controller.getCreditGrid().getStore();
                
                debStore.clearFilter(true);
                credStore.clearFilter(true);
                debStore.filter([{ property: 'paymentAccountId', value: rec.get('Id') }, { property: 'isCredit', value: false }]);
                credStore.filter([{ property: 'paymentAccountId', value: rec.get('Id') }, { property: 'isCredit', value: true }]);
            },
            name: 'formpanel'
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'realtypaymentaccpanel': {
                'updateme': me.updatePanel,
                'updatebalance': me.updateBalance
            },
            'realtypayatranfsergrid[type="debet"]': {
                beforerender: { fn: me.onBeforeRenderDebetGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = this.getMainView() || Ext.widget('realtypaymentaccpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
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
            params: {
                id: id
            }
        }).next(function() {
            me.unmask();
            asp.loadRecord();
        }).error(function() {
            me.unmask();
        });
    },
    
    onBeforeRenderDebetGrid: function (grid) {
        // Показ формы есть только у "кредитного" грида
        grid.down('b4editcolumn').setVisible(false);
    }
});