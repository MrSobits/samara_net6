﻿Ext.define('B4.view.specialobjectcr.TypeWorkCrHistoryGridForDeleted', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeWorkCrHistoryAction',
        'B4.enums.TypeWorkCrReason'
    ],
    alias: 'widget.typeworkspecialcrhistorygridfordeleted',
    
    title: 'Журнал изменений',
    store: 'specialobjectcr.TypeWorkCrHistory',
    closable: false,
    cls: 'x-large-head',
    
    initComponent: function () {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
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
                    dataIndex: 'WorkName',
                    flex: 1,
                    text: 'Вид работы',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinanceSourceName',
                    flex: 1,
                    text: 'Источник финансирвоания',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'YearRepair',
                    flex: 1,
                    text: 'Год выполнения по Долгосрочной программе',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewYearRepair',
                    flex: 1,
                    text: 'Новый год выполнения',
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
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeReason',
                    flex: 1,
                    text: 'Причина',
                    renderer: function (val) {
                        if (val === 0)
                            return '';

                        return B4.enums.TypeWorkCrReason.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeWorkCrReason.getItemsWithEmpty([null, '-']),
                        valueField: 'Value',
                        displayField: 'Display',
                        editable: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateDoc',
                    flex: 1,
                    text: 'Дата документа',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameDoc',
                    flex: 1,
                    text: 'Документ (основание)',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileDoc',
                    flex: 1,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});