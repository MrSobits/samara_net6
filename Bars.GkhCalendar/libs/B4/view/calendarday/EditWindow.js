Ext.define('B4.view.calendarday.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.dayeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 300,
    height: 150,
    minWidth: 300,
    minHeight: 150,
    bodyPadding: 5,
    itemId: 'calendarDayEditWindow',
    title: 'День месяца',
    closeAction: 'hide',
    modal: true,
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.DayType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1,
                labelAlign: 'right',
                labelWidth: '100px',
                margins: '0 10 0 10'
            },
            items: [
                {
                    xtype: 'textfield',
                    editable: false,
                    readOnly: true,
                    width: '100%',
                    name: 'DayDate'
                },
                {
                    xtype: 'combobox',
                    //width: '100%',
                    editable: false,
                    fieldLabel: 'Тип дня',
                    store: B4.enums.DayType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'DayType'
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