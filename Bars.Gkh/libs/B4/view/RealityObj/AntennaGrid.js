Ext.define('B4.view.realityobj.AntennaGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjantennagrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.enums.YesNoMinus',
        'B4.enums.YesNoNotSet',
        'B4.enums.AntennaRange',
        'B4.enums.AntennaReason'
    ],

    title: 'Сведения о СКПТ',
    store: 'realityobj.RealityObjectAntenna',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Availability',
                    text: 'Наличие СКПТ',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Workability',
                    text: 'Работоспособность СКПТ',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.YesNoMinus.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Range',
                    flex: 1,
                    text: 'Диапазон',
                    renderer: function (val) {
                        return B4.enums.AntennaRange.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FrequencyFrom',
                    flex: 1,
                    text: 'Частота от'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FrequencyTo',
                    flex: 1,
                    text: 'Частота до'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberApartments',
                    flex: 1,
                    text: 'Количество квартир, использующих СКПТ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Причины отсутствия СКПТ',
                    enumName: 'B4.enums.AntennaReason',
                    renderer: function (val) {
                        return B4.enums.AntennaReason.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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