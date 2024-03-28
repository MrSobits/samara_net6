Ext.define('B4.view.contragent.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 650,
    minHeight: 450,
    maxHeight: 450,
    maxWidth: 650,
    minWidth: 650,
    bodyPadding: 5,
    itemId: 'contragentAddWindow',
    title: 'Форма добавления контрагента',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.dict.OrganizationForm',
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 200,
                        anchor: '100%',
                        labelAlign: 'right',
                        allowBlank: false
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'b4selectfield',
                            itemId: 'sfCtrgOrgForm',
                            name: 'OrganizationForm',
                            fieldLabel: 'Организационно-правовая форма',
                            store: 'B4.store.dict.OrganizationForm',
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Parent',
                            fieldLabel: 'Головная организация',
                            store: 'B4.store.contragent.ContragentForSelect',
                            editable: false,
                            columns: [
                                {
                                    xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальный район', flex: 1, filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            allowBlank: true,
                            itemId: 'sfParent'
                        }
                    
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Реквизиты',
                    defaults: {
                        labelWidth: 120,
                        anchor: '100%',
                        allowBlank: false,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Inn',
                            itemId: 'tfCtrgInn',
                            fieldLabel: 'ИНН',
                            flex: 1,
                            maxLength: 15
                        },
                        {
                            xtype: 'textfield',
                            name: 'Kpp',
                            itemId: 'tfCtrgKpp',
                            fieldLabel: 'КПП',
                            flex: 1,
                            maxLength: 15
                        },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'FiasJuridicalAddress',
                            itemId: 'addWindowContragentFiasJuridicalAddressField',
                            fieldLabel: 'Юридический адрес',
                            allowBlank: false,
                            flatIsVisible: true,
                            flex: 1,
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
                            anchor: '100%',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 120
                            },
                            items: [
                                {
                                    xtype: 'b4fiasselectaddress',
                                    labelAlign: 'right',
                                    name: 'FiasFactAddress',
                                    itemId: 'addWindowContragentFiasFactAddressField',
                                    fieldLabel: 'Фактический адрес',
                                    allowBlank: false,
                                    flatIsVisible: true,
                                    flex: 1,
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
                                    xtype: 'button',
                                    name: 'CopyButtonFactAddress',
                                    itemId: 'btnCopyButtonFactAddress',
                                    text: 'Заполнить',
                                    width: 70,
                                    margin: '0 0 0 10'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            flex: 1,
                            name: 'AddressOutsideSubject',
                            itemId: 'tfctrgAddWindowOutsideAddress',
                            fieldLabel: 'Адрес за пределами субъекта',
                            allowBlank: true,
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Ogrn',
                    itemId: 'tfCtrgOgrn',
                    labelAlign: 'right',
                    fieldLabel: 'ОГРН',
                    labelWidth: 130,
                    allowBlank: false,
                    flex: 1,
                    maxLength: 15
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