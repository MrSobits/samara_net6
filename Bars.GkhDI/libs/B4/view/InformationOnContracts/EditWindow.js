Ext.define('B4.view.informationoncontracts.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 580,
    height: 400,
    bodyPadding: 5,
    itemId: 'informationOnContractsEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.menu.ManagingOrgRealityObjDataMenu'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    height: 150,
                    region: 'north',
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                         {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            itemId: 'sflRealityObject',
                            fieldLabel: 'Объект недвижимости',
                            anchor: '100%',
                            textProperty: 'AddressName',
                           

                            store: 'B4.store.menu.ManagingOrgRealityObjDataMenu',
                            editable: false,
                            allowBlank: false,
                            columns: [{ text: 'Адрес', dataIndex: 'AddressName', flex: 1 }]
                        },                        
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'container',
                            defaults: {
                                xtype: 'container',
                                layout: 'anchor',
                                flex: 1,
                                defaults: {
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    anchor: '100%'
                                }
                            },
                            layout: {
                                type: 'hbox',
                                pack: 'start',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'Number',
                                        fieldLabel: '№',
                                        allowBlank: false,
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        name: 'DateStart',
                                        fieldLabel: 'Дата начала',
                                        width: 290
                                    }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'From',
                                            fieldLabel: 'От',
                                            allowBlank: false,
                                            width: 290
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'DateEnd',
                                            fieldLabel: 'Дата окончания',
                                            width: 290
                                        }]
                                }
                            ]
                        },
                        {
                            xtype: 'textareafield',
                            name: 'PartiesContract',
                            padding: '10 0 0 0',
                            fieldLabel: 'Стороны договора'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Cost',
                            fieldLabel: 'Стоимость, руб.',
                            allowBlank: false
                        },
                        {
                            xtype: 'textareafield',
                            name: 'Comments',
                            padding: '10 0 0 0',
                            fieldLabel: 'Примечание'
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