Ext.define('B4.view.realityobj.AddWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 130,
    maxHeight: 130,
    bodyPadding: 5,
    itemId: 'realityobjAddWindow',
    title: 'Жилой дом',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FiasSelectAddress',
        
        'B4.enums.TypeHouse',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.RoleTypeHouse'
    ],

    initComponent: function () {
        var me = this;
        
        

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4fiasselectaddress',
                    labelWidth: 100,
                    name: 'FiasAddress',
                    flatIsReadOnly: true,
                    fieldsToHideNames: ['tfFlat'],
                    fieldsRegex: {
                        tfHousing: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        },
                        tfBuilding: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        }
                    },
                    fieldLabel: 'Адрес',
                    allowBlank: false
                },
                 {
                     xtype: 'combobox',
                     name: 'TypeHouse',
                     fieldLabel: 'Вид дома',
                     displayField: 'Name',
                     store: Ext.create('B4.store.RoleTypeHouse'),
                     valueField: 'Value',
                     allowBlank: false,
                     itemId: 'cbTypeHouseRealityObject',
                     maxWidth: 553,
                     readOnly: false,
                     editable: false
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