Ext.define('B4.view.version.RecordsGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.versionrecordsgrid',
    title: 'Долгосрочная программа',
    enableColumnHide: true,

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',        
        'B4.store.version.VersionRecord',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo'
    ],
  
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.version.VersionRecord');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,

            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
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
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HouseNumber',
                    flex: 0.5,
                    text: 'Номер дома',
                    filter: {
                        xtype: 'textfield', operand: CondExpr.operands.eq
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
                    dataIndex: 'Remark',
                    flex: 1,
                    text: 'Примечание',
                    hidden: false,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElements',
                    flex: 1,
                    text: 'Конструктивные элементы',
                    hidden: false,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KPKR',
                    flex: 1,
                    text: 'КПКР',
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
                    dataIndex: 'FixedYear',
                    width: 150,
                    text: 'Зафиксировано',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearCalculated',
                    text: 'Расчетный год',
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
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Changes',
                    width: 150,
                    text: 'Изменения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsChangedYear',
                    width: 150,
                    text: 'Плановый год изменен',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Hidden',
                    width: 150,
                    text: 'Удаленный',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsSubProgram',
                    width: 150,
                    text: 'Подпрограмма',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'actioncolumn',
                    action: 'hide',
                    width: 18,
                    hidden: false,
                    text: 'Удалить',
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/script_delete.png'),
                    tooltip: 'Удалить',
                },
                {
                    xtype: 'actioncolumn',
                    action: 'restore',
                    width: 18,
                    text: 'Вернуть',
                    hidden: false,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/script_add.png'),
                    tooltip: 'Вернуть',
                },
                {
                    xtype: 'actioncolumn',
                    action: 'insubdpkr',
                    width: 18,
                    align: 'center',
                    hidden: false,
                    text: 'Перенести в подпрограмму',
                    icon: B4.Url.content('content/img/icons/script_delete.png'),
                    tooltip: 'Перенести в подпрограмму',
                },
                {
                    xtype: 'actioncolumn',
                    action: 'reinsubdpkr',
                    width: 18,
                    align: 'center',
                    hidden: false,
                    text: 'Вернуть из подпрограммы',
                    icon: B4.Url.content('content/img/icons/script_add.png'),
                    tooltip: 'Вернуть из подпрограммы',
                },
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
                            columns: 11,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    width: 80,
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    width: 80,
                                },
                                {
                                    xtype: 'button',
                                    text: 'Очередность',
                                    textAlign: 'left',
                                    action: 'order',
                                    dataIndex: 'Order',
                                    menu: [
                                        {
                                            text: 'Рассчитать очередность 1 этапа',
                                            action: 'Order1Stage'
                                        },
                                        {
                                            text: 'Рассчитать очередность 2 этапа',
                                            action: 'Order2Stage'
                                        },
                                        {
                                            text: 'Рассчитать очередность 3 этапа',
                                            action: 'Order3Stage'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'export'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать ДПКР',
                                    textAlign: 'left',
                                    action: 'actualize',
                                    dataIndex: 'Actualize',
                                    menu: [
                                        {
                                            text: 'Добавить новые записи',
                                            action: 'AddNewRecords'
                                        },
                                        {
                                            text: 'Рассчитать очередность',
                                            action: 'ActualizePriority'
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
                                            text: 'Актуализировать из КПКР',
                                            action: 'ActualizeFromShortCr'
                                        },
                                        {
                                            text: ' Актуализация изменения года',
                                            action: 'ActualizeYearForStavropol'
                                        },
                                        {
                                            text: ' Актуализировать основную версию',
                                            action: 'ActualizeByFilters'
                                        },
                                        {
                                            text: ' Актуализировать подпрограмму',
                                            action: 'ActualizeSubrecord'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать ДПКР СК',
                                    textAlign: 'left',
                                    dataIndex: 'StavrActualize',
                                    action: 'actualize',
                                    menu: [
                                        {
                                            text: '1.Корректировка КЭ "Кровля"',
                                            action: 'RoofCorrection'
                                        },
                                        {
                                            text: '2.Удалить лишние записи',
                                            action: 'ActualizeDeletedEntries'
                                        },
                                        {
                                            text: '3.Добавить новые записи',
                                            action: 'AddNewRecords'
                                        },
                                        {
                                            text: '4.Актуализация изменения года',
                                            action: 'ActualizeYearForStavropol'
                                        },
                                        {
                                            text: '5.Актуализировать из КПКР',
                                            action: 'ActualizeFromShortCr'
                                        },
                                        {
                                            text: '6.Актуализировать стоимость',
                                            action: 'ActualizeSum'
                                        },
                                        {
                                            text: '7.Рассчитать очередность',
                                            action: 'ActualizePriority'
                                        },
                                        {
                                            text: '8.Скопировать скорректированные года работ в плановые',
                                            action: 'CopyCorrectedYears'
                                        },
                                        {
                                            text: '9.Удалить повторные работы',
                                            action: 'DeleteRepeatedWorks'
                                        }                      
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    text: 'Массовое изменение года',
                                    textAlign: 'left',
                                    action: 'massyearchange'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Создать КПКР из ДПКР',
                                    textAlign: 'left',
                                    action: 'makeKPKR'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Создать подпрограмму КПКР из ДПКР',
                                    textAlign: 'left',
                                    action: 'makeSubKPKR'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-book-go',
                                    text: 'Опубликовать как есть',
                                    textAlign: 'left',
                                    action: 'publish',
                                },        
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-coins-add',
                                    text: 'Перерасчитать стоимость',
                                    textAlign: 'left',
                                    action: 'calculatecosts',
                                },
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