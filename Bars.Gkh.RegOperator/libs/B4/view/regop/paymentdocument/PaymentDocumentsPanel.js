Ext.define('B4.view.regop.paymentdocument.PaymentDocumentsPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod'
    ],

    title: 'Документы на оплату',
    alias: 'widget.paymentdocumentspanel',
    layout: 'form',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items:
            [
                {
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'container',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Здесь вы можете выбрать период начислений для получения ссылки на скачивание файлов</span>'
                        },
                        {
                            defaults: {
                                labelAlign: 'left',
                                labelWidth: 40
                            },
                            margin: '15 15 15 15',
                            layout: {
                                type: 'hbox'
                            },
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.ChargePeriod',
                                    textProperty: 'Name',
                                    editable: false,
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
                                            renderer: function (value) {
                                                return value ? 'Закрыт' : 'Открыт';
                                            }
                                        }
                                    ],
                                    name: 'ChargePeriod',
                                    fieldLabel: 'Период',
                                    labelWidth: 55
                                },
                                {
                                    xtype: 'button',
                                    text: 'Получить ссылку на ftp',
                                    action: 'getLink'
                                },
                                {
                                    xtype: 'displayfield',
                                    fieldLabel: 'Ссылка на файл',
                                    labelWidth: 100,
                                    padding: '0 0 0 5',
                                    width: 400,
                                    name: 'Link'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
