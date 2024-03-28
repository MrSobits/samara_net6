Ext.define('B4.view.dict.federalstandardfeecr.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.federalstandardfeecreditwin',
    title: 'Федеральный стандарт взноса на КР',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Дата начала',
                    allowBlank: false                    
                },
                {
                    xtype: 'numberfield',
                    decimalSeparator: ',',
                    fieldLabel: 'Значение',
                    name: 'Value',
                    allowBlank: false,
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    minValue: 0
                },
                {
                    xtype: 'datefield',
                    readOnly: true,
                    name: 'DateEnd',
                    fieldLabel: 'Дата окончания'
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