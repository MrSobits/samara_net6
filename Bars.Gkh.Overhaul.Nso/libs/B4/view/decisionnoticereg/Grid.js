Ext.define('B4.view.decisionnoticereg.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.decisionnoticereggrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.enums.MethodFormFundCr',
        'B4.store.DecisionNoticeRegister'
    ],

    title: 'Сводный реестр уведомлений о решениях общего собрания',
    closable: true,
    enableColumnHide: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.DecisionNoticeRegister');

        Ext.applyIf(me, {
            store: store,
            cls:'x-large-head',
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
                    width: 150,
                    scope: me,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'ovrhl_decision_notice';
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
                    dataIndex: 'Address',
                    flex: 2,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Наименование владельца спец. счета',
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentMailingAddress',
                //    flex: 2,
                //    text: 'Почтовый адрес',
                //    filter: { xtype: 'textfield' }
                //}, 
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentOgrn',
                //    width: 80,
                //    text: 'ОГРН',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentInn',
                //    width: 80,
                //    text: 'ИНН',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentOktmo',
                //    width: 80,
                //    text: 'ОКTMO',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentKpp',
                //    width: 80,
                //    text: 'КПП',
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RegDate',
                    text: 'Дата регистрации в ГЖИ',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GjiNumber',
                    width: 100,
                    text: 'Регистрационный номер в ГЖИ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'NoticeDate',
                    text: 'Дата уведомления',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MethodFormFund',
                    flex: 1,
                    text: 'Способ формирования фонда',
                    renderer: function (val) {
                        return B4.enums.MethodFormFundCr.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.MethodFormFundCr.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProtocolDate',
                    text: 'Дата протокола',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DecOpenDate',
                    text: 'Дата открытия специального счета',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'export'
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