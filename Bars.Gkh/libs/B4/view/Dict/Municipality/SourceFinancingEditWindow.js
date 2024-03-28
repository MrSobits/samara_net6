Ext.define('B4.view.dict.municipality.SourceFinancingEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'municipalitySourceFinancingEditWindow',
    title: 'Форма редактирования источника финансирования',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'SourceFinancing',
                    fieldLabel: 'Разрез финансирования',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'AddKr',
                    fieldLabel: 'Доп. КР',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'AddFk',
                    fieldLabel: 'Доп. ФК',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'AddEk',
                    fieldLabel: 'Доп. ЭК',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kvr',
                    fieldLabel: 'КВР',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kvsr',
                    fieldLabel: 'КВСР',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kif',
                    fieldLabel: 'КИФ',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kfsr',
                    fieldLabel: 'КФСР',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kcsr',
                    fieldLabel: 'КЦСР',
                    anchor: '100%',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Kes',
                    fieldLabel: 'КЭС',
                    anchor: '100%',
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