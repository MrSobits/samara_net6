Ext.define('B4.controller.longtermprobject.PaymentAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.enums.AccountType',
        'B4.aspects.StateButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'account.Payment',
        'account.Operation',
        'account.BankStatement'
    ],
    
    stores: [
        'account.Payment',
        'account.Operation',
        'account.BankStatement',
        'dict.AccountOperationNoPaging'
    ],
    views: [
        'longtermprobject.paymentaccount.Grid',
        'longtermprobject.paymentaccount.EditWindow',
        'longtermprobject.paymentaccount.OperationGrid',
        'longtermprobject.paymentaccount.OperationEditWindow',
        'longtermprobject.paymentaccount.BankStatGrid',
        'longtermprobject.paymentaccount.BankStatEditWindow'
    ],

    mainView: 'longtermprobject.paymentaccount.Grid',
    mainViewSelector: 'paymentaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'paymentaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'paymentAccBankStatGridWindow',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Ovrhl.AccBankStatement.Edit', applyTo: 'b4savebutton', selector: 'paymentaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Field.Number_Edit', applyTo: 'textfield[name=Number]', selector: 'paymentaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Field.DocumentDate_Edit', applyTo: 'datefield[name=DocumentDate]', selector: 'paymentaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Operation.Create', applyTo: 'b4addbutton', selector: 'paymentaccountoperationgrid' },
                {
                    name: 'Ovrhl.AccBankStatement.Operation.View', applyTo: 'b4editcolumn', selector: 'paymentaccountoperationgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Ovrhl.AccBankStatement.Operation.Delete', applyTo: 'b4deletecolumn', selector: 'paymentaccountoperationgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Ovrhl.AccBankStatement.Operation.Edit', applyTo: 'b4savebutton', selector: 'paymentaccountoperationeditwin' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Ovrhl.AccBankStatement.Delete' }],
            name: 'deletePaymentAccBankStatemenPerm'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Payment.Create', applyTo: 'b4addbutton', selector: 'paymentaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Payment.Edit', applyTo: 'b4savebutton', selector: 'paymentaccounteditwin' },
                { name: 'Gkh.RealityObject.Register.Accounts.Payment.Delete', applyTo: 'b4deletecolumn', selector: 'paymentaccountgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'payAccBankStatTransfer',
            gridSelector: 'paymentaccountbankstatgrid',
            stateType: 'ovrhl_bank_statement',
            menuSelector: 'payAccBankStatGridStateMenu'
        },
        {
            xtype: 'statebuttonaspect',
            name: 'payAccBankStatStateButton',
            stateButtonSelector: 'paymentaccountbankstateditwin button[action="StateChange"]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                    var editWindowAspect = asp.controller.getAspect('paymentAccBankStatGridWindow');
                    editWindowAspect.updateGrid();
                    
                    // получаем модель
                    var model = editWindowAspect.getModel();
                    model.load(entityId, {
                        success: function (rec) {
                            //Еще раз обновляем форму поскольку и права могли изменится со сменой статуса
                            editWindowAspect.setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentAccGridWindow',
            gridSelector: 'paymentaccountgrid',
            editFormSelector: 'paymentaccounteditwin',
            storeName: 'account.Payment',
            modelName: 'account.Payment',
            editWindowView: 'longtermprobject.paymentaccount.EditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            onSaveSuccess: function(asp, rec) {
                var grid = asp.getForm().down('paymentaccountbankstatgrid');
                grid.setDisabled(false);
                asp.controller.payAccId = rec.getId();
            },
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                    }
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    var store,
                        grid = asp.getForm().down('paymentaccountbankstatgrid');


                    if (record.getId() > 0) {
                        grid.setDisabled(false);

                        store = grid.getStore();

                        store.clearFilter(true);
                        store.filter('accountId', record.getId());

                        asp.controller.payAccId = record.getId();
                    } else {
                        grid.setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentAccBankStatGridWindow',
            gridSelector: 'paymentaccountbankstatgrid',
            editFormSelector: 'paymentaccountbankstateditwin',
            storeName: 'account.BankStatement',
            modelName: 'account.BankStatement',
            editWindowView: 'longtermprobject.paymentaccount.BankStatEditWindow',
            onSaveSuccess: function (asp, rec) {
                var grid = asp.getForm().down('paymentaccountoperationgrid');
                grid.setDisabled(false);
                asp.controller.bankStatId = rec.getId();
                asp.editRecord(rec);
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            // Для тго чтобы проверить права перекрываем метод Удаления
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deletePaymentAccBankStatemenPerm').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function() {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            },
            listeners: {
                beforesave: function (asp, rec) {
                    if (asp.controller.payAccId > 0) {
                        rec.set('BankAccount', asp.controller.payAccId);
                    } 
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    var store,
                        grid = asp.getForm().down('paymentaccountoperationgrid'),
                        stateAsp = asp.controller.getAspect('payAccBankStatStateButton');

                    stateAsp.setStateData(record.get('Id'), record.get('State'));

                    if (record.getId() > 0) {
                        grid.setDisabled(false);

                        store = grid.getStore();

                        store.clearFilter(true);
                        store.filter('bankStatId', record.getId());

                        asp.controller.bankStatId = record.getId();
                    } else {
                        grid.setDisabled(true);
                    }
                },
                savesuccess: function (asp) {

                    var me = this,
                        model = me.controller.getModel('account.Payment'),
                        grid = asp.getForm().down('paymentaccountoperationgrid');

                    grid.setDisabled(false);

                    model.load(me.controller.payAccId, {
                        success: function (rec) {
                            me.controller.getAspect('paymentAccGridWindow').setFormData(rec);
                            asp.getForm().show();
                        },
                        scope: this
                    });


                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentAccOperationGridWindow',
            gridSelector: 'paymentaccountoperationgrid',
            editFormSelector: 'paymentaccountoperationeditwin',
            storeName: 'account.Operation',
            modelName: 'account.Operation',
            editWindowView: 'longtermprobject.paymentaccount.OperationEditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function (asp, rec) {
                    if (asp.controller.bankStatId > 0) {
                        rec.set('BankStatement', asp.controller.bankStatId);
                    }
                    
                    return true;
                },
                savesuccess: function () {
                    var me = this,
                        model = me.controller.getModel('account.BankStatement');

                    model.load(me.controller.bankStatId, {
                        success: function (rec) {
                            me.controller.getAspect('paymentAccBankStatGridWindow').setFormData(rec);
                        },
                        scope: this
                    });


                }
            }
        }
    ],
    
    onLaunch: function() {
        var store = this.getMainView().getStore();
        store.clearFilter(true);
        store.filter('roId', this.params.realityObjectId);
    }
});