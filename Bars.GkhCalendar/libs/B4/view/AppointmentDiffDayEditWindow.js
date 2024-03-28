Ext.define('B4.view.AppointmentDiffDayEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.AppointmentDiffDayGridStore',
        'B4.view.AppointmentDiffDayGrid',
        'B4.view.AppointmentTimeGrid',
        //'B4.view.smevegrip.FileInfoGrid',
        //'B4.view.Control.GkhButtonPrint',
        //'B4.enums.DayOfWeekRus'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'appointmentDiffDayEditWindow',
    title: 'Нестандартный день',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Реквизиты субъекта запроса',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            defaults: {
                                //     margin: '10 0 5 0',
                                labelWidth: 170,
                                labelAlign: 'right',
                            },
                            items: [
                                //{
                                //    xtype: 'combobox',
                                //    name: 'DayOfWeek',
                                //    fieldLabel: 'День недели',
                                //    displayField: 'Display',
                                //    itemId: 'dfDayOfWeek',
                                //    width: 400,
                                //    store: B4.enums.DayOfWeekRus.getStore(),
                                //    valueField: 'Value',
                                //    allowBlank: false,
                                //    editable: false
                                //},
                                //{
                                //    xtype: 'combobox',
                                //    name: 'TimeSlot',
                                //    fieldLabel: 'Временной интервал',
                                //    displayField: 'Display',
                                //    itemId: 'dfTimeSlot',
                                //    width: 400,
                                //    store: B4.enums.TimeSlot.getStore(),
                                //    valueField: 'Value',
                                //    allowBlank: false,
                                //    editable: false
                                //},
                                //{
                                //    xtype: 'textfield',
                                //    name: 'RecordTo',
                                //    fieldLabel: 'К кому запись',
                                //    allowBlank: false,
                                //    width: 450,
                                //    disabled: false,
                                //    editable: true,
                                //    maxLength: 255,
                                //    itemId: 'dfRecordTo'
                                //},
                                //{
                                //    xtype: 'textfield',
                                //    name: 'Description',
                                //    fieldLabel: 'Описание',
                                //    allowBlank: false,
                                //    width: 450,
                                //    disabled: false,
                                //    editable: true,
                                //    maxLength: 255,
                                //    itemId: 'dfDescription'
                                //}
                            ]
                        },

                    ]
                },
                
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
                                },
                                //{
                                //    xtype: 'gkhbuttonprint'
                                //}
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