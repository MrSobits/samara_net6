Ext.define('B4.view.emergencyobj.ResettlementProgramEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 250,
    maxHeight: 250,
    bodyPadding: 5,
    itemId: 'emergencyObjResetProgEditWindow',
    title: 'Программа переселения',
    closeAction: 'hide',
    closable: true,
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ResettlementProgramSource',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ResettlementProgramSource',
                    fieldLabel: 'Источник по программе переселения',
                    store: 'B4.store.dict.ResettlementProgramSource',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhintfield',
                    name: 'CountResidents',
                    fieldLabel: 'Количество жителей',
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Area',
                    fieldLabel: 'Площадь',
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Cost',
                    fieldLabel: 'Плановая стоимость',
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'ActualCost',
                    fieldLabel: 'Фактическая стоимость',
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным'
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