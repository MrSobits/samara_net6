Ext.define('B4.aspects.GkhGridEditForm', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.gkhgrideditformaspect',

    controllerEditName: null,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.editFormSelector + ' b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };

        controller.control(actions);

        this.on('aftersetformdata', this.onAfterSetFormData, this);
        this.on('savesuccess', this.onSaveSuccess, this);
    },

    deleteWithRelatedEntities: false,

    deleteRecord: function (record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                if (me.deleteWithRelatedEntities) {
                    Ext.Msg.confirm('Удаление записи!', 'При удалении данной записи произойдет удаление всех связанных объектов. Продолжить удаление?',
                        function (resultWithRelatedEntities) {
                            if (resultWithRelatedEntities == 'yes') {
                                var modelWithRelEnt = me.getModel(record);
                                var recWithRelEnt = new modelWithRelEnt({ Id: record.getId() });
                                me.mask('Удаление', B4.getBody());
                                recWithRelEnt.destroy()
                                    .next(function () {
                                        me.fireEvent('deletesuccess', me);
                                        me.updateGrid();
                                        me.unmask();
                                    }, me)
                                    .error(function (resultWithRelatedEntities) {
                                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(resultWithRelatedEntities.responseData) ? resultWithRelatedEntities.responseData : resultWithRelatedEntities.responseData.message);
                                        me.unmask();
                                    }, me);
                            }
                        });
                } else {
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
            }
        }, me);
    },

    /*closeWindowHandler: function () {
        this.getForm().close();
    },*/

    editRecord: function (record) {
        var me = this,
            id = record ? record.data.Id : null,
            model;


        model = this.controller.getModel(me.modelName);

        if (id) {
            if (me.controllerEditName) {
                var portal = me.controller.getController('PortalController');

                if (!me.controller.hideMask) {
                    me.controller.hideMask = function () { me.controller.unmask(); };
                }

                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                me.controller.mask('Загрузка', me.componentQuery(me.gridSelector));
                portal.loadController(me.controllerEditName, record, portal.containerSelector, me.controller.hideMask);

            }
            else {
                model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                });
            }
        }
        else {
            me.setFormData(new model({ Id: 0 }));
        }
    },

    /*getForm: function () {
        var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

        if (editWindow && editWindow.isHidden() && editWindow.rendered) {
            editWindow = editWindow.destroy();
        }

        if (!editWindow) {
            editWindow = this.controller.getView(this.editWindowView).create({ constrain: true, autoDestroy: true });
            if (B4.getBody().getActiveTab()) {
                console.log(B4.getBody().getActiveTab());
            }
            if (B4.getBody().getActiveTab()) {
                B4.getBody().getActiveTab().add(editWindow);
            } else {
                B4.getBody().add(editWindow);
            }
        }

        return editWindow;
    },*/

    onAfterSetFormData: function (aspect, rec, form) {
        form.show();
    },

    onSaveSuccess: function (aspect, rec) {
        //Закрываем окно после добавления новой записи
        this.callParent(arguments);
        //И открываем только что созданную запись в боковой панели
        this.editRecord(rec);
    }
});