Ext.define('B4.view.realityobj.entrance.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    height: 480,
    minWidth: 600,
    minHeight: 150,
    bodyPadding: 5,
    title: 'Сведения о подъезде',
    closeAction: 'hide',
    trackResetOnLoad: true,

    alias: 'widget.realityobjentrancewindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.RealEstateType',
        'B4.view.realityobj.entrance.RoomGrid',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'numberfield',
                    name: 'Number',
                    fieldLabel: 'Номер подъезда',
                    minValue: 0,
                    allowBlank: false,
                    hideTrigger: true,
                    value: null
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealEstateType',
                    fieldLabel: 'Тип дома',
                    allowBlank: false,
                    store: 'B4.store.RealEstateType',
                    columns: [
                        { dataIndex: 'Name', text: 'Наименование', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'numberfield',
                    name: 'Tariff',
                    fieldLabel: 'Тариф',
                    readOnly: true,
                    hideTrigger: true,
                    disabled: true
                },
                {
                    xtype: 'realityobjentranceroomgrid',
                    name: 'RoomGrid',
                    flex: 1
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
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