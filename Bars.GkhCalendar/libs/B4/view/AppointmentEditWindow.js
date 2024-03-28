Ext.define('B4.view.AppointmentEditWindow', {
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
        //'B4.enums.InnOgrn'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'appointmentEditWindow',
    title: 'Очередь приёма',
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
                                xtype: 'combobox',
                                //     margin: '10 0 5 0',
                                labelWidth: 170,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'TypeOrganisation',
                                    fieldLabel: 'Тип организации',
                                    displayField: 'Display',
                                    itemId: 'dfTypeOrganisation',
                                    width: 400,
                                    store: B4.enums.TypeOrganisation.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'TimeSlot',
                                    fieldLabel: 'Временной интервал',
                                    displayField: 'Display',
                                    itemId: 'dfTimeSlot',
                                    width: 400,
                                    store: B4.enums.TimeSlot.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'RecordTo',
                                    fieldLabel: 'К кому запись',
                                    allowBlank: false,
                                    width: 450,
                                    disabled: false,
                                    editable: true,
                                    maxLength: 255,
                                    itemId: 'dfRecordTo'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    allowBlank: false,
                                    width: 450,
                                    disabled: false,
                                    editable: true,
                                    maxLength: 255,
                                    itemId: 'dfDescription'
                                }
                            ]
                        },

                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            //bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            //title: 'Приемное время',
                            //border: false,
                            //bodyPadding: 10,
                            //xtype: 'container',
                            //layout: 'vbox',
                            xtype: 'appointmenttimegrid',
                            //flex: 1
                        },
                        {
                            //bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            //title: 'Нестандартные дни',
                            //border: false,
                            //bodyPadding: 10,
                            //xtype: 'container',
                            //layout: 'vbox',
                            xtype: 'appointmentdiffdaygrid',
                            //flex: 1
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