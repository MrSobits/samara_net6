Ext.define('B4.view.creditorg.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.creditorgwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 800,
    minHeight: 500,
    maxHeight: 500,
    bodyPadding: 5,
    title: 'Кредитная организация',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.creditorg.ExceptChildren',
        'B4.view.Control.GkhIntField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Реквизиты',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300,
                            itemId: 'tfName'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsFilial',
                            fieldLabel: 'Является филиалом',
                            itemId: 'cbIsFilial',
                            listeners: {
                                change: function (cmp, newValue) {
                                    var sfParent = me.down('#sfParent');
                                    sfParent.setDisabled(!newValue);
                                    sfParent.allowBlank = newValue;
                                }
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.creditorg.ExceptChildren',
                            name: 'Parent',
                            textProperty: 'Name',
                            fieldLabel: 'Головная организация',
                            itemId: 'sfParent',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                render: {
                                    fn: function () {
                                        var cbIsFilial = me.down('#cbIsFilial');
                                        this.setDisabled(!cbIsFilial.checked);
                                        if (!cbIsFilial.checked)
                                            this.reset();
                                    }
                                }
                            },
                            //#warning убрать переопределение после добавления fireEvent('select', field, data); в платформенный компонент
                            onSelectValue: function () {
                                var field = this,
                                    rec = field.gridView.getSelectionModel().getSelected(),
                                    data;
                                if (rec.length == 0) {
                                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                    return;
                                }

                                if (field.selectionMode.toUpperCase() == 'SINGLE') {
                                    data = rec[0].getData();
                                } else {
                                    data = [];
                                    for (var i in rec) {
                                        data.push(rec[i].getData());
                                    }
                                }

                                field.setValue(data);
                                field.fireEvent('select', field, data);

                                field.onSelectWindowClose();
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                flex: 1,
                                xtype: 'textfield'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    maxLength: 20,
                                    itemId: 'tfInn',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Kpp',
                                    fieldLabel: 'КПП',
                                    maxLength: 20,
                                    itemId: 'tfKpp'
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
                                xtype: 'textfield'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Bik',
                                    fieldLabel: 'БИК',
                                    minLength: 9,
                                    maxLength: 9,
                                    itemId: 'tfBik',
                                    minLengthText: "Количество символов в БИК должно быть равно {0} знакам.",
                                    maxLengthText: "Количество символов в БИК должно быть равно {0} знакам."
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Okpo',
                                    fieldLabel: 'ОКПО',
                                    maxLength: 20,
                                    itemId: 'tfOkpo'
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
                                xtype: 'textfield'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Ogrn',
                                    fieldLabel: 'ОГРН'
                                },
                                {
                                    xtype: 'gkhintfield',
                                    name: 'Oktmo',
                                    hideTrigger: true,
                                    fieldLabel: 'ОКТМО'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'CorrAccount',
                            fieldLabel: 'Корреспондентский счет',
                            maxLength: 20,
                            itemId: 'tfCorrAccount'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Юридический адрес',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'FiasAddress',
                            fieldLabel: 'Адрес в пределах субъекта',
                            flatIsVisible: false,
                            allowBlank: false,
                            itemId: 'fsaAddress',
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsAddressOut',
                                    labelWidth: 180,
                                    fieldLabel: 'Адрес за пределами субъекта',
                                    flex: 0.05,
                                    itemId: 'cbIsAddressOut',
                                    listeners: {
                                        change: function (cmp, newValue) {
                                            me.down('#tfAddressOutSubject').setDisabled(!newValue);
                                            me.down('#fsaAddress').setDisabled(newValue);
                                        }
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'AddressOutSubject',
                                    padding: '0 0 0 200',
                                    flex: 1,
                                    fieldlabel: 'Адрес',
                                    hideLabel: true,
                                    itemId: 'tfAddressOutSubject',
                                    allowBlank: false,
                                    maxLength: 500
                                }
                            ]
                        }
                    ]
                },
                
                {
                    xtype: 'fieldset',
                    title: 'Почтовый адрес',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'FiasMailingAddress',
                            fieldLabel: 'Адрес в пределах субъекта',
                            flatIsVisible: false,
                            itemId: 'fsaMailingAddress',
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsMailingAddressOut',
                                    labelWidth: 180,
                                    fieldLabel: 'Адрес за пределами субъекта',
                                    flex: 0.05,
                                    itemId: 'cbIsMailingAddressOut',
                                    listeners: {
                                        change: function (cmp, newValue) {
                                            me.down('#tfMailingAddressOutSubject').setDisabled(!newValue);
                                            me.down('#fsaMailingAddress').setDisabled(newValue);
                                        }
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MailingAddressOutSubject',
                                    padding: '0 0 0 200',
                                    flex: 1,
                                    fieldlabel: 'Адрес',
                                    hideLabel: true,
                                    itemId: 'tfMailingAddressOutSubject',
                                    allowBlank: false,
                                    maxLength: 500
                                }
                            ]
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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