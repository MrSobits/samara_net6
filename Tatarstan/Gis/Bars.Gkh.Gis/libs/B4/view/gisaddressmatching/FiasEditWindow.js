Ext.define('B4.view.gisaddressmatching.FiasEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    resizable: false,
    bodyPadding: 5,
    alias: 'widget.gisaddressmatchingfiaseditwindow',
    title: 'Добавление адреса',    

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.address.AreaPaging',
        'B4.store.address.PlacePaging',
        'B4.store.address.StreetPaging'
    ],

    initComponent: function() {
        var me = this;            

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Area',
                    fieldLabel: 'Район',
                    allowBlank: false,
                    editable: false,
                    store: 'B4.store.address.AreaPaging',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]         
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Place',
                    fieldLabel: 'Населенный пункт',
                    allowBlank: true,
                    editable: false,
                    store: 'B4.store.address.PlacePaging',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'container',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        pack: 'center',
                        align: 'center'
                    },                    
                    maxLength: 300,
                    items: [                        
                        {
                            xtype: 'b4selectfield',                            
                            store: 'B4.store.address.StreetPaging',
                            modalWindow: true,
                            name: 'Street',                            
                            fieldLabel: 'Улица',
                            allowBlank: false,
                            editable: true,
                            idProperty: 'FormalName',
                            textProperty: 'FormalName',
                            columns: [                                
                                {
                                    text: 'Префикс',
                                    dataIndex: 'ShortName',
                                    width: 60,
                                    align:'right',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    text: 'Наименование',
                                    dataIndex: 'FormalName',
                                    align: 'left',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                            ],
                            windowCfg: {
                                closeAction: 'destroy'
                            },
                            flex: 4
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'StreetShortName',
                            editable: false,
                            url: '/GisAddress/StreetShortNameList',
                            allowBlank: false,
                            margin: '0 0 0 5',
                            flex: 1
                        }
                    ]
                },
                
                {
                    xtype: 'textfield',
                    name: 'House',
                    fieldLabel: 'Дом',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Building',
                    fieldLabel: 'Корпус',
                    allowBlank: true,
                    maxLength: 300
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});