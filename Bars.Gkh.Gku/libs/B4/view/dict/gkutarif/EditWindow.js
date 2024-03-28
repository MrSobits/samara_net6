Ext.define('B4.view.dict.gkutarif.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 600,
    minHeight: 450,
    bodyPadding: 5,
    itemId: 'gkutarifEditWindow',
    title: 'Тариф ЖКУ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ServiceKindGku',
        'B4.store.Contragent',
        'B4.form.SelectField',
        'B4.store.SupplyResourceOrg',
        'B4.store.ManagingOrganization',
        'B4.store.dict.PublicService'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'ServiceKind',
                    fieldLabel: 'Тип услуги',
                    displayField: 'Display',
                    store: B4.enums.ServiceKindGku.getStore(),
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.SupplyResourceOrg',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    textProperty: 'ContragentName',
                    name: 'ResourceOrg',
                    fieldLabel: 'Поставщик',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.ManagingOrganization',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    textProperty: 'ContragentName',
                    name: 'ManOrg',
                    fieldLabel: 'Управляющая организация',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Service',
                    store: 'B4.store.dict.PublicService',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    fieldLabel: 'Услуга',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        flex: 1,
                        labelWidth: 110,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            fieldLabel: 'Дата начала',
                            name: 'DateStart',
                            allowBlank: false
                        },
                        {
                            fieldLabel: 'Дата окончания',
                            name: 'DateEnd'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        allowBlank: false,
                        decimalSeparator: ',',
                        minValue: 0,
                        labelWidth: 110,
                        labelAlign: 'right',
                        flex: 1,
                        hideTrigger: true
                    },
                    items: [
                        {
                            name: 'TarifRso',
                            fieldLabel: 'Тариф РСО'
                        },
                        {
                            name: 'TarifMo',
                            fieldLabel: 'Тариф УО'
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    name: 'PurchaseVolume',
                    fieldLabel: 'Объем закупаемых ресурсов',
                    allowBlank: false,
                    decimalSeparator: ',',
                    hideTrigger: true,
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'NormativeValue',
                    fieldLabel: 'Норматив',
                    decimalSeparator: ',',
                    hideTrigger: true,
                    minValue: 0,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'PurchasePrice',
                    fieldLabel: 'Закупочная стоимость коммунального ресурса',
                    decimalSeparator: ',',
                    hideTrigger: true,
                    minValue: 0,
                    labelWidth: 300
                },
                {
                    xtype: 'textarea',
                    name: 'NormativeActInfo',
                    fieldLabel: 'Реквизиты нормативного акта, устанавливающего тариф',
                    flex: 1
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
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