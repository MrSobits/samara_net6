Ext.define('B4.view.administration.EmailMessageGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.store.administration.EmailMessage',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Update',
        'B4.enums.EmailMessageType',
        'B4.enums.EmailSendStatus'
    ],

    title: 'Реестр писем',
    alias: 'widget.emailMessageGrid',
    closable: true,
    store: 'administration.EmailMessage',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'EmailMessageType',
                    text: 'Тип отправляемого сообщения',
                    enumName: 'B4.enums.EmailMessageType',
                    flex: 0.7,
                    width: 150,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecipientName',
                    text: 'Наименование организации',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EmailAddress',
                    text: 'Почта адресата',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AdditionalInfo',
                    text: 'Дополнительные сведения',
                    flex: 1.5,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SendingTime',
                    width: 150,
                    text: 'Время отправки письма',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'SendingStatus',
                    text: 'Статус отправки',
                    enumName: 'B4.enums.EmailSendStatus',
                    width: 130,
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'LogFileId',
                    width: 85,
                    text: 'Лог операции',
                    renderer: function (value, metadata) {
                        metadata.style = 'text-align: center;';
                        this.icon = value > 0 ? B4.Url.content('content/img/download.png') : null;
                        return "";
                    },
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var fileId = rec.getData().LogFileId;
                        if (fileId > 0) {
                            window.open(B4.Url.action('Download', 'FileUpload', { Id: fileId }));
                            return;
                        }
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата с',
                            labelWidth: 45,
                            itemId: 'beginDate',
                            value: new Date()
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'по',
                            labelWidth: 20,
                            itemId: 'endDate',
                            value: new Date()
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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