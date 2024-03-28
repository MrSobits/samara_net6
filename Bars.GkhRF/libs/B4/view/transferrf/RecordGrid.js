Ext.define('B4.view.transferrf.RecordGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.transferrfrecordgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Сведения о перечислениях по договору',
    store: 'transferrf.Record',
    itemId: 'transferRfRecordGrid',

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
                    xtype: 'actioncolumn',
                    width: 20,
                    itemId: 'transferRfRecPrintColumn',
                    icon: 'content/img/icons/printer.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { transferRfRecordId: rec.get('Id') };
                        this.params = {};
                        this.params.userParams = Ext.JSON.encode(param);

                        Ext.apply(this.params, { reportId: 'TransferRfRecord' });
                        var urlParams = Ext.urlEncode(this.params);

                        var newUrl = Ext.urlAppend('/GkhReport/ReportPrint/?' + urlParams, '_dc=' + (new Date().getTime()));
                        newUrl = B4.Url.action(newUrl);

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: newUrl
                        });
                    }
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 200,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TransferDate',
                    flex: 1,
                    format: 'd.m.Y',
                    text: 'Месяц перечисления'
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
                    dataIndex: 'CountRecords',
                    flex: 1,
                    text: 'Итого объектов'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumRecords',
                    flex: 1,
                    text: 'Итого перечисленно'
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