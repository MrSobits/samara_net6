Ext.define('B4.view.reminder.InspectorGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',

        'B4.enums.CategoryReminder',
        'B4.enums.TypeReminder'
    ],

    alias: 'widget.reminderInspectorGjiGrid',
    store: 'reminder.InspectorGji',
    itemId: 'reminderInspectorGjiGrid',
    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.reminder.InspectorGji');
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeReminder',
                    flex: 1,
                    text: 'Задача',
                    renderer: function (val, meta, rec) {
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
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrespondentAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Адрес корреспондента'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LocationProblem',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Место возникновения проблемы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 2,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                 }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var checkDate = record.get('CheckDate');
                    var typeReminder = record.get('TypeReminder');

                    if (typeReminder == 20 || typeReminder == 80) {
                        return '';
                    }
                    
                    var deltaDate = ((new Date(checkDate)).getTime() - (new Date()).getTime()) / (1000 * 60 * 60 * 24);
                    if (deltaDate >= 0 && deltaDate <= 5) {
                        return 'back-yellow';
                    }

                    if (deltaDate < 0) {
                        return 'back-light-red';
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