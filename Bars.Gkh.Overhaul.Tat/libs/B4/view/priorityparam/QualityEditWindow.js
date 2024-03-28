Ext.define('B4.view.priorityparam.QualityEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.priorparamqualitywindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    minHeight: 100,
    maxHeight: 100,
    width: 450,
    minWidth: 450,
    bodyPadding: 5,
    title: 'Параметр',
    itemId: 'priorparamqualitywindow',
    closeAction: 'hide',
    trackResetOnLoad: true,

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
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 100,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'Value',
                            fieldLabel: 'Значение',
                            readOnly: true,
                            items: B4.enums.TypePresence.getItems()
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