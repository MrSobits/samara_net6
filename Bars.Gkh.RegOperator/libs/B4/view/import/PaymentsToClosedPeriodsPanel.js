/**
 * Форма импорта оплат в закрытый период
 */
Ext.define('B4.view.import.PaymentsToClosedPeriodsPanel', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'Ext.ux.CheckColumn',
        'B4.enums.TaskStatus',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.CashPaymentCenter',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum'        
    ],
    title: 'Импорт оплат (в закрытые периоды)',
    alias: 'widget.paymentstoclosedperiodspanel',
    closable: true,

    initComponent: function () {
        var me = this,
            periodStore = Ext.create('B4.store.regop.ClosedChargePeriod'),
            runningTasksStore = Ext.create('B4.store.import.closedperiodsimport.RunningTasks'),
            logsStore = Ext.create('B4.store.import.closedperiodsimport.Logs');

        Ext.applyIf(me, {
            columnLines: true,
            store: logsStore,
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'SINGLE'
            }),
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 30,
                    sortable: false,
                    tooltip: 'Открыть журнал',
                    align: 'center',
                    name: 'ViewDetails',
                    renderer: function (value, meta, record) {
                        var wc = record.get('CountWarning');
                        var ec = record.get('CountError');
                        if (wc === 0 && ec === 0) {
                            this.iconCls = 'icon-accept';
                        }
                        else {
                            if (wc !== 0) {
                                this.iconCls = 'icon-error';
                            }
                            if (ec !== 0) {
                                this.iconCls = 'icon-exclamation';
                            }
                        }

                        // Переход к деталям не доступен по ролевым ограничениям. Устанавливается в gkhpermissionaspect контроллера.
                        if (!this.isAllowed) {
                            this.tooltip = '';
                            this.iconCls += ' x-action-col-img-noaction'; // Убрать курсор «Рука»
                        }

                        return value;
                    },
                    handler: function (gridView, rowIndex, clollIndex, el, e, rec) {
                        // Переход к деталям доступен по ролевым ограничениям. Устанавливается в gkhpermissionaspect контроллера.
                        if (this.isAllowed) {
                            var scope = this.up('grid');
                            scope.fireEvent('rowaction', scope, 'gotoresult', rec);
                        }
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',                    
                    text: 'Дата запуска',
                    width: 200,
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileName',
                    text: 'Файл',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountError',
                    text: 'Количество ошибок',
                    width: 200,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountWarning',
                    text: 'Количество предупреждений',
                    width: 200,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                }
            ],            
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: logsStore,
                    dock: 'bottom'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    style: 'background: none repeat scroll 0 0 #DFE9F6; padding-right: 10px;',
                    layout: {
                        type: 'anchor'
                    },
                    defaults: {
                        anchor: '100%',
                        layout: {
                            type: 'anchor'
                        }
                    },
                    items: [
                        {
                            xtype: 'container',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                allowBlank: false,
                                layout: {
                                    type: 'anchor'
                                }
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    itemId: 'ctnText',
                                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите период и импортируемые данные. Допустимые типы файлов: txt.</span>'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.CashPaymentCenter',
                                    textProperty: 'Name',
                                    labelWidth: 60,                                    
                                    labelAlign: 'right',
                                    editable: false,
                                    emptyText: 'Не выбран',
                                    windowContainerSelector: '#' + me.getId(),
                                    windowCfg: {
                                        modal: true
                                    },
                                    columns: [
                                        {
                                            text: 'Внешний ид.',
                                            dataIndex: 'Identifier',
                                            width: 90,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    name: 'CashPaymentCenter',
                                    fieldLabel: 'РКЦ'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Period',
                                    fieldLabel: 'Период',
                                    labelWidth: 60,
                                    emptyText: 'Не выбран',
                                    windowCfg: {
                                        modal: true
                                    },
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            text: 'Наименование',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            dataIndex: 'StartDate',
                                            text: 'Дата начала',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            dataIndex: 'EndDate',
                                            text: 'Дата окончания',
                                            format: 'd.m.Y'
                                        }
                                    ],
                                    store: periodStore
                                },
                                {
                                    xtype: 'form',
                                    border: false,
                                    bodyStyle: Gkh.bodyStyle,
                                    name: 'paymentsToClosedPeriodsImportForm', // Указывается в аспекте импорта
                                    itemId: 'importForm',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 60
                                    },
                                    items: [
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileImport',
                                            fieldLabel: 'Файл',
                                            allowBlank: false,
                                            flex: 1,
                                            itemId: 'fileImport',
                                            possibleFileExtensions: 'txt',
                                            disabled: true
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Загрузить',
                                            tooltip: 'Загрузить',
                                            iconCls: 'icon-accept',
                                            action: 'import',
                                            disabled: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Не обновлять сальдо',
                                    fieldStyle: 'vertical-align: middle;',
                                    style: 'font-size: 11px !important;',
                                    margin: '0 0 0 65',
                                    action: 'ShowAll',
                                    width: 130,
                                    name: 'UpdateSaldoIn'
                                },                                                               
                                {
                                    xtype: 'displayfield',
                                    itemId: 'log'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    style: 'background: #DFE9F6; padding: 5px 20px 20px 20px;',
                    anchor: '100%',
                    items: [

                            {
                                xtype: 'gridpanel',
                                itemId: 'runningTasksGrid',
                                collapsible: true,
                                title: 'Текущие задачи',
                                store: runningTasksStore,
                                columnLines: true,
                                columns: [
                                    {
                                        xtype: 'datecolumn',
                                        dataIndex: 'ObjectCreateDate',                                        
                                        text: 'Дата запуска',
                                        width: 150,
                                        format: 'd.m.Y H:i:s',
                                        filter: {
                                            xtype: 'datefield',
                                            operand: CondExpr.operands.eq,
                                            format: 'd.m.Y H:i:s'
                                        }
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Percentage',
                                        text: 'Процент выполнения',
                                        width: 250,
                                        filter: {
                                            xtype: 'numberfield',
                                            operand: CondExpr.operands.eq,
                                            minValue: 0,
                                            maxValue: 100
                                        }
                                    },
                                    {
                                        xtype: 'b4enumcolumn',
                                        enumName: 'B4.enums.TaskStatus',
                                        dataIndex: 'Status',
                                        header: 'Статус',
                                        flex: 1,
                                        filter: true,
                                        sortable: false
                                    }
                                ],                                
                                viewConfig: {
                                    loadMask: true
                                },
                            },
                            {
                                xtype: 'b4pagingtoolbar',
                                displayInfo: true,
                                store: runningTasksStore
                            }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'reImportButton',
                                    text: 'Повторить импорт',
                                    iconCls: 'icon-build',
                                    action: 'ReImport',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});