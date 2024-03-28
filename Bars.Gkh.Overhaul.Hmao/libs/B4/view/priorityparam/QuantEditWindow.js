Ext.define('B4.view.priorityparam.QuantEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.priorparamquantwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minHeight: 130,
    maxHeight: 130,
    width: 500,
    minWidth: 500,
    bodyPadding: 5,
    itemId: 'priorparamquantwindow',
    title: 'Параметр',
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
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'numberfield',
                        labelAlign: 'right',
                        labelWidth: 130,
                        flex: 1,
                        hideTrigger: true,
                        decimalSeparator: ',',
                        minValue: 0
                    },
                    items: [
                        {                            
                            name: 'MinValue',
                            fieldLabel: 'Начальное значение'
                        },
                        {
                            name: 'MaxValue',
                            fieldLabel: 'Конечное значение'
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    labelAlign: 'right',
                    labelWidth: 130,
                    hideTrigger: true,
                    decimalSeparator: ',',
                    minValue: 0,
                    name: 'Point',
                    fieldLabel: 'Балл'
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