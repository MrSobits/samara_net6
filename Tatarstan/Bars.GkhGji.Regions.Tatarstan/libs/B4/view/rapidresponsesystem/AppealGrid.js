Ext.define('B4.view.rapidresponsesystem.AppealGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.store.rapidresponsesystem.AppealDetails'
    ],

    alias: 'widget.rapidResponseSystemAppealGrid',
    store: 'rapidresponsesystem.AppealDetails',
    title: 'Оперативное реагирование на обращение граждан',
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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус обращения',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_rapid_response_system_appeal_details';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    text: 'Номер обращения',
                    filter: {xtype: 'textfield'},
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес дома',
                    filter: {xtype: 'textfield'},
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    text: 'Контрагент',
                    filter: {xtype: 'textfield'},
                    flex: 2
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AppealDate',
                    text: 'Дата обращения',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ReceiptDate',
                    text: 'Дата поступления',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ControlPeriod',
                    text: 'Контрольный срок',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Subjects',
                    text: 'Тематики обращений',
                    filter: {xtype: 'textfield'},
                    flex: 2
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    if (record.data.IsWarningControlPeriod) {
                        return 'back-red';
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'appealFilterPanel',
                    padding: '5 0 5 5',
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                    },
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    itemId: 'mainFilter',
                                    margin: '0 0 -5 0',
                                    defaults: {
                                        margin: '0 0 5 0'
                                    },
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {

                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'AppealDateFrom',
                                                    fieldLabel: 'Дата обращения с',
                                                    labelWidth: 110,
                                                    flex: 1.5,
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'AppealDateTo',
                                                    fieldLabel: 'по',
                                                    labelWidth: 30,
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ControlPeriodFrom',
                                                    fieldLabel: 'Контрольный срок с',
                                                    labelWidth: 110,
                                                    flex: 1.5,
                                                    format: 'd.m.Y',
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ControlPeriodTo',
                                                    fieldLabel: 'по',
                                                    labelWidth: 30,
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'RealityObjects',
                                            fieldLabel: 'Жилой дом',
                                            labelAlign: 'right',
                                            labelWidth: 110,
                                            itemId: 'tfRealityObject'
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'Contragents',
                                            fieldLabel: 'Контрагент',
                                            labelAlign: 'right',
                                            labelWidth: 110,
                                            itemId: 'tfContragent'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 3,
                                    layout: {
                                        type: 'vbox',
                                        pack: 'end'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4updatebutton',
                                            margin: '0 0 0 5',
                                            width: 100
                                        }
                                    ]
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '5 0 0 5',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузить в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport',
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