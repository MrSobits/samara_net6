Ext.define('B4.view.emergencyobj.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'emergencyObjAddWindow',
    title: 'Форма добавления аварийности жилого дома',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ConditionHouse'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    name: 'RealityObject',
                    fieldLabel: 'Жилой дом',
                    allowBlank: false
                },
                {
                    xtype: 'combobox', editable: false,
                    name: 'ConditionHouse',
                    fieldLabel: 'Состояние дома',
                    displayField: 'Display',
                    valueField: 'Value',
                    store: B4.enums.ConditionHouse.getStore()
                },
                {
                    xtype: 'checkboxfield',
                    name: 'IsRepairExpedient',
                    fieldLabel: 'Ремонт целесообразен'
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