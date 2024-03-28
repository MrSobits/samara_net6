/**
 * Чекбокс групп для отображения flag enum значений в виде чекбоксов
    {
        xtype: 'enumcheckboxgroup',
        name: 'ButtonSet',
        fieldLabel: 'Отображаемые кнопки',
        enumName: 'B4.enums.ButtonType',
        allowButtons: [
            B4.enums.ButtonType.Accept,
            B4.enums.ButtonType.Decline,
            B4.enums.ButtonType.Familiarize
        ],
        labelAlign: 'left',
        labelWidth: 150,
        columns: 3,
        allowBlank: false,
        blankText: 'Необходимо выбрать хотя бы одно значение'
    }
 */
Ext.define('B4.ux.form.field.EnumCheckboxGroup', {
    extend: 'Ext.form.CheckboxGroup',

    alias: 'widget.enumcheckboxgroup',
    enumName: '',

    allowButtons: null,

    initComponent: function () {
        var me = this,
            enumInstance = Ext.ClassManager.get(me.enumName),
            buttonItems = [];

        if (!enumInstance) {
            Ext.Error.raise('Не передан тип перечисления. Имя параметра "enumName"');
        }

        if (!Ext.isArray(me.allowButtons)) {
            me.allowButtons = [];
            Ext.each(enumInstance.getItemsMeta(), function(meta) {
                me.allowButtons.push(meta.Value);
            });
        }

        Ext.each(enumInstance.getItemsMeta(),
            function(meta) {
                if ( me.allowButtons.indexOf(meta.Value) !== -1) {
                    buttonItems.push(
                        {
                            inputValue: meta.Value,
                            boxLabel: meta.Display
                        }
                    );
                }
            });

        Ext.apply(me, {
            items: buttonItems
        });

        me.callParent(arguments);
    },

    setValue: function (value) {
        var me = this;
        Ext.each(me.items.items,
            function (item) {
                if (me.allowButtons.indexOf(item.inputValue) !== -1 && (value & item.inputValue) !== 0) {
                    item.setValue(true);
                }
            });
        me.checkChange();
        return this;
    },
    getValue: function () {
        var me = this,
            result = 0;
        Ext.each(me.items.items,
            function (item) {
                if (item.checked && me.allowButtons.indexOf(item.inputValue) !== -1) {
                    result |= item.inputValue;
                }
            });

        return result;
    },
    isEqual: function(newVal, oldVal) {
        return newVal === oldVal;
    },
    getModelData: function () {
        var me = this,
            res = {};
        res[me.name] = me.getValue();
        return res;
    }
});