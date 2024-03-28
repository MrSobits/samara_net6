Ext.define('B4.view.confirmcontribution.RecordGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.confirmContribRecordGrid',
    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Сведения о поступлении взносов',
    store: 'confirmcontribution.Record',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me,
                    getClass: function(val, meta, record, rowIndex)
                    {
                        var id = record.get('Id');
                        if(!id)
                        {
                            return 'x-hidden';
                        }

                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес дома',
                    summaryRenderer: function()
                    {
                        return Ext.String.format('Итого по всем загруженным платежкам:');
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TransferDate',
                    flex: 1,
                    format: 'M Y',
                    text: 'Период перечисления'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер платежного поручения'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата платежного поручения'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Amount',
                    flex: 1,
                    text: 'Сумма платежного поручения',
                    renderer: function(value)
                    {
                        return Ext.util.Format.currency(value);
                    },
                    decimalSeparator: ',',
                    summaryType: 'sum',
                    minAmount: 0,
                    summaryRenderer: function(value)
                    {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Scan',
                    flex: 1,
                    text: 'Скан документа',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    getClass: function (val, meta, record, rowIndex) {
                        var id = record.get('Id');
                        if (!id) {
                            return 'x-hidden';
                        }

                        return '';
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
                        items: [
                            {
                                xtype: 'buttongroup',
                                columns: 3,
                                items: [
                                    {
                                        xtype: 'b4addbutton'
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
                        store: this.store,
                        dock: 'bottom'
                    }
            ]
        });

        me.callParent(arguments);
    }
});