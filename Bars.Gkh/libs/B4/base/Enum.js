Ext.define('B4.base.Enum', {

    requires: ['Ext.data.ArrayStore'],

    isEnum: true,

    constructor: function (config) {
        Ext.apply(this, config || {});
    },

    nameRenderer: function (val, style, rec) {
        var meta = this.getMeta(val);
        return Ext.isEmpty(meta) ? val : meta.Name;
    },

    displayRenderer: function (val) {
        var meta = this.getMeta(val);
        return Ext.isEmpty(meta) ? '' : meta.Display;
    },

    displayLegacyRenderer: function (val) {
        var meta = this.getMeta(val);
        return Ext.isEmpty(meta) ? val : meta.Display;
    },

    getMeta: function (val) {
        var me = this;

        if (Ext.isEmpty(val) || Ext.isEmpty(me.Meta) || !Ext.isObject(me.Meta))
            return null;

        var meta = null;

        Ext.iterate(me.Meta, function (key, value) {
            if (!Ext.isObject(value))
                return true;
            if (value['Value'] == val || value['Name'] == val) {
                meta = value;
                return false;
            }
        });

        return meta;
    },

    getItemsMeta: function (defaultValues) {
        var me = this;
        var items = defaultValues || [];
        if (!Ext.isEmpty(me.Meta)) {
            Ext.iterate(me.Meta, function (key, val) {
                if (Ext.isObject(val)) {
                    items.push(val);
                }
            });
        }
        return items;
    },

    check: function (val, enumVal) {
        return val & enumVal !== 0;
    },

    //Метод получения всех значений вида [[Value, Display], [Value, Display]]
    //defaultValues - это значения которыми нужно дополнить список 
    getItems: function (defaultValues) {
        var me = this;
        var items = defaultValues || [];
        if (!Ext.isEmpty(me.Meta)) {
            Ext.iterate(me.Meta, function (key, val) {
                if (Ext.isObject(val)) {
                    var row = [];
                    row.push(val.Value);
                    row.push(val.Display);

                    items.push(row);
                }
            });
        }
        return items;
    },

    //Метод получения всех значений с возможностью выбора пустого
    //входной параметр например такой [null, '-']
    //используется при фильтрации в гридах
    getItemsWithEmpty: function (emptyValue) {
        var empty = [emptyValue];
        return this.getItems(empty);
    },

    /**
    * Создание хранилища (Ext.data.ArrayStore) c элементами перечисления.
    * @param {boolean} includeEmptyElement Включать ли в хранилище пустой элемент
    * @param {int[]} Список элементов для включения в хранилище
    * @return {Ext.data.ArrayStore}
    */
    getStore: function () {
        var me = this;
        var processMeta = {};
        var arr = Ext.Array.from(arguments);
        if (arr.length) {
            if (Ext.isArray(arr[0]))
                arr = arr[0];
            if (Ext.isBoolean(arr[0])) {
                if (arr[0] == true) {
                    processMeta['.All'] = {
                        Name: '.All',
                        Value: null,
                        Display: 'Все'
                    };
                }
                arr = arr.splice(1);
            }

            if (Ext.isArray(arr[0])) {
                arr = arr[0];
            }

            if (Ext.isArray(arr) && arr.length) {
                Ext.each(arr, function (i) {
                    var meta = this.getMeta(i);
                    if (!Ext.isEmpty(meta)) {
                        processMeta[meta.Name] = meta;
                    }
                }, this);
            } else {
                Ext.apply(processMeta, me.Meta);
            }
        } else {
            processMeta = Ext.apply({}, me.Meta || {});
        }

        var fields = new Ext.util.MixedCollection();
        var data = [];
        Ext.iterate(processMeta, function (key, val) {
            if (Ext.isObject(val)) {
                var row = [];
                Ext.iterate(val, function (k, v) {
                    row.push(v);
                    if (!fields.containsKey(k)) {
                        fields.add(k, null);
                    }
                });
                data.push(row);
            }
        });

        return Ext.create('Ext.data.ArrayStore', {
            fields: fields.keys,
            data: data
        });
    }
});