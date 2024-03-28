Ext.define('B4.view.objectcr.BuildContractTerminationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.buildcontractterminationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn'
    ],
    itemId: 'buildContractTerminationGrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.BuildContractTermination');

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
                    dataIndex: 'DocumentNumber',
                    flex: 0.5,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'TerminationDate',
                    flex: 0.5,
                    text: 'Дата расторжения',
                    filter: { xtype: 'datefield', format: 'd.m.Y' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Основание расторжения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DictReason',
                    flex: 1,
                    text: 'Причина расторжения из справочника',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/TerminationReason/List'
                    },
                    renderer: function (val) {
                        if (val != null) {
                            return val.Name;
                        }
                        else {
                            return val;
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentFile',
                    flex: 1,
                    text: 'Документ-основание',
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
                                {
                                    xtype: 'button',
                                    text: 'Добавить',
                                    iconCls: 'icon-accept',
                                    itemId: 'addTerminationButton',
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    iconCls: 'icon-arrow-refresh',
                                    itemId: 'refreshTerminationButton',
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