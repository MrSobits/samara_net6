Ext.define('B4.view.regop.periodclosecheck.ResultsGrid',
{
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.PeriodCloseCheckStateType',
        'B4.form.SelectField'
    ],

    title: 'Проверка и закрытие месяца',

    alias: 'widget.periodclosecheckresultsgrid',

    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.period.CloseCheckResult');

        Ext.applyIf(me,
        {
            store: store,
            columns: [
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1,
                    minWidth: 250
                },
                {
                    text: 'Обязательность',
                    dataIndex: 'IsCritical',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        items: [[null, 'Все'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Статус',
                    dataIndex: 'CheckState',
                    enumName: 'B4.enums.PeriodCloseCheckStateType',
                    filter: true
                },
                {
                    text: 'Группа ЛС',
                    dataIndex: 'PersAccGroup',
                    width: 300,
                    renderer: function (val, meta) {
                        var v = val && val.Name;

                        if (v) {
                            meta.style = 'cursor:pointer;';
                        }

                        return v;
                    }
                },
                {
                    text: 'Отчет',
                    dataIndex: 'LogFile',
                    width: 75,
                    renderer: function(v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    text: 'Дата проверки',
                    dataIndex: 'CheckDate',
                    width: 200
                },
                {
                    text: 'Пользователь',
                    dataIndex: 'User',
                    width: 150,
                    renderer: function (val) {
                        return val && (val.Name || val.Login);
                    }
                },
                {
                    text: 'Сообщение',
                    dataIndex: 'Note',
                    flex: 1,
                    minWidth: 100
                },
                {
                    text: 'Лог',
                    dataIndex: 'FullLogFile',
                    width: 75,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            padding: '5 0 0 0',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function(b) {
                                        b.up('grid').getStore().load();
                                    }
                                },
                                {
                                    xtype: 'b4selectfield',
                                    width: 300,
                                    name: 'ChargePeriod',
                                    fieldLabel: 'Расчетный месяц',
                                    labelAlign: 'right',
                                    labelWidth: 100,
                                    store: 'B4.store.regop.ChargePeriod',
                                    editable: false,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            text: 'Наименование',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'StartDate',
                                            width: 80,
                                            text: 'Начало',
                                            filter: { xtype: 'datefield', format: 'd.m.Y' }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'EndDate',
                                            width: 80,
                                            text: 'Окончание',
                                            filter: { xtype: 'datefield', format: 'd.m.Y' }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            padding: '5 0 5 0',
                            disabled: true,
                            name: 'bgChecks',
                            items: [
                                {
                                    xtype: 'button',
                                    name: 'RollbackClosedPeriod',
                                    text: 'Откатить закрытый период'
                                },
                                {
                                    xtype: 'button',
                                    name: 'RunChecks',
                                    text: 'Выполнить проверки'
                                },
                                {
                                    xtype: 'button',
                                    name: 'RunChecksAndClose',
                                    text: 'Закрыть месяц'
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing',
                {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});