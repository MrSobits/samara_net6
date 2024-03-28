Ext.define('B4.view.regop.report.PaymentDocumentTemplateEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.paydoctemplateedit',
    width: 500,
    layout: 'form',
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
    ],

    modal: true,
    title: 'Редактирование шаблона',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                labelAlign: 'right',
                margin: 7
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ChargePeriod',
                    name: 'Period',
                    disabled: true,
                    allowBlank: false,
                    fieldLabel: 'Период',
                    columns: [
                        {
                            dataIndex: 'Name',
                            text: 'Наименование',
                            flex: 1
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'StartDate',
                            text: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'EndDate',
                            text: 'Дата окончания',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    name: 'TemplateCode',
                    disabled: true,
                    fieldLabel: 'Базовый отчет',
                    editable: false,
                    allowBlank: false,
                    storeAutoLoad: true,
                    valueField: 'Key',
                    url: '/PaymentDocReportManager/TemplateList'
                },
                {
                    xtype: 'hidden',
                    margin: 0,
                    name: 'Template'
                },
                {
                    xtype: 'b4filefield',
                    name: 'TemplateFile',
                    fieldLabel: 'Файл шаблона',
                    allowBlank: false,
                    flex: 1,
                    editable: false,
                    possibleFileExtensions: 'mrt',
                    getFileUrl: function (id) {
                        return B4.Url.content(Ext.String.format('{0}/{1}?templateCode={2}&periodId={3}',
                            'PaymentDocReportManager', 'DownloadTemplate', id.templateCode, id.periodId));
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-disk',
                                    text: 'Скачать',
                                    menu: [
                                        {
                                            text: 'Скачать оригинал',
                                            action: 'DownloadOrigin',
                                            disabled: true,
                                            iconCls: 'icon-script-lightning'
                                        },
                                        {
                                            text: 'Скачать пустой шаблон',
                                            iconCls: 'icon-script',
                                            disabled: true,
                                            action: 'DownloadEmpty'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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