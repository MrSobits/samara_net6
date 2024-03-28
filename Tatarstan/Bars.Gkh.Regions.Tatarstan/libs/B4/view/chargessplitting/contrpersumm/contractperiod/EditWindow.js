Ext.define('B4.view.chargessplitting.contrpersumm.contractperiod.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.contractperiodeditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.MonthPicker'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Формирование отчетного периода',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'b4monthpicker',
                    name: 'StartDate',
                    fieldLabel: 'Период',
                    format: 'd.m.Y',
                    labelWidth: 100,
                    width: 200,
                    allowBlank: false
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