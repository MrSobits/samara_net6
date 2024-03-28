Ext.define('B4.aspects.SchedulerEditWindow', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.schedulereditwindowaspect',

    constructor: function (config) {
        var asp = this;
        asp.callParent(arguments);

        Ext.apply(asp, config);
        asp.on('aftersetformdata', asp._onAfterSetFormData, asp);
    },

    _onAfterSetFormData: function (asp, record) {
        var form = asp.getForm(),
            schedulerPanel = form.down('schedulerpanel'),
            saveButton = form.down('b4savebutton'),
            rawRecord = record.raw,
            id = record.get('Id') || 0;

        if (id === 0) {
            schedulerPanel.setValues({ StartNow: true });
            asp.updateStartButton(saveButton);
        } else {
            asp.customSetValues(rawRecord, 'PeriodType');
            asp.customSetValues(rawRecord, 'StartDayOfWeekList');
            asp.customSetValues(rawRecord, 'StartMonthList');
            asp.customSetValues(rawRecord, 'StartDaysList');

            asp.updateRestartButton(saveButton);
            record.setDirty();
        }
    },

    updateStartButton: function (button) {
        button.setText('Создать задачу');
    },

    updateRestartButton: function (button) {
        button.setIconCls('icon-arrow-refresh')
        button.setText('Перезапустить задачу');
    },

    getRecordBeforeSave: function (record) {
        var form = this.getForm(),
            schedulerPanel = form.down('schedulerpanel');
        return schedulerPanel.getRecord(record);
    },

    deleteRecord: function (record) {
        var asp = this;

        Ext.Msg.confirm('Удаление задачи', 'Выполняемая задача будет прервана. Вы действительно хотите удалить задачу?',
            function(result) {
                if (result == 'yes') {
                    var model = asp.getModel(record);
                    var rec = new model({ Id: record.getId() });
                    asp.mask('Удаление', B4.getBody());
                    rec.destroy()
                        .next(function() {
                            asp.fireEvent('deletesuccess', asp);
                            asp.updateGrid();
                            asp.unmask();
                        }, asp)
                        .error(function(result) {
                            Ext.Msg.alert('Ошибка удаления!',
                                Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            asp.unmask();
                        }, asp);
                }
            }, asp);
    },

    customSetValues: function (record, paramName) {
        var asp = this,
            form = asp.getForm(),
            value = {};
        value[paramName] = record[paramName];
        form.down('[name=' + paramName + ']').setValue(value);
    }
});