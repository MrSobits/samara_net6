Ext.define('B4.view.objectcr.BuildControlTypeWorkSmrEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'buildControlTypeWorkSmrEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
//  layout: { type: 'hbox', align: 'stretch' },
    width: 750,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Контроль хода работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.store.objectcr.TypeWorkCrAddWork',
        'B4.view.objectcr.FileInfoGrid',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'TypeWorkCrAddWork',
                    itemId: 'sfTypeWorkCrAddWork',
                    editable: false,
                    fieldLabel: 'Этап работы',
                    store:  'B4.store.objectcr.TypeWorkCrAddWork',
                    textProperty: 'AdditWorkName',
                    columns: [{ text: 'Наименование этапа', dataIndex: 'AdditWorkName', flex: 1, filter: { xtype: 'textfield' } }],
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    itemId: 'sfContragent',
                    editable: false,
                    fieldLabel: 'Исполнитель',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Controller',
                    editable: false,
                    fieldLabel: 'Стройконтроль',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'gkhdecimalfield',
                        labelWidth: 150,
                        labelAlign: 'right',
                        margin: '0 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'VolumeOfCompletion',
                            fieldLabel: 'Объем выполнения'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'DeadlineMissed',
                            fieldLabel: 'Срыв сроков'
                        }
                        //{
                        //    xtype: 'gkhdecimalfield',
                        //    name: 'CostSum',
                        //    fieldLabel: 'Сумма расходов'
                        //}
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        margin: '0 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Latitude',
                            editable: false,
                            decimalPrecision: 10,
                            fieldLabel: 'Широта',
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Longitude',
                            editable: false,
                            decimalPrecision: 10,
                            fieldLabel: 'Долгота',
                        },

                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        margin: '0 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'PercentOfCompletion',
                            fieldLabel: 'Процент выполнения',
                            maxValue: 100
                        },
                        {
                            xtype: 'datefield',
                            name: 'MonitoringDate',
                            itemId: 'dfDateFrom',
                            fieldLabel: 'Дата СК',
                            format: 'd.m.Y'
                        }
                       
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    height: 60,
                    maxLength: 500,
                    readOnly: false
                },
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                         { xtype: 'objectcrfileinfogrid' }
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-map-go',
                                    text: 'Карта',
                                    itemId: 'btnMap'
                                }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});