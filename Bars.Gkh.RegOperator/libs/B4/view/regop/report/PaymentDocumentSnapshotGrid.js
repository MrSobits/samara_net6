Ext.define('B4.view.regop.report.PaymentDocumentSnapshotGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paydocdatasnapshotgrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.store.regop.report.PaymentDocumentSnapshot',
        'B4.enums.PaymentDocumentPaymentState',
        'B4.enums.YesNo',
        'B4.enums.PaymentDocumentSendingEmailState'
    ],

    title: 'Реестр документов на оплату',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.report.PaymentDocumentSnapshot');

        store.on('beforeload', function(st, opts) {
            return this.fireEvent('beforeload', st, opts);
        }, me);

        Ext.apply(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            }),
            store: store,
            columns: [
                { xtype: 'b4editcolumn', iconCls: 'icon-zoom', icon: null, tooltip: 'Просмотр' },
                { header: 'Id', dataIndex: 'Id', width: 50, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                { header: 'Номер документа', dataIndex: 'DocNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата документа', dataIndex: 'DocDate', xtype: 'datecolumn', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Плательщик', dataIndex: 'Payer', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Инн плательщика', dataIndex: 'OwnerInn', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Муниципальный район', hidden: true, dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Муниципальное образование', hidden: true, dataIndex: 'Settlement', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Тип плательщика', dataIndex: 'OwnerType', flex: 1, xtype: 'b4enumcolumn', enumName: 'B4.enums.regop.PersonalAccountOwnerType', filter: true },
                { header: 'Расчетный счет получателя', hidden: true, dataIndex: 'PaymentReceiverAccount', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Всего к оплате', dataIndex: 'TotalCharge', flex: 1, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                { header: 'Статус оплаты', dataIndex: 'PaymentState', flex: 1, xtype: 'b4enumcolumn', enumName: 'B4.enums.PaymentDocumentPaymentState', filter: true },
                { header: 'Наличие эл. почты', dataIndex: 'HasEmail', flex: 1, xtype: 'b4enumcolumn', enumName: 'B4.enums.YesNo', filter: true },
                { header: 'Отправлено на эл.почту', dataIndex: 'SendingEmailState', flex: 1, xtype: 'b4enumcolumn', enumName: 'B4.enums.PaymentDocumentSendingEmailState', filter: true },
                { header: 'Базовый', dataIndex: 'IsBase', flex: 1, xtype: 'b4enumcolumn', enumName: 'B4.enums.YesNo', filter: true, hidden: true },
                { header: 'Количество ЛС', dataIndex: 'AccountCount', flex: 1, hidden: true, filter: { xtype: 'numberfield', operand: CondExpr.operands.eq } },
                {
                    xtype: 'actioncolumn',
                    header: 'Скачать документ',
                    iconCls: 'icon-arrow-right',
                    handler: function(view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'download', record);
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4updatebutton' },
                                /* убираем кнопку, пока кто-то не заметит
                                {
                                    xtype: 'button',
                                    text: 'Выгрузить документы',
                                    action: 'multidownload'
                                },*/
                                {
                                    xtype: 'b4deletebutton',
                                    text: 'Удалить записи',
                                    disabled: true,
                                    handler: function() {
                                        var grid = this.up('b4grid');
                                        grid.fireEvent('gridaction', grid, 'delete');
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить на эл.почту',
                                    action: 'SendEmail',
                                    iconCls: 'icon-email-transfer'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Заполнить наличие эл.почты',
                                    action: 'SetEmail',
                                    iconCls: 'icon-email-edit'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.regop.ChargePeriod',
                            textProperty: 'Name',
                            editable: false,
                            allowBlank: false,
                            windowContainerSelector: '#' + me.getId(),
                            windowCfg: {
                                modal: true
                            },
                            trigger2Cls: '',
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Дата открытия',
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'StartDate',
                                    flex: 1,
                                    filter: { xtype: 'datefield' }
                                },
                                {
                                    text: 'Дата закрытия',
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'EndDate',
                                    flex: 1,
                                    filter: { xtype: 'datefield' }
                                },
                                {
                                    text: 'Состояние',
                                    dataIndex: 'IsClosed',
                                    flex: 1,
                                    renderer: function(value) {
                                        return value ? 'Закрыт' : 'Открыт';
                                    }
                                }
                            ],
                            name: 'ChargePeriod',
                            labelAlign: 'right',
                            fieldLabel: 'Период',
                            labelWidth: 175
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});