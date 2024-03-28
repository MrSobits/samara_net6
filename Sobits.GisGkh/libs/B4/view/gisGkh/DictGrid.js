Ext.define('B4.view.gisGkh.DictGrid', {
    //extend: 'Ext.tree.Panel',
    extend: 'B4.ux.grid.Panel', 
    alias: 'widget.gisgkhdictgrid',

    requires: [
        //'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.GisGkhListGroup'
    ],

    store: 'gisGkh.DictGridStore',
    title: 'Справочники',
    closable: true,
    enableColumnHide: true,
    

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
                    xtype: 'gridcolumn',
                    dataIndex: 'EntityName',
                    flex: 1,
                    text: 'Название класса в системе',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.GisGkhListGroup',
                    dataIndex: 'ListGroup',
                    text: 'Тип справочника',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisGkhCode',
                    flex: 1,
                    text: 'Код справочника',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisGkhName',
                    flex: 1,
                    text: 'Наименование справочника',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'MatchDate',
                    flex: 1,
                    text: 'Дата сопоставления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ModifiedDate',
                    flex: 1,
                    text: 'Дата модификации',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RefreshDate',
                    flex: 1,
                    text: 'Дата обновления',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: true,
                //    showClearAllButton: true,
                //    pluginId: 'headerFilter'
                //},
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
                            columns: 4,
                            items: [
                                //{
                                //    xtype: 'b4addbutton'
                                //},
                                {
                                    xtype: 'b4updatebutton'
                                },
                                //{
                                //    xtype: 'button',
                                //    text: 'Получить справочники',
                                //    tooltip: 'Получить справочники',
                                //    iconCls: 'icon-accept',
                                //    width: 150,
                                //    itemId: 'btnGetDictionaries'
                                //},
                                //{
                                //    xtype: 'button',
                                //    text: 'Получить ответы',
                                //    tooltip: 'Получить ответы',
                                //    iconCls: 'icon-accept',
                                //    width: 150,
                                //    itemId: 'btnCheckAnswers'
                                //}
                                //{
                                //    xtype: 'button',
                                //    text: 'История запросов',
                                //    tooltip: 'Посмотреть историю запросов',
                                //    iconCls: 'icon-accept',
                                //    width: 150,
                                //    itemId: 'btnGetPaymentsHistory'
                                //}
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
