Ext.define('B4.view.dict.paymentpenalties.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    minHeight: 600,
    maxHeight: 600,
    bodyPadding: 5,
    itemId: 'paymentpenaltiesEditWindow',
    title: 'Расчет пеней',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField',
        'B4.form.ComboBox',
        'B4.enums.CrFundFormationDecisionType',
        'B4.view.dict.paymentpenalties.ExcludePersAccGrid'
    ],

    initComponent: function () {
        var me = this,
            items = [
                [0, 'Специальный счет' ],
                [1, 'Счет регионального оператора' ]
            ];

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'DecisionType',
                    fieldLabel: 'Способ формирования фонда',
                    items: items,
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhintfield',
                    hideTrigger: true,
                    name: 'Days',
                    fieldLabel: 'Установленныйы срок оплаты',
                    minValue: 0
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Percentage',
                    fieldLabel: 'Ставка рефинансирования, %',
                    minValue: 0
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Дата начала',
                    allowBlank: false                    
                },
                {
                    xtype: 'datefield',
                    readOnly: true,
                    name: 'DateEnd',
                    fieldLabel: 'Дата окончания'
                },
                {
                    xtype: 'paymentpenaltiesexcludepersaccgrid',
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