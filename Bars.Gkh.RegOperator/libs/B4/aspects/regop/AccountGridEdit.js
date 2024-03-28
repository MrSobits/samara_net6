// Этот "псевдо"-аспект используется в 2-х местах: в контроллере реестра абонентов и в контроллере детализации ЛС
// Этот аспект я считаю "псевдо", так как тут уже прописаны конкретные гриды и формы
// Так как этот "псевдо"-аспект используется в 2-х местах, то могут быть случаи, когда подписка на события происходила дважды (соответственно, например, сохрание создавало 2 объекта вместо 1)
// Во избежание этого метод init переписан таким образом, что в нем фильтруются дублирующие подписки

Ext.define('B4.aspects.regop.AccountGridEdit', {
    extend: 'B4.aspects.GridEditCtxWindow',

    alias: 'widget.accounteditgrid',

    requires: ['B4.view.regop.owner.PersonalAccountAddWindow'],

    gridSelector: 'paowneraccountgrid',
    editFormSelector: 'paowneraccountaddwin',
    modelName: 'regop.personal_account.BasePersonalAccount',
    storeName: 'regop.personal_account.BasePersonalAccount',
    editWindowView: 'regop.owner.PersonalAccountAddWindow',

    ownerIdFieldSelector: 'paownereditwin hiddenfield[name=Id]',
    personalAccountWindowSelector: 'paowneraccountaddwin',
    historyGridSelector: 'paowneraccountaddwin paowneraccounthistorygrid',
    closeDateContainerSelector: 'paowneraccountaddwin container[name=CloseDateContainer]',

    realityObjectFieldSelector: 'paowneraccountaddwin [name="RealityObject"]',
    roomTextFieldSelector: 'paowneraccountaddwin textfield[name="RoomInfo"]',
    roomButtonSelector: 'paowneraccountaddwin b4addbutton',
    crTariffFieldSelector: 'paowneraccountaddwin numberfield[name="Tariff"]',

    init: function(controller) {
        this.controller = controller;
        var actions = {};

        actions[this.gridSelector] = {
            'rowaction': {
                fn: this.rowAction,
                scope: this
            },
            'itemdblclick': {
                fn: this.rowDblClick,
                scope: this
            },
            'gridaction': {
                fn: this.gridAction,
                scope: this
            }
        };

        actions[this.gridSelector + ' b4addbutton'] = {
            'click': {
                fn: this.btnAction,
                scope: this
            }
        };

        actions[this.gridSelector + ' b4updatebutton'] = {
            'click': {
                fn: this.btnAction,
                scope: this
            }
        };

        if (this.editFormSelector) {
            actions[this.editFormSelector + ' b4savebutton'] = {
                'click': {
                    fn: this.saveRequestHandler,
                    scope: this
                }
            };
        }

        if (this.editFormSelector) {
            actions[this.editFormSelector + ' b4closebutton'] = {
                'click': {
                    fn: this.closeWindowHandler,
                    scope: this
                }
            };
        }

        this.otherActions(actions);

        actions = this.filterExistingListeners(controller, actions);

        controller.control(actions);
    },

    // Метод фильтрации существующих подписок
    filterExistingListeners: function(controller, actions) {
        var bus,
            result = {},
            arrayToExclude = [];

        if (controller
            && controller.application
            && controller.application.eventbus
            && controller.application.eventbus.bus) {

            bus = controller.application.eventbus.bus;

            Ext.each(Ext.Object.getKeys(actions), function(action) {
                Ext.each(Ext.Object.getKeys(actions[action]), function(event) {
                    if (!bus.hasOwnProperty(event)) {
                        return false;
                    }

                    var ev = bus[event][action];

                    if (ev == null) {
                        return false;
                    }

                    Ext.each(Ext.Object.getKeys(ev), function(controllerName) {
                        Ext.each(ev[controllerName], function(obj) {
                            Ext.each(obj.listeners, function(listener) {
                                if (listener && listener.fn === actions[action][event].fn) {
                                    if (!Ext.Array.contains(arrayToExclude, action)) {
                                        arrayToExclude.push(action);
                                    }
                                }
                            });
                        });
                    });
                });
            });
        }

        Ext.each(Ext.Object.getKeys(actions), function(action) {
            if (!Ext.Array.contains(arrayToExclude, action)) {
                result[action] = actions[action];
            }
        });

        return result;
    },

    listeners: {
        beforesetformdata: function(asp, record) {
            var owner = this.componentQuery(this.ownerIdFieldSelector).getValue();
            record.set('AccountOwner', owner);

            asp.getForm().setAddLayout(!record.get('Id'));
        },
        deletesuccess: function(asp, record) {
            var grid = asp.controller.getPersonalAccountGrid();
            grid.getStore().load();
        },
        aftersetformdata: function(asp, record) {
            var id = record.get('Id'),
                accWin,
                btns,
                openDateField,
                areaShareField;

            if (+id) {
                accWin = this.componentQuery(this.personalAccountWindowSelector);
                btns = Ext.ComponentQuery.query('changevalbtn', accWin);

                Ext.Array.each(btns, function(el) {
                    el.setEntityId(id);
                });

                var historyGrid = this.componentQuery(this.historyGridSelector);

                historyGrid.entityId = id;
                historyGrid.getStore().load();

                openDateField = accWin.down('[name=OpenDate]');
                openDateField.setReadOnly(+id != 0);
                openDateField.setHideTrigger(+id != 0);

                areaShareField = accWin.down('[name=AreaShare]');
                areaShareField.setReadOnly(+id != 0);

                Ext.Array.each(btns, function(el) {
                    el.setDisabled(+id == 0);
                });
                var state = record.get('State');
                this.componentQuery(this.closeDateContainerSelector).setVisible(state && Ext.isObject(state) && state.FinalState);
                B4.Ajax.request({
                    url: B4.Url.action('GetOpenPeriod', 'ChargePeriod'),
                    timeout: 9999999
                }).next(function (response) {
                    if (response == null) {
                        return;
                    }
                    var res = Ext.decode(response.responseText);

                    if (!res.data) {
                        return;
                    }

                    if (!accWin.params) {
                        accWin.params = {};
                    }
                    accWin.params['periodId'] = res.data.Id;
                });

                areaShareField.setDisabled(state.Code != "1");
                btns.forEach(function(btn) {
                    if (btn.propertyName == "AreaShare") {
                        btn.setDisabled(state.Code != "1")
                    }
                });

                if (accWin.params) {
                    accWin.params['realityObjectRoomId'] = record.get('Room').Id;
                    accWin.params['realityObjectId'] = record.get('RealityObject').Id;
                    accWin.params['personalAccountId'] = record.get('Id');
                } else {
                    accWin.params = {
                        realityObjectRoomId: record.get('Room').Id,
                        realityObjectId: record.get('RealityObject').Id,
                        personalAccountId: record.get('Id')
                    };
                }
            }
        }
    },
    otherActions: function(actions) {
        var me = this;
        actions[me.editFormSelector + ' changevalbtn[propertyName="AreaShare"]'] = { beforevaluesave: { fn: me.onValidateNewAreaShare, scope: me } };
        actions[me.realityObjectFieldSelector] = { change: { fn: me.onRealityObjectChange, scope: me } };
    },

    onRealityObjectChange: function (field, newVal, oldVal) {
        var me = this,
            roomTextField = me.componentQuery(me.roomTextFieldSelector),
            rommButton = me.componentQuery(me.roomButtonSelector),
            crTariffField = me.componentQuery(me.crTariffFieldSelector);

        if (oldVal && newVal != oldVal) {
            if (roomTextField) { roomTextField.reset(); }
            if (crTariffField) { crTariffField.reset(); }
        }

        if (roomTextField) {
            roomTextField.setDisabled(!newVal);
        }
        if (rommButton) {
            rommButton.setDisabled(!newVal);
        }
    },

    onSaveSuccess: function(asp) {
        var grid = asp.controller.getPersonalAccountGrid();
        grid.getStore().load();

        this.callParent(arguments);
    },

    onValidateNewAreaShare: function(button, params) {
        var frm = button.editWindow.down('form');

        button.editWindow.body.mask('Сохранение', frm);

        Ext.apply(params, {
            className: button.className,
            propertyName: button.propertyName,
            entityId: button.entityId
        });

        frm.submit({
            url: B4.Url.action('UpdateAreaShare', 'BasePersonalAccount'),
            params: params,
            success: function() {
                button.editWindow.close();
                button.onValueSaved.apply(button, [params.value]);
                button.fireEvent('valuesaved', button, params.value);
                B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                button.editWindow.body.unmask();
            },
            failure: function() {
                var message = 'Произошла ошибка при изменении значения параметра.',
                    typeMessage = 'error';

                if (this.result && this.result.message) {
                    message = this.result.message;
                    typeMessage = 'warning';
                }
                button.editWindow.body.unmask();
                B4.QuickMsg.msg('Изменение параметра', message, typeMessage);
            }
        });

        // Сохранение изменений берем на себя
        return false;
    },

    saveRecordHasNotUpload: function(rec) {
        var me = this;
        if (!+rec.get('Id')) {
            me.createAccounts(rec);
            return;
        }

        var frm = me.getForm();
        me.mask('Сохранение', frm);
        rec.save({ id: rec.getId() })
            .next(function(result) {
                me.unmask();
                me.updateGrid();
                me.fireEvent('savesuccess', me, result.record);
            }, this)
            .error(function(result) {
                me.unmask();
                me.fireEvent('savefailure', result.record, result.responseData);

                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },

    deleteRecord: function (record) {
        var me = this;
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                var model = this.getModel(record);
                
                var rec = new model({ Id: record.getId() });

                me.mask('Удаление', me.getGrid());
                rec.destroy()
                    .next(function () {
                        this.fireEvent('deletesuccess', this);
                        me.updateGrid();
                        me.unmask();
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, me);
    },

    createAccounts: function(rec) {
        var me = this,
            frm = me.getForm();

        me.mask('Сохранение', frm);
        B4.Ajax.request({
                url: B4.Url.action('CreateAccounts', 'BasePersonalAccount'),
                method: 'POST',
                timeout: 999999,
                params: {
                    AccountOwner: rec.get('AccountOwner'),
                    OpenDate: rec.get('OpenDate'),
                    Tariff: rec.get('Tariff'),
                    Rooms: rec.get('Rooms'),
                    RealityObject: rec.get('RealityObject')
                }
            })
            .next(function() {
                me.unmask();
                me.updateGrid();
                me.fireEvent('savesuccess', me);
            })
            .error(function(result) {
                me.unmask();
                me.fireEvent('savefailure', null, result);
                Ext.Msg.alert('Ошибка сохранения!', result.message);
            });
    }
});