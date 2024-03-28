/*
* Договоры кап. ремонта
*/
Ext.define('B4.controller.workscr.Contract', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.typeworkcr.ContractCr',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['ObjectCr', 'objectcr.ContractCr'],

    stores: ['objectcr.ContractCr'],

    views: [
        'objectcr.ContractCrGrid',
        'objectcr.ContractCrEditWindow'
    ],

    mainView: 'objectcr.ContractCrGrid',
    mainViewSelector: 'objectcrcontractgrid',

    parentCtrlCls: 'B4.controller.workscr.Navi',

    aspects: [
        {
            xtype: 'contracttypeworkcrperm',
            name: 'contracteditPermissionAspect',
            editFormAspectName: 'editWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'contractcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.ContractCrViewCreate.Create', applyTo: 'b4addbutton', selector: 'objectcrcontractgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'contractdeletepermissionaspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Delete' }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'editWindowAspect',
            gridSelector: 'objectcrcontractgrid',
            editFormSelector: 'objectcrcontractwin',
            storeName: 'objectcr.ContractCr',
            modelName: 'objectcr.ContractCr',
            editWindowView: 'objectcr.ContractCrEditWindow',
            otherActions: function(actions) {
                var me = this;
                actions[me.editFormSelector + ' #sflContragent'] = {
                    'beforeload': function(arg0, operation) {
                        operation.params = operation.params || {};
                        operation.params.showAll = true;
                    }
                };
            },
            deleteRecord: function(record) {
                var me = this;
                if (record.getId()) {
                    me.controller.getAspect('contractdeletepermissionaspect').loadPermissions(record)
                        .next(function(response) {
                            var grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    var model, rec;
                                    if (result == 'yes') {
                                        model = me.getModel(record),
                                        rec = new model({ Id: record.getId() });

                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            })
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            });
                                    }
                                });
                            }
                        });
                }
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                        record.set('ObjectCr', asp.controller.getObjectId());
                    }
                },
                aftersetformdata: function(asp, rec) {
                    this.controller.getAspect('contractStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        },
        {
            /**
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'contractStateButtonAspect',
            stateButtonSelector: 'objectcrcontractwin #btnState',
            listeners: {
                transfersuccess: function(me, entityId, newState) {
                    var aspect = me.controller.getAspect('editWindowAspect'),
                        model = me.controller.getModel(aspect.modelName);

                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    aspect.updateGrid();

                    model.load(entityId, {
                        success: function(rec) {
                            aspect.setFormData(rec);
                        }
                    });
                }
            }
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView(),
            model = me.getModel('ObjectCr');

        if (!view) {
            view = Ext.widget('objectcrcontractgrid');
            view.getStore().on('beforeload', function(arg0, operation) {
                operation.params.twId = id;
                operation.params.objectCrId = objectId;
            });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();

        me.getAspect('contractcreatepermissionaspect').setPermissionsByRecord(new model({ Id: id }));
    },

    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});