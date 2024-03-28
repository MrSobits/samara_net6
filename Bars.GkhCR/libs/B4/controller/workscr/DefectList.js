/*
* Дефектные ведомости
*/
Ext.define('B4.controller.workscr.DefectList', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.typeworkcr.DefectList'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['objectcr.DefectList'],
    
    stores: ['objectcr.DefectList'],
    
    views: [
        'workscr.DefectListGrid',
        'workscr.DefectListEditWindow'
    ],

    mainView: 'objectcr.DefectListGrid',
    mainViewSelector: 'workscrdefectlistgrid',

    aspects: [
        {
            xtype: 'defectlisttypeworkcrperm',
            name: 'defectListPermissionAspect',
            editFormAspectName: 'editWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'defectlistcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.DefectListViewCreate.Create', applyTo: 'b4addbutton', selector: 'workscrdefectlistgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'defectlistdeletepermissionaspect',
            permissions: [{ name: 'GkhCr.TypeWorkCr.Register.DefectList.Delete' }]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'editWindowAspect',
            gridSelector: 'workscrdefectlistgrid',
            editFormSelector: 'workscrdefectlistwin',
            storeName: 'objectcr.DefectList',
            modelName: 'objectcr.DefectList',
            editWindowView: 'workscr.DefectListEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                        record.set('ObjectCr', asp.controller.getObjectId());
                    }
                }
            },
            deleteRecord: function(record) {
                var me = this;
                if (record.getId()) {
                    me.controller.getAspect('defectlistdeletepermissionaspect').loadPermissions(record)
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

    index: function (id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('workscrdefectlistgrid');
            
            view.getStore().on('beforeload', function (arg0, operation) {
                operation.params.twId = id;
                operation.params.objectCrId = objectId;
            });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
        me.getAspect('defectlistcreatepermissionaspect').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    getTypeWorkId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});