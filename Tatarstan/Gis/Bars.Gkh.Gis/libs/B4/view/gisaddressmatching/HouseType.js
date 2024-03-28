Ext.define('B4.view.gisaddressmatching.HouseType', {
    extend: 'B4.form.Window',

    alias: 'widget.gisaddressmatchinghouseType',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 450,
    minWidth: 450,
    height: 130,
    minHeight: 130,
    resizable: false,
    bodyPadding: 5,
    title: 'Присвоение типа дома для выбранных домов',
    closeAction: 'hide',
    //trackResetOnLoad: true,
    modal: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create("B4.store.gisaddressmatching.GisAddress");        

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 140,
                editable: false,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MatchedHouses',
                    fieldLabel: 'Сопоставленные дома',
                    store: store,                    
                    blankText: 'Это поле обязательно для заполнения',
                    selectionMode: 'MULTI',
                    idProperty: 'Id',
                    textProperty: 'Address',
                    columns: [
                        {
                            text: 'Регион',
                            dataIndex: 'RegionName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Населенный пункт',
                            dataIndex: 'CityName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Улица',
                            dataIndex: 'StreetName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Номер дома',
                            dataIndex: 'Number',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Поставщик',
                            dataIndex: 'Supplier',
                            flex: 2,
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            text: 'Тип',
                            dataIndex: 'HouseType',
                            flex: 2,
                            filter: {
                                xtype: 'b4combobox',
                                store: B4.enums.TypeHouse.getStore(),
                                valueField: 'Value',
                                displayField: 'Display',
                                editable: false,
                                emptyItem: { Display: '-' },
                                operand: CondExpr.operands.eq
                            },
                            renderer: function (val) {
                                return B4.enums.TypeHouse.displayRenderer(val);
                            }
                        }
                    ],
                    onSelectAll: function () {
                        var me = this,
                            oldValue = me.getValue(),
                            isValid = me.getErrors() != '';

                        me.updateDisplayedText('Выбраны все');
                        me.value = 'All';
                        
                        me.fireEvent('validitychange', me, isValid);
                        me.fireEvent('change', me, 'All', oldValue);
                        me.validate();
                        
                        me.selectWindow.hide();
                    },
                },
                {
                    xtype: 'b4combobox',
                    name: 'houseTypes',
                    store: B4.enums.TypeHouse.getStore(),
                    fieldLabel: 'Тип дома',
                    valueField: 'Value',
                    displayField: 'Display',
                    queryMode: 'local'
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
                                },
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