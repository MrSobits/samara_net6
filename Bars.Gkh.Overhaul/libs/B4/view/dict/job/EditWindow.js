Ext.define('B4.view.dict.job.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.jobwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.view.dict.work.Grid',
        'B4.store.dict.Work',
        'B4.view.dict.unitmeasure.Grid',
        'B4.store.dict.Unitmeasure'
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
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Вид работы',
                    name: 'Work',
                    store: 'B4.store.dict.Work',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Единица измерения',
                    name: 'UnitMeasure',
                    store: 'B4.store.dict.UnitMeasure',
                    editable: false,
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