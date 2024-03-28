Ext.define('B4.view.emergencyobj.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.ux.grid.filter.YesNo',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.ConditionHouse'
    ],

    title: 'Реестр аварийных домов',
    store: 'EmergencyObject',
    alias: 'widget.emergencyObjGrid',
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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_emergency_object';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResettlementProgramName',
                    flex: 2,
                    text: 'Программа переселения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальный район',
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
                    dataIndex: 'Settlement',
                    width: 160,
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressName',
                    flex: 2,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsRepairExpedient',
                    flex: 1,
                    text: 'Ремонт целесообразен',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter:
                    {
                        xtype: 'b4dgridfilteryesno',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResettlementFlatAmount',
                    flex: 1,
                    text: 'Кол-во расселяемых жилых помещений',
                    filter:
                    {
                        xtype: 'gkhintfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResettlementFlatArea',
                    flex: 1,
                    text: 'Площадь расселяемых жилых помещений',
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConditionHouse',
                    hidden: true,
                    flex: 1,
                    text: 'Состояние дома',
                    renderer: function (val) {
                        return B4.enums.ConditionHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ConditionHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
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
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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