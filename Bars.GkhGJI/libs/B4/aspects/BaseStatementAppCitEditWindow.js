Ext.define('B4.aspects.BaseStatementAppCitEditWindow', {
    extend: 'B4.aspects.GkhGridEditForm',

    alias: 'widget.basestatementappciteditwindowaspect',

    gridSelector: '#baseStatementAppCitsGrid',
    storeName: 'appealcits.BaseStatement',
    modelName: 'appealcits.BaseStatement',
    editFormSelector: '#baseStatementAppCitsAddWindow',
    editWindowView: 'appealcits.BaseStatementAddWindow',
    controllerEditName: 'B4.controller.basestatement.Navigation',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function(controller) {
        var me = this,
            actions = {};
        me.callParent(arguments);

        actions[me.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: me.onChangeType, scope: me } };
        actions[me.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: me.onChangePerson, scope: me } };
        actions[me.editFormSelector + ' #sfContragent'] =
            { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
        actions[me.editFormSelector + ' #sfRealityObject'] = {
            'beforeload': { fn: me.onBeforeLoadRealityObject, scope: me },
            'change': { fn: me.onChangeRealityObject, scope: me }
        };

        controller.control(actions);
    },

    saveRecord: function(rec) {
        var me = this;
        if (this.fireEvent('beforesave', this, rec) !== false) {
            var frm = me.getForm();
            me.mask('Сохранение', frm);

            // Проверяем наличие тематик
            me.checkAppealCits(me.controller.appealCitizensId)
                .next(function() {
                    var realtyObjId =
                        Ext.ComponentQuery.query(me.controller.baseStatementRealityObjectSelector)[0]
                            .getValue();
                    var contragentId =
                        Ext.ComponentQuery.query(me.controller.baseStatementContragentSelector)[0]
                            .getValue();

                    var storeAppealCits = me.controller.getStore('appealcits.AppealCitsBaseStatement');

                    var appealCits = [];
                    Ext.Array.each(storeAppealCits.getRange(0, storeAppealCits.getCount()),
                        function(item) {
                            appealCits.push(item.get('Id'));
                        });

                    B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('CreateWithAppealCits', 'BaseStatement'),
                            params: {
                                realtyObjId: realtyObjId,
                                contragentId: contragentId,
                                baseStatement: Ext.encode(rec.data),
                                appealCits: Ext.encode(appealCits)
                            }
                        })
                        .next(function(result) {
                            me.unmask();
                            me.updateGrid();
                            var res = Ext.decode(result.responseText);
                            var baseStatementId = res.data.Id;

                            var model = me.controller.getModel('BaseStatement');
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
            model = this.controller.getModel('BaseStatement');

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
        operation.params.roId = this.getForm().down('#sfRealityObject').getValue();
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
    onChangeType: function(field, newValue) {
        this.controller.params = this.controller.params || {};
        this.controller.params.typeJurOrg = newValue;
        this.getForm().down('#sfContragent').setValue(null);
        if (newValue == B4.enums.TypeJurPerson.ManagingOrganization) {
            this.controller.setManOrg();
        }
        this.getForm().down('#tfPhysicalPerson').setValue(null);
    },
    onChangeRealityObject: function(filed, newValue) {
        var me = this,
            cbTypeJurPerson = me.getForm().down('#cbTypeJurPerson'),
            sfContragent = me.getForm().down('#sfContragent');

        if (newValue &&
            sfContragent &&
            cbTypeJurPerson &&
            cbTypeJurPerson.getValue() == B4.enums.TypeJurPerson.ManagingOrganization) {
            sfContragent.setValue(null);
            me.controller.setManOrg(newValue.Id);
        }
    },
    onChangePerson: function(field, newValue) {
        var form = this.getForm(),
            sfContragent = form.down('#sfContragent'),
            tfPhysicalPerson = form.down('#tfPhysicalPerson'),
            cbTypeJurPerson = form.down('#cbTypeJurPerson');
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
            url: B4.Url.action('CheckAppealCits', 'BaseStatement'),
            params: {
                appealCitizensId: appealCitizensId
            }
        });
    }
});