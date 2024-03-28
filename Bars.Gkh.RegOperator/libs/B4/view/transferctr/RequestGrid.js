Ext.define('B4.view.transferctr.RequestGrid', {
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
    
    alias: 'widget.requesttransferctrgrid',
    store: 'transferrf.TransferCtr',
    itemId: 'requestTransferCtrGrid',
    enableColumnHide: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel'),
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
                    itemId: 'transferCtrRecCopy',
                    icon: 'content/img/icons/page_copy.png',
                    tooltip: "Копирование заявки на перечисление средств подрядчикам",
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        Ext.MessageBox.confirm('Подтверждение', 'Вы действительно хотите скопировать заявку?', function (btn) {

                            if (btn == 'yes') {
                                B4.Ajax.request({
                                    url: B4.Url.action('Copy', 'TransferCtr'),
                                    params: {
                                        transferCtrRecordId: rec.get('Id')
                                    },
                                    timeout: 5 * (60 * 1000) //5 минут
                                }).next(function (msg) {

                                    var obj = Ext.JSON.decode(msg.responseText);
                                    var model = gridView.getStore().model;
                                    var record = new model({ Id: obj.data.Id });

                                    gridView.ownerCt.fireEvent('rowaction', gridView.ownerCt, 'edit', record);

                                    return true;
                                }).error(function (msg) {
                                    Ext.Msg.alert('Ошибка', 'Произошла ошибка при копировании');
                                    return true;
                                });
                            }
                        });
                    }
                },
                {
                    xtype: 'actioncolumn',
                    hideable: false,
                    width: 20,
                    itemId: 'transferCtrPrintColumn',
                    icon: 'content/img/icons/printer.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { transferCtrId: rec.get('Id') };
                        this.params = {};
                        this.params.userParams = Ext.JSON.encode(param);

                        Ext.apply(this.params, { reportId: 'TransferCtrForm' });
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
                    hideable: false,
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
                    filter: { xtype: 'textfield' }
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
                    dataIndex: 'ProgramCr',
                    text: 'Программа',
                    flex:1 ,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
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
                    dataIndex: 'ObjectCr',
                    width: 60,
                    flex:2,
                    text: 'Объект',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Builder',
                    flex: 1,
                    text: 'Подрядная организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contract',
                    flex: 1,
                    text: 'Договор подряда',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
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
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidSum',
                    flex: 1,
                    text: 'Оплачено',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    align: 'right',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsExport',
                    flex: 1,
                    text: 'Сформирован документ',
                    renderer: function(value) {
                        return value === true ? 'Да' : 'Нет';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BuilderInn',
                    flex: 1,
                    text: 'ИНН подрядной организации',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BuilderSettlAcc',
                    flex: 1,
                    text: 'Счет подрядной организации',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CalcAccNumber',
                    flex: 1,
                    text: 'Счет плательщика',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Perfomer',
                    hidden: true,
                    flex: 1,
                    text: 'Исполнитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Document',
                    flex: 1,
                    text: 'Документ',
                    renderer: function (value) {
                        if (value) {
                            var fileId = value.Id;
                            if (fileId > 0) {
                                var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                                return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                            }
                            return '';
                        }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnRequestTransferCtrExport'
                                },
                                {
                                    xtype: 'button',
                                    action: 'ExportToTxt',
                                    iconCls: 'icon-table-go',
                                    text: 'Сформировать документ'
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