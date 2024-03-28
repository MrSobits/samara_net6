Ext.define('B4.view.manorglicense.LicenseGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.manorglicense.License',

        'B4.enums.ContragentState'
    ],

    title: 'Реестр лицензий',
    store: 'manorglicense.License',
    alias: 'widget.manorglicensegrid',
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
                    menuText: 'Статус',
                    width: 175,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_manorg_license';
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
                    dataIndex: 'ContragentState',
                    hidden: false,
                    width: 120,
                    text: 'Статус организации',
                    renderer: function (val) {
                        return B4.enums.ContragentState.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ContragentState.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'LicNum',
                //    text: 'Номер',
                //    filter: {
                //        xtype: 'numberfield',
                //        hideTrigger: true,
                //        minValue: 0,
                //        operand: CondExpr.operands.eq
                //    },
                //    width: 100
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LicNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'textfield',
                    },
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateIssued',
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    },
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateTermination',
                    text: 'Дата аннулирования',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisposalNumber',
                    flex: 0.5,
                    text: 'Номер приказа',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateDisposal',
                    text: 'Дата приказа',
                    flex: 0.5,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentMunicipality',
                    width: 160,
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
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                 {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    flex: 0.5,
                    text: 'ИНН',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                 {
                    xtype: 'gridcolumn',
                    dataIndex: 'Ogrn',
                    flex: 0.5,
                    text: 'ОГРН',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ERULNumber',
                    flex: 0.5,
                    text: 'Номер ЕРУЛ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ERULDate',
                    text: 'Дата ЕРУЛ',
                    flex: 0.5,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        ormat: 'd.m.Y'
                    },
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, me) {
                    state = record.get('State');
                    contragentState = record.get('ContragentState');
                    if ((contragentState != B4.enums.ContragentState.Active)
                        && (state.Code == "002" || state.Code == "004")
                    ) {
                        return 'back-red';
                    }
                    return '';
                }
            },
            dockedItems: [

                {
                    xtype: 'toolbar',
                    layout: 'vbox',
                    dock: 'top',
                    items: [
                       {
                            xtype: 'container',
                            border: false,
                            width: 750,
                            padding: '5 0 0 0',
                            layout: 'hbox',
                            defaults: {
                                format: 'd.m.Y',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 125,
                                    fieldLabel: 'Дата выдачи с',
                                    width: 290,
                                    itemId: 'dfDateFromStart',
                                    value: new Date(2015, 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    fieldLabel: 'по',
                                    width: 210,
                                    itemId: 'dfDateFromEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                         {
                             xtype: 'container',
                             border: false,
                             width: 750,
                             padding: '5 0 0 0',
                             layout: 'hbox',
                             defaults: {
                                 format: 'd.m.Y',
                                 labelAlign: 'right'
                             },
                             items: [
                                 {
                                     xtype: 'datefield',
                                     labelWidth: 125,
                                     fieldLabel: 'Дата аннулирования с',
                                     width: 290,
                                     itemId: 'dfEndDateStart'
                                     //value: new Date(new Date().getFullYear(), 0, 1)
                                 },
                                 {
                                     xtype: 'datefield',
                                     labelWidth: 50,
                                     fieldLabel: 'по',
                                     width: 210,
                                     itemId: 'dfEndDateEnd'
                                     //value: new Date(new Date().setDate(new Date().getDate() + 7))
                                 },
                                 {
                                     xtype: 'button',
                                     iconCls: 'icon-report',
                                     text: 'Экспорт лицензий в формате ГИС',
                                     itemId: 'btnExportTR'
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