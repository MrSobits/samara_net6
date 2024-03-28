Ext.define('B4.view.actcheck.docrequestaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.BaseActionEditWindow',

    requires: [
        'B4.form.FileField',
        'B4.form.FiasSelectAddress',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.view.actcheck.actioneditwindowbaseitem.RequisiteInfoFieldSet',
        'B4.view.actcheck.docrequestaction.RequestInfoGrid',
        'B4.store.Contragent',
        'B4.enums.TypeExecutantProtocol'
    ],
    
    alias: 'widget.actcheckdocrequestactioneditwindow',
    title: 'Истребование документов',

    editFormItems: [
        {
            xtype: 'actcheckactionrequisiteinfofieldset'
        },
        {
            xtype: 'fieldset',
            title: 'Основные сведения',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                labelWidth: 140,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
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
                                    var fieldSet = field.up('actcheckdocrequestactioneditwindow'),
                                        contrPersFioField = fieldSet.down('textfield[name=ContrPersFio]'),
                                        contrPersContragent = fieldSet.down('b4selectfield[name=ContrPersContragent]'),
                                        isIndividual = newValue === B4.enums.TypeExecutantProtocol.Individual,
                                        isOfficial = newValue === B4.enums.TypeExecutantProtocol.Official;

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
                            xtype: 'textfield',
                            fieldLabel: 'ФИО',
                            name: 'ContrPersFio',
                            padding: '5 0 5 0',
                            maxLength: 255
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.Contragent',
                    textProperty: 'ShortName',
                    name: 'ContrPersContragent',
                    fieldLabel: 'Контрагент',
                    editable: false,
                    modalWindow: true,
                    padding: '5 0 0 0',
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
                    xtype: 'textfield',
                    fieldLabel: 'Адрес контролируемого лица',
                    name: 'ContrPersRegistrationAddress',
                    maxLength: 500
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Срок предоставления документов (сутки)',
                            name: 'DocProvidingPeriod',
                            minValue: 0,
                            maxLength: 10,
                            allowDecimals: false,
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'DocProvidingAddress',
                            fieldLabel: 'Адрес предоставления документов',
                            flatIsVisible: false,
                            flex: 2.5,
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
                            xtype: 'textfield',
                            fieldLabel: 'Адрес эл. почты',
                            name: 'ContrPersEmailAddress',
                            padding: '5 0 5 0',
                            labelWidth: 120,
                            maxLength: 100,
                            flex: 1.25
                        }
                    ]
                }
            ]
        },
        {
            xtype: 'fieldset',
            title: 'Представитель',
            layout: 'hbox',
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'ФИО',
                    name: 'RepresentFio',
                    maxLength: 255,
                    flex: 3
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Доверенность номер',
                    name: 'RepresentProcurationNumber',
                    maxLength: 50,
                    flex: 2
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата',
                    name: 'RepresentProcurationIssuedOn',
                    labelWidth: 50,
                    format: 'd.m.Y',
                    flex: 1
                }
            ]
        },
        {
            xtype: 'docrequestactionrequestinfogrid',
            height: 200
        },
        {
            xtype: 'fieldset',
            title: 'Направление копии определения',
            layout: 'hbox',
            defaults: {
                xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 1,
                defaults: {
                    labelWidth: 110,
                    labelAlign: 'right',
                    flex: 1
                },
            },
            items: [
                {
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер почтового отделения',
                            name: 'PostalOfficeNumber',
                            maxLength: 50
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес эл. почты',
                            name: 'EmailAddress',
                            maxLength: 50
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата',
                            name: 'CopyDeterminationDate',
                            padding: '5 0 5 0',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер письма',
                            name: 'LetterNumber',
                            maxLength: 50
                        }
                    ]
                }
            ]
        }
    ]
});