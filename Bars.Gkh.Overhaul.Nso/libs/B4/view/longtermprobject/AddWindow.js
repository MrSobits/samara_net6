Ext.define('B4.view.longtermprobject.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 130,
    maxHeight: 130,
    bodyPadding: 5,
    itemId: 'longtermprobjectAddWindow',
    title: 'Жилой дом',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FiasSelectAddress',

        'B4.enums.TypeHouse',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
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
                        tfHouse: {
                            regex: /^(\d+[А-Яа-я]{0,2}((\/)\d*[А-Яа-я]{0,2})?)(\s*)$/,
                            regexText:  'В это поле можно вводить значение следующих форматов: 33/22, 33/А, 33/22А, 33А, 33Б/44В'
                        },
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
                     editable: false,
                     floating: false,
                     name: 'TypeHouse',
                     fieldLabel: 'Вид дома',
                     displayField: 'Display',
                     store: B4.enums.TypeHouse.getStore(),
                     valueField: 'Value',
                     allowBlank: false,
                     itemId: 'cbTypeHouseRealityObject',
                     maxWidth: 553
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