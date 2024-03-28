/*
* Договоры подряда
*/
Ext.define('B4.controller.workscr.BuildContract', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.typeworkcr.BuildContract',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'ObjectCr',
        'objectcr.BuildContract',
        'objectcr.BuildContractTypeWork'
    ],

    stores: [
        'objectcr.BuildContract',
        'objectcr.BuildContractTypeWork',
        'objectcr.TypeWorkCrForSelect',
        'objectcr.TypeWorkCrForSelected'
    ],

    views: [
        'workscr.BuildContractEditWindow',
        'objectcr.BuildContractGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'objectcr.BuildContractGrid',
    mainViewSelector: 'buildContractGrid',

    parentCtrlCls: 'B4.controller.workscr.Navi',

    aspects: [
        {
            xtype: 'buildcontracttypeworkcrperm',
            name: 'buildContractPermissionAspect',
            editFormAspectName: 'workBuildContractGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'buildcontractcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.BuildContractViewCreate.Create', applyTo: 'b4addbutton', selector: 'buildContractGrid' },
                {
                    name: 'GkhCr.TypeWorkCr.Register.BuildContractViewCreate.Column.Sum',
                    applyTo: '[dataIndex=Sum]',
                    selector: 'buildContractGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'buildcontractdeletepermissionaspect',
            permissions: [{ name: 'GkhCr.TypeWorkCr.Register.BuildContract.Delete' }]
        },
        {
            /**
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'buildContractStateTransferAspect',
            gridSelector: 'buildContractGrid',
            stateType: 'cr_obj_build_contract',
            menuSelector: 'buildContractGridStateMenu'
        },
        {
            /**
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'buildContractStateButtonAspect',
            stateButtonSelector: 'workscrbuildcontractwin #btnState',
            listeners: {
                transfersuccess: function(asp, entityId, newState) {
                    var editWindowAspect = asp.controller.getAspect('workBuildContractGridWindowAspect'),
                        model;

                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();

                    model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function(rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования договоров подряда
            */
            xtype: 'grideditwindowaspect',
            name: 'workBuildContractGridWindowAspect',
            gridSelector: 'buildContractGrid',
            editFormSelector: 'workscrbuildcontractwin',
            storeName: 'objectcr.BuildContract',
            modelName: 'objectcr.BuildContract',
            editWindowView: 'workscr.BuildContractEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getObjectId());
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                    }
                },
                aftersetformdata: function(asp, rec) {
                    asp.controller.getAspect('buildContractStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            },
            deleteRecord: function(record) {
                var me = this;
                if (record.getId()) {
                    me.controller.getAspect('buildcontractdeletepermissionaspect').loadPermissions(record)
                        .next(function(response) {
                            var grants = Ext.decode(response.responseText),
                                model, rec;

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        model = me.getModel(record);

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
            }
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView(),
            model = me.getModel('ObjectCr');

        if (!view) {
            view = Ext.widget('buildContractGrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
        me.getAspect('buildcontractcreatepermissionaspect').setPermissionsByRecord(new model({ Id: objectId }));
    },

    getObjectId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    },
    
    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    }
});