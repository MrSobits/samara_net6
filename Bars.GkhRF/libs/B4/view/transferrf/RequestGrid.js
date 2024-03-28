Ext.define('B4.view.transferrf.RequestGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.grid.feature.Summary',
        
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        
        'B4.enums.TypeProgramRequest'
    ],
    alias: 'widget.requesttransferrfgrid',
    store: 'transferrf.Request',
    itemId: 'requestTransferRfGrid',

    features: [{
        ftype: 'b4_summary'
    }],

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
                    scope: me,
                    width: 20,
                    itemId: 'transferRfRecCopy',
                    icon: 'content/img/icons/page_copy.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        Ext.MessageBox.confirm('Подтверждение', 'Вы действительно хотите скопировать заявку?', function (btn) {

                            if (btn == 'yes') {
                                B4.Ajax.request({
                                    url: B4.Url.action('Copy', 'TransferRf'),
                                    params: {
                                        transferRfRecordId: rec.get('Id')
                                    }
                                }).next(function(msg) {

                                    var obj = Ext.JSON.decode(msg.responseText);
                                    var model = gridView.getStore().model;
                                    var record = new model({ Id: obj.data.Id });

                                    gridView.ownerCt.fireEvent('rowaction', gridView.ownerCt, 'edit', record);

                                    return true;
                                }).error(function(msg) {
                                    Ext.Msg.alert('Ошибка', 'Произошла ошибка при копировании');
                                    return true;
                                });
                            }
                        });
                    }
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    itemId: 'transferRfRecPrintColumn',
                    icon: 'content/img/icons/printer.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { gisuRequestTransferId: rec.get('Id') };
                        this.params = {};
                        this.params.userParams = Ext.JSON.encode(param);

                        Ext.apply(this.params, { reportId: 'GisuRequestTransferForm' });
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
                    scope: this,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'rf_request_transfer';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 80,
                    text: 'Номер заявки',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    width: 100,
                    text: 'Дата заявки',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeProgramRequest',
                    flex: 2,
                    text: 'Программа',
                    renderer: function (val) { return B4.enums.TypeProgramRequest.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeProgramRequest.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManagingOrganizationName',
                    flex: 2,
                    text: 'Управляющая компания',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TransferFundsCount',
                    width: 90,
                    text: 'Количество объектов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TransferFundsSum',
                    width: 130,
                    text: 'Итого сумма',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    align: 'right',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    itemId: 'gcRequestTransferRfDeleteColumn'
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnRequestTransferRfExport'
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