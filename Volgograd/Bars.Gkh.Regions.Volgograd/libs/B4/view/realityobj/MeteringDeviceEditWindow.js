Ext.define('B4.view.realityobj.MeteringDeviceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjmetdeviceeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    height: 220,
    minHeight: 200,
    bodyPadding: 5,
    
    title: 'Прибор учета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.MeteringDevice',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MeteringDevice',
                    fieldLabel: 'Тип прибора',
                    listView: 'B4.view.dict.meteringdevice.Grid',
                    store: 'B4.store.dict.MeteringDevice',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateRegistration',
                    fieldLabel: 'Дата постановки на учет',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    name: 'DateInstallation',
                    fieldLabel: 'Дата установки',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: false,
                    minValue: 1800,
                    maxValue: 2100
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100% -50',
                    maxLength: 1000
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});