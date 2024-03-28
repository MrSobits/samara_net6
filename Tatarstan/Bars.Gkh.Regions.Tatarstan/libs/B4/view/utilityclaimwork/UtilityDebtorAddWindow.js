Ext.define('B4.view.utilityclaimwork.UtilityDebtorAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.utilitydebtoraddwin',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 130,
    maxHeight: 130,
    bodyPadding: 5,

    title: 'Задолженность по оплате ЖКУ',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelAlign: 'right',
                labelWidth: 50
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'Municipality',
                            flex: 1,
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
                    fieldLabel: 'Адрес',
                    allowBlank: false
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