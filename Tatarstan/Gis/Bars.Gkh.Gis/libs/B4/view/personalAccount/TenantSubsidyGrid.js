Ext.define('B4.view.personalAccount.TenantSubsidyGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.tenantSubsidyGrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Начисленные субсидии по жильцам',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.personalaccount.TenantSubsidy');

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
                    dataIndex: 'Surname',
                    flex: 1, text: 'Фамилия',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1, text: 'Имя',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Patronymic',
                    flex: 1, text: 'Отчество',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOfBirth',
                    width: 90,
                    text: 'Дата рождения',
                    format: 'd.m.Y',
                    renderer: me.datetimeRenderer,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleCode',
                    flex: 1,
                    text: 'Код статьи субсидии',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'BankName',
                    flex: 1, text: 'Банк',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BeginDate',
                    format: 'd.m.Y',
                    renderer: me.datetimeRenderer,
                    width: 90,
                    text: 'Дата начала<br/>предоставления'
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'IncomingSaldo',
                    flex: 0.9, text: 'Входящее сальдо<br/>субсидии',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'AccruedSum',
                    flex: 0.8, text: 'Начисленная<br/>субсидия',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'AdvancedPayment',
                    flex: 0.8, text: 'Авансовый<br/>платеж',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'PaymentSum',
                    flex: 0.9, text: 'Сумма к выплате<br/>субсидии',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'SmoSum',
                    flex: 0.8, text: 'Субсидия<br/>СМО РФ',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ChangesSum',
                    flex: 0.9, text: 'Сумма изменения<br/>субсидии',
                    renderer: me.moneyRenderer
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    format: 'd.m.Y',
                    renderer: me.datetimeRenderer,
                    width: 90,
                    text: 'Дата окончания<br/>предоставления'
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

    datetimeRenderer: function (value) {
        if (value.getFullYear() != 1) {
            return Ext.Date.format(value, 'd.m.Y');
        }
        return null;
    },

    moneyRenderer: function (value) {
        return value.toFixed(2);
    }
});