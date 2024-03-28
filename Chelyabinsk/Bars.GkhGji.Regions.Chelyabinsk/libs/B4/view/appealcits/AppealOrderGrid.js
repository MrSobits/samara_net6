﻿Ext.define('B4.view.appealcits.AppealOrderGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.appealordergrid',
    store: 'appealcits.AppealOrder',
    closable: true,
    title: 'Реестр СОПР',

    initComponent: function() {
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
                    dataIndex: 'Executant',
                    flex: 1,
                    text: 'Контрагент'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentINN',
                    flex: 1,
                    text: 'ИНН'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OrderDate',
                    flex: 0.5,
                    text: 'Дата размещения в СОПР',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerformanceDate',
                    flex: 0.5,
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'YesNoNotSet',
                    text: 'Устранено',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Confirmed',
                    text: 'Подтверждено',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'ConfirmedGJI',
                    text: 'Принято ГЖИ',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    itemId: 'gcCorrespondent',
                    flex: 1,
                    text: 'Заявитель'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrespondentAddress',
                    itemId: 'gcCorrespondentAddress',
                    flex: 1,
                    text: 'Адрес заявителя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    itemId: 'gcFile',
                    width: 100,
                    text: 'Файл обращения',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать PDF</a>') : '';
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
                            columns: 6,
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
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowCloseAppeals',
                                    boxLabel: 'Показать закрытые обращения',
                                    labelAlign: 'right',
                                    checked: false
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