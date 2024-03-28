Ext.define('B4.view.appealcits.motivatedpresentation.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.motivatedpresentationappealcitsaddwindow',
    itemId: 'appealCitsMotivatedPresentationAddWindow',
    title: 'Мотивированное представление по обращению',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.MotivatedPresentationType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'PresentationType',
                    fieldLabel: 'Вид мотивированного представления',
                    enumName: 'B4.enums.MotivatedPresentationType'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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