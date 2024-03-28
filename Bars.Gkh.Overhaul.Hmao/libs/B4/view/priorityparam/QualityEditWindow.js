Ext.define('B4.view.priorityparam.QualityEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.priorparamqualitywindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    minHeight: 130,
    maxHeight: 130,
    width: 450,
    minWidth: 450,
    bodyPadding: 5,
    title: 'Параметр',
    itemId: 'priorparamqualitywindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypePresence',
        'B4.form.ComboBox'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'EnumDisplay',
                    fieldLabel: 'Значение',
                    readOnly: true
                },
                {
                    xtype: 'numberfield',
                    name: 'Point',
                    fieldLabel: 'Балл',
                    allowBlank: false,
                    hideTrigger: true,
                    decimalSeparator: ',',
                    minValue: 0
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