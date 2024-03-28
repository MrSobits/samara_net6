Ext.define('B4.view.version.Add', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.version.RealityObjectforAdd',
        'B4.store.version.KEforAdd',
    ],
    alias: 'widget.versionadd',
    title: 'Добавить запись',
    width: 500,
    bodyPadding: 5,
    //height: 600,
    //minHeight: 300,
    layout: 'form',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Объект недвижимости',
                    textProperty: 'Address',
                    anchor: '100%',
                    store: 'B4.store.version.RealityObjectforAdd',
                    editable: false,
                    columns: [
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    allowBlank: false,
                    listeners: {
                    beforeload: function (field, options, store) {
                        options.params = options.params || {};

                        var form = field.up(),
                            versionId = form.versionId;

                        options.params.versionId = versionId;
                    }
                }
                },
                {
                    xtype: 'b4selectfield',
                    name: 'KE',
                    fieldLabel: 'Конструктивные элементы',
                    store: 'B4.store.version.KEforAdd',
                    textProperty: 'Name',                    
                    store: 'B4.store.version.KEforAdd',
                    flex: 1,
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ООИ', dataIndex: 'OOI', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    listeners: {
                        beforeload: function (field, options, store) {
                            options.params = options.params || {};

                            var houseField = field.up().down('b4selectfield[name = RealityObject]'),
                                houseId = houseField.getValue();

                            options.params.houseId = houseId;
                        }
                    }
                },
                {
                    xtype: 'numberfield',
                    name: 'Sum',
                    itemId: 'dfSum',
                    fieldLabel: 'Сумма',
                    decimalSeparator: ',',
                    minValue: 0,
                    allowBlank: false,
                    anchor: '100%',
                }, 
                {
                    xtype: 'numberfield',
                    name: 'Volume',
                    itemId: 'dfVolume',
                    fieldLabel: 'Объем',
                    decimalSeparator: ',',
                    minValue: 0,
                    allowBlank: false,
                    anchor: '100%',
                }, 
                {
                    xtype: 'numberfield',
                    allowBlank: false,
                    name: 'Year',
                    fieldLabel: 'Год',
                    minValue: 2014,
                    itemId: 'dfYear',
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
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    //margin:'2 10 3 10',
                                    xtype: 'b4closebutton',
                                    text: 'Отмена'
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