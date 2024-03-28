Ext.define('B4.controller.calcaccount.Overdraft', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'CalcAccount',
        'calcaccount.Overdraft'
    ],

    stores: [
        'CalcAccount',
        'calcaccount.Overdraft'
    ],

    views: [
        'calcaccount.OverdraftEditWindow',
        'calcaccount.OverdraftGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'calcaccountoverdraftgrid'
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
                { name: 'GkhRegOp.Loans.Overdraft.Create', applyTo: 'b4addbutton', selector: 'calcaccountoverdraftgrid' },
                { name: 'GkhRegOp.Loans.Overdraft.Edit', applyTo: 'b4savebutton', selector: 'calcaccountoverdrafteditwindow' },
                {
                    name: 'GkhRegOp.Loans.Overdraft.Delete', applyTo: 'b4deletecolumn', selector: 'calcaccountoverdraftgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'OverdraftGridEditWindowAspect',
            gridSelector: 'calcaccountoverdraftgrid',
            editFormSelector: 'calcaccountoverdrafteditwindow',
            modelName: 'calcaccount.Overdraft',
            editWindowView: 'calcaccount.OverdraftEditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('calcaccountoverdraftgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },
    init: function () {
        var me = this;
        
        me.control({
            'calcaccountoverdrafteditwindow [name=Account]': {
                change: function (cmp, newValue) {
                    cmp.up('calcaccountoverdrafteditwindow')
                        .down('[name=AccountOwner]')
                        .setValue(
                            newValue
                                ? newValue.AccountOwner
                                : null
                        );
                }
            }
        });
        
        me.callParent(arguments);
    }
});