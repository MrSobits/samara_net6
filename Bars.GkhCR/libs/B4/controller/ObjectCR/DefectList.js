Ext.define('B4.controller.objectcr.DefectList', {
    /*
    * Контроллер раздела дефектных ведомостей
    */
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateContextButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.objectcr.DefectList',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['objectcr.DefectList'],
    stores: ['objectcr.DefectList'],
    views: [
        'objectcr.DefectListEditWindow',
        'objectcr.DefectListGrid'

    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'objectcrdefectlistgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'defectlistobjectcrperm',
            name: 'defectListPermissionAspect',
            editFormAspectName: 'defectListGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'defectlistcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.DefectListViewCreate.Create', applyTo: 'b4addbutton', selector: 'objectcrdefectlistgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'defectlistdeletepermissionaspect',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.DefectList.Delete' }]
        },
        {
            /**
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'defectListStateButtonAspect',
            stateButtonSelector: 'objectcrdefectlistwin #btnState',
            listeners: {
                transfersuccess: function(me, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);

                    var editWindowAspect = me.controller.getAspect('defectListGridWindowAspect');

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();

                    var model = me.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function(rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'defectListStateTransferAspect',
            gridSelector: 'objectcrdefectlistgrid',
            stateType: 'cr_obj_defect_list',
            menuSelector: 'defectListGridStateMenu'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования дефектных ведомостей
            */
            xtype: 'grideditctxwindowaspect',
            name: 'defectListGridWindowAspect',
            gridSelector: 'objectcrdefectlistgrid',
            editFormSelector: 'objectcrdefectlistwin',
            modelName: 'objectcr.DefectList',
            editWindowView: 'objectcr.DefectListEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.data.Id) {
                        record.set('ObjectCr', asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId'));
                    }
                },
                aftersetformdata: function (asp, record) {
                    var win = asp.getForm(),
                        grid = asp.getGrid(),
                        typeView = grid.typeView,
                        crSumField = win.down('[type=CrSum]'),
                        calcByField = win.down('[name=CalculateBy]'),
                        dpkrInfoFieldSet = win.down('[fieldsetType=DpkrInfo]'),
                        crInfoFieldSet = win.down('[fieldsetType=CrInfo]'),
                        volumeField = win.down('[name=Volume]'),
                        costPerUnitField = win.down('[name=CostPerUnitVolume]'),
                        calcButton = win.down('[action=Calculate]'),
                        typeDefectListField = win.down('[name=TypeDefectList]');

                    asp.controller.getAspect('defectListStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    win.down('#btnState').setDisabled(record.phantom);

                    if (typeView == 0) {
                        crSumField.hide();
                        calcByField.show();
                        dpkrInfoFieldSet.show();
                        crInfoFieldSet.show();
                    } else {
                        crSumField.show();
                        calcByField.hide();
                        dpkrInfoFieldSet.hide();
                        crInfoFieldSet.hide();
                    }

                    if (!record.get('TypeDefectList')) {
                        typeDefectListField.setValue('Не задано');
                    }

                    if (volumeField && costPerUnitField && calcButton) {
                        volumeField.setReadOnly(record.get('CalculateBy') !== 0);
                        costPerUnitField.setReadOnly(record.get('CalculateBy') !== 0);
                        calcButton.setDisabled(record.get('CalculateBy') !== 0);
                    } else {
                        volumeField.setReadOnly(false);
                        costPerUnitField.setReadOnly(false);
                        calcButton.setDisabled(false);
                    }

                    B4.Ajax.request(B4.Url.action('UseTypeDefectList', 'Defect', {})).next(
                        function (response) {
                            var obj = Ext.JSON.decode(response.responseText);
                            if (!obj.UseTypeDefectList) {
                                typeDefectListField.hide();
                }
                        }).error(function () { });
                }
            },
            saveRecordHasUpload: function (rec) {
                var me = this,
                    frm = me.getForm(),
                    typeView = me.getGrid().typeView,
                    sumTotal = rec.get('SumTotal');

                if (typeView == 0 && rec.get('Sum') !== sumTotal) {
                    rec.set('Sum', sumTotal);
                }

                me.mask('Сохранение', frm);
                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function (form, action) {
                        me.unmask();
                        me.updateGrid();
                        var model = me.getModel(rec);
                        if (action.result.data.length > 0) {
                            var id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                            model.load(id, {
                                success: function (newRec) {
                                    me.setFormData(newRec);
                                    me.fireEvent('savesuccess', me, newRec);
                                }
                            });
                        }
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message, function (btn, text) {
                            if (!rec.phantom && btn == 'ok') {
                                me.editRecord(rec);
                            }
                        });
                    }
                });
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('defectlistdeletepermissionaspect').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'objectcrdefectlistwin fieldcontainer button[action=Calculate]': {
                'click': {
                    fn: me.calculate,
                    scope: me
                }
            },
            'objectcrdefectlistgrid': {
                'afterrender': {
                    fn: me.onAfterRenderGrid,
                    scope: me
                }
            },
            'objectcrdefectlistwin [type=DpkrSum]': { 'change': { fn: me.onChangeSum, scope: me } },
            'objectcrdefectlistwin [name=CostPerUnitVolume]': { 'change': { fn: me.onChangeCostPerUnitVolume, scope: me } },
            'objectcrdefectlistwin [name=Work]': { 'beforeload': { fn: me.beforeLoadWork, scope: me } },
            'objectcrdefectlistwin [name=Volume]': { 'change': { fn: me.onChangeVolume, scope: me } }
        });

        me.callParent(arguments);
    },

    onAfterRenderGrid: function (grid) {
        B4.Ajax.request(B4.Url.action('UseTypeDefectList', 'Defect', {})).next(
            function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            if (!obj.UseTypeDefectList) {
                grid.columns[2].setVisible(false);
            } else {
                grid.columns[2].setVisible(true);
            }
        }).error(function () {});
    },
    onChangeVolume: function (fld, newValue, oldValue) {
        var win = fld.up('objectcrdefectlistwin'),
            calcBy = win.getRecord().get('CalculateBy');

        if (oldValue && newValue && newValue != oldValue && calcBy == 0) {
            win.down('[type=DpkrSum]').setValue(null);
        }
    },

    onChangeSum: function (fld, newValue, oldValue) {
        var win = fld.up('objectcrdefectlistwin'),
            calcBy = win.getRecord().get('CalculateBy');

        if (oldValue && newValue && newValue != oldValue && calcBy == 0) {
            win.down('[name=CostPerUnitVolume]').setValue(null);
        }
    },
    onChangeCostPerUnitVolume: function (fld, newValue, oldValue) {
        var win = fld.up('objectcrdefectlistwin'),
            calcBy = win.getRecord().get('CalculateBy');

        if (oldValue && newValue && newValue != oldValue && calcBy == 0) {
            win.down('[type=DpkrSum]').setValue(null);
        }
    },
    beforeLoadWork: function (store, operation) {
        operation.params.objCrId = this.getContextValue(this.getMainComponent(), 'objectcrId');
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectcrdefectlistgrid'),
            json,
            store,
            colVolume = view.down('gridcolumn[dataIndex=Volume]'),
            colSum = view.down('gridcolumn[dataIndex=Sum]');

        B4.Ajax.request({
            url: B4.Url.action('GetDefectListViewValue', 'Defect', {
                objectCrId: id
            })
        }).next(function (response) {
            
            json = Ext.JSON.decode(response.responseText);

            view.typeView = json.data;

            if (json.data == 0) {
                colVolume.show();
                colSum.show();
            } else {
                colVolume.hide();
                colSum.hide();
            }
        }).error(function () {
            Log('Ошибка получения параметров приложения');
        });

        me.getAspect('defectlistcreatepermissionaspect').setPermissionsByRecord({ getId: function() { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');
        view.getStore('objectcr.DefectList').load();

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },

    onBeforeLoad: function (store, operation) {
        var objectId = this.getContextValue(this.getMainComponent(), 'objectcrId');
        operation.params.objectCrId = objectId;
    }
});