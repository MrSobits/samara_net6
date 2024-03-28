Ext.require('B4.enums.regop.PersonalAccountOwnerType');
Ext.define('B4.controller.distribution.PersonalAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.grid.feature.Summary',
        'B4.aspects.regop.Distribution',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.DistributeOn',
        'B4.enums.CrFundFormationType',
        'B4.ux.grid.column.Enum'
    ],

    models: [],

    stores: [
        'regop.personal_account.BasePersonalAccount',
        'regop.owner.PersonalAccountOwner',
        'regop.personal_account.Distribution',
        'distribution.PersonalAccount'
    ],

    views: [
        'suspenseaccount.DistributionObjectsEditWindow',
        'suspenseaccount.PersAccDistributionPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'suspenseaccount.DistributionPanel',
    mainViewSelector: 'suspaccdistribpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'suspaccpersaccdistribpanel'
        },
        {
            ref: 'leftGrid',
            selector: 'suspaccpersaccdistribpanel b4grid[name=selectGrid]'
        },
        {
            ref: 'rightGrid',
            selector: 'suspaccpersaccdistribpanel b4grid[name=selectedGrid]'
        }
    ],

    getCurrentContextKey: function() {
        return 'personalAccountDistribution';
    },

    aspects: [
        {
            xtype: 'gkhregopdistributionaspect',
            name: 'trancferCrDistributionAspect',
            distribPanel: 'suspenseaccount.PersAccDistributionPanel',
            distribPanelSelector: 'suspaccpersaccdistribpanel',
            storeSelect: 'regop.personal_account.Distribution',
            storeSelected: 'regop.personal_account.BasePersonalAccount',
            columnsGridSelect: [
                {
                    header: 'Статус',
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    width: 100,
                    renderer: function(v) {
                        return '<div style="float: left;">' + (v && v.Name ? v.Name : '') + '</div>';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function(field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    }
                },
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 0.8, filter: { xtype: 'textfield' } },
                { header: 'Р/С получателя', xtype: 'gridcolumn', dataIndex: 'RoPayAccountNum', flex: 0.8, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Владелец', xtype: 'gridcolumn', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'AccountFormationVariant',
                    text: 'Способ формирования фонда КР',
                    enumName: 'B4.enums.CrFundFormationType',
                    flex: 1
                },
                {
                    header: 'Площадь ЛС',
                    xtype: 'gridcolumn',
                    dataIndex: 'RealArea',
                    sortable: true,
                    width: 80
                },
                { header: 'Дата открытия', xtype: 'datecolumn', dataIndex: 'OpenDate', width: 90, filter: { xtype: 'datefield' }, format: 'd.m.Y' }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1, sortable: false }
            ],
            distribObjEditWindowSelector: 'distributionobjectseditwindow',
            distribObjStore: 'B4.store.distribution.PersonalAccount',
            distribObjColumnsGrid: [
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Р/С получателя',
                    flex: 1,
                    dataIndex: 'RoPayAccountNum',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Статус',
                    flex: 1,
                    dataIndex: 'State',
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Name',
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    }

                },
                { text: 'Абонент', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } },
                { text: 'Адрес', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } },
                {
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    flex: 1,
                    renderer: function(value) {
                        try {
                            return B4.enums.regop.PersonalAccountOwnerType.getStore().findRecord('Value', value || 0).get('Display');
                        } catch(e) {
                            return '';
                        }
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.regop.PersonalAccountOwnerType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    text: 'Накопления за период',
                    dataIndex: 'PeriodAccumulation',
                    summaryType: 'sum',
                    hidden: true,
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Задолженность',
                    dataIndex: 'Debt',
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Распределенная сумма на взносы КР',
                    dataIndex: 'Sum',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Распределенная сумма на оплаты пени',
                    dataIndex: 'SumPenalty',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                }
            ],
            onBeforeLoad: function(store, operation) {
                var me = this,
                    params = me.getObjectsUrlParams(),
                    panel = me.controller.getMainView(),
                    grid = panel.down('grid[name=selectGrid]'),
                    crFoundType = grid.down('b4selectfield[name=crFoundType]').value,
                    rospNumber = grid.down('textfield[name=rospNumber]').value;

                operation.params.showAll = true;
                operation.params.distributionId = params.distributionId;
                operation.params.distributionIds = params.distributionIds;
                if (me.controller.getMainView().down('b4selectfield[name="accountNum"]').getValue()) {
                    operation.params.bankAccNum = true;
                }

                if (params.ownerId) {
                    operation.params.ownerId = params.ownerId;
                }

                if (params.roId) {
                    operation.params.roId = params.roId;
                }
                
                if (params.snapshotId) {
                    operation.params.snapshotId = params.snapshotId;
                }

                if (crFoundType) {
                    operation.params.crFoundType = Ext.JSON.encode(crFoundType);
                }
                if (rospNumber) {
                    operation.params.rospNumber = rospNumber;
                }
            },
            otherActions: function(actions) {
                var me = this,
                    ctxKey = me.controller.getCurrentContextKey();

                actions[me.distribObjEditWindowSelector + '[ctxKey=' + ctxKey + ']'] = { 'show': { fn: me.controller.onShowDistributionGrid, scope: me } };

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4grid[name="selectedGrid"] b4deletebutton'] = {
                    click: function (btn) {
                        var panel = btn.up('suspaccpersaccdistribpanel'),
                            selectGrid = panel.down('b4grid[name=selectGrid]'),
                            selectedGrid = panel.down('b4grid[name=selectedGrid]'),
                            selection = selectGrid.getSelectionModel();

                        selectedGrid.getStore().removeAll();
                        selection.deselectAll();
                    }
                };

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4selectfield[name=crFoundType]'] = { change: { fn: me.updateSelectGrid, scope: me } };
                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' textfield[name=rospNumber]'] = {
                    specialkey: {
                        fn: function (scope, e) {
                            debugger;
                            if (e.getKey() == 13) {
                                me.updateSelectGrid();
                            }
                        }, scope: me
                    }
                };

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' [name=distributeUsingFilters]'] = {
                    change: function(cbx, newValue) {
                        var panel = cbx.up('suspaccpersaccdistribpanel'),
                            selectedGrid = panel.down('b4grid[name=selectedGrid]'),
                            removeAllBtn = selectedGrid.down('b4deletebutton');

                        selectedGrid.setDisabled(newValue);
                        removeAllBtn.fireEvent('click', removeAllBtn);
                    }
                };
            },

            getApplyUrlParams: function(win, store) {
                var me = this,
                    panel = me.controller.getMainView(),
                    params,
                    distrSumFld = win.down('[name=DistrSum]'),
                    distrSum = distrSumFld ? distrSumFld.getValue() : this.initialSum,
                    distributionView = panel.down('[name=DistributionView]').getValue(),
                    mapped = Ext.Array.map(store.proxy.data, function(item) {
                        return {
                            AccountId: item.Id,
                            Sum: item.Sum,
                            SumPenalty: item.SumPenalty
                        };
                    });

                params = {
                    code: win.code,
                    distributionId: win.distributionId,
                    distributionIds: win.distributionIds,
                    distributionSource: win.src,
                    distribSum: distrSum,
                    distributionView: distributionView,
                    records: Ext.encode(mapped)
                };

                if (distributionView == B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument) {
                    params.periodId = panel.down('[name=ChargePeriod]').getValue();
                    params.snapshotId = panel.down('[name=Snapshot]').getValue();
                }

                return params;
            },

            getObjectsUrlParams: function() {
                var me = this,
                    win = me.getForm(),
                    mainView = me.controller.getMainView(),
                    distributeUsingFilters = mainView.down('[name="distributeUsingFilters"]').getValue(),
                    storeSelected = me.getSelectedGrid().getStore(),
                    distrFld = win.down('[name=DistributionView]'),
                    val = [],
                    params = {},
                    ownerId = mainView.down('b4selectfield[name="legalOwner"]').getValue(),
                    roId = mainView.down('b4selectfield[name="accountNum"]').getValue(),
                    rospNumber = mainView.down('textfield[name="rospNumber"]').getValue(),
                    distrWindow = me.controller.getCmpInContext(me.distribObjEditWindowSelector),
                    distrSumFld = distrWindow ? distrWindow.down('[name=DistrSum]') : null,
                    distrSum = distrSumFld ? distrSumFld.getValue() : this.initialSum,
                    snapshotId = mainView.down('[name=Snapshot]').getValue(),
                    crFundTypesField = me.getSelectGrid().down('b4selectfield[name=crFoundType]');

                if (storeSelected) {
                    storeSelected.each(function(rec) {
                        val.push(rec.get('Id'));
                    });
                }

                if (distrFld.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions) {
                    params.periodStartDate = win.down('[name=PeriodStartDate]').getValue();
                    params.periodEndDate = win.down('[name=PeriodEndDate]').getValue();
                }

                params.distributeUsingFilters = distributeUsingFilters;
                params.complexFilter = Ext.encode(me.getSelectGrid().getHeaderFilters());
                params.showAll = true;

                if (snapshotId) {
                    params.snapshotId = snapshotId;
                }
                if (ownerId) {
                    params.ownerId = ownerId;
                }
                if (roId) {
                    params.roId = roId;
                    params.bankAccNum = true;
                }

                if (crFundTypesField && crFundTypesField.value) {
                    params.crFoundType = Ext.JSON.encode(crFundTypesField.value);
                }

                if (rospNumber) {
                    params.rospNumber = rospNumber;
                }

                params.ids = val;
                params.code = win.code;
                params.distributionType = distrFld ? distrFld.getValue() : null;
                params.distributionIds = win.distributionIds;
                params.distributionId = win.distributionId;
                params.distributionSource = win.src;
                params.distribSum = distrSum;
                params.distributeOn = win.down('[name=DistributeOn]').getValue();

                return params;
            }
        }
    ],

    init: function() {
        var me = this;
        me.control({
            'suspaccpersaccdistribpanel b4selectfield[name="legalOwner"]': {
                change: function(fld, nv) {
                    var panel = fld.up('suspaccpersaccdistribpanel'),
                        accNumFld = panel.down('b4selectfield[name="accountNum"]');

                    accNumFld.setDisabled(!nv);

                    panel.down('b4grid[name="selectGrid"]').getStore().load();
                }
            },
            'suspaccpersaccdistribpanel b4selectfield[name="accountNum"]': {
                change: function(fld) {
                    fld.up('suspaccpersaccdistribpanel').down('b4grid[name="selectGrid"]').getStore().load();
                }
            },
            'suspaccpersaccdistribpanel combobox[name="DistributionView"]': {
                'change': { fn: me.distribViewComboChange, scope: me }
            },
            'suspaccpersaccdistribpanel [name=ChargePeriod]': {
                'change': { fn: me.onChangePeriod, scope: me }
            },
            'suspaccpersaccdistribpanel [name=Snapshot]': {
                'beforeload': { fn: me.onBeforeLoadSnapshot, scope: me },
                'change': { fn: me.onChangeSnapshot, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function(id, code, sum, src, inn) {
        var me = this,
            view = me.getMainView(),
            freshDeploy = false,
            aspect = me.getAspect('trancferCrDistributionAspect');

        if (!view) {
            freshDeploy = true;
            view = Ext.widget('suspaccpersaccdistribpanel');
        }
        me.bindContext(view);
        me.application.deployView(view);

        if (id.includes(',')) {
            view.distributionIds = id;
        }
        else {
            view.distributionId = id;
            view.inn = inn;
            view.isPicking = true;
        }
        view.code = code;
        view.src = src;

        aspect.storeSelect = 'regop.personal_account.Distribution';

        if (freshDeploy) {
            aspect.reconfigure(sum.replace('dot', '.'));
        }

        var legalOwner = view.down('b4selectfield[name="legalOwner"]');

        if (view.isPicking) {
            B4.Ajax.request({
                url: B4.Url.action('ListLegalOwners', 'PersonalAccountOwner'),
                params: {
                    inn: view.inn,
                    isPicking: view.isPicking
                }
            }).next(function (response) {
                var data = Ext.JSON.decode(response.responseText).data;
                legalOwner.setValue(data[0]);
                view.isPicking = false;
            }).error(function (e) {

            });
        }
    },

    distribViewComboChange: function(combo, newVal) {
        var me = this,
            leftGrid = me.getLeftGrid(),
            rightGrid = me.getRightGrid(),
            panel = combo.up(me.distribPanelSelector),
            distributeOn = panel.down('[name=DistributeOn]'),
            periodFld = panel.down('[name=ChargePeriod]'),
            snapshotFld = panel.down('[name=Snapshot]'),
            jurFilters = leftGrid.query('[jurFilterMustHide=true]'),
            periodStartDate = panel.down('[name=PeriodStartDate]'),
            periodEndDate = panel.down('[name=PeriodEndDate]');

        if (newVal === B4.enums.SuspenseAccountDistributionParametersView.ByDebt && panel.code === 'TransferCrDistribution') {
            distributeOn.show();
        }
        else if (newVal === B4.enums.SuspenseAccountDistributionParametersView.ByDebt && panel.code === 'TransferCrROSPDistribution') {
            distributeOn.show();
        }
        else {
            distributeOn.setValue(B4.enums.DistributeOn.ChargesPenalties);
            distributeOn.hide();
        }
        debugger;
        if (newVal === B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument && panel.code === 'TransferCrDistribution') {

            Ext.each(jurFilters, function(cmp) {
                cmp.setVisible(false);
            });
            leftGrid.setDisabled(true);
            leftGrid.getStore().removeAll();

            rightGrid.getStore().removeAll();
            
            periodFld.markInvalid();
            periodFld.setValue(null);
            periodFld.show();
            
            snapshotFld.show();
        }
        else if (newVal === B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument && panel.code === 'TransferCrROSPDistribution') {

            Ext.each(jurFilters, function (cmp) {
                cmp.setVisible(false);
            });
            leftGrid.setDisabled(true);
            leftGrid.getStore().removeAll();

            rightGrid.getStore().removeAll();

            periodFld.markInvalid();
            periodFld.setValue(null);
            periodFld.show();

            snapshotFld.show();
        }
        else {
            periodFld.setValue(null);
            periodFld.hide();
            
            snapshotFld.hide();

            Ext.each(jurFilters, function (cmp) {
                cmp.setVisible(true);
            });
            leftGrid.setDisabled(false);
            leftGrid.getStore().load();
        }

        var isProportionallyToOwnersContributions = (newVal === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions);

        periodStartDate.setVisible(isProportionallyToOwnersContributions);
        periodEndDate.setVisible(isProportionallyToOwnersContributions);
    },

    onShowDistributionGrid: function(view) {
        var me = this,
            distrCombo = me.controller.getCmpInContext('[name=DistributionView]'),
            grid = view.down('[type=distribObjects]'),
            distributeOn = view.distributeOn;

        if (distrCombo && distrCombo.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument) {
            view.down('[name=DistrSum]').setReadOnly(true);
        }

        if (distributeOn === B4.enums.DistributeOn.Penalties) {
            grid.down('[dataIndex=Sum]').hide();
        } else if (distributeOn === B4.enums.DistributeOn.Charges) {
            grid.down('[dataIndex=SumPenalty]').hide();
        }

        if (view.code === 'RentPayment' || view.code === 'AccumulatedFunds') {
            grid.down('[dataIndex=SumPenalty]').hide();
            grid.down('[dataIndex=Sum]').setText('Распределенная сумма');
        }
        else if (view.code === 'RefundDistribution') {
            grid.down('[dataIndex=SumPenalty]').hide();
        }

        var isProportionallyToOwnersContributions = distrCombo && distrCombo.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions;
        grid.down('[dataIndex=PeriodAccumulation]').setVisible(isProportionallyToOwnersContributions);
    },

    onChangePeriod: function(cmp, newValue) {
        var snapshotFld = this.getMainView().down('[name=Snapshot]');

        snapshotFld.setDisabled(!newValue);
        snapshotFld.markInvalid();
        snapshotFld.setValue(null);
    },

    onBeforeLoadSnapshot: function(store, operation) {
        var periodId = this.getMainView().down('[name=ChargePeriod]').getValue();

        operation.params.periodId = periodId;
        operation.params.onlyLegal = true;
        operation.params.onlyBase = true;
    },
    
    onChangeSnapshot: function (cmp, newValue) {
        var me = this,
            leftGrid = me.getLeftGrid(),
            rightGrid = me.getRightGrid(),
            distrCombo = me.getMainView().down('[name=DistributionView]');

        rightGrid.getStore().removeAll();
        
        if (distrCombo.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument) {
            if (newValue) {
                leftGrid.setDisabled(false);
                leftGrid.getStore().load();
            } else {
                leftGrid.setDisabled(true);
                leftGrid.getStore().removeAll();
            }
        }
    }
});