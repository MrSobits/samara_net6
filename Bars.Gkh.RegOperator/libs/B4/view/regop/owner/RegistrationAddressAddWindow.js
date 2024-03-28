Ext.define('B4.view.regop.owner.RegistrationAddressAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.registrationaddressaddwin',

    modal: true,

    width: 700,
    height: 170,
    bodyPadding: 5,
    closeAction: 'destroy',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.store.RealityObject',
        'B4.store.realityobj.Room'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Адрес прописки',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Информация о помещении',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 180
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
                            fieldLabel: 'Жилой дом',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.realityobj.Room',
                            textProperty: 'RoomNum',
                            editable: false,
                            columns: [
                                {
                                    text: 'Номер квартиры',
                                    dataIndex: 'RoomNum',
                                    flex: 1
                                }
                            ],
                            name: 'Room',
                            fieldLabel: '№ квартиры/помещения',
                            allowBlank: true
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                hideTrigger: true,
                                flex: 1,
                                allowDecimals: true
                            }
                        }
                    ]
                },
            ],
            dockedItems:
               {
                   xtype: 'toolbar',
                   dock: 'top',
                   items:
                       {
                           xtype: 'buttongroup',
                           columns: 2,
                           items: [
                               {
                                   xtype: 'b4savebutton'
                               }
                           ]
                       }
               }
        });

        me.callParent(arguments);
    },

    setAddLayout: function (isAdd) {
        var section = this.query(isAdd ? '[section="edit"]' : '[section="add"]')[0];

        if (section) {
            section.destroy();
        }

        this.setHeight(isAdd ? 300 : 550);
        this.center();
    }
});