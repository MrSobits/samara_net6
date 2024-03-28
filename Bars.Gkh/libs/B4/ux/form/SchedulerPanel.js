Ext.define('B4.ux.form.SchedulerPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.schedulerpanel',

    bodyStyle: Gkh.bodyStyle,
    border: false,

    beforeRender: function () {
        var me = this;

        me.down('radiogroup[name=PeriodType]').on('change', me.onPeriodicityChange, me);
        me.down('checkbox[name=StartNow]').on('change', me.onStartNowChange, me);
        me.down('checkboxgroup[name=StartDaysList] [inputValue=0]').on('change', me.onLastDayChange, me);

        me.callParent(arguments);
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'radiogroup',
                            name: 'PeriodType',
                            vertical: true,
                            columns: 1,
                            width: 150,
                            padding: '5',
                            border: '0 2 0 0',
                            style: {
                                borderStyle: 'solid',
                                borderColor: Gkh.borderColor
                            },
                            defaults: {
                                padding: '5',
                                name: 'PeriodType'
                            },
                            items: [
                                { boxLabel: 'Однократно', inputValue: 0 },
                                { boxLabel: 'Ежедневно', inputValue: 1 },
                                { boxLabel: 'Еженедельно', inputValue: 2 },
                                { boxLabel: 'Ежемесячно', inputValue: 3 }
                            ],
                        },
                        {
                            xtype: 'container',
                            flex: 6,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            padding: '5 5 5 15',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    padding: '0 0 10 0',
                                    defaults: {
                                        labelWidth: 100,
                                        padding: '5 15 5 0',
                                        width: 220
                                    },
                                    name: 'DateInterval',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'StartDate',
                                            fieldLabel: 'Дата начала',
                                            disabled: true,
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EndDate',
                                            fieldLabel: 'Дата окончания',
                                            disabled: true,
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'StartNow',
                                            fieldLabel: 'Запустить сейчас',
                                            margin: '5 20 0 0',
                                            checked: true,
                                            labelWidth: 110
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            name: 'TimeInterval',
                                            items: [
                                                {
                                                    xtype: 'label',
                                                    text: 'Время запуска:',
                                                    padding: '5 0',
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'StartTimeHour',
                                                    width: 60,
                                                    padding: '0 10',
                                                    allowDecimals: false,
                                                    disabled: true,
                                                    maxValue: 23,
                                                    minValue: 0,
                                                    value: 0
                                                },
                                                {
                                                    xtype: 'label',
                                                    text: 'час.',
                                                    padding: '5 0'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'StartTimeMinutes',
                                                    width: 60,
                                                    padding: '0 10',
                                                    allowDecimals: false,
                                                    disabled: true,
                                                    maxValue: 59,
                                                    minValue: 0,
                                                    value: 0
                                                },
                                                {
                                                    xtype: 'label',
                                                    text: 'мин.',
                                                    padding: '5 0'
                                                }
                                            ],
                                        },
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'каждую неделю по',
                                    margin: '10 0',
                                    name: 'StartDayOfWeekList',
                                    allowBlank: false,
                                    disabled: true,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartDayOfWeekList',
                                    },
                                    items: [
                                        { boxLabel: 'Пн', inputValue: 1 },
                                        { boxLabel: 'Вт', inputValue: 2 },
                                        { boxLabel: 'Ср', inputValue: 3 },
                                        { boxLabel: 'Чт', inputValue: 4 },
                                        { boxLabel: 'Пт', inputValue: 5 },
                                        { boxLabel: 'Сб', inputValue: 6 },
                                        { boxLabel: 'Вс', inputValue: 7 }
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'в течение следующих месяцев',
                                    columns: 6,
                                    margin: '15 0',
                                    name: 'StartMonthList',
                                    allowBlank: false,
                                    disabled: true,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartMonthList',
                                    },
                                    items: [
                                        { boxLabel: 'Январь', inputValue: 1 },
                                        { boxLabel: 'Февраль', inputValue: 2 },
                                        { boxLabel: 'Март', inputValue: 3 },
                                        { boxLabel: 'Апрель', inputValue: 4 },
                                        { boxLabel: 'Май', inputValue: 5 },
                                        { boxLabel: 'Июнь', inputValue: 6 },
                                        { boxLabel: 'Июль', inputValue: 7 },
                                        { boxLabel: 'Август', inputValue: 8 },
                                        { boxLabel: 'Сентябрь', inputValue: 9 },
                                        { boxLabel: 'Октябрь', inputValue: 10 },
                                        { boxLabel: 'Ноябрь', inputValue: 11 },
                                        { boxLabel: 'Декабрь', inputValue: 12 }
                                    ]
                                },
                                {
                                    xtype: 'checkboxgroup',
                                    labelAlign: 'top',
                                    fieldLabel: 'каждый день месяца по',
                                    columns: 8,
                                    margin: '15 0',
                                    name: 'StartDaysList',
                                    allowBlank: false,
                                    disabled: true,
                                    blankText: 'Необходимо выбрать хотя бы одно значение',
                                    defaults: {
                                        name: 'StartDaysList',
                                    },
                                    items: [
                                        { boxLabel: '1', inputValue: 1 },
                                        { boxLabel: '2', inputValue: 2 },
                                        { boxLabel: '3', inputValue: 3 },
                                        { boxLabel: '4', inputValue: 4 },
                                        { boxLabel: '5', inputValue: 5 },
                                        { boxLabel: '6', inputValue: 6 },
                                        { boxLabel: '7', inputValue: 7 },
                                        { boxLabel: '8', inputValue: 8 },
                                        { boxLabel: '9', inputValue: 9 },
                                        { boxLabel: '10', inputValue: 10 },
                                        { boxLabel: '11', inputValue: 11 },
                                        { boxLabel: '12', inputValue: 12 },
                                        { boxLabel: '13', inputValue: 13 },
                                        { boxLabel: '14', inputValue: 14 },
                                        { boxLabel: '15', inputValue: 15 },
                                        { boxLabel: '16', inputValue: 16 },
                                        { boxLabel: '17', inputValue: 17 },
                                        { boxLabel: '18', inputValue: 18 },
                                        { boxLabel: '19', inputValue: 19 },
                                        { boxLabel: '20', inputValue: 20 },
                                        { boxLabel: '21', inputValue: 21 },
                                        { boxLabel: '22', inputValue: 22 },
                                        { boxLabel: '23', inputValue: 23 },
                                        { boxLabel: '24', inputValue: 24 },
                                        { boxLabel: '25', inputValue: 25 },
                                        { boxLabel: '26', inputValue: 26 },
                                        { boxLabel: '27', inputValue: 27 },
                                        { boxLabel: '28', inputValue: 28 },
                                        { boxLabel: '29', inputValue: 29 },
                                        { boxLabel: '30', inputValue: 30 },
                                        { boxLabel: '31', inputValue: 31 },
                                        { boxLabel: 'посл.', inputValue: 0 }
                                    ]
                                },
                                {
                                    xtype: 'tbfill',
                                    height: 10
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onPeriodicityChange: function(component, newValue, oldValue) {
        var parentContainer = component.up('container'),
            dateIntervalField = parentContainer.down('container[name=DateInterval]'),
            timeIntervalField = parentContainer.down('container[name=TimeInterval]'),
            dayOfWeekField = parentContainer.down('checkboxgroup[name=StartDayOfWeekList]'),
            monthField = parentContainer.down('checkboxgroup[name=StartMonthList]'),
            dayField = parentContainer.down('checkboxgroup[name=StartDaysList]'),
            startNowCheckBox = parentContainer.down('checkbox[name=StartNow]');

        Ext.each(dateIntervalField.query(), function(component) { component.disable() });
        Ext.each(timeIntervalField.query(), function(component) { component.disable() });
        dayOfWeekField.disable();
        dayOfWeekField.unsetActiveError();
        monthField.disable();
        monthField.unsetActiveError();
        dayField.disable();
        dayField.unsetActiveError();
        startNowCheckBox.setValue(false);
        startNowCheckBox.disable();

        switch (newValue.PeriodType) {
            case 0:
                Ext.each(timeIntervalField.query(), function(component) { component.enable() });
                startNowCheckBox.enable();
                break;

            case 1:
                Ext.each(dateIntervalField.query(), function(component) { component.enable() });
                Ext.each(timeIntervalField.query(), function(component) { component.enable() });
                break;

            case 2:
                Ext.each(dateIntervalField.query(), function(component) { component.enable() });
                Ext.each(timeIntervalField.query(), function(component) { component.enable() });
                dayOfWeekField.enable();
                break;

            case 3:
                Ext.each(dateIntervalField.query(), function(component) { component.enable() });
                Ext.each(timeIntervalField.query(), function(component) { component.enable() });
                monthField.enable();
                dayField.enable();
                break;
        }
    },

    onStartNowChange: function (component, newValue) {
        var parentContainer = component.up('container'),
            timeIntervalField = parentContainer.down('container[name=TimeInterval]');

        Ext.each(timeIntervalField.query(), function (component) { component.setDisabled(newValue) });
    },

    onLastDayChange: function (component, newValue) {
        var checkboxGroup = component.up('checkboxgroup');

        Ext.each(checkboxGroup.query(), function (checkBox) {
            if (checkBox != component) {
                checkBox.setDisabled(newValue);
            }
        });
    },

    getFormValues: function() {
        var me = this,
            asString = false,
            dirtyOnly = false,
            includeEmptyText = false,
            useDataValues = true;

        return me.getForm().getValues(asString, dirtyOnly, includeEmptyText, useDataValues);
    },

    getValues: function() {
        var me = this,
            values = me.getFormValues(),
            periodType = values.PeriodType,
            startDayOfWeekList = values.StartDayOfWeekList,
            startMonthList = values.StartMonthList,
            startDaysList = values.StartDaysList,
            startDayOfWeekListValue = [],
            startMonthListValue = [],
            startDaysListValue = [];

        switch (periodType) {
            case 2:
                startDayOfWeekListValue = (function (value) {
                    var codes = [];
                    if (value) {
                        Ext.each(value,
                            function (v, i) {
                                if (v) {
                                    codes.push(i + 1);
                                }
                            });
                    }
                    return codes;
                })(startDayOfWeekList);
                break;
            case 3:
                startMonthListValue = (function (value) {
                    var codes = [];
                    if (value) {
                        Ext.each(value,
                            function (v, i) {
                                if (v) {
                                    codes.push(i + 1);
                                }
                            });
                    }
                    return codes;
                })(startMonthList);
                startDaysListValue = (function (value) {
                    var codes = [];
                    if (value) {
                        if (typeof value === 'boolean') {
                            codes.push(0);
                        } else {
                            Ext.each(value,
                                function (v, i) {
                                    if (v) {
                                        codes.push(i + 1);
                                    }
                                });
                        }
                    }
                    return codes;
                })(startDaysList);
                break;
        }
        values.StartDayOfWeekList = startDayOfWeekListValue;
        values.StartMonthList = startMonthListValue;
        values.StartDaysList = startDaysListValue;

        return values;
    },

    setValues: function(value) {
        var me = this;

        me._setValue('StartDate', value);
        me._setValue('EndDate', value);
        me._setValue('StartTimeHour', value);
        me._setValue('StartTimeMinutes', value);
        me._setObjectValue('StartDayOfWeekList', value);
        me._setObjectValue('StartMonthList', value);
        me._setObjectValue('StartDaysList', value);

        me._setObjectValue('PeriodType', value);
        me._setValue('StartNow', value);
    },

    getRecord: function (record) {
        var me = this,
            values = me.getValues();

        me._setRecordValue(record, values, 'PeriodType');
        me._setRecordValue(record, values, 'StartNow');
        me._setRecordValue(record, values, 'StartDate');
        me._setRecordValue(record, values, 'EndDate');
        me._setRecordValue(record, values, 'StartTimeHour');
        me._setRecordValue(record, values, 'StartTimeMinutes');
        me._setRecordValue(record, values, 'StartDayOfWeekList');
        me._setRecordValue(record, values, 'StartMonthList');
        me._setRecordValue(record, values, 'StartDaysList');

        return record;
    },

    _setRecordValue: function(record, values, paramName) {
        var value = values[paramName];
        if (Ext.isDefined(value) && value !== null) {
            record.set(paramName, value);
        }
    },

    setRecord: function (record, startNowIfNoPeriodicity) {
        var me = this;
        if (startNowIfNoPeriodicity && record.get('PeriodType') === 0) {
            record.set('StartNow', true);
        }
        me.setValues(record.data);
    },

    isValid: function() {
        var me = this;
        return me.getForm().isValid();
    },

    _setValue: function (name, values) {
        var me = this,
            form = me.getForm(),
            field = form.findField(name);

        if (typeof values[name] !== 'undefined' && field) {
            field.setValue(values[name]);
        }
    },

    _setObjectValue: function(name, values) {
        var me = this,
            form = me.getForm(),
            field = form.findField(name),
            newValue = {};

        if (typeof values[name] !== "undefined" && field) {
            newValue[name] = values[name];
            field.setValue(newValue);
        }
    }
});