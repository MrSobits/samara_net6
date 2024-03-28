Ext.define('B4.view.gisrealestatetype.CommonParamGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.giscommonparameditgrid',
    
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.column.Delete',
        'B4.store.gisrealestate.realestatetype.RealEstateTypeCommonParam',
        'Ext.grid.plugin.CellEditing',
        
        'B4.enums.CommonParamType',
        'B4.enums.TypeHouse',
        'B4.enums.TypeRoof',
        'B4.enums.HeatingSystem',
        'B4.form.ComboBox',
        'B4.store.dict.WallMaterial',
        'B4.store.dict.RoofingMaterial',
        'B4.store.dict.TypeProject'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.gisrealestate.realestatetype.RealEstateTypeCommonParam'),
            cpStore = me.commonParamStore = Ext.create('B4.base.Store', {
                fields: ['Code', 'Name', 'CommonParamType', 'IsPrecision'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'CommonParams'
                },
                autoLoad: true
            }),
            wmStore = me.wallMaterialStore = Ext.create('B4.store.dict.WallMaterial'),
            rmStore = me.roofMaterialStore = Ext.create('B4.store.dict.RoofingMaterial'),
            tpStore = me.typeProjectStore = Ext.create('B4.store.dict.TypeProject');
        cpStore.load();
        wmStore.load();
        rmStore.load();
        tpStore.load();
        
        var editors = {};
        editors[me.getEditorKey('Date')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Date', {
                selectOnFocus: true
            })
        });
        editors[me.getEditorKey('Decimal')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Number', {
                selectOnFocus: true,
                allowDecimals: true,
                decimalPrecision: 2
            })
        });
        editors[me.getEditorKey('Integer')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Number', {
                selectOnFocus: true
            })
        });
        editors[me.getEditorKey('TypeHouse')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: B4.enums.TypeHouse.getStore(),
                displayField: 'Display',
                valueField: 'Value',
                editable: false                
            }),
            listeners: {
                beforestartedit: function (editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();
                        
                        return false;
                    }
                    
                    return true;
                }
            }
        });
        editors[me.getEditorKey('TypeRoof')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: B4.enums.TypeRoof.getStore(),
                displayField: 'Display',
                valueField: 'Value',
                editable: false
            }),
            listeners: {
                beforestartedit: function (editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();

                        return false;
                    }

                    return true;
                }
            }
        });
        editors[me.getEditorKey('WallMaterial')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: wmStore,
                valueField: 'Id',
                editable: false
            }),
            listeners: {
                beforestartedit: function (editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();

                        return false;
                    }

                    return true;
                }
            }
        });
        editors[me.getEditorKey('RoofingMaterial')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: rmStore,
                displayField: 'Name',
                valueField: 'Id',
                editable: false
            }),
            listeners: {
                beforestartedit: function(editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();

                        return false;
                    }

                    return true;
                }
            }
        });
        editors[me.getEditorKey('TypeProject')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: tpStore,
                valueField: 'Id',
                editable: false
            }),
            listeners: {
                beforestartedit: function(editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();

                        return false;
                    }

                    return true;
                }
            }
        });
        editors[me.getEditorKey('HeatingSystem')] = Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('B4.form.ComboBox', {
                selectOnFocus: true,
                store: B4.enums.HeatingSystem.getStore(),
                displayField: 'Display',
                valueField: 'Value',
                editable: false
            }),
            listeners: {
                beforestartedit: function (editor, boundEl, value) {
                    if (value) {
                        editor.field.reset();
                        editor.field.setValue(+value);

                        editor.realign(true);
                        editor.editing = true;
                        editor.show();

                        return false;
                    }

                    return true;
                }
            }
        });
        
        me.editors = editors;
        
        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonParamCode',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'b4combobox',
                        store: cpStore,
                        displayField: 'Name',
                        valueField: 'Code',
                        editable: false
                    },
                    renderer: function (val) {
                        var record;
                        if (val) {
                            record = cpStore.findRecord('Code', val);
                            return record.get('Name');
                        }
                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PrecisionValue',
                    flex: 1,
                    text: 'Точное значение',
                    renderer: Ext.Function.bind(me.renderCell, me),
                    getEditor: Ext.Function.bind(me.getCellEditor, me)
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Min',
                    flex: 1,
                    text: 'Минимальное значение',
                    renderer: Ext.Function.bind(me.renderCell, me),
                    getEditor: Ext.Function.bind(me.getCellEditor, me)
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Max',
                    flex: 1,
                    text: 'Максимальное значение',
                    renderer: Ext.Function.bind(me.renderCell, me),
                    getEditor: Ext.Function.bind(me.getCellEditor, me)
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
                        beforeedit: function (e, eOpts) {
                            var record = eOpts.record,
                                isPrecision,
                                commonParamCode;
                            
                            if (eOpts.field == "CommonParamCode") {
                                return true;
                            }

                            commonParamCode = this.grid.commonParamStore.findRecord('Code', eOpts.record.get('CommonParamCode'));
                            
                            if (commonParamCode) {                              
                                isPrecision = commonParamCode.get('IsPrecision');

                                // Если тип не является точным значением значит он может быть задан и как диапазон и как точное значение
                                if (!isPrecision) {
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
                                    }
                                    else if (eOpts.field == 'Min' || eOpts.field == 'Max') {
                                        if (!Ext.isEmpty(record.get('PrecisionValue'))) {
                                            Ext.Msg.confirm('Внимание', 'Должно быть заполнено только одно из значений - либо "Точное значение", ' +
                                                'либо диапазон значений. Очистить поле точного значения?', function (confirmationResult) {
                                                    if (confirmationResult == 'yes') {
                                                        record.set('PrecisionValue', null);

                                                        return true;
                                                    }

                                                    return false;
                                                });
                                        }

                                        return true;
                                    }
                                }

                                return eOpts.field == 'PrecisionValue';
                            }
                            
                            return false;
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
            result = val,
            field = Ext.ComponentQuery.query('giscommonparameditgrid')[0].columns[colIndex].dataIndex,
            commonParamType,
            commonParamCode,
            isPrecision,
            record;

        commonParamCode = me.commonParamStore.findRecord('Code', rec.get('CommonParamCode'));
        if (commonParamCode) {
            isPrecision = commonParamCode.get('IsPrecision');
            if (isPrecision) {
                // дизейблим колонки минимального и максимального значения
                if (field == 'Min' || field == 'Max') {
                    meta.style = 'background: #EAEAEA';
                    return null;
                }
            }
            else {
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
            }
        }
        
        if (val) {
            if (Ext.isDate(val)) {
                result = Ext.util.Format.date(val, 'd.m.Y');
            }
            else {
                if (commonParamCode) {
                    commonParamType = commonParamCode.get('CommonParamType');
                    switch (commonParamType) {
                    case 30:
                        result = Ext.util.Format.date(val, 'd.m.Y');
                        break;
                    case 40:
                        record = B4.enums.TypeHouse.getStore().findRecord('Value', val);

                        result = record ? record.get('Display') : '';
                        break;
                    case 50:
                        record = B4.enums.TypeRoof.getStore().findRecord('Value', val);

                        result = record ? record.get('Display') : '';
                        break;
                    case 60:
                        result = me.getDisplayName(me.wallMaterialStore, val);
                        break;
                    case 70:
                        result = me.getDisplayName(me.roofMaterialStore, val);
                        break;
                    case 80:
                        result = me.getDisplayName(me.typeProjectStore, val);
                        break;
                    case 90:
                        result = B4.enums.HeatingSystem.getStore().findRecord('Value', val).get('Display');
                        break;
                    }
                }
            }
        }
        
        return Ext.util.Format.htmlEncode(result);
    },

    getCellEditor: function (record) {
        var commonParam = record.get('CommonParamCode'),
            type;
        if (Ext.isObject(commonParam)) {
            type = commonParam.CommonParamType;
        } else {
            type = Ext.isEmpty(commonParam) 
                ? '' 
                : this.commonParamStore.findRecord("Code", commonParam).get('CommonParamType');
        }

        return this.editors[type] || Ext.create('Ext.grid.CellEditor', {
            field: Ext.create('Ext.form.field.Text', {
                selectOnFocus: true
            })
        });
    },

    getEditorKey: function (typeName) {
        return B4.enums.CommonParamType.getStore().findRecord('Name', typeName).get('Value');
    },
    
    getDisplayName: function (store, val) {
        var findRec = store.findRecord('Id', val);

        if (findRec) {
            return findRec.get('Name');
        } 
            
        return '';
    }
});