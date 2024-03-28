Ext.define('B4.view.shortprogram.RecordEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.shortprogramrecordwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'shortprogramrecordwindow',
    minHeight: 125,
    maxHeight: 125,
    width: 550,
    minWidth: 550,
    bodyPadding: 5,
    
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'WorkName',
                    fieldLabel: 'Работа',
                    labelAlign: 'right',
                    labelWidth: 80
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        labelAlign: 'right',
                        labelWidth: 80,
                        hideTrigger: true,
                        flex: 1,
                        decimalSeparator: ',',
                        minValue: 0
                    },
                    items: [
                        {
                            fieldLabel: 'Объем',
                            name: 'Volume'
                        },
                        {
                            fieldLabel: 'Сумма (руб)',
                            name: 'Cost'
                        }
                    ]
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
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
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