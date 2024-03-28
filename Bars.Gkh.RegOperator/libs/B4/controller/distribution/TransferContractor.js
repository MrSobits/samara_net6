Ext.define('B4.controller.distribution.TransferContractor', {
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
        return "transferContractorDistribution";
    },

    aspects: [
        {
            xtype: 'gkhregopdistributionaspect',
            name: 'transferctrdistribaspect',
            distribPanel: 'suspenseaccount.DistributionPanel',
            distribPanelSelector: 'suspaccdistribpanel',
            storeSelect: 'transferrf.TransferCtr',
            storeSelected: 'transferrf.TransferCtr',
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
                { header: 'Сумма заявки', dataIndex: 'Sum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                { header: 'Оплаченная сумма', dataIndex: 'PaidSum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } }
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
                { header: 'Сумма заявки', dataIndex: 'Sum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } }
            ],
            distribObjEditWindowSelector: 'distributionobjectseditwindow',
            distribObjStore: Ext.create('Ext.data.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'ObjectCr' },
                    { name: 'DocumentNum' },
                    { name: 'DateFrom' },
                    { name: 'TypeWorkCr' },
                    { name: 'Sum' },
                    { name: 'PaidSum' },
                    { name: 'Index' }
                ]
            }),
            distribObjColumnsGrid: [
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
                { header: 'Сумма заявки', dataIndex: 'Sum', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } }
            ],
            otherActions: function(actions) {
                actions[this.distribObjEditWindowSelector] = { 'show': { fn: this.distrObjEditWinShow, scope: this } }
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
                grid.getPlugin('cellEdit').disable();
                balanceFld.setValue(this.initialSum - transfersSum);
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
            me.getAspect('transferctrdistribaspect').reconfigure(sum.replace('dot', '.'));
        }
    }
});