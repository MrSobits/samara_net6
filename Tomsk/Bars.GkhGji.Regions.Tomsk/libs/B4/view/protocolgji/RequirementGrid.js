Ext.define('B4.view.protocolgji.RequirementGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.enums.TypeRequirement',
        'B4.store.protocol.ProtocolRequirement'
    ],

    alias: 'widget.protocolrequirementgrid',
    title: 'Требования',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.protocol.ProtocolRequirement');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 175,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gji_document_requirement';
                            },
                            storeloaded: {
                                fn: function(me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function(type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'DocumentNumber',
                    text: 'Номер документа',
                    width: 100,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'ArticleLaw',
                    text: 'Статьи закона',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'Destination',
                    text: 'Адресат',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'TypeRequirement',
                    flex: 1,
                    text: 'Тип требования',
                    renderer: function(val) {
                        return B4.enums.TypeRequirement.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        operand: CondExpr.operands.eq,
                        store: B4.enums.TypeRequirement.getItemsWithEmpty([null, '-']),
                        displayField: 'Display',
                        valueField: 'Value',
                        editable: false
                    }
                },
                {
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function(v) {
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});