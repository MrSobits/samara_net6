Ext.define('B4.ValueTypes', {
    singleton: true,

    constructor: function () {
        var me = this;
        me.items = {};
        Ext.iterate(me.typeRefKinds, function (name, typeDescr) {
            me[name] = function (config) { return Ext.apply(typeDescr, config); };
        });
    },

    init: function () {
        var me = this;

        return B4.Service.invoke("Bars.B4.UI.ExtJs4.Services.ValueTypesMetaService", "GetEnums")
        .next(function (data) {
            me.typeRefKinds.Enum.Items = data;
        })
        .error(function () { console.log('ValueTypesMetaService warning') });
    },

    getTypeDisplayName: function (typeCode) {
        var type = this.typeRefKinds[typeCode];
        return type && type['displayName'] ? type['displayName'] : 'Не определен';
    },

    register: function (descriptor) {
        var me = this;
        var descr = {};
        descr[descriptor.$className] = descriptor;

        me.items[descriptor.context] = Ext.applyIf(me.items[descriptor.context] || {}, descr);
    },

    getConfig: function (valueTypeRef, context) {
        var me = this,
            descriptors = me.items[context],
            config = {};

        if (descriptors) {
            Ext.iterate(descriptors, function (key, descr) {
                config = Ext.apply(config, descr.getConfig(valueTypeRef));
            });
        }

        return config;
    },

    typeRefKinds: {
        /**
    *  Целое число (знаковое, 64 бита)
    */
        Int: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Int',
            displayName: 'Целое число'
        },

        /**
         * Вещественное число
         */
        Decimal: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Decimal',
            displayName: 'Вещественное число',
            pression: 2
        },

        /**
        *  Строка
        */
        String: {
            $type: 'PrimitiveTypeRef',
            Kind: 'String',
            displayName: 'Строка'
        },

        /**
        *  Дата (без времени)
        */
        Date: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Date',
            displayName: 'Дата (без времени)'
        },

        /**
        *  Время (без даты)
        */
        Time: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Time',
            displayName: 'Время (без даты)'
        },

        /**
        *  Дата с временем
        */
        DateTime: {
            $type: 'PrimitiveTypeRef',
            Kind: 'DateTime',
            displayName: 'Дата с временем'
        },

        /**
        *  Логический тип
        */
        Boolean: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Boolean',
            displayName: 'Логический тип'
        },

        /**
        *  UUID/GUID
        */
        Uuid: {
            $type: 'PrimitiveTypeRef',
            Kind: 'Uuid',
            displayName: 'Уникальный идентификатор (UUID, GUID)'
        },

        /**
        *  Перечисление
        */
        Enum: {
            $type: 'EnumTypeRef',
            displayName: 'Перечисление'
        },

        /**
        *  Композитный тип
        */
        Composite: {
            $type: 'CompositeTypeRef',
            displayName: 'Композитный тип'
        },

        /**
        *  Идентификатор сущности
        */
        EntityId: {
            $type: 'EntityIdTypeRef',
            displayName: 'Идентификатор сущности'
        },

        /**
        *  Список
        */
        List: {
            $type: 'ListTypeRef',
            displayName: 'Список'
        },

        /**
        * Файл
        */
        File: {
            $type: 'FileTypeRef',
            displayName: 'Файл'
        }
    }
});