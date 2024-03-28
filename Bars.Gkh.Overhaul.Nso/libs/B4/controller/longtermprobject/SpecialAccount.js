Ext.define('B4.controller.longtermprobject.SpecialAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'account.Special',
        'account.Operation',
        'account.BankStatement'
    ],

    stores: [
        'account.Special',
        'account.Operation',
        'account.BankStatement',
        'dict.AccountOperationNoPaging'
    ],

    views: [
        'longtermprobject.specialaccount.Grid',
        'longtermprobject.specialaccount.EditWindow',
        'longtermprobject.specialaccount.OperationGrid',
        'longtermprobject.specialaccount.OperationEditWindow',
        'longtermprobject.specialaccount.BankStatGrid',
        'longtermprobject.specialaccount.BankStatEditWindow'
    ],

    mainView: 'longtermprobject.specialaccount.Grid',
    mainViewSelector: 'specialaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'specialaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'specAccBankStatGridWindow',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Ovrhl.AccBankStatement.Edit', applyTo: 'b4savebutton', selector: 'specialaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Field.Number_Edit', applyTo: 'textfield[name=Number]', selector: 'specialaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Field.DocumentDate_Edit', applyTo: 'datefield[name=DocumentDate]', selector: 'specialaccountbankstateditwin' },
                { name: 'Ovrhl.AccBankStatement.Operation.Create', applyTo: 'b4addbutton', selector: 'specialaccountoperationgrid' },
                {
                    name: 'Ovrhl.AccBankStatement.Operation.View',
                    applyTo: 'b4editcolumn',
                    selector: 'specialaccountoperationgrid',
                    applyBy: function(component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Ovrhl.AccBankStatement.Operation.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'specialaccountoperationgrid',
                    applyBy: function(component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Ovrhl.AccBankStatement.Operation.Edit', applyTo: 'b4savebutton', selector: 'specialaccountoperationeditwin' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Ovrhl.AccBankStatement.Delete' }],
            name: 'deleteSpecialAccBankStatemenPerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'specAccBankStatTransfer',
            gridSelector: 'specialaccountbankstatgrid',
            stateType: 'ovrhl_bank_statement',
            menuSelector: 'specAccBankStatGridStateMenu'
        },
        {
            xtype: 'statebuttonaspect',
            name: 'specAccBankStatStateButton',
            stateButtonSelector: 'specialaccountbankstateditwin button[action="StateChange"]',
            listeners: {
                transfersuccess: function(asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                    var editWindowAspect = asp.controller.getAspect('specAccBankStatGridWindow');
                    editWindowAspect.updateGrid();

                    // получаем модель
                    var model = editWindowAspect.getModel();
                    model.load(entityId, {
                        success: function(rec) {
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
            name: 'specialAccountGridWindow',
            gridSelector: 'specialaccountgrid',
            editFormSelector: 'specialaccounteditwin',
            storeName: 'account.Special',
            modelName: 'account.Special',
            editWindowView: 'longtermprobject.specialaccount.EditWindow',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            onSaveSuccess: function(asp, rec) {
                var grid = asp.getForm().down('specialaccountbankstatgrid');
                grid.setDisabled(false);
                asp.controller.specAccId = rec.getId();
            },
            listeners: {
                beforesave: function (asp, obj) {

                    var decision = obj.get('Decision');

                    if (decision && typeof(decision) === "object") {
                        obj.set('Decision', decision.Id);
                    }

                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                        obj.set('AccountType', 20);
                    }
                    return true;
                },
                aftersetformdata: function(asp, record, form) {
                    var store,
                        sfDecision = form.down('b4selectfield[name=Decision]'),
                        grid = form.down('specialaccountbankstatgrid');

                    if (record.getId() > 0) {
                        grid.setDisabled(false);

                        if (!record.get('Decision')) {
                            sfDecision.setReadOnly(false);
                        } else {
                            sfDecision.setReadOnly(true);
                        }

                        store = grid.getStore();

                        store.clearFilter(true);
                        store.filter('accountId', record.getId());

                        asp.controller.specAccId = record.getId();
                    } else {
                        sfDecision.setReadOnly(false);

                        grid.setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'specAccBankStatGridWindow',
            gridSelector: 'specialaccountbankstatgrid',
            editFormSelector: 'specialaccountbankstateditwin',
            storeName: 'account.BankStatement',
            modelName: 'account.BankStatement',
            editWindowView: 'longtermprobject.specialaccount.BankStatEditWindow',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            onSaveSuccess: function(asp, rec) {
                var grid = asp.getForm().down('specialaccountoperationgrid');
                grid.setDisabled(false);
                asp.controller.bankStatId = rec.getId();
                asp.editRecord(rec);
            },
            // Для тго чтобы проверить права перекрываем метод Удаления
            deleteRecord: function(record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteSpecialAccBankStatemenPerm')
                        .loadPermissions(record)
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
                beforesave: function(asp, rec) {
                    if (asp.controller.specAccId > 0) {
                        rec.set('BankAccount', asp.controller.specAccId);
                    }
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    var store,
                        grid = asp.getForm().down('specialaccountoperationgrid'),
                        stateAsp = asp.controller.getAspect('specAccBankStatStateButton');

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
                savesuccess: function(asp) {

                    var me = this,
                        model = me.controller.getModel('account.Special'),
                        grid = asp.getForm().down('specialaccountoperationgrid');

                    grid.setDisabled(false);

                    model.load(me.controller.specAccId, {
                        success: function(rec) {
                            me.controller.getAspect('specialAccountGridWindow').setFormData(rec);
                            asp.getForm().show();
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'specAccOperationGridWindow',
            gridSelector: 'specialaccountoperationgrid',
            editFormSelector: 'specialaccountoperationeditwin',
            storeName: 'account.Operation',
            modelName: 'account.Operation',
            editWindowView: 'longtermprobject.specialaccount.OperationEditWindow',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function(asp, rec) {
                    if (asp.controller.bankStatId > 0) {
                        rec.set('BankStatement', asp.controller.bankStatId);
                    }

                    return true;
                },
                savesuccess: function() {
                    var me = this,
                        model = me.controller.getModel('account.BankStatement');

                    model.load(me.controller.bankStatId, {
                        success: function(rec) {
                            me.controller.getAspect('specAccBankStatGridWindow').setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Create', applyTo: 'b4addbutton', selector: 'specialaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Edit', applyTo: 'b4savebutton', selector: 'specialaccounteditwin' },
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Delete', applyTo: 'b4deletecolumn', selector: 'specialaccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'specialaccounteditwin b4selectfield[name=Decision]': {
                select: {
                    fn: me.onSelectDecision,
                    scope: me
                },
                beforeload: function(store, operation) {
                    operation.params.objectId = me.params.realityObjectId;
                }
            }
        });

        me.callParent(arguments);
    },

    onSelectDecision: function (field, data) {
        var editWindow = this.getEditWindow();
        editWindow.down('textfield[name=CreditOrgName]').setValue(data.CreditOrgName),
        editWindow.down('textfield[name=Number]').setValue(data.AccountNumber),
        editWindow.down('datefield[name=OpenDate]').setValue(new Date(data.OpenDate));
        editWindow.down('datefield[name=CloseDate]').setValue(new Date(data.CloseDate));
        editWindow.down('textfield[name=OwnerName]').setValue(data.OwnerName);
    },

    onLaunch: function() {
        var store = this.getMainView().getStore();
        store.clearFilter(true);
        store.filter('roId', this.params.realityObjectId);
    }
});