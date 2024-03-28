Ext.define('B4.view.dict.sharefinancingceo.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.sharefinancingceowindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.CommonEstateObject'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Редактирование',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 70
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'ООИ',
                    name: 'CommonEstateObject',
                    store: 'B4.store.CommonEstateObject',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'Share',
                    fieldLabel: 'Доля, %',
                    minValue: 0,
                    maxValue: 100,
                    hideTrigger: true,
                    allowDecimals: true,
                    negativeText: 'Значение должно быть в промежутке от 0 до 100 %',
                    anchor: '30%',
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