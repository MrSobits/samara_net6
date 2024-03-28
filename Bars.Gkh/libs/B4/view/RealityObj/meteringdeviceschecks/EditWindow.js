Ext.define('B4.view.realityobj.meteringdeviceschecks.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.meteringdeviceschecksEditWindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 400,
    bodyPadding: 5,
    title: 'Прибор учета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeAccounting',
        'B4.store.realityobj.MeteringDevice',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 250,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MeteringDevice',
                    fieldLabel: 'Прибор учёта',
                    store: 'B4.store.realityobj.MeteringDevice',
                    columns: [
                        { text: 'Лицевой счет', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Наименование', dataIndex: 'MeteringDevice', flex: 1, filter: { xtype: 'textfield' } }],
                    editable: false,
                    textProperty: 'PersonalAccountNum'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'ControlReading',
                    fieldLabel: 'Контрольное показание',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'RemovalControlReadingDate',
                    fieldLabel: 'Дата снятия контрольного показания',
                    labelAlign: 'right',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'StartDateCheck',
                    fieldLabel: 'Дата начала проверки',
                    labelAlign: 'right',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'StartValue',
                    fieldLabel: 'Значение показаний прибора учета на момент начала проверки',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'EndDateCheck',
                    fieldLabel: 'Дата окончания проверки',
                    labelAlign: 'right',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'EndValue',
                    fieldLabel: 'Значение показаний на момент окончания проверки',
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'MarkMeteringDevice',
                    fieldLabel: 'Марка прибора учёта',
                    maxLength: 30
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'IntervalVerification',
                    fieldLabel: 'Межпроверочный интервал (лет)',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'NextDateCheck',
                    fieldLabel: 'Плановая дата следующей проверки',
                    labelAlign: 'right',
                    format: 'd.m.Y'
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