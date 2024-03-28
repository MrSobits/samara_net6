Ext.define('B4.controller.distribution.RealtyAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.regop.Distribution',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.form.SelectField',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.CrFundFormationType',
        'B4.ux.grid.column.Enum',
        'B4.enums.regop.DistributionCode'
    ],

    models: [],

    stores: ['regop.realty.RealtyPaymentAccount', 'B4.store.distribution.RealtyAccount'],

    views: [
        'suspenseaccount.DistributionPanel',
        'suspenseaccount.DistributionObjectsEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'suspenseaccount.RealtyAccDistributionPanel',
    mainViewSelector: 'realtyaccdistribpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'realtyaccdistribpanel'
        },
        {
            ref: 'leftGrid',
            selector: 'realtyaccdistribpanel b4grid[name=selectGrid]'
        },
        {
            ref: 'rightGrid',
            selector: 'realtyaccdistribpanel b4grid[name=selectedGrid]'
        }
    ],

    getCurrentContextKey: function () {
        return 'realtyAccountDistribution';
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.ProgramCr_View',
                    applyTo: 'b4selectfield[name=ProgramCr]',
                    selector: 'realtyaccdistribpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.RealityObjectOriginator_View',
                    applyTo: 'b4selectfield[name=RealityObjectOriginator]',
                    selector: 'realtyaccdistribpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhregopdistributionaspect',
            name: 'realtyaccountdistribaspect',
            distribPanel: 'suspenseaccount.RealtyAccDistributionPanel',
            distribPanelSelector: 'realtyaccdistribpanel',
            storeSelect: 'regop.realty.RealtyPaymentAccount',
            storeSelected: 'regop.realty.RealtyPaymentAccount',
            getApplyUrlParams: function(win, store) {
                var parentResult = B4.aspects.regop.Distribution.prototype.getApplyUrlParams.apply(this, arguments);
                return Ext.apply(parentResult, {
                        originatorName: this.controller.getMainView().down('b4selectfield[name=RealityObjectOriginator]').getText()
                    });
            },
            otherActions: function (actions) {
                var me = this,
                ctxKey = me.controller.getCurrentContextKey();

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4selectfield[name=crFundTypes]'] = { change: { fn: me.updateSelectGrid, scope: me } };
                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4selectfield[name="accountNum"]'] = {
                    change: function(field) { field.up('b4grid[name="selectGrid"]').getStore().load(); }
                },

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4grid[name="selectedGrid"] b4deletebutton'] = {
                    click: function(btn) {
                        var panel = btn.up('realtyaccdistribpanel'),
                            selectGrid = panel.down('b4grid[name=selectGrid]'),
                            selectedGrid = panel.down('b4grid[name=selectedGrid]'),
                            selection = selectGrid.getSelectionModel();

                        selectedGrid.getStore().removeAll();
                        selection.deselectAll();
                    }
                };

                actions[me.distribObjEditWindowSelector + '[ctxKey=' + ctxKey + ']'] = { 'show': { fn: me.controller.onShowDistributionGrid, scope: me } };

                actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' [name=distributeUsingFilters]'] = {
                    change: function (cbx, newValue) {
                        var panel = cbx.up('realtyaccdistribpanel'),
                            selectedGrid = panel.down('b4grid[name=selectedGrid]'),
                            removeAllBtn = selectedGrid.down('b4deletebutton');

                        selectedGrid.setDisabled(newValue);
                        removeAllBtn.fireEvent('click', removeAllBtn);
                    }
                };
            },
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    header: 'Муниципальное образование',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    header: 'Жилой дом',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BankAccountNumber',
                    header: 'Расчетный счет',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    },
                    sortable: false
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'CrFundType',
                    text: 'Способ формирования фонда КР',
                    enumName: 'B4.enums.CrFundFormationType',
                    flex: 1.2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealArea',
                    header: 'Площадь дома',
                    sortable: false,
                    flex: 0.8
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    header: 'Муниципальное образование',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    text: 'Жилой дом',
                    flex: 1
                }
            ],
            distribObjEditWindowSelector: 'distributionobjectseditwindow',
            distribObjStore: 'B4.store.distribution.RealtyAccount',
            distribObjColumnsGrid: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    header: 'Муниципальное образование',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    text: 'Жилой дом',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodAccumulation',
                    text: 'Накопления за период',
                    hidden: true,
                    flex: 1,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    flex: 1,
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    },
                    editor: {
                        xtype: 'numberfield',
                        minValue: 0,
                        decimalSeparator: ',',
                        hideTrigger: true
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this,
                    panel = me.controller.getMainView(),
                    grid = panel.down('grid[name=selectGrid]'),
                    programCr = panel.down('[name=ProgramCr]').getValue(),
                    crFundTypes = grid.down('b4selectfield[name=crFundTypes]').getValue(),
                    roIds = grid.down('b4selectfield[name=accountNum]').getValue();

                if (programCr) {
                    operation.params.programCr = programCr;
                }
                if (crFundTypes) {
                    operation.params.crFundTypes = Ext.JSON.encode(crFundTypes);
                }
                if (roIds) {
                    operation.params.roIds = roIds;
                    operation.params.bankAccNum = true;
                }
            },
            setOriginatorName: function(data) {
                var me = this,
                    panel = me.controller.getMainView(),
                    originatorHolder = panel.down('b4selectfield[name=RealityObjectOriginator]');

                if (data && originatorHolder) {
                    originatorHolder.setValue(data);
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'realtyaccdistribpanel b4selectfield[name="ProgramCr"]': {
                'beforeload': { fn: me.onProgramCrBeforeLoad, scope: me },
                'change': { fn: me.onProgramCrChange, scope: me }
            },
            'realtyaccdistribpanel [name="DistributionView"]': {
                'change': { fn: me.onChangeDistributionView, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function(id, code, sum, src) {
        var me = this,
            view = me.getMainView(),
            freshDeploy = false;
        if (!view) {
            freshDeploy = true;
            view = Ext.widget('realtyaccdistribpanel');
        }
        me.bindContext(view);
        me.application.deployView(view);

        if (id.includes(',')) {
            view.distributionIds = id;
        }
        else {
            view.distributionId = id;
        }
        view.code = code;
        view.src = src;

        view.down('b4selectfield[name=RealityObjectOriginator]').setVisible(code === 'BankPercentDistribution');

        if (freshDeploy) {
            me.getAspect('realtyaccountdistribaspect').reconfigure(sum.replace('dot', '.'));
        }
    },

    onProgramCrBeforeLoad: function(selFld, operation) {
        operation.params.onlyFull = true;
        operation.params.activePrograms = true;
    },

    onProgramCrChange: function () {
        this.getLeftGrid().getStore().load();
    },

    onChangeDistributionView: function (field, newValue) {
        var me = this,
            panel = field.up('realtyaccdistribpanel'),
            periodStartDate = panel.down('[name=PeriodStartDate]'),
            periodEndDate = panel.down('[name=PeriodEndDate]');

        var isProportionallyToOwnersContributions = (newValue === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions);

        periodStartDate.setVisible(isProportionallyToOwnersContributions);
        periodEndDate.setVisible(isProportionallyToOwnersContributions);
    },

    onShowDistributionGrid: function (view) {
        var me = this,
            distrCombo = me.controller.getCmpInContext('[name=DistributionView]'),
            grid = view.down('[type=distribObjects]');
        
        var isProportionallyToOwnersContributions = distrCombo && distrCombo.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions;

        grid.down('[dataIndex=PeriodAccumulation]').setVisible(isProportionallyToOwnersContributions);
    }
});