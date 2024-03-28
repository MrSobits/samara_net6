Ext.define('B4.controller.CashPaymentCenter', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.cashpaymentcenter.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['CashPaymentCenter'],
    stores: ['CashPaymentCenter'],
    views: [
        'cashpaymentcenter.Grid',
        'cashpaymentcenter.AddWindow'
    ],

    refs: [
        {
            ref: 'AddWindow',
            selector: 'cashpaymentcenteraddwindow'
        },
        {
            ref: 'mainView',
            selector: 'cashpaymentcentergrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.CashPaymentCenter.Create', applyTo: 'b4addbutton', selector: 'cashpaymentcentergrid' },
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.Delete', applyTo: 'b4deletecolumn', selector: 'cashpaymentcentergrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'cashpaymentcentergrid',
            editFormSelector: 'cashpaymentcenteraddwindow',
            storeName: 'CashPaymentCenter',
            modelName: 'CashPaymentCenter',
            editWindowView: 'cashpaymentcenter.AddWindow',
            controllerEditName: 'B4.controller.cashpaymentcenter.Navigation',
            deleteWithRelatedEntities: true,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('cashpaymentcenteredit/{0}', id));
                    }
                    else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                }
                else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        }
    ],
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcentergrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});