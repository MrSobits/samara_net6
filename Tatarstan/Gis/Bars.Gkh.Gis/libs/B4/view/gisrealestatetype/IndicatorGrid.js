Ext.define('B4.view.gisrealestatetype.IndocatorGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisrealestateindicatorgrid',
    
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.column.Delete',
        'B4.store.gisrealestate.realestatetype.RealEstateTypeIndicator',
        'Ext.grid.plugin.CellEditing',
        
        'B4.form.ComboBox'
    ],
      
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.gisrealestate.realestatetype.RealEstateTypeIndicator'),
            indicatorStore = Ext.create('B4.store.gisrealestate.Indicator');
        indicatorStore.load();

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealEstateIndicator',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'b4combobox',
                        store: indicatorStore,
                        displayField: 'Name',
                        valueField: 'Id',
                        editable: false
                    },
                    renderer: function (val) {
                        if (Ext.isObject(val)) {
                            return val.Name; 
                        }

                        return Ext.isEmpty(val)
                            ? ''
                            : indicatorStore.findRecord('Id', val).get('Name');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PrecisionValue',
                    flex: 1,
                    text: 'Точное значение',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    renderer: Ext.Function.bind(me.renderCell, me)
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Min',
                    flex: 1,
                    text: 'Минимальное значение',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    renderer: Ext.Function.bind(me.renderCell, me)
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Max',
                    flex: 1,
                    text: 'Максимальное значение',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    renderer: Ext.Function.bind(me.renderCell, me)
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                {
                    ptype: 'cellediting',
                    pluginId: 'cellEditing',
                    clicksToEdit: 1,
                    listeners: {
                        beforeedit: function(e, eOpts) {
                            var record = eOpts.record;

                            if (eOpts.field == "RealEstateIndicator") {
                                return true;
                            }

                            // Но надо проверять чтобы было задано только что-то одно - либо диапазон, либо точное значение
                            if (eOpts.field == 'PrecisionValue') {
                                if (!Ext.isEmpty(record.get('Min')) || !Ext.isEmpty(record.get('Min'))) {
                                    Ext.Msg.confirm('Внимание', 'Должно быть заполнено только одно из значений - либо "Точное значение", ' +
                                        'либо диапазон значений. Очистить диапазон?', function(confirmationResult) {
                                            if (confirmationResult != 'yes') {
                                                return false;
                                            }

                                            record.set('Min', '');
                                            record.set('Max', '');

                                            return true;
                                        });
                                }

                                return true;
                            } else if (eOpts.field == 'Min' || eOpts.field == 'Max') {
                                if (!Ext.isEmpty(record.get('PrecisionValue'))) {
                                    Ext.Msg.confirm('Внимание', 'Должно быть заполнено только одно из значений - либо "Точное значение", ' +
                                        'либо диапазон значений. Очистить поле точного значения?', function(confirmationResult) {
                                            if (confirmationResult == 'yes') {
                                                record.set('PrecisionValue', null);
                                                
                                                return true;
                                            }

                                            return false;
                                        });
                                }

                                return true;
                            }

                            return eOpts.field == 'PrecisionValue';
                        }
                    }
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    },
    
    renderCell: function (val, meta, rec, rowIndex, colIndex) {
        var me = this,
            field = Ext.ComponentQuery.query('gisrealestateindicatorgrid')[0].columns[colIndex].dataIndex;


                // дизейблим колонку незаполненную группу - либо точное значение, либо диапазон
                if (field == 'PrecisionValue') {
                    if (!Ext.isEmpty(rec.get('Min')) || !Ext.isEmpty(rec.get('Max'))) {
                        meta.style = 'background: #EAEAEA';
                        return null;
                    }
                } else if (field == 'Min' || field == 'Max')  {
                    if (!Ext.isEmpty(rec.get('PrecisionValue'))) {
                        meta.style = 'background: #EAEAEA';
                        return null;
                    }
                }
        
        return val;
    }
});