Ext.define('B4.view.claimwork.DocumentClwAccountDetailGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.store.claimwork.DocumentClwAccountDetail',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    cls: 'x-large-head',
    alias: 'widget.documentclwaccountdetailgrid',
    title: 'Расчет суммы пени по лицевым счетам',
    minHeight: 300,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.DocumentClwAccountDetail');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'AccountNumber',
                    text: 'Номер ЛС',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'DebtBaseTariffSum',
                    text: 'Сумма задолженности по базовому тарифу',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'DebtDecisionTariffSum',
                    text: 'Сумма задолженности по тарифу решения',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'DebtSum',
                    text: 'Сумма задолженности',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'PenaltyDebtSum',
                    text: 'Сумма задолженности по пени',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'PenaltyCalcFormula',
                    text: 'Расчет суммы пени',
                    flex: 4,
                    filter: { xtype: 'textfield' }
                },
                
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});