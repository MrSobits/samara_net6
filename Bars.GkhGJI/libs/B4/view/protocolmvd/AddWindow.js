Ext.define('B4.view.protocolmvd.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'protocolMvdAddWindow',
    title: 'Протокол МВД',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeExecutantProtocolMvd',
        'B4.store.dict.OrganMvd'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Municipality',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.OrganMvd',
                    name: 'OrganMvd',
                    fieldLabel: 'Орган МВД',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'combobox',
                    itemId: 'cbExecutant',
                    name: 'TypeExecutant',
                    editable: false,
                    fieldLabel: 'Тип исполнителя',
                    displayField: 'Display',
                    store: B4.enums.TypeExecutantProtocolMvd.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер протокола',
                    itemId: 'tfDocumentNumber',
                    maxLength: 50
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