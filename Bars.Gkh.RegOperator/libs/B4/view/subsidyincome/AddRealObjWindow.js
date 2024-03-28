Ext.define('B4.view.subsidyincome.AddRealObjWindow', {
    extend: 'B4.form.Window',

    modal: true,

    width: 500,
    height: 100,
    bodyPadding: 5,
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.RealityObject'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Сопоставление дома',
    alias: 'widget.subsidyincomeaddrealobjwin',

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
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
                    fieldLabel: 'Жилой дом',
                    labelAlign: 'right'
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
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{
                                xtype: 'b4closebutton',
                                listeners: {
                                    'click': function() {
                                        me.close();
                                    }
                                }
                            }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});