Ext.define('B4.view.claimwork.LawSuitDebtWorkSSPGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.lawsuitsspgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.ComboBox',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.LawsuitCollectionDebtDocumentType',
        'B4.store.claimwork.LawSuitDebtWorkSSP'
    ],
    title: 'Долевое ИП',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.LawSuitDebtWorkSSP');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
            
                {
                    xtype: 'b4enumcolumn',
                    text: 'Тип документа',
                    filter: true,
                    flex: 1,
                    enumName: 'B4.enums.LawsuitCollectionDebtDocumentType',
                    dataIndex: 'CbDocumentType'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    dataIndex: 'LawsuitOwnerInfo',
                    text: 'Собственник',
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    dataIndex: 'CbNumberDocument',
                    text: 'Номер',
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CbDateDocument',
                    text: 'Дата',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CbFile',
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