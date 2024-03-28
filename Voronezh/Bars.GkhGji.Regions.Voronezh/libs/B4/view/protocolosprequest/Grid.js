Ext.define('B4.view.protocolosprequest.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.protocolosprequestgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Enum',
        'B4.form.GridStateColumn',
        'B4.enums.FuckingOSSState',
        'B4.enums.OSSApplicantType'
    ],
    closable: true,
    title: 'Заявки на доступ к протоколам обращений',
    store: 'ProtocolOSPRequest',

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
                    text: 'Статус заявления',
                    width: 160,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'oss_request';
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
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    flex: 1,
                    text: 'Дата заявления',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequestNumber',
                    flex: 1,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FIO',
                    flex: 1,
                    text: 'Заявитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.OSSApplicantType',
                    dataIndex: 'ApplicantType',
                    text: 'Статус заявителя',
                    flex: 0.5,
                    filter: true
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
                    text: 'Адрес МКД',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerProtocolType',
                    flex: 1,
                    text: 'Вопрос повестки ОСС',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Исполнитель',
                    dataIndex: 'Inspector',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolFile',
                    width: 100,
                    text: 'Протокол ОСС',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FuckingOSSState',
                    dataIndex: 'Approved',
                    text: 'Результат ',
                    flex: 0.5,
                    filter: true
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
                                xtype: 'b4updatebutton'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'icon-table-go',
                                text: 'Экспорт',
                                textAlign: 'left',
                                itemId: 'btnExport'
                            },
                            {
                                xtype: 'checkbox',
                                itemId: 'cbShowCloseAppeals',
                                boxLabel: 'Показать закрытые',
                                labelAlign: 'right',
                                checked: false,
                                margin: '10px 10px 0 0'
                            },
                            {
                                xtype: 'datefield',
                                labelWidth: 60,
                                fieldLabel: 'Период с',
                                width: 160,
                                itemId: 'dfDateStart',
                                value: new Date(new Date().getFullYear(), 0, 1)
                            },
                            {
                                xtype: 'datefield',
                                labelWidth: 30,
                                fieldLabel: 'по',
                                width: 130,
                                itemId: 'dfDateEnd',
                                value: new Date(new Date().getFullYear(), 11, 31)
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