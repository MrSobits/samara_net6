Ext.define('B4.view.regop.owner.PersonalAccountOwnerEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.paownereditwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.view.regop.owner.PersonalAccountGrid',
        'B4.store.dict.PrivilegedCategory',
        'B4.model.dict.PrivilegedCategory',
        'B4.form.FiasSelectAddress',
        'B4.enums.Gender',
        'B4.view.contragent.ActivityStageGrid',
        'B4.store.RealityObject',
        'B4.store.realityobj.Room',
        'B4.view.regop.owner.RegistrationAddressAddWindow'
    ],

    title: 'Карточка абонента',

    modal: true,
    layout: 'fit',

    width: 700,
    height: 641,

    __INDIV: 0,
    __LEGAL: 1,

    initComponent: function () {
        var me = this,
            st = Ext.create('Ext.data.Store', {
                fields: ['BillingAddressType', 'AddressType', 'Address'],
                proxy: {
                    type: 'memory',
                    reader: {
                        type: 'json'
                    }
                }
            });

        Ext.apply(me, {
            closable: false,
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Общие',
                            autoScroll: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                autoScroll: true
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 120,
                                margin: 5
                            },
                            style: 'background: none repeat scroll 0 0 #DFE9F6',
                            border: 0,
                            flex: 1,
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    name: 'Id'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип абонента',
                                    enumName: 'B4.enums.regop.PersonalAccountOwnerType',
                                    includeEmpty: false,
                                    enumItems: [],
                                    allowBlank: false,
                                    name: 'OwnerType',
                                    listeners: {
                                        change: me.onOwnerTypeChange,
                                        scope: me
                                    }
                                },
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    name: 'PrivilegedCategory',
                                    fieldLabel: 'Льготная категория',
                                    store: 'B4.store.dict.PrivilegedCategory',
                                    columns: [
                                        {
                                            text: 'Код',
                                            dataIndex: 'Code',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield',
                                                maxLength: 300
                                            }
                                        },
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield',
                                                maxLength: 300
                                            }
                                        },
                                        {
                                            text: 'Процент льготы',
                                            dataIndex: 'Percent',
                                            flex: 1,
                                            filter: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                keyNavEnabled: false,
                                                mouseWheelEnabled: false,
                                                minValue: 0,
                                                maxValue: 100,
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            text: 'Действует с',
                                            dataIndex: 'DateFrom',
                                            flex: 1,
                                            format: 'd.m.Y',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            text: 'Действует по',
                                            dataIndex: 'DateTo',
                                            flex: 1,
                                            format: 'd.m.Y',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Общие сведения об абоненте',
                                    pao: true,
                                    ownerType: me.__LEGAL,
                                    hidden: true,
                                    defaults: {
                                        labelWidth: 162,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            textProperty: 'Name',
                                            name: 'Contragent',
                                            store: 'B4.store.contragent.ContragentForLegalAccount',
                                            model: 'B4.model.Contragent',
                                            itemName: 'contragentCa',
                                            allowBlank: false,
                                            columns: [
                                                {
                                                    header: 'Муниципальное образование',
                                                    flex: 1,
                                                    dataIndex: 'Municipality',
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
                                                    header: 'Наименование',
                                                    flex: 1,
                                                    dataIndex: 'Name',
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Inn',
                                                    width: 80,
                                                    text: 'ИНН',
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Kpp',
                                                    width: 80,
                                                    text: 'КПП',
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            fieldLabel: 'Контрагент'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'proxyInn',
                                            fieldLabel: 'ИНН',
                                            maxLength: 300,
                                            maskRe: /[0-9]/
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'proxyKpp',
                                            fieldLabel: 'КПП',
                                            maxLength: 300,
                                            maskRe: /[0-9]/
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            textProperty: 'Address',
                                            valueProperty: 'BillingAddressType',
                                            isGetOnlyIdProperty: false,
                                            name: 'Address',
                                            store: st,
                                            columns: [
                                                {
                                                    header: 'Тип адреса',
                                                    flex: 1,
                                                    dataIndex: 'AddressType'
                                                },
                                                {
                                                    header: 'Значение',
                                                    flex: 2,
                                                    dataIndex: 'Address'
                                                }
                                            ],
                                            fieldLabel: 'Адрес для корреспонденции',
                                            listView: {
                                                dockedItems: []
                                            },
                                            maskRe: /[]/
                                        },
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Печатать акт при печати документов на оплату',
                                            fieldStyle: 'vertical-align: middle; margin-left: 167px;',
                                            name: 'PrintAct'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    pao: true,
                                    title: 'Общие сведения об абоненте',
                                    ownerType: me.__INDIV,
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        anchor: '100%',
                                        labelWidth: 110
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Фамилия',
                                            name: 'Surname',
                                            maskRe: /[0-9А-ЯЁ-\s]/i,
                                            maxLength: 100
                                        },
                                        {
                                            fieldLabel: 'Имя',
                                            name: 'FirstName',
                                            maskRe: /[0-9А-ЯЁ-\s]/i,
                                            maxLength: 100
                                        },
                                        {
                                            fieldLabel: 'Отчество',
                                            name: 'SecondName',
                                            maskRe: /[0-9А-ЯЁ-\s]/i,
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    fieldLabel: 'Дата рождения',
                                                    xtype: 'datefield',
                                                    name: 'BirthDate',
                                                    format: 'd.m.Y',
                                                    labelAlign: 'right',
                                                    labelWidth: 110,
                                                    allowBlank: true,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'Gender',
                                                    enumName: 'B4.enums.Gender',
                                                    fieldLabel: 'Пол',
                                                    labelAlign: 'right',
                                                    labelWidth: 50,
                                                    value: 0,
                                                    allowBlank: false,
                                                    includeEmpty: false,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RegistrationAddressShowInfo',
                                            fieldLabel: 'Адрес прописки',
                                            labelAlign: 'right',
                                            labelWidth: 110,
                                            allowBlank: true,
                                            onTrigger1Click: function () {
                                                debugger;
                                                var pickAddressViget = Ext.create('B4.view.regop.owner.RegistrationAddressAddWindow');
                                                pickAddressViget.show();
                                            }
                                        },
                                        {
                                            xtype: 'hidden',
                                            name: 'RegistrationAddress',
                                            allowBlank: true
                                        },
                                        {
                                            xtype: 'hidden',
                                            name: 'RegistrationRoom',
                                            allowBlank: true
                                        },
                                        {
                                            fieldLabel: 'Место рождения',
                                            name: 'BirthPlace',
                                            maxLength: 300,
                                            allowBlank: true
                                        },
                                        {
                                            xtype: 'fieldset',
                                            name: 'BillingAddress',
                                            pao: true,
                                            title: 'Адрес для отправки корреспонденции',
                                            id: 'billingAddressFieldset',
                                            ownerType: me.__INDIV,
                                            defaults: {
                                                xtype: 'textfield',
                                                labelAlign: 'right',
                                                allowBlank: false,
                                                anchor: '100%',
                                                labelWidth: 110
                                            },
                                            items: [
                                                {
                                                    xtype: 'hidden',
                                                    name: 'BillingAddressType'
                                                },
                                                {
                                                    xtype: 'container',
                                                    anchor: '100%',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 250,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'checkboxfield',
                                                            inputValue: 10,
                                                            id: 'isAddress10',
                                                            fieldLabel: 'Фактический адрес нахождения',
                                                            checked: false,
                                                            uncheckedValue: 0,
                                                            listeners: {
                                                                change: {
                                                                    fn: function (isFactAddressElement) {
                                                                        debugger;
                                                                        var fiasFactAddressField = isFactAddressElement.up().down('#editWindowPersAccFiasFactAddressField'),
                                                                            fiasFactAddressDoct = isFactAddressElement.up().up(),
                                                                            fiasFactAddressDoc = fiasFactAddressDoct.down('#docPersAccFiasFactAddressField'),
                                                                            isEmailElement = isFactAddressElement.up('#billingAddressFieldset').down('#isAddress30'),
                                                                            isAddressOutsideSubjectElement = isFactAddressElement.up('#billingAddressFieldset').down('#isAddress20');

                                                                        isEmailElement.setValue(false);
                                                                        isAddressOutsideSubjectElement.setValue(false);

                                                                        fiasFactAddressField.setReadOnly(!this.checked);
                                                                        fiasFactAddressField.allowBlank = !this.checked;
                                                                        debugger;
                                                                        fiasFactAddressDoc.setReadOnly(!this.checked);
                                                                        fiasFactAddressDoc.allowBlank = !this.checked;
                                                                    }
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            width: 10
                                                        },
                                                        {
                                                            xtype: 'b4fiasselectaddress',
                                                            labelAlign: 'right',
                                                            name: 'FiasFactAddress',
                                                            id: 'editWindowPersAccFiasFactAddressField',
                                                            flex: 1,
                                                            readOnly: true,
                                                            allowBlank: true,
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
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    anchor: '100%',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 275,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'label',
                                                            text: 'Файл - основание фактического адреса:',
                                                            width: 250,
                                                            padding: '5 0 5 0',
                                                            style: {
                                                                textAlign: 'right',
                                                                verticalAlign: 'down'
                                                            }
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            width: 30
                                                        }
                                                        // ,
                                                        // {
                                                        //     id: 'docPersAccFiasFactAddressField',
                                                        //     xtype: 'b4filefield',
                                                        //     //fieldLabel: 'Файл',
                                                        //     name: 'FactAddrDoc',
                                                        //     width: '55%'
                                                        // }
                                                    ]
                                                },

                                                {
                                                    xtype: 'container',
                                                    anchor: '100%',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 250,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'checkboxfield',
                                                            id: 'isAddress20',
                                                            fieldLabel: 'Адрес за пределами субъекта',
                                                            inputValue: 20,
                                                            uncheckedValue: 0,
                                                            checked: false,
                                                            listeners: {
                                                                change: {
                                                                    fn: function (isAddressOutsideSubjectElement) {
                                                                        var outsideAddressField = isAddressOutsideSubjectElement.up().down('#editWindowPersAccOutsideAddress'),
                                                                            isEmailElement = isAddressOutsideSubjectElement.up('#billingAddressFieldset').down('#isAddress30'),
                                                                            isFactAddressElement = isAddressOutsideSubjectElement.up('#billingAddressFieldset').down('#isAddress10');

                                                                        isEmailElement.setValue(false);
                                                                        isFactAddressElement.setValue(false);

                                                                        outsideAddressField.setReadOnly(!this.checked);
                                                                        outsideAddressField.allowBlank = !this.checked;
                                                                    }
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            width: 10
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            flex: 1,
                                                            name: 'AddressOutsideSubject',
                                                            id: 'editWindowPersAccOutsideAddress',
                                                            allowBlank: true,
                                                            readOnly: true,
                                                            maxLength: 500
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    anchor: '100%',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 250,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'checkboxfield',
                                                            id: 'isAddress30',
                                                            fieldLabel: 'Электронная почта',
                                                            inputValue: 30,
                                                            checked: false,
                                                            uncheckedValue: 0,
                                                            listeners: {
                                                                change: {
                                                                    fn: function (isEmailElement) {
                                                                        var emailField = isEmailElement.up().down('#editWindowPersAccEmail'),
                                                                            isAddressOutsideSubjectElement = isEmailElement.up('#billingAddressFieldset').down('#isAddress20'),
                                                                            isFactAddressElement = isEmailElement.up('#billingAddressFieldset').down('#isAddress10');

                                                                        isAddressOutsideSubjectElement.setValue(false);
                                                                        isFactAddressElement.setValue(false);

                                                                        emailField.setReadOnly(!this.checked);
                                                                        emailField.allowBlank = !this.checked;
                                                                    }
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            width: 10
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'Email',
                                                            id: 'editWindowPersAccEmail',
                                                            maxLength: 255,
                                                            readOnly: true,
                                                            validator: function (enteredValue) {
                                                                var emailCheck = this.up().down('#isAddress30');
                                                                if (emailCheck.checked && !RegExp(/^\w+([.]?-*\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/).test(enteredValue)) {
                                                                    return 'Ошибка, неверный формат email';
                                                                }
                                                                return true;
                                                            }
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    pao: true,
                                    title: 'Документ',
                                    ownerType: me.__INDIV,
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        // allowBlank: false,
                                        labelWidth: 110,
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.regop.IdentityType',
                                            includeEmpty: false,
                                            enumItems: [],
                                            fieldLabel: 'Тип документа',
                                            name: 'IdentityType',
                                            listeners: {
                                                change: {
                                                    fn: function (field, newValue) {
                                                        var identitySerial = field.up().down('#identitySerial');
                                                        if (newValue == B4.enums.regop.IdentityType.InsuranceNumber) {
                                                            identitySerial.hide();
                                                        } else {
                                                            identitySerial.show();
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            fieldLabel: 'Серия документа',
                                            id: 'identitySerial',
                                            name: 'IdentitySerial',
                                            maxLength: 200
                                        },
                                        {
                                            fieldLabel: 'Номер документа',
                                            name: 'IdentityNumber',
                                            maxLength: 200
                                        },
                                        {
                                            fieldLabel: 'Дата выдачи документа',
                                            xtype: 'datefield',
                                            name: 'DateDocumentIssuance',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            maxLength: 500,
                                            name: 'DocumentIssuededOrg',
                                            fieldLabel: 'Кем выдан документ'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'ActivityStage',
                                    pao: true,
                                    title: 'Финансовое состояние',
                                    ownerType: me.__INDIV,
                                    items: [
                                        {
                                            xtype: 'activitystagegrid',
                                            title: 'Финансовое состояние'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            title: 'Сведения о помещениях',
                            xtype: 'paowneraccountgrid',
                            border: false,
                            disabled: true
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
                        }, '->', {
                            xtype: 'buttongroup',
                            name: 'closebuttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ],
            listeners: {
                close: function () {
                    var me = this;
                    if (me.editMode) {
                        var items = B4.getBody().items;
                        var index = items.findIndexBy(function (tab) {
                            return tab.urlToken != null && tab.urlToken.indexOf('regop_personal_acc_owner') === 0;
                        });

                        if (index != -1) {
                            B4.getBody().remove(items.items[index], true);
                        }
                    }
                }
            }
        });
        me.callParent(arguments);
    },

    onOwnerTypeChange: function (combo, newVal, oldVal) {
        var me = this,
            individuals = Ext.ComponentQuery.query(Ext.String.format('[ownerType={0}] field', me.__INDIV), this.up('paownereditwin')),
            legals = Ext.ComponentQuery.query(Ext.String.format('[ownerType={0}] field', me.__LEGAL), this.up('paownereditwin')),
            fieldsets = Ext.ComponentQuery.query('fieldset[pao=true]', this.up('paownereditwin')),
            form = combo.up('paownereditwin').getForm(),
            record = form.getRecord(),
            model, accType = newVal;

        Ext.each(individuals, function (item) {
            item.setDisabled(newVal == me.__LEGAL);
        }, me);

        Ext.each(legals, function (item) {
            item.setDisabled(newVal == me.__INDIV);
        }, me);

        Ext.each(fieldsets, function (item) {
            item.setVisible(item.ownerType == newVal);
        }, me);

        if (oldVal != newVal) {
            if (newVal == me.__INDIV) {
                model = b4app.getController('regop.owner.PersonalAccountOwner').getModel('regop.owner.IndividualAccountOwner');
            } else if (newVal == me.__LEGAL) {
                model = b4app.getController('regop.owner.PersonalAccountOwner').getModel('regop.owner.LegalAccountOwner');
            }

            record = new model(record.raw);
            record.set('OwnerType', accType);
            form.loadRecord(record);
        }
    }
});