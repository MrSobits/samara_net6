Ext.define('B4.view.program.NewVersionWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.programversionwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

         'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.view.dict.municipality.Grid'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    title: 'Новая версия программы',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170,
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата',
                    allowBlank: false,
                    value: new Date(),
                    maxValue: new Date()
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Основная',
                    name: 'IsMain',
                    margin: '0 0 0 115',
                    boxLabelAlign: 'before'
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