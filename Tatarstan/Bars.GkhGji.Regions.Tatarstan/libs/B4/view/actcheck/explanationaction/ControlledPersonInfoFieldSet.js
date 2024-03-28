Ext.define('B4.view.actcheck.explanationaction.ControlledPersonInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.explanationactioncontrolledpersoninfofieldset',

    requires: [
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.view.actcheck.actioneditwindowbaseitem.IdentityDocInfoFieldSet',
        'B4.view.Control.GkhTriggerField'
    ],

    title: 'Контролируемое лицо',
    layout: 'hbox',
    items: [
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'ContrPersType',
                    fieldLabel: 'Тип контролируемого лица',
                    enumName: 'B4.enums.TypeExecutantProtocol',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('explanationactioncontrolledpersoninfofieldset'),
                                contrPersFioField = fieldSet.down('textfield[name=ContrPersFio]'),
                                contrPersContragent = fieldSet.down('b4selectfield[name=ContrPersContragent]'),
                                isIndividual = newValue === B4.enums.TypeExecutantProtocol.Individual,
                                isOfficial = newValue === B4.enums.TypeExecutantProtocol.Official;

                            contrPersFioField.allowBlank = !(isIndividual || isOfficial);
                            contrPersFioField.validate();

                            contrPersContragent.allowBlank = isIndividual;
                            contrPersContragent.validate();

                            if (!isIndividual) {
                                contrPersContragent.show();
                            }
                            else {
                                contrPersContragent.hide();
                                contrPersContragent.setValue(null);
                            }
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.Contragent',
                    textProperty: 'ShortName',
                    name: 'ContrPersContragent',
                    fieldLabel: 'Контрагент',
                    editable: false,
                    modalWindow: true,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'ContrPersBirthDate',
                    fieldLabel: 'Дата рождения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Место рождения',
                    name: 'ContrPersBirthPlace',
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Адрес регистрации',
                    name: 'ContrPersRegistrationAddress',
                    maxLength: 500
                },
                {
                    xtype: 'checkbox',
                    name: 'ContrPersLivingAddressMatched',
                    boxLabel: 'Адрес проживания совпадает с адресом регистрации',
                    fieldStyle: 'vertical-align: middle;',
                    margin: '-2 0 4 135',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('explanationactioncontrolledpersoninfofieldset')
                                livingAddressField = fieldSet.down('textfield[name=ContrPersLivingAddress]');

                            if (!newValue) {
                                livingAddressField.show();
                            }
                            else {
                                livingAddressField.hide();
                                livingAddressField.setValue(null);
                            }
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Адрес проживания',
                    name: 'ContrPersLivingAddress',
                    maxLength: 500,
                    padding: '5 0 5 0'
                }
            ]
        },
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelWidth: 130,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'ФИО',
                    name: 'ContrPersFio',
                    padding: '5 0 5 0',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Место работы',
                    name: 'ContrPersWorkPlace',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Должность',
                    name: 'ContrPersPost',
                    maxLength: 255
                },
                {
                    xtype: 'actcheckactionidentitydocinfofieldset',
                    margin: '0 0 0 10'
                }
            ]
        }
    ]
});