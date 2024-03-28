Ext.define('B4.controller.regoperator.Accounts', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.SelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'calcaccount.Special',
        'calcaccount.Regop',
        'regoperator.ServicedHouse'
    ],

    views: [
        'regoperator.calcaccount.ServicedHousesGrid',
        'regoperator.accounts.Main',
        'regoperator.calcaccount.Panel',
        'regoperator.calcaccount.Grid',
        'regoperator.calcaccount.EditWindow',
        'regoperator.accounts.SpecialAccountGrid',
        'regoperator.accounts.SpecialAccountEditWin'
    ],

    stores: [
        'calcaccount.Special',
        'calcaccount.Regop',
        'regoperator.ServicedHouse',
        'regoperator.RobjectToAddAccount',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    mainView: 'regoperator.accounts.Main',
    mainViewSelector: 'regopaccmain',

    refs: [
        { ref: 'calcAccEditWin', selector: 'regopcalcacceditwin' },
        { ref: 'servicedHouseGrid', selector: 'regopservicedhousesgrid' },
        { ref: 'debtGrid', selector: 'regopcalcacceditwin gridpanel[name=DebtGrid]' },
        { ref: 'creditGrid', selector: 'regopcalcacceditwin gridpanel[name=CreditGrid]' }
    ],

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'regopServicedHousesGridAspect',
            gridSelector: 'regopservicedhousesgrid',
            storeName: 'deliveryagent.RealObjForAdd',
            modelName: 'deliveryagent.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#deliveryAgentAddRealObjMultiSelectWindow',
            storeSelect: 'regoperator.RobjectToAddAccount',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        view = me.controller.getMainView(),
                        grid = view.down('regopspecaccgrid'),
                        recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('MassCreate', 'CalcAccountRealityObject', {
                            roIds: Ext.encode(recordIds),
                            accId: me.controller.accId
                        })).next(function (resp) {
                            me.getGrid().getStore().load();
                            me.controller.unmask();

                            var tryDecoded = Ext.JSON.decode(resp.responseText);
                            if (tryDecoded.message) {
                                Ext.Msg.alert('Сохранение!', tryDecoded.message);
                            }

                            grid.getStore().load();
                            
                            return true;
                        }).error(function (result) {
                            Ext.Msg.alert('Ошибка!', result.message || 'Произошла ошибка');
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'regOpCalcAccountGridWindowAspect',
            gridSelector: 'regopcalcaccountgrid',
            editFormSelector: 'regopcalcacceditwin',
            modelName: 'calcaccount.Regop',
            editWindowView: 'regoperator.calcaccount.EditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('AccountOwner', asp.controller.params.get('ContragentId'));
                        obj.set('TypeAccount', 20);
                    }

                    return true;
                },
                aftersetformdata: function (asp, rec) {
                    var controller = asp.controller;
                    controller.accId = rec.get('Id') || rec.get('id');
                    //controller.loadDebtCreditStores();  временный костыль необходимо оптимизировать сами запросы
                    
                    if (controller.accId) {
                        controller.getServicedHouseGrid().enable();
                        controller.getDebtGrid().enable();
                        controller.getCreditGrid().enable();
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = me.getModel(record);

                if (id) {
                    me.controller.mask('Подождите', B4.getBody().getActiveTab());
                    model.load(id, {
                        success: function (rec) {
                            me.controller.unmask();
                            me.setFormData(rec);
                        },
                        failure: function() {
                            me.controller.unmask();
                        },
                        scope: me
                    });
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        }
    ],

    init: function () {
        var me = this,
            onRenderAccGrid = function (grid) {
                grid.getStore().on('beforeload', function (store, operation) {
                    operation.params['ownerId'] = me.params.get('ContragentId');
                    operation.params['typeOwner'] = 20;
                });
                grid.getStore().load();
            };
        me.control({
            'regopcalcacceditwin b4selectfield[name="ContragentCreditOrg"]': {
                beforeload: me.beforeBankAccsLoad,
                change: me.bankAccountChanged
            },
            'regopcalcacceditwin [name="DebtGrid"]': {
                render: function (grid) {
                    grid.getStore().on('beforeload', me.onDebtBeforeLoad, me);
                }
            },
            'regopcalcacceditwin [name="CreditGrid"]': {
                render: function (grid) {
                    grid.getStore().on('beforeload', me.onCreditBeforeLoad, me);
                }
            },
            'regopservicedhousesgrid': {
                render: function (grid) {
                    grid.getStore().on('beforeload', me.onServicedHousesBeforeLoad, me);
                    grid.getStore().load();
                },
                'rowaction': { fn: me.rowAction }
            },
            'regopcalcaccountgrid': { render: onRenderAccGrid },
            'regopspecaccgrid': { render: onRenderAccGrid },
            'regopcalcaccountpanel': { updateme: me.updatePanel }
        });

        me.callParent(arguments);
    },

    rowAction: function (grid, action, record) {
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'delete':
                    this.deleteRecord(grid, record);
                    break;
            }
        }
    },

    deleteRecord: function (grid, record) {
        grid.getStore().remove(record);
    },

    onLaunch: function () {
        this.updateDebtCreditSaldo();
    },

    beforeBankAccsLoad: function (cmp, opts, store) {
        var contrId = this.params.get('ContragentId');
        if (!contrId) {
            var contragent = this.params.get('Contragent');
            contrId = Ext.isObject(contragent) ? contragent.Id : "";
        }

        Ext.apply(store.getProxy().extraParams, {
            contragentId: contrId
        });
    },

    bankAccountChanged: function (cmp, data) {
        var win = cmp.up('window'),
            corrAccFld = win.down('[name="CorrAcc"]'),
            bikFld = win.down('[name="Bik"]'),
            credOrgFld = win.down('[name="Name"]');

        if (Ext.isObject(data)) {
            corrAccFld.setValue(data.CorrAccount);
            bikFld.setValue(data.Bik);
            credOrgFld.setValue(data.Name);
        }
    },

    onServicedHousesBeforeLoad: function (store, operation) {
        var me = this,
            showAll = me.getServicedHouseGrid().down('checkbox[name=showAll]').getValue();

        Ext.apply(operation.params, {
            accId: me.accId,
            showAll: showAll
        });
    },

    onCalcAccountsBeforeLoad: function (store) {
        Ext.apply(store.getProxy().extraParams, {
            regopId: this.params.getId()
        });
    },

    loadDebtCreditStores: function () {
        var win = this.getCalcAccEditWin();
        win.down('[name="DebtGrid"]').getStore().load(),
        win.down('[name="CreditGrid"]').getStore().load();
    },

    onDebtBeforeLoad: function (store) {
        this.setAccId(store, false);
    },

    onCreditBeforeLoad: function (store) {
        this.setAccId(store, true);
    },

    setAccId: function (store, isCredit) {
        Ext.apply(store.getProxy().extraParams, {
            accId: this.accId,
            isCredit: isCredit
        });
    },

    updatePanel: function () {
        var me = this;
        me.getMainView().down('regopcalcaccountgrid').getStore().load();
        me.updateDebtCreditSaldo();
    },

    updateDebtCreditSaldo: function () {
        var me = this,
            mainView = me.getMainView(),
            panel = mainView.down('regopcalcaccountpanel'),
            debtFld = panel.down('[name="Debet"]'),
            creditFld = panel.down('[name="Credit"]'),
            saldoFld = panel.down('[name="Saldo"]'),
            expenditureShareFld = panel.down('[name="ExpenditureShare"]');

        me.mask('Загрузка..', mainView);

        B4.Ajax.request({
            url: B4.Url.action('GetRegopAccountSummary', 'CalcAccount'),
            params: {
                ownerId: me.params.get('ContragentId')
            }
        }).next(function (resp) {
            var decoded = Ext.JSON.decode(resp.responseText).data;
            me.unmask();
            debtFld.setValue(decoded.Debet);
            creditFld.setValue(decoded.Credit);
            saldoFld.setValue(decoded.Saldo);
            expenditureShareFld.setValue(decoded.Debet == 0 ? '' : decoded.ExpenditureShare);
        }).error(function () {
            me.unmask();
        });
    }
});