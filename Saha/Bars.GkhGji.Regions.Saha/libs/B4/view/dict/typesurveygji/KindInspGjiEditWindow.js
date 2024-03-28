Ext.define('B4.view.dict.typesurveygji.KindInspGjiEditWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.kindInspGjiEditWindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 400,
    height: 125,
    minHeight: 125,
    maxHeight: 125,
    bodyPadding: 5,
    itemId: 'kindInspGjiEditWindow',
    title: 'Вид обследования',
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
                xtype: 'textfield',
                anchor: '100%',
                labelAlign: 'right'
            },
            items:
            [
                {
                    name: 'KindCheckName',
                    fieldLabel: 'Наименование',
                    readOnly: true
                },
                {
                    name: 'Code',
                    fieldLabel: 'Код',
                    maxLength: 100
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