Ext.define('B4.view.personalAccount.ServiceSubsidyGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.serviceSubsidyGrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Начисленные субсидии по услугам',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.personalaccount.ServiceSubsidy');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Pss',
                    flex: 1, text: 'ПСС',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    flex: 1,
                    text: 'Услуга',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'AccruedBenefitSum',
                    flex: 1, text: 'Начисленная сумма субсидий - льгот',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'AccruedEdvSum',
                    flex: 1, text: 'Начисленная сумма субсидий - ЕДВ',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'RecalculatedBenefitSum',
                    flex: 1, text: 'Начисленная перерасчетом<br/>сумма субсидий - льгот',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'RecalculatedEdvSum',
                    flex: 1, text: 'Начисленная перерасчетом<br/>сумма субсидий - ЕДВ',
                    renderer: me.moneyRenderer
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 1,
                    items: [
                        { xtype: 'b4updatebutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    },

    moneyRenderer: function (value) {
        return value.toFixed(2);
    }
});