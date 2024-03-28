Ext.define('B4.view.mobileappaccountcomparsion.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.mobileappaccountcomparsiongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.MobileAccountComparsionDecision',
    ],

    title: 'Реестр несопоставленных ЛС',
    store: 'MobileAppAccountComparsion',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OperatinDate',
                    flex: 0.5,
                    text: 'Дата запроса',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNumber',
                    flex: 1,
                    text: 'Номер л/с в системе',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalAccountNumber',
                    flex: 1,
                    text: 'Внешний номер л/с',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MobileAccountNumber',
                    flex: 1,
                    text: 'Номер л/с из мобильного приложения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountOwnerFIO',
                    flex: 1,
                    text: 'ФИО в системе',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MobileAccountOwnerFIO',
                    flex: 1,
                    text: 'ФИО в мобильном приложении',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'DecisionType',
                    enumName: 'B4.enums.MobileAccountComparsionDecision',
                    filter: true,
                    flex: 1,
                    text: 'Решение'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var isOpened = record.get('IsViewed');
                    if (!isOpened) {
                        return 'x-summary';
                    }

                    return '';
                },

            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowClose',
                                    boxLabel: 'Показать отработанные записи',
                                    labelAlign: 'right',
                                    checked: false
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});