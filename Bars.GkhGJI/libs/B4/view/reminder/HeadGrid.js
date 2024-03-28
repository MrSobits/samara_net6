Ext.define('B4.view.reminder.HeadGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.enums.CategoryReminder',
        'B4.GjiTextValuesOverride',
        'B4.enums.TypeReminder'
    ],

    alias: 'widget.reminderHeadGjiGrid',
    itemId: 'reminderHeadGjiGrid',
    initComponent: function() {
        var me = this;
        me.store = Ext.create('B4.store.reminder.HeadGji');
        
        Ext.applyIf(me, {
            
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CategoryReminder',
                    flex: 1,
                    text: 'Категория',
                    renderer: function(val) {
                        return B4.enums.CategoryReminder.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.CategoryReminder.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeReminder',
                    flex: 1,
                    text: 'Задача',
                    renderer: function(val) {
                        return B4.enums.TypeReminder.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Id',
                        displayField: 'Display',
                        emptyItem: { Name: '-' },
                        url: '/Reminder/ListTypeReminder'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckDate',
                    text: 'Контрольный срок',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Num',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: B4.GjiTextValuesOverride.getText('номер'),
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 2,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Инспектор',
                    filter: { xtype: 'textfield' }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    var checkDate = record.get('CheckDate'),
                        typeReminder = record.get('TypeReminder'),
                        deltaDate,
                        hasAppealCitizensInWorkState = record.get('HasAppealCitizensInWorkState');

                    if (typeReminder == 20 || typeReminder == 80 || hasAppealCitizensInWorkState) {
                        return '';
                    }

                    deltaDate = ((new Date(checkDate)).getTime() - (new Date()).getTime()) / (1000 * 60 * 60 * 24);
                    if (deltaDate >= 0 && deltaDate <= 5) {
                        return 'back-coralyellow';
                    }

                    if (deltaDate < 0) {
                        return 'back-coralred';
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});