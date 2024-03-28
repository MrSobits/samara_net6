Ext.define('B4.view.realityobj.RealityObjectConditionHouse', {
    extend: 'Ext.container.Container',
    alias: 'widget.realityobjectconditionhousecmp',
    requires: [
        'B4.enums.ConditionHouse',
        'B4.ux.button.ChangeValue'
    ],

    layout: 'hbox',
    defaults: {
        labelAlign: 'right'
    },
    items: [
        {
            fieldLabel: 'Состояние дома',
            forPermission: 'ConditionHouse',
            itemId: 'cbConditionHouseRealityObject',
            name: 'ConditionHouse',
            labelWidth: 170,
            xtype: 'b4enumcombo',
            enumName: 'B4.enums.ConditionHouse',
            allowBlank: false,
            editable: false,
            readOnly: true,
            includeEmpty: false,
            enumItems: [],
            hideTrigger: false,
            flex: 1
        },
        {
            xtype: 'changevalbtn',
            forPermission: 'ConditionHouse',
            itemId: 'cbConditionHouseRealityObjectbutton',
            className: 'RealityObject',
            propertyName: 'ConditionHouse',
            valueFieldConfig: {
                xtype: 'b4enumcombo',
                enumName: 'B4.enums.ConditionHouse',
                includeEmpty: false,
                fieldLabel: 'Новое значение',
                allowBlank: false
            },
            valueGridColumnConfig: {
                xtype: 'gridcolumn'
            },
            windowContainerSelector: 'realityobjEditPanel',
            changeValue: function (btn) {
                var that = this,
                    frm = btn.up('form'),
                    conditionComboVal = frm.down('b4enumcombo').value,
                    dateField = frm.down('datefield'),
                    params = frm.getValues(),
                    fields = frm.getForm().getFields();

                params.deferredUpdate = true;

                if (!frm.getForm().isValid()) {
                    var invalidFields = '';
                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<br>' + field.fieldLabel;
                        }
                    });
                    Ext.Msg.alert('Ошибка заполнения полей!', 'Не заполнены обязательные поля: ' + invalidFields);
                }

                if (that.fireEvent('beforevaluesave', that, params) === false) {
                    return;
                }

                Ext.applyIf(params, {
                    className: that.className,
                    propertyName: that.propertyName,
                    entityId: that.entityId
                });

                B4.Ajax.request({
                    url: B4.Url.action('PersonalAccountsByRo', 'BasePersonalAccount'),
                    timeout: 1000 * 60 * 5, // 5 минут
                    params: {
                        roId: that.entityId
                    }
                }).next(function (resp) {
                    var data = Ext.JSON.decode(resp.responseText).data;
                    if (data.length > 0
                        && (conditionComboVal === B4.enums.ConditionHouse.Dilapidated
                            || conditionComboVal === B4.enums.ConditionHouse.Emergency)) {
                        Ext.Msg.confirm('Внимание!', 'После изменения состояния дома ЛС по нему будут закрыты. Продолжить?', function (result) {
                            if (result === 'yes') {
                                frm.submit({
                                    url: B4.Url.action('ChangeParameter', 'Parameters'),
                                    params: params,
                                    success: function () {
                                        that.editWindow.close();
                                        that.onValueSaved.apply(that, [params.value, dateField.value]);
                                        that.fireEvent('valuesaved', that, params.value);
                                        Ext.each(fields.items, function (field) {
                                            field.setValue(null);
                                        });
                                        B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                                    },
                                    failure: function (resp, res) {
                                        var msg;

                                        if (res && res.result && res.result.message.length == 0) {
                                            msg = 'Произошла ошибка при изменении значения параметра.';
                                        } else {
                                            msg = res.result.message;
                                        }

                                        B4.QuickMsg.msg('Изменение параметра', msg, 'error');
                                    }
                                });
                            }
                        }, me);
                    } else {
                        frm.submit({
                            url: B4.Url.action('ChangeParameter', 'Parameters'),
                            params: params,
                            success: function () {
                                that.editWindow.close();
                                that.onValueSaved.apply(that, [params.value, dateField.value]);
                                that.fireEvent('valuesaved', that, params.value);
                                Ext.each(fields.items, function (field) {
                                    field.setValue(null);
                                });
                                B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                            },
                            failure: function (resp, res) {
                                var msg;

                                if (res && res.result && res.result.message.length == 0) {
                                    msg = 'Произошла ошибка при изменении значения параметра.';
                                } else {
                                    msg = res.result.message;
                                }

                                B4.QuickMsg.msg('Изменение параметра', msg, 'error');
                            }
                        });
                    }

                }).error(function () {
                    frm.submit({
                        url: B4.Url.action('ChangeParameter', 'Parameters'),
                        params: params,
                        success: function () {
                            that.editWindow.close();
                            that.onValueSaved.apply(that, [params.value, dateField.value]);
                            that.fireEvent('valuesaved', that, params.value);
                            Ext.each(fields.items, function (field) {
                                field.setValue(null);
                            });
                            B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                            that.unmask();
                        },
                        failure: function (resp, res) {
                            var msg;

                            if (res && res.result && res.result.message.length == 0) {
                                msg = 'Произошла ошибка при изменении значения параметра.';
                            } else {
                                msg = res.result.message;
                            }

                            B4.QuickMsg.msg('Изменение параметра', msg, 'error');
                            that.unmask();
                        }
                    });
                });
            },
            onValueSaved: function (val, date) {
                var enumfield = this.up('container').down('b4enumcombo[name=ConditionHouse]'),
                    dateDemolitionField = this.up('realityobjEditPanel').down('datefield[name=DateDemolition]'),
                    today = new Date(),
                    myToday = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0);

                if (date <= myToday) {
                    enumfield.setValue(val);
                }
                if (enumfield.value === B4.enums.ConditionHouse.Razed) {
                    dateDemolitionField.setDisabled(false);
                }
                else {
                    dateDemolitionField.setDisabled(true);
                }
            },
            margins: '0 0 5 0',
            withHistory: true,
            parameter: 'real_obj_condition_house'
        }
    ]
});