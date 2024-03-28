Ext.define('B4.view.protocolmkd.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.protocolmkdgrid',

    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.dict.Inspector',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Реестр протоколов ОСС в МКД',
    store: 'PropertyOwnerProtocols',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolMKDState',
                    flex: 0.5,
                    text: 'Статус протокола',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'МО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес МКД',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 0.5,
                    text: 'Номер протокола',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    flex: 0.5,
                    text: 'Дата протокола',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolTypeId',
                    flex: 1,
                    text: 'Повестка ОСС',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolMKDSource',
                    flex: 1,
                    text: 'Источник поступления',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegistrationNumber',
                    flex: 0.5,
                    text: 'Регистрационный номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RegistrationDate',
                    format: 'd.m.Y',
                    flex: 0.5,
                    text: 'Дата регистрации',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Inspector',
                //    flex: 1,
                //    text: 'Сотрудник',
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Примечание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentFile',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
                //{
                //    xtype: 'b4deletecolumn',
                //    scope: me
                //}
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
                            columns: 6,
                            items: [                           
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 60,
                                    fieldLabel: 'Период с',
                                    labelAlign: 'right',
                                    width: 160,
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    labelAlign: 'right',
                                    fieldLabel: 'по',
                                    width: 130,
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
                                },
                                {
                                    xtype: 'b4selectfield',
                                    itemId: 'sfEmployer',
                                    labelWidth: 80,
                                    width: 300,
                                    editable: false,
                                    labelAlign: 'right',
                                    fieldLabel: 'Сотрудник',
                                    textProperty: 'Fio',
                                    store: 'B4.store.dict.Inspector',
                                    columns: [
                                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 110,
                                    fieldLabel: 'Дата регистрации с',
                                    labelAlign: 'right',
                                    width: 200,
                                    itemId: 'dfDateRegStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    labelAlign: 'right',
                                    fieldLabel: 'по',
                                    width: 130,
                                    itemId: 'dfDateRegEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
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