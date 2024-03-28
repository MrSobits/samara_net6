Ext.define('B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.HistoryGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'Ext.ux.CheckColumn',
        'B4.form.ComboBox',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeWorkCrHistoryAction',
        'B4.enums.TypeWorkCrReason'
    ],
    alias: 'widget.typeworkrealityobjectoutdoorhistorygrid',

    title: 'Журнал изменений',
    closable: false,
    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoorHistory');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'single'
            }),
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAction',
                    flex: 1,
                    text: 'Действие',
                    renderer: function (val) { return B4.enums.TypeWorkCrHistoryAction.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeWorkCrHistoryAction.getItemsWithEmpty([null, '-']),
                        valueField: 'Value',
                        displayField: 'Display',
                        editable: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWorkRealityObjectOutdoor',
                    flex: 1,
                    text: 'Вид работы',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    flex: 1,
                    text: 'Объем',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма (руб.)',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    text: 'Дата изменения',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    flex: 1,
                    text: 'Пользователь',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        store.load();
                                    }
                                },
                                {
                                    xtype: 'button',
                                    action: 'recover',
                                    iconCls: 'icon-accept',
                                    text: 'Восстановить'
                                }
                            ]
                        }
                    ]
                },
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