Ext.define('B4.view.Control.GkhTriggerField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.gkhtriggerfield',

    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);

        me.callParent(arguments);

        me.addEvents(
              'triggerOpenForm',
              'triggerClear'
        );
    },
    
    title: 'Выбор элементов',
    value: '',

    trigger1Cls: 'x-form-search-trigger',
    trigger2Cls: 'x-form-clear-trigger',

    /*
     * Показываем окно со справочником
     */
    onTrigger1Click: function () {
        var me = this;

        me.fireEvent('triggerOpenForm', me);
    },

    /*
     * Очищаем поле
     */
    onTrigger2Click: function () {
        var me = this;

        me.fireEvent('triggerClear', this);
        me.setValue('');
        me.updateDisplayedText();
    },

    /**
     * Устанавливаем значение поля. 
     */
    setValue: function (value) {
        var me = this;

        me.value = value;
        me.isValid();
    },

    /**
     * Возвращает значение поля. 
     */
    getValue: function () {
        var me = this;

        if (me.isGetOnlyIdProperty && me.value) {
            return me.value[me.valueProperty];
        }

        return me.value;
    },

    /**
     * Обновление отображаемого текста в поле
     */
    updateDisplayedText: function (text) {
        var me = this;

        me.setRawValue.call(me, text);
    }

});