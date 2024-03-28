Ext.define('B4.controller.controlorg.ControlOrganization', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.ControlOrganization',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'controlorg.ControlOrganization',
        'controlorg.ControlOrganizationControlTypeRelation',
        'controlorg.TatarstanZonalInspection'
    ],
    stores: [
        'controlorg.ControlOrganization',
        'controlorg.ControlOrganizationControlTypeRelation',
        'controlorg.TatarstanZonalInspection'
    ],

    views: [
        'controlorg.Grid',
        'controlorg.AddWindow',
        'controlorg.editwindow.EditWindow',
        'controlorg.editwindow.ControlTypeRelationGrid',
        'controlorg.editwindow.TatarstanZonalInspectionGrid'
    ],

    mainView: 'controlorg.Grid',
    mainViewSelector: 'controlorganizationgrid',

    aspects: [
        {
            xtype: 'controlorgperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'controlorgorgGridWindowAspect',
            gridSelector: 'controlorganizationgrid',
            storeName: 'controlorg.ControlOrganization',
            modelName: 'controlorg.ControlOrganization',
            editWindowView: 'controlorg.AddWindow',
            editFormSelector: 'controlorgAddWindow',

            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model = me.controller.getModel(me.modelName);

                if (id) {
                    me.controller.setContextValue(me.controller.getMainView(), 'controlOrgId', id);

                    me.controller.getStore('controlorg.ControlOrganizationControlTypeRelation').load();
                    me.controller.getStore('controlorg.TatarstanZonalInspection').load();

                    var window = Ext.widget('controlorgeditwindow');

                    Ext.Object.each(record.data, function (property, value) {
                        var component = window.down('[name=' + property + ']');
                        if (component) {
                            component.setValue(value);
                        }
                    });

                    window.show();
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        Ext.Msg.confirm('Удаление записи!', 'При удалении данной записи произойдет удаление всех связанных объектов. Продолжить удаление?', function (result) {
                            if (result == 'yes') {
                                var model = this.getModel(record);

                                var rec = new model({ Id: record.getId() });
                                me.mask('Удаление', B4.getBody());
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
                    }
                }, me);
            },

            listeners: {
                beforesave: function(asp, rec) {
                    var store = asp.controller.getMainView().store,
                        isCorrect = true;
                    store.each(function(record) {
                        if (record.get('Contragent').Id === rec.get('Contragent')) {
                            Ext.Msg.alert('Ошибка', 'Контрагент ранее был добавлен в качестве КНО.');
                            isCorrect = false;
                            return false;
                        }
                    });
                    return isCorrect;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'сontrolOrganizationControlTypeRelationGridAspect',
            storeName: 'controlorg.ControlOrganizationControlTypeRelation',
            modelName: 'controlorg.ControlOrganizationControlTypeRelation',
            gridSelector: 'controltyperelationgrid',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('ControlOrganization', asp.controller.getContextValue(asp.controller.getMainView(), 'controlOrgId'));
                },

                beforesave: function (asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        isValid = Ext.Array.each(modifRecords, function (rec) {
                            var sameRecords = 0;
                            store.each(function(record) {
                                if (record.get('ControlType').Id === rec.get('ControlType').Id) {
                                    sameRecords++;
                                }
                            });
                            if (rec.get('ControlType') == null || sameRecords > 1) {
                                var message = rec.get('ControlType') == null
                                    ? 'Поле "Наименование вида контроля" является обязательным для заполнения'
                                    : 'Невозможно сохранить несколько идентичных видов контроля';
                                Ext.Msg.alert('Ошибка', message);
                                return false;
                            }
                        });

                    if (!isValid) {
                        return false;
                    }

                    Ext.Array.each(modifRecords, function (rec) {
                        rec.set('ControlType', rec.get('ControlType').Id);
                    });
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tatarstanZonalInspectionGridAspect',
            storeName: 'controlorg.TatarstanZonalInspection',
            modelName: 'controlorg.TatarstanZonalInspection',
            gridSelector: 'tatarstanzonalinspectiongrid',
            listeners: {
                beforesave: function (asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        isValid = Ext.Array.each(modifRecords, function (rec) {
                            if (rec.get('Name') == null) {
                                Ext.Msg.alert('Ошибка', 'Поле "Наименование зональной инспекции" является обязательным для заполнения');
                                return false;
                            }
                        });

                    return isValid;
                }
            },

            save: function () {
                var me = this,
                    store = me.getStore(),
                    modifiedRecs = store.getModifiedRecords(),
                    removedRecs = store.getRemovedRecords();
                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (this.fireEvent('beforesave', this, store) !== false) {
                        me.mask('Сохранение', this.getGrid());
                        var modRecordsArray = [],
                            removedRecordsArray = [];

                        Ext.each(modifiedRecs, function(rec) {
                            modRecordsArray.push(rec.get('ZoneName').Id)
                        });

                        Ext.each(removedRecs, function (rec) {
                            removedRecordsArray.push(rec.get('Id'))
                        });

                        B4.Ajax.request({
                            url: B4.Url.action('SaveControlOrgReference', 'TatarstanZonalInspection'),
                            params: {
                                controlOrganizationId: me.controller.getContextValue(me.controller.getMainView(), 'controlOrgId'),
                                modifiedRecords: modRecordsArray,
                                removedRecords: removedRecordsArray
                            },
                            timeout: 9999999
                        }).next(function (response) {
                            me.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function () {
                            me.controller.handleDataSyncError();
                        });
                    }
                }
            },
        },
    ],

    init: function () {
        var me = this;
        me.getStore('controlorg.ControlOrganizationControlTypeRelation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('controlorg.TatarstanZonalInspection').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.isKnoWindow = true;
        operation.params.controlOrgId = me.getContextValue(me.getMainView(), 'controlOrgId');
    }
});