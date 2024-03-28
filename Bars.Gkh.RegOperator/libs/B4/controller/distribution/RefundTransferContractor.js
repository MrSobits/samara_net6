Ext.define('B4.controller.distribution.RefundTransferContractor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.regop.Distribution',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [],

    stores: ['transferrf.TransferCtr'],

    views: [
        'suspenseaccount.DistributionPanel',
        'suspenseaccount.DistributionObjectsEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'suspenseaccount.DistributionPanel',
    mainViewSelector: 'suspaccdistribpanel',

    getCurrentContextKey: function () {
        return "refundTransferContractor";
    },

    aspects: [
        {
            xtype: 'gkhregopdistributionaspect',
            name: 'refundtransferctrdistribaspect',
            distribPanel: 'suspenseaccount.DistributionPanel',
            distribPanelSelector: 'suspaccdistribpanel',
            storeSelect: 'transferrf.TransferCtr',
            storeSelected: 'transferrf.TransferCtr',
            onBeforeLoad: function(store, operation) {
                operation.params.showWithPaidSum = true;
            },
            columnsGridSelect: [
                { header: 'Адрес', dataIndex: 'ObjectCr', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Номер заявки', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'datecolumn',
                    header: 'Дата заявки',
                    format: 'd.m.Y',
                    dataIndex: 'DateFrom',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                { header: 'Вид работы', dataIndex: 'TypeWorkCr', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Сумма заявки', dataIndex: 'Sum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq, hideTrigger: true } },
                { header: 'Оплачено', dataIndex: 'PaidSum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq, hideTrigger: true } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', dataIndex: 'ObjectCr', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Номер заявки', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                {
                    xtype: 'datecolumn',
                    header: 'Дата заявки',
                    format: 'd.m.Y',
                    dataIndex: 'DateFrom',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                { header: 'Вид работы', dataIndex: 'TypeWorkCr', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Сумма заявки', dataIndex: 'Sum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                { header: 'Оплачено', dataIndex: 'PaidSum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } }
            ],
            distribObjEditWindowSelector: 'distributionobjectseditwindow',
            distribObjStore: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'WalletName' },
                    { name: 'Amount' },
                    { name: 'Sum' }
                ]
            }),
            distribObjColumnsGrid: [
                {
                    dataIndex: 'WalletName',
                    text: 'Источник поступления',
                    flex: 1
                },
                {
                    dataIndex: 'Amount',
                    text: 'Оплата, руб.',
                    width: 100
                },
                {
                    dataIndex: 'Sum',
                    text: 'Возврат, руб.',
                    width: 100,
                    editor: {
                        xtype: 'numberfield',
                        minValue: 0,
                        decimalSeparator: ','
                    }
                }
            ],
            otherActions: function (actions) {
                var me = this;
                actions[me.distribObjEditWindowSelector] = { 'show': { fn: me.distrObjEditWinShow, scope: this } }
                actions[me.distribPanelSelector] = { 'show': { fn: me.distrPanelShow, scope: this } }
            },
            distrObjEditWinShow: function (win) {
                var distrSumFld = win.down('[name=DistrSum]'),
                    balanceFld = win.down('[name=Balance]'),
                    grid = win.down('b4grid[type=distribObjects]'),
                    transfersSum = 0,
                    data = grid.getStore().proxy.data;

                Ext.Array.each(data, function (item) {
                    transfersSum += item.Sum;
                });

                distrSumFld.setValue(transfersSum);
                distrSumFld.readOnly = true;
                balanceFld.setValue(this.initialSum - transfersSum);
            },
            distrPanelShow: function (win) {
                var distrCombo = win.down('[name=DistributionView]');

                    distrCombo.hide();
            },
            onShowDistributionGrid: function (view) {
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
            },
            listeners: {
                beforeaccept: function() {
                    var me = this,
                        store = me.getDistribGrid().getStore(),
                        result = true;

                    store.each(function (rec) {
                        if (rec.get('Amount') < rec.get('Sum')) {
                            Ext.Msg.alert('Ошибка', 'Cумма возврата не может быть больше оплаченной суммы!');
                            result = false;
                        }
                    });

                    return result;
                }
            }
        }
    ],

    index: function (id, code, sum, src) {
        var me = this,
            view = me.getMainView(),
            freshDeploy = false;
        if (!view) {
            freshDeploy = true;
            view = Ext.widget('suspaccdistribpanel');
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

        if (freshDeploy) {
            me.getAspect('refundtransferctrdistribaspect').reconfigure(sum.replace('dot', '.'));
        }
    }
});