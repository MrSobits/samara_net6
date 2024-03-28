Ext.define('B4.aspects.WarningInspectionAppCitEditWindow', {
    extend: 'B4.aspects.GkhGridEditForm',

    alias: 'widget.warninginspectionappciteditwindowaspect',

    gridSelector: null,
    storeName: 'appealcits.WarningInspection',
    modelName: 'appealcits.WarningInspection',
    editFormSelector: '#warningInspectionAppCitsAddWindow',
    editWindowView: 'appealcits.WarningInspectionAddWindow',
    controllerEditName: 'B4.controller.warninginspection.Navigation',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function(controller) {
        var me = this,
            actions = {};
        me.callParent(arguments);

        actions[me.editFormSelector + ' [name=TypeJurPerson]'] = { 'change': { fn: me.onChangeType, scope: me } };
        actions[me.editFormSelector + ' [name=PersonInspection]'] = { 'change': { fn: me.onChangePerson, scope: me } };
        actions[me.editFormSelector + ' [name=Contragent]'] =
            { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
        actions[me.editFormSelector + ' [name=Address]'] = {
            'beforeload': { fn: me.onBeforeLoadRealityObject, scope: me },
            'change': { fn: me.onChangeRealityObject, scope: me }
        };
        controller.control(actions);
    },

    saveRecord: function(rec) {
        var me = this;
        if (this.fireEvent('beforesave', this, rec) !== false) {
            var frm = me.getForm(),
                roSelector = me.editFormSelector + ' b4selectfield[name=Address]',
                contragentSelector = me.editFormSelector + ' b4selectfield[name=Contragent]';
            me.mask('Сохранение', frm);

            // Проверяем наличие тематик
            me.checkAppealCits(me.controller.appealCitizensId)
                .next(function() {
                    var realtyObjId =
                        Ext.ComponentQuery.query(roSelector)[0]
                            .getValue();
                    var contragentId =
                        Ext.ComponentQuery.query(contragentSelector)[0]
                            .getValue();

                    B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('CreateWithAppealCits', 'WarningInspection'),
                            params: {
                                realtyObjId: realtyObjId,
                                contragentId: contragentId,
                                baseStatement: Ext.encode(rec.data),
                                appealCits: me.controller.appealCitizensId
                            }
                        })
                        .next(function(result) {
                            me.unmask();
                            me.updateGrid();
                            var res = Ext.decode(result.responseText);
                            var baseStatementId = res.data.Id;

                            var model = me.controller.getModel('appealcits.WarningInspection');
                            model.load(baseStatementId, {
                                success: function(recBaseStatement) {
                                    me.fireEvent('savesuccess', me, recBaseStatement);
                                },
                                scope: me
                            });
                            return true;
                        })
                        .error(function(result) {
                            me.unmask();
                            me.fireEvent('savefailure', result.record, result.responseData);

                            Ext.Msg.alert('Ошибка сохранения!',
                                Ext.isString(result.responseData) ? result.responseData : result.message);
                        });
                })
                .error(function(resp) {
                    me.unmask();
                    B4.QuickMsg.msg("Ошибка", resp.message, "error");
                });
        }
    },
    editRecord: function(record) {
        var me = this,
            id = record ? record.getId() : null,
            model = me.controller.getModel('appealcits.WarningInspection');

        if (id) {
            if (me.controllerEditName) {
                var portal = me.controller.getController('PortalController');

                if (!me.controller.hideMask) {
                    me.controller.hideMask = function() {
                        me.controller.unmask();
                    };
                }

                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                me.controller.mask('Загрузка', me.controller.getMainComponent());
                portal.loadController(me.controllerEditName, record, portal.containerSelector,
                    me.controller.hideMask);
            } else {
                model.load(id, {
                    success: function(rec) {
                        me.setFormData(rec);
                    }
                });
            }
        } else {
            me.setFormData(new model({ Id: 0 }));
        }
    },
    onBeforeLoadContragent: function(store, operation) {
        operation = operation || {};
        operation.params = operation.params || {};

        operation.params.typeJurOrg = this.controller.params.typeJurOrg;
        operation.params.roId = this.getForm().down('[name=Address]').getValue();
    },
    onBeforeLoadRealityObject: function(store, operation) {
        operation = operation || {};
        operation.params = operation.params || {};
        var realityObjIds = [];
        this.controller.getStore('appealcits.RealityObject')
            .each(function(obj) {
                realityObjIds.push(obj.get('RealityObjectId'));
            });
        operation.params.realityObjIds = realityObjIds.length == 0 ? -1 : realityObjIds;
    },
    onChangeType: function (field, newValue) {
        var me = this;
        me.controller.params = me.controller.params || {};
        me.controller.params.typeJurOrg = newValue;
        me.getForm().down('[name=Contragent]').setValue(null);
        if (newValue == B4.enums.TypeJurPerson.ManagingOrganization) {
            me.setManOrg();
        }
        me.getForm().down('[name=PhysicalPerson]').setValue(null);
    },
    onChangeRealityObject: function(filed, newValue) {
        var me = this,
            cbTypeJurPerson = me.getForm().down('[name=TypeJurPerson]'),
            sfContragent = me.getForm().down('[name=Contragent]');

        if (newValue &&
            sfContragent &&
            cbTypeJurPerson &&
            cbTypeJurPerson.getValue() == B4.enums.TypeJurPerson.ManagingOrganization) {
            sfContragent.setValue(null);
            me.setManOrg(newValue.Id);
        }
    },
    onChangePerson: function(field, newValue) {
        var form = this.getForm(),
            sfContragent = form.down('[name=Contragent]'),
            tfPhysicalPerson = form.down('[name=PhysicalPerson]'),
            cbTypeJurPerson = form.down('[name=TypeJurPerson]');
        sfContragent.setValue(null);
        tfPhysicalPerson.setValue(null);
        cbTypeJurPerson.setValue(10);

        switch (newValue) {
            case B4.enums.TypeJurPerson.ManagingOrganization:
                //физлицо
                sfContragent.setDisabled(true);
                tfPhysicalPerson.setDisabled(false);
                cbTypeJurPerson.setDisabled(true);
                break;
            case B4.enums.TypeJurPerson.SupplyResourceOrg:
                //организацияы
                sfContragent.setDisabled(false);
                tfPhysicalPerson.setDisabled(true);
                cbTypeJurPerson.setDisabled(false);
                break;
            case B4.enums.TypeJurPerson.LocalGovernment:
                //должностное лицо
                sfContragent.setDisabled(false);
                tfPhysicalPerson.setDisabled(false);
                cbTypeJurPerson.setDisabled(false);
                break;
        }
    },

    checkAppealCits: function (appealCitizensId) {
        var me = this;

        return B4.Ajax.request({
            url: B4.Url.action('CheckAppealCits', 'WarningInspection'),
            params: {
                appealCitizensId: appealCitizensId
            }
        });
    },

    setManOrg: function (roId) {
        var me = this,
            appealCitizensId = me.controller.appealCitizensId;

        B4.Ajax.request({
            url: B4.Url.action('GetJurOrgs', 'AppealCitsRealObject'),
            params: {
                appealCitizensId: appealCitizensId,
                realityObjectId: roId
            }
        }).next(function (resp) {

            var res = Ext.JSON.decode(resp.responseText);

            if (res != null && res.length > 0) {
                var sfContragent = Ext.ComponentQuery.query(me.editFormSelector + ' b4selectfield[name=Contragent]')[0];
                if (sfContragent) {
                    sfContragent.setValue(res[0].Id);
                    sfContragent.setRawValue(res[0].Name);
                    sfContragent.validate();
                }
            }
        });
    }
});