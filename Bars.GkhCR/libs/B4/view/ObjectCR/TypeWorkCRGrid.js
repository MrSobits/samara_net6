Ext.define('B4.view.objectcr.TypeWorkCrGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.feature.GroupingSummaryTotal',
        
        'B4.enums.TypeWork'
    ],

    alias: 'widget.objectcr_type_work_cr_grid',
    
    title: 'Виды работ',
    closable: false,

    // необходимо для того чтобы неработали восстановления для грида поскольку колонки показываются и скрываются динамически
    provideStateId: Ext.emptyFn,
    stateful: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.TypeWorkCr');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    align: 'center',
                    type: 'changeyear',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/page_copy.png'),
                    scope: me,
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'changeyear', rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Тип работы',
                    renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    flex: 3,
                    text: 'Вид работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumMaterialsRequirement',
                    flex: 1,
                    text: 'Потребность материалов (руб.)',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasPsd',
                    flex: 1,
                    text: 'Наличие ПСД',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    text: 'Долгосрочная программа',
                    name: 'groupDpkr',
                    flex: 3,
                    columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DpkrUnitMeasure',
                                width: 100,
                                text: 'Ед. изм.'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DpkrVolume',
                                width: 80,
                                text: 'Объем',
                                renderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DpkrSum',
                                width: 100,
                                text: 'Сумма (руб.)',
                                renderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                },
                                summaryType: 'sum',
                                summaryRenderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                }
                            }
                    ]
                },
                {
                    text: 'Краткосрочная программа',
                    flex: 3,
                    name: 'groupCr',
                    columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UnitMeasureName',
                                width: 80,
                                text: 'Ед. изм.'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Volume',
                                flex: 1,
                                text: 'Объем',
                                width: 80,
                                renderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Sum',
                                width: 100,
                                text: 'Сумма (руб.)',
                                renderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                },
                                summaryType: 'sum',
                                summaryRenderer: function (val) {
                                    return val ? Ext.util.Format.currency(val) : '';
                                }
                            }
                    ]
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [
                {
                    ftype: 'groupingsummarytotal',
                    groupHeaderTpl: '{name}'
                }
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
                                // специально добавляю такую кнопку чтобы не привязываться к b4addbutton иначе сработает другой аспект который в контроллере будет не так добавлять
                                // вообщем добавляю 2 кнопки которые в контроллере либо скрываются либо открываются в зависимости от Признака Программы
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Добавить',
                                    iconCls: 'icon-add',
                                    hidden: true,
                                    name: 'AddButtonDpkr',
                                    menu: [
                                        {
                                            text: 'Работа из ДПКР',
                                            action: 'WorkDpkr'
                                        },
                                        {
                                            text: 'Услуга/Доп. работа',
                                            action: 'ServiceAdditionalWork'
                                        }
                                    ]
                                },
                                { xtype: 'b4updatebutton' }
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