Ext.define('B4.view.version.RecordsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.versionrecordsgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',        
        'B4.store.version.VersionRecord',
        'B4.enums.VersionActualizeType',
        'B4.form.ComboBox'
    ],

    title: 'Записи версии',
    closable: false,
    enableColumnHide: true,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.version.VersionRecord');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListByOperator'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElements',
                    flex: 1,
                    text: 'Конструктивные элементы',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrectYear',
                    text: 'Скорректированный год',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function(value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Point',
                    text: 'Балл очередности',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 110,
                    renderer: function(value) {
                        return Ext.util.Format.currency(value);
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
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'btnState',
                                    iconCls: 'icon-accept',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать ДПКР',
                                    textAlign: 'left',
                                    action: 'actualize',
                                    menu: [
                                        {
                                            text: 'Добавить новые записи',
                                            action: 'ActualizeNewRecords',
                                            itemId: 'actualizeNewRecordsItem',
                                            additionalParams: [
                                                // Исключенные ООИ
                                                // 10 - "Лифты"
                                                [ 'groupTypeCodes', [ 10 ] ],
                                                [ 'groupTypeCodeIncluding', false ],
                                                [ 'versionActualizeType', B4.enums.VersionActualizeType.ActualizeNewRecords ]
                                            ]
                                        },
                                        {
                                            text: 'Добавить новые записи по ООИ "Лифты"',
                                            action: 'ActualizeNewRecords',
                                            itemId: 'actualizeLiftNewRecordsItem',
                                            withOutWindow: true,
                                            additionalConfigParams: [
                                                {
                                                    configName: 'Overhaul.OverhaulTat',
                                                    params: [
                                                        [ 'yearStart', 'ActualizePeriodEnd', (val) => { return ++val; } ]
                                                    ]
                                                }
                                            ],
                                            additionalParams: [
                                                // Только по указанным ООИ
                                                // 10 - "Лифты"
                                                [ 'groupTypeCodes', [ 10 ] ],
                                                [ 'groupTypeCodeIncluding', true ],
                                                [ 'versionActualizeType', B4.enums.VersionActualizeType.ActualizeLiftNewRecords ]
                                            ]
                                        },
                                        {
                                            text: 'Актуализировать стоимость',
                                            action: 'ActualizeSum'
                                        },
                                        {
                                            text: 'Актуализировать год',
                                            action: 'ActualizeYear'
                                        },
                                        {
                                            text: 'Удалить лишние записи',
                                            action: 'ActualizeDeletedEntries'
                                        },
                                        {
                                            text: 'Группировка ООИ',
                                            action: 'ActualizeGroup'
                                        },
                                        {
                                            text: 'Рассчитать очередность',
                                            action: 'ActualizeOrder'
                                        },
                                        {
                                            text: 'Актуализировать из КПКР',
                                            action: 'ActualizeFromShortCr'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                },
                                {
                                    xtype: 'b4updatebutton'
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