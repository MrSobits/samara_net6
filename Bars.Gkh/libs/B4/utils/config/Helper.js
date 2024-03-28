Ext.define('B4.utils.config.Helper', {
    singleton: true,

    getItems: function (metas) {
        var plainItems = [],
            groups = Object.create(null),
            groupItems = [],
            itemsWithIgnoreRestriction = [];

        // validation type для полей, от которых зависят другие
        var defaultRestrictionVType = {
            defaultRestriction: function(val, field){
                var dependedField = Ext.ComponentQuery.query('[name=' + field.dependedField + ']')[0];
                
                if(field.value === field.restrictionValue){
                    if(dependedField){
                        Ext.each(field.restrictions, function(restriction) {
                           dependedField[restriction] = undefined; 
                        });
                    }
                }
                else{
                    if(dependedField){
                        Ext.each(dependedField.restrictionValues, function(restriction) {
                            dependedField[restriction.field] = restriction.value;
                        });
                    }
                }

                dependedField.validate();
                return true;
            }
        };
        Ext.apply(Ext.form.field.VTypes, defaultRestrictionVType);
        
        Ext.each(metas, function (meta) {
            var item = B4.utils.config.Helper.getEditor(meta);
            
            // запоминаем поля с условным снятием ограничений
            if(item.editorRestrictionIgnoreCondition){
                itemsWithIgnoreRestriction.push(item);
            }
            
            if (meta.groupName) {
                if (!groups[meta.groupName]) {
                    groups[meta.groupName] = [item];
                } else {
                    groups[meta.groupName].push(item);
                }
            } else {
                plainItems.push(item);
            }
        });
        
        Ext.each(itemsWithIgnoreRestriction, function(item) {
            var restrictionIgnoreObject = Ext.JSON.decode(item.editorRestrictionIgnoreCondition),
                dependencyField = plainItems.find(function(x) {
                    if (x.name === restrictionIgnoreObject.dependedFieldName) {
                        return true;
                    }
                }),
                dependencyFieldProxy = dependencyField,
                itemProxy = item,
                restrictionValues = [];

            // запоминаем значения ограничений ведомого поля
            Ext.each(restrictionIgnoreObject.restrictions, function(restriction) {
                restrictionValues.push({
                    field: restriction,
                    value: item[restriction]
                });
            });
            Ext.apply(itemProxy, {restrictionValues: restrictionValues});
            plainItems.splice(plainItems.indexOf(item), 1, itemProxy);
            
            // передаем параметры ограничений полю, от которого зависит ведомое
            if(dependencyField) {
                var restrictionObject = {
                    dependedField: item.name, // имя поля
                    restrictionValue:
                        restrictionIgnoreObject.dependedFieldValue, // значение, при котором ограничения снимаются
                    restrictions: restrictionIgnoreObject.restrictions, // ограничения
                    vtype: 'defaultRestriction'
                };
                Ext.apply(dependencyFieldProxy, restrictionObject);
                plainItems.splice(plainItems.indexOf(dependencyField), 1, dependencyFieldProxy);
            }
        });

        for (var name in groups) {
            groupItems.push({
                xtype: 'fieldset',
                title: name,
                items: groups[name],
                defaults: {
                    labelWidth: 150,
                    anchor: '100%',
                    margin: '0 0 5 0'
                }
            });
        }

        return groupItems.concat(plainItems);
    },

    getEditor: function (meta) {
        var type = meta.type || { editor: 'raw' },
            fieldLabel = meta.displayName,
            editor = {
                name: meta.id || meta.name,
                fieldLabel: fieldLabel,
                displayName: fieldLabel,
                value: meta.value,
                defaultValue: meta.defaultValue,
                readOnly: meta.readOnly,
                labelWidth: 250,
                labelAlign: 'right'
            };

        switch (type.editor) {
            case 'custom':
                if (type.path && typeof type.path == 'string' && type.path.length > 0) {
                    Ext.Loader.syncRequire(type.path);
                }

                editor.xtype = type.xtype;
                editor.meta = type.meta;
                editor.minValue = type.minValue;
                editor.maxValue = type.maxValue;
                break;
            case 'text':
                editor.xtype = 'textfield';
                break;
            case 'bool':
                editor.xtype = 'checkbox';
                editor.checked = meta.value;
                break;
            case 'number':
                editor.xtype = 'numberfield';
                editor.allowDecimals = type.decimals;
                editor.minValue = type.minValue;
                editor.maxValue = type.maxValue;
                editor.decimalSeparator = ',';
                editor.decimalPrecision = type.decimals ? (type.decimalsCount ? type.decimalsCount : 2) : 0;
                break;
            case 'date':
                editor.xtype = 'datefield';
                editor.format = 'd.m.Y';
                editor.submitFormat = 'Y-m-dTH:i:s';
                editor.maxWidth = 370;
                break;
            case 'enum':
                editor.xtype = 'enumeditor';
                editor.values = type.values;
                break;
            case 'collection':
                editor.xtype = 'collectioneditor';
                editor.hideToolbar = type.hideToolbar;
                editor.elementType = type.elementType;
                editor.minHeight = 100;
                break;
            case 'dictionary':
                editor.xtype = 'dictionaryeditor';
                editor.keyType = type.keyType;
                editor.valueType = type.valueType;
                editor.minHeight = 100;
                break;
            case 'object':
                editor.xtype = 'objecteditor';
                editor.properties = type.properties;
                editor.typeName = type.typeName;
                editor.displayName = type.displayName;
                break;
            case 'raw':
                editor.xtype = 'raweditor';
                editor.typeName = type.typeName;
                editor.sample = type.sample;
                editor.minHeight = 100;
                break;
            case 'section':
                return {
                    xtype: 'fieldset',
                    title: meta.displayName,
                    items: B4.utils.config.Helper.getItems(meta.children),
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        margin: '0 0 5 0'
                    }
                };
            default:
                if (rec.get(this.dataIndex) instanceof Object) {
                    editor.xtype = 'raweditor';
                } else {
                    editor.xtype = 'textfield';
                }
                break;
        }

        Ext.apply(editor, meta.extraParams || {});
        return editor;
    }
});