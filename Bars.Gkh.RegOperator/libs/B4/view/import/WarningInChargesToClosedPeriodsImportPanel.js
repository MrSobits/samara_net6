/**
 * Форма предупреждений по импорту начислений в закрытый период
 */
Ext.define('B4.view.import.WarningInChargesToClosedPeriodsImportPanel', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNo'
    ],
    title: 'Импорт начислений (в закрытые периоды) предупреждения',
    closable: true,
    alias: 'widget.warninginchargestoclosedperiodsimportpanel',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.AccountWarningInChargesToClosedPeriodsImport'),
            existWarningsStore = Ext.create('B4.store.import.ExistWarningInChargesToClosedPeriodsImport');

        Ext.applyIf(me, {
            columLines: true,
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'actioncolumn',
                    sortable: false,
                    iconCls: 'icon-link-add',
                    tooltip: 'Сопоставить вручную',
                    width: 30,
                    align: 'center',
                    handler: function (gridView, rowIndex, clollIndex, el, e, rec) {
                        var scope = this.up('grid');
                        scope.fireEvent('rowaction', scope, 'manualcompare', rec);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    dataIndex: 'IsCanAutoCompared',
                    text: 'Автоматическое сопоставление',
                    width: 180,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    dataIndex: 'IsProcessed',
                    text: 'Обработана',
                    width: 100,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalNumber',
                    text: 'Внешний номер ЛС',
                    width: 130,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalRkcId',
                    text: 'Внешний идентификатор РКЦ',
                    width: 180,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'ФИО',
                    width: 150,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    width: 350,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComparingInfo',
                    text: 'Информация по сопоставленияю',
                    width: 300,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Title',
                    text: 'Заголовок',
                    width: 100,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Message',
                    text: 'Сообщение',
                    width: 100,
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
                    style: 'background: none repeat scroll 0 0 #DFE9F6; padding: 20px;',
                    layout: {
                        type: 'anchor'
                    },
                    items: [
                        {
                            xtype: 'gridpanel',
                            itemId: 'existWarningsGrid',
                            collapsible: true,
                            title: 'Предупреждения про существование начислений',
                            store: existWarningsStore,
                            columLines: true,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Title',
                                    text: 'Заголовок',
                                    flex: 0.5,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Message',
                                    text: 'Сообщение',
                                    flex: 0.5,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'ChargeDescriptorName',
                                    text: 'Описатель',
                                    width: 200,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: existWarningsStore,
                            dock: 'bottom'
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
                                    itemId: 'confirmAutoCompareButton',
                                    text: 'Подтвердить автоматическое сопоставление',
                                    textAlign: 'left',
                                    iconCls: 'x-btn-icon icon-accept',
                                    action: 'ConfirmAutoCompare',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'manualCompareButton',
                                    text: 'Сопоставить вручную',
                                    textAlign: 'left',
                                    iconCls: 'x-btn-icon icon-link-add',
                                    action: 'ManualCompare',
                                    disabled: true
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