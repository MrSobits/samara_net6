Ext.define('B4.view.tatarstanprotocolgji.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.tatarstanprotocolgjiaddwindow',
    title: 'Протокол ГЖИ',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.ZonalInspection',
        'B4.form.ComboBox',
        'B4.enums.TypeExecutantProtocol'
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
                    textProperty: 'Name',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ZonalInspection',
                    name: 'ZonalInspection',
                    fieldLabel: 'Орган ГЖИ',
                    textProperty: 'ZoneName',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'ZoneName', flex: 1 }]
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'combobox',
                    name: 'Executant',
                    fieldLabel: 'Тип исполнителя',
                    displayField: 'Display',
                    store: B4.enums.TypeExecutantProtocol.getStore(),
                    valueField: 'Value'
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