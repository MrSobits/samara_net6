Ext.define('B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.KindWorkOutdoor',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],

    alias: 'widget.typeworkrealityobjectoutdoorgrid',

    title: 'Виды работ',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectoutdoorcr.typeworkrealityobjectoutdoor.TypeWorkRealityObjectOutdoor');

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
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Тип работы',
                    enumName: 'B4.enums.KindWorkOutdoor'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkRealityObjectOutdoorName',
                    flex: 3,
                    text: 'Вид работы'
                },
                {
                    text: 'Программа по благоустройству дворов',
                    name: 'realityObjectOutdoorProgram',
                    flex: 3,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UnitMeasure',
                            width: 100,
                            text: 'Ед. изм.'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Volume',
                            width: 80,
                            text: 'Объем',
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
            viewConfig: {
                loadMask: true
            },
            features: [
                {
                    ftype: 'groupingsummarytotal',
                    groupHeaderTpl: '{name}'
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
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Добавить',
                                    iconCls: 'icon-add',
                                    name: 'AddButton',
                                    menu: [
                                        {
                                            text: 'Основная работа',
                                            action: 'addMainWork'
                                        },
                                        {
                                            text: 'Дополнительная работа',
                                            action: 'addAdditionalWork'
                                        }
                                    ]
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