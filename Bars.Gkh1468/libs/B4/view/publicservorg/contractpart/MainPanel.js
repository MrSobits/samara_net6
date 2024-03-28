Ext.define('B4.view.publicservorg.contractpart.MainPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractpartymainpanelpanel',

    requires: [
        'B4.form.SelectField',
        'B4.enums.TypeContractPart',
        'B4.enums.CommercialMeteringResourceType',
        'B4.enums.TypeOwnerContract',
        'B4.enums.TypeContactPerson',
        'B4.enums.OwnerDocumentType',
        'B4.enums.GenderR'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 5,

    closeAction: 'hide',

    closable: false,
    title: 'Стороны договора',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    trackResetOnLoad: true,
    header: true,
    border: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150,
                required: true
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: false,
                        required: true
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'TypeContractPart',
                            displayField: 'Display',
                            valueField: 'Value',
                            items: B4.enums.TypeContractPart.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Вид договора',
                            allowBlank: false,
                            editable: false,
                            flex: 1,
                            onCreate: true
                        },

                        // РСО и испольнитель коммунальных услуг
                        {
                            xtype: 'b4selectfield',
                            name: 'ManagingOrganization',
                            fieldLabel: 'Управляющая организация',
                            store: 'B4.store.ManagingOrganization',
                            textProperty: 'ContragentShortName',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'ContragentShortName',
                                    flex: 2,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'ИНН',
                                    dataIndex: 'ContragentInn',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'CommercialMeteringResourceType',
                            displayField: 'Display',
                            valueField: 'Value',
                            items: B4.enums.CommercialMeteringResourceType.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Коммерческий учёт ресурса осуществляет',
                            allowBlank: false,
                            editable: false,
                            flex: 1
                        },

                        // Юридическое лицо
                        {
                            xtype: 'b4combobox',
                            name: 'TypeContactPerson',
                            displayField: 'Display',
                            valueField: 'Value',
                            items: B4.enums.TypeContactPerson.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Кто является стороной договора',
                            allowBlank: false,
                            editable: false,
                            flex: 1
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'TypeOwnerContract',
                            displayField: 'Display',
                            valueField: 'Value',
                            items: B4.enums.TypeOwnerContract.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Тип лица/организация',
                            allowBlank: false,
                            editable: false,
                            flex: 1,
                            onCreate: true
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            store: 'B4.store.Contragent',
                            textProperty: 'Name',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'ИНН',
                                    dataIndex: 'Inn',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },

                        // Физическое лицо
                        {
                            xtype: 'textfield',
                            name: 'LastName',
                            fieldLabel: 'Фамилия',
                            maxLenght: 300,
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'FirstName',
                            fieldLabel: 'Имя',
                            maxLenght: 300,
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'MiddleName',
                            fieldLabel: 'Отчество',
                            maxLenght: 300,
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            name: 'AdditionalInfo',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                required: true
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Gender',
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    items: B4.enums.GenderR.getItemsWithEmpty([null, ' - ']),
                                    fieldLabel: 'Пол',
                                    allowBlank: false,
                                    editable: false,
                                    flex: 1
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'BirthDate',
                                    fieldLabel: 'Дата рождения',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            name: 'PassportInfo',
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Документ, удостоверяющий личность',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 5 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false,
                                        required: true
                                    },
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            name: 'OwnerDocumentType',
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            items: B4.enums.OwnerDocumentType.getItemsWithEmpty([null, ' - ']),
                                            fieldLabel: 'Тип документа',
                                            allowBlank: false,
                                            editable: false,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'IssueDate',
                                            fieldLabel: 'Дата выдачи',
                                            format: 'd.m.Y',
                                            allowBlank: false,
                                            required: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 5 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        required: true
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentSeries',
                                            fieldLabel: 'Серия',
                                            maxLenght: 100,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер',
                                            maxLenght: 100,
                                            allowBlank: false
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'BirthPlace',
                            fieldLabel: 'Место рождения',
                            maxLenght: 500,
                            allowBlank: true,
                            required: false
                        },

                        // РСО и бюджетные организации
                        {
                            xtype: 'b4selectfield',
                            name: 'TypeCustomer',
                            fieldLabel: 'Вид бюджета',
                            store: 'B4.store.dict.TypeCustomer',
                            textProperty: 'Name',
                            editable: false,
                            allowBlank: false,
                            required: true,
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Organization',
                            fieldLabel: 'Организация',
                            store: 'B4.store.Contragent',
                            textProperty: 'Name',
                            editable: false,
                            allowBlank: true,
                            required: false,
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'ИНН',
                                    dataIndex: 'Inn',
                                    flex: .7,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'КПП',
                                    dataIndex: 'Kpp',
                                    flex: .7,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                       

                        // РСО и поставщик ТЭР
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.PublicServiceOrg',
                            selectionMode: 'SINGLE',
                            windowCfg: { modal: true },
                            textProperty: 'ContragentName',
                            labelAlign: 'right',
                            fieldLabel: 'Поставщик ТЭР',
                            name: 'FuelEnergyResourceOrg',
                            editable: false,
                            required: true,
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'ContragentName',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'ИНН',
                                    dataIndex: 'Inn',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
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