Ext.define('B4.view.longtermprobject.propertyownerdecision.SpecAccEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionspecaccwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.TypeOrganization',
        'B4.form.FileField',
        'B4.view.creditorg.Grid',
        'B4.store.CreditOrg',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MethodFormFundCr',
        'B4.enums.MoOrganizationForm',
        'B4.view.longtermprobject.propertyownerdecision.SpecAccNoticePanel'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 730,
    minHeight: 500,
    height: 500,
    bodyPadding: 5,
    title: 'Редактирование',
    closable: false,

    initComponent: function() {
        var me = this,
            lVbox = {
                type: 'vbox',
                align: 'stretch'
            };

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            layout: lVbox,
            items: [
                {
                    xtype: 'container',
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; line-height: 16px; padding: 0px 10px 10px 30px;',
                    html: '<span style="display: table-cell">' +
                                '<span class="im-info" style="vertical-align: top;">' +
                                '</span>' +
                            '</span>' +
                            '<span style="display: table-cell">' +
                                '       Текущий договор управления можно посмотреть в разделе "Управление домом"  ' +
                            '</span>'
                },
                {
                    xtype: 'combobox',
                    readOnly: true,
                    name: 'PropertyOwnerDecisionType',
                    fieldLabel: 'Наименование решения',
                    displayField: 'Display',
                    store: B4.enums.PropertyOwnerDecisionType.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'combobox',
                    readOnly: true,
                    name: 'MethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    displayField: 'Display',
                    store: B4.enums.MethodFormFundCr.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    name: 'MoOrganizationForm',
                    fieldLabel: 'Способ управления',
                    displayField: 'Display',
                    store: B4.enums.MoOrganizationForm.getStore(),
                    valueField: 'Value',
                    readOnly: true
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'panel',
                            autoScroll: true,
                            title: 'Общие сведения',
                            bodyStyle: Gkh.bodyStyle,
                            padding: 5,
                            layout: lVbox,
                            defaults: {
                                labelAlign: 'right',
                                padding: 5
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    layout: lVbox,
                                    title: 'Информация о владельце лицевого счета',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            floating: false,
                                            name: 'TypeOrganization',
                                            fieldLabel: 'Тип организации-владельца',
                                            displayField: 'Display',
                                            store: B4.enums.TypeOrganization.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'Наименование организации-владельца',
                                            name: 'ManagingOrganization',

                                            store: 'B4.store.ManagingOrganization',
                                            editable: false,
                                            hidden: true,
                                            allowBlank: false,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            textProperty: 'ContragentName'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: lVbox,
                                    title: 'Информация о спец. счете',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Номер счета',
                                            name: 'AccountNumber'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                labelAlign: 'right',
                                                labelWidth: 150,
                                                format: 'd.m.Y',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    fieldLabel: 'Дата открытия',
                                                    name: 'OpenDate'
                                                },
                                                {
                                                    name: 'CloseDate',
                                                    fieldLabel: 'Дата закрытия'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'BankHelpFile',
                                            padding: '5 0 0 0',
                                            fieldLabel: 'Справка банка',
                                            possibleFileExtensions: 'pdf'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: lVbox,
                                    title: 'Информация о кредитной организации',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'Наименование',
                                            name: 'CreditOrg',

                                            store: 'B4.store.CreditOrg',
                                            editable: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'MailingAddress',
                                            readOnly: true,
                                            fieldLabel: 'Почтовый адрес'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 150,
                                                flex: 1,
                                                xtype: 'textfield',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    name: 'Inn',
                                                    fieldLabel: 'ИНН'
                                                },
                                                {
                                                    name: 'Kpp',
                                                    fieldLabel: 'КПП'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 150,
                                                flex: 1,
                                                xtype: 'textfield',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    name: 'Ogrn',
                                                    fieldLabel: 'ОГРН'
                                                },
                                                {
                                                    name: 'Okpo',
                                                    fieldLabel: 'ОКПО'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 150,
                                                flex: 1,
                                                xtype: 'textfield',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    name: 'Bik',
                                                    fieldLabel: 'БИК'
                                                },
                                                {
                                                    name: 'CorrAccount',
                                                    fieldLabel: 'Кор. счет'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'longtermdecisionspecaccnoticepanel',
                            autoScroll: true
                        }
                    ]
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
                                {
                                    xtype: 'b4savebutton',
                                    type: 'Decision'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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