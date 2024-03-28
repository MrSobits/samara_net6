Ext.define('B4.view.claimwork.LawSuitDebtWorkSSPDocumentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.lawsuitsspdocgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ClaimWork.TypeLawsuitDocument',
        'B4.store.claimwork.LawSuitDebtWorkSSPDocument'
    ],
    minHeight: 350,

    title: 'Документы',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.LawSuitDebtWorkSSPDocument');

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
                    flex: 1,
                    dataIndex: 'TypeLawsuitDocument',
                    text: 'Документ',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ClaimWork.TypeLawsuitDocument.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) { return B4.enums.ClaimWork.TypeLawsuitDocument.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    dataIndex: 'NumberString',
                    text: 'Номер',
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    text: 'Дата',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
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
                            items: [
                                { xtype: 'b4addbutton' },
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