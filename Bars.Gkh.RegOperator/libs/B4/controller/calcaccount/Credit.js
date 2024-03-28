Ext.define('B4.controller.calcaccount.Credit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'CalcAccount',
        'calcaccount.Credit'
    ],

    stores: [
        'CalcAccount',
        'calcaccount.Credit'
    ],

    views: [
        'calcaccount.CreditAddWindow',
        'calcaccount.CreditGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'calcaccountcreditgrid'
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
                { name: 'GkhRegOp.Loans.AccountCredit.Create', applyTo: 'b4addbutton', selector: 'calcaccountcreditgrid' }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'CreditGridEditWindowAspect',
            gridSelector: 'calcaccountcreditgrid',
            editFormSelector: 'calcaccountcreditaddwindow',
            modelName: 'calcaccount.Credit',
            editWindowView: 'calcaccount.CreditAddWindow',
            rowDblClick: function () { },
            updateGrid: function() {
                this.getGrid().getStore().load();
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('calcaccountcreditgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },
    
    init: function () {
        var me = this;

        me.control({
            'calcaccountcreditaddwindow [name=Account]': {
                change: function (cmp, newValue) {
                    cmp.up('calcaccountcreditaddwindow')
                        .down('[name=AccountOwner]')
                        .setValue(
                            newValue
                                ? newValue.AccountOwner
                                : null
                        );
                }
            }
        });

        this.callParent(arguments);
    }
});