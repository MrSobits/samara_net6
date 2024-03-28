Ext.define('B4.controller.realityobj.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.realityobj.RealityObjEditPanelAspect',
        'B4.aspects.StateContextButtonMenu',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.realityobj.RealityObjectFields',
        'B4.aspects.BackForward',
        'B4.aspects.permission.realityobj.RealityObjectButtonsPermission',
        'B4.aspects.fieldrequirement.RealityObject'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'RealityObject'
    ],

    views: [
        'realityobj.EditPanel',
        'realityobj.AddAvatarImage'
    ],

    mainView: 'realityobj.EditPanel',
    mainViewSelector: 'realityobjEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjEditPanel'
        },
        {
            selector: '#sfBuildingFeatureRealityObject',
            ref: 'featuresField'
        },
        {
            ref: 'conditionHouseChangeValBtn',
            selector: 'realityobjEditPanel changevalbtn[propertyName=ConditionHouse]'
        }
    ],

    aspects: [
        {
            xtype: 'realityobjbuttonsperm',
            name: 'RealityObjButtonsPerm'
        },
        {
            xtype: 'realityobjfieldrequirement'
        },
         {
             xtype: 'realityobjfieldsperm',
             name: 'realityobjfieldsperm',
             afterSetRequirements: function (rec) {
                 var asp = this;

                 var privDateArr = Ext.ComponentQuery.query(asp.controller.mainViewSelector + ' [name="PrivatizationDateFirstApartment"]');
                 var hasPrivatizedFlatsArr = Ext.ComponentQuery.query(asp.controller.mainViewSelector + ' #hasPrivatizedFlats');

                 if (privDateArr.length > 0 && hasPrivatizedFlatsArr.length) {

                     var privDate = privDateArr[0];
                     var hasPrivatizedFlats = hasPrivatizedFlatsArr[0];

                     if (hasPrivatizedFlats.value && (!hasPrivatizedFlats.isDisabled() || hasPrivatizedFlats.resetDisable)) {
                         privDate.enable();
                     }
                     else {
                         privDate.disable();
                     }
                 }
             }
         },
        {
            xtype: 'backforwardaspect',
            panelSelector: 'realityobjEditPanel',
            backForwardController: 'RealityObject'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            fieldSelector: '#sfBuildingFeatureRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#buildingFeatureMunicipalitySelectWindow',
            storeSelect: 'dict.BuildingFeature',
            storeSelected: 'dict.BuildingFeature',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            getSelectField: function () {
                var me = this;

                return me.controller.getMainView().down(me.fieldSelector);
            }
        },
        {
            xtype: 'statecontextbuttonmenu',
            name: 'realityobjStateButtonAspect',
            buttonSelector: '#btnState',
            stateType: 'gkh_real_obj',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('realityobjEditPanelAspect').setData(entityId);
                    asp.controller.getAspect('realityObjInfoPerm').setPermissionsByRecord({ getId: function () { return entityId; } });
                }
            }
        },
         {
             xtype: 'realityobjeditpanelaspect',
             otherActions: function (actions) {
                 actions[this.editPanelSelector + ' #btnMap'] = { 'click': { fn: this.onClickbtnMap, scope: this } };
                 actions[this.editPanelSelector + ' #btnExportTp'] = { 'click': { fn: this.onClickbtnExportTp, scope: this } };
                 actions[this.editPanelSelector + ' #btnPrescr'] = { 'click': { fn: this.onClickbtnExportToGZHI, scope: this } };
                 actions[this.editPanelSelector + ' checkbox[name=IsNotInvolvedCr]'] = { 'change': { fn: this.controller.onIsNotInvolvedCrChange, scope: this } };
                 actions[this.editPanelSelector + ' checkbox[name=IsCulturalHeritage]'] = { 'change': { fn: this.onIsCulturalHeritageChange, scope: this } };
             },

             onClickbtnExportToGZHI: function () {
                 var asp = this,
                     params,
                     url;
                 debugger;
                 asp.controller.mask('Отправка Email', asp.controller.getMainComponent());
                var result = B4.Ajax.request(B4.Url.action('SendFKRToGZHIMail', 'ObjectCr', {
                    taskId: asp.controller.getContextValue(asp.controller.getMainComponent(), 'realityObjectId')
                }
                )).next(function (response) {

                    asp.controller.unmask();
                    Ext.Msg.alert('Внимание!', 'Письмо отправлено');
                    var data = Ext.decode(response.responseText);
                    return true;
                }).error(function (response) {
                    var data = response.message;
                    asp.controller.unmask();
                    Ext.Msg.alert('Внимание!', data);
                });
             },

             onClickbtnExportTp: function () {
                 var asp = this,
                     params,
                     url;

                 asp.controller.mask('Загрузка', asp.controller.getMainComponent());

                 try {
                     params = Ext.Object.toQueryString({
                         house: asp.controller.getContextValue(asp.controller.getMainComponent(), 'realityObjectId')
                     });

                     url = Ext.urlAppend('/RealityObject/GetPassportReport/?id=0&' + params, '_dc=' + (new Date().getTime()));

                     Ext.DomHelper.append(document.body, {
                         tag: 'iframe',
                         id: 'downloadIframe',
                         frameBorder: 0,
                         width: 0,
                         height: 0,
                         css: 'display:none;visibility:hidden;height:0px;',
                         src: B4.Url.action(url)
                     });
                 } catch (e) {
                     Ext.Msg.alert('Ошибка', 'Не удалось распечатать форму');
                 } finally {
                     asp.controller.unmask();
                 }
             },

             onIsCulturalHeritageChange: function (field, isChecked) {
                 if (!field.disabled) {
                     var panel = field.up('realityobjEditPanel'),
                         culturalHeritageAssignmentDateField = panel.down('[name=CulturalHeritageAssignmentDate]');

                     if (culturalHeritageAssignmentDateField) {
                         culturalHeritageAssignmentDateField.setDisabled(!isChecked);
                     }
                 }
             },

             listeners: {
                 beforesetpaneldata: function (asp, rec, panel) {
                     var val = rec.get('HasPrivatizedFlats'),
                         newVal = 30; //не задано
                     if (val === true) {
                         newVal = 10; //да
                     } else if (val === false) {
                         newVal = 20; //нет
                     }
                     rec.set('HasPrivatizedFlats', newVal);
                 },
                 aftersetpaneldata: function (asp, rec, panel) {
                     var relEstTypeField = panel.down('[name=RealEstateType]'),
                         conditionHouseChangeValBtn = asp.controller.getConditionHouseChangeValBtn(); //может отсутствовать в регионах

                     asp.controller.getRealityObjectImage(rec.get('Id'));
                     asp.controller.getAspect('realityobjStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                     asp.controller.setBuildingFeatures(rec.get('Id'));
                     if (conditionHouseChangeValBtn) {
                         conditionHouseChangeValBtn.setEntityId(rec.get('Id'));
                     }

                     if (relEstTypeField) {
                         if (rec.get('IsAutoRealEstType') === true) {
                             relEstTypeField.setReadOnly(true);
                             relEstTypeField.updateDisplayedText(rec.get('AutoRealEstType'));
                         } else {
                             relEstTypeField.setReadOnly(false);
                         }
                     }
                 },
                 beforesave: function(asp, rec, panel) {
                     var val = rec.get('HasPrivatizedFlats'),
                         newVal = false;
                     if (val === 10) { // да
                         newVal = true;
                     } else if (val === 20) { // нет
                         newVal = false;
                     } else if (val === 30) { // не задано
                         newVal = null;
                     }
                     rec.set('HasPrivatizedFlats', newVal);
                 },
                 savesuccess: function (asp, rec) {
                     var controller = asp.controller,
                         field = controller.getFeaturesField();
                     if (field.getValue() || !field.getRawValue()) {
                         controller.saveBuildingFeatures(rec.get('Id'), field.getValue());
                     }
                 },
                 validate: function (asp) {
                     var panel = asp.getPanel(),

                         areaLivingNotLivingMkd = panel.down('field[name=AreaLivingNotLivingMkd]'),
                         areaLiving = panel.down('field[name=AreaLiving]'),
                         areaNotLivingPremises = panel.down('field[name=AreaNotLivingPremises]'),
                         areaMkd = panel.down('field[name=AreaMkd]'),
                         areaLivingOwned = panel.down('field[name=AreaLivingOwned]'),
                         eps = 0.01;

                     if ((areaLivingNotLivingMkd.isDirty() || areaMkd.isDirty()) && (areaLivingNotLivingMkd.getValue() - areaMkd.getValue()) > eps) {
                         B4.QuickMsg.msg('Внимание!', 'Общая площадь МКД меньше, чем общая площадь жилых и нежилых помещений в МКД', 'warning', 5000);

                         return false;
                     }
            
                     if ((areaLivingNotLivingMkd.isDirty() || areaLiving.isDirty()) && (areaLivingNotLivingMkd.getValue() && areaLiving.getValue() - areaLivingNotLivingMkd.getValue()) > eps) {
                         B4.QuickMsg.msg('Внимание!', 'Общая площадь жилых и нежилых помещений в МКД должна быть не меньше, чем жилых всего', 'warning', 5000);

                         return false;
                     }

                     if ((areaLivingOwned.isDirty() || areaLiving.isDirty()) && (areaLivingOwned.getValue() && areaLivingOwned.getValue() - areaLiving.getValue()) > eps) {
                         B4.QuickMsg.msg('Внимание!', 'Площадь жилых помещений должна быть не меньше, чем жилых, находящихся в собственности граждан', 'warning', 5000);

                         return false;
                     }

                     if ((areaNotLivingPremises.isDirty() || areaLiving.isDirty() || areaLivingNotLivingMkd.isDirty()) && (areaLiving.getValue() + areaNotLivingPremises.getValue() - areaLivingNotLivingMkd.getValue()) > eps) {
                         B4.QuickMsg.msg('Внимание!', 'Общая площадь жилых и нежилых помещений в МКД должна быть не меньше, чем сумма площадей "Жилых всего" и "Нежилых всего"', 'warning', 5000);
                             
                         return false;     
                     }

                     return true;
                 }
             }
         }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjEditPanel'),
            permissionAspect = me.getAspect('realityobjfieldsperm');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);

        me.mask(me.getMainComponent());

        permissionAspect.afterSetRequirements = function (rec) {
            me.application.deployView(view, 'reality_object_info');
            me.hideEmptyFieldset();
            me.getAspect('realityobjEditPanelAspect').setData(rec.getId());
            me.unmask();
        };

        permissionAspect.setPermissionsByRecord({ getId: function () { return id; } });

        me.getAspect('RealityObjButtonsPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    init: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjEditPanel'),
            actions = {};

        actions[me.mainViewSelector + ' button[name="btnAddPhoto"]'] = { click: { fn: me.onAddPhoto, scope: me } };
        actions[me.mainViewSelector + ' button[name="btnDeletePhoto"]'] = { click: { fn: me.onRemovePhoto, scope: me } };
        actions[me.mainViewSelector + ' #hasPrivatizedFlats'] = { change: me.onHasPrivatizedFlatsChange, scope: me };
        actions['addavatarimagewin b4savebutton'] = { click: { fn: me.onSaveImage, scope: me } };
        actions['addavatarimagewin b4closebutton'] = { click: { fn: me.closeWindow, scope: me } };
        actions[me.mainViewSelector] = { render: { fn: me.onEditPanelRender, scope: me } };

        actions[me.mainViewSelector + ' button[action=FillAreaOwned]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaMunicipalOwned]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaGovernmentOwned]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaLivingNotLivingMkd]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaLiving]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaNotLivingPremises]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillPercentDebt]'] = {
            click: me.fillButtonAction,
            scope: me
        };
        actions[me.mainViewSelector + ' button[action=FillAreaLivingOwned]'] = {
            click: me.fillButtonAction,
            scope: me
        };

        me.control(actions);

        me.callParent(arguments);
    },

    onEditPanelRender: function (p) {
        B4.Ajax.request({
            url: B4.Url.action('Get', 'GkhParams'),
            params: {
                paramName: 'UseAdminOkrug'
            }
        }).next(function (resp) {
            var useAdminOkrug = Ext.JSON.decode(resp.responseText),
                tf = p.down('[name=District]');

            if (tf) {
                tf.setVisible(useAdminOkrug);
                tf.allowBlank = !useAdminOkrug;
            }
        });
    },


    onSaveImage: function (btn) {
        var me = this,
            addImgForm = btn.up('addavatarimagewin'),
            filefield = addImgForm.down('b4filefield[name="File"]'),
            groupId,
            objectId;

        B4.enums.ImagesGroup.getStore().findBy(function (record) {
            if (record.get('Name') == 'Avatar') {
                groupId = record.get('Value');
            }
        });

        objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');

        me.mask('Сохранение', me.getMainComponent());
        addImgForm.submit({
            url: B4.Url.action('Create', 'RealityObjectImage'),
            params: {
                records: Ext.encode([
                    {
                        RealityObject: objectId,
                        ImagesGroup: groupId
                    }])
            },
            success: function () {
                me.unmask();
                addImgForm.close();

                me.getRealityObjectImage(objectId);

                B4.QuickMsg.msg('Успешно', 'Изображение успешно сохранено', 'success');
            },
            failure: function (form, action) {
                me.unmask();
                addImgForm.close();

                Ext.Msg.alert('Ошибка сохранения!', action.result.message);
            }
        });
    },

    getRealityObjectImage: function (realityObjectId) {
        var me = this,
            btnAdd = me.getMainView().down('button[name="btnAddPhoto"]'),
            btnDelete = me.getMainView().down('button[name="btnDeletePhoto"]');

        // Получаем и загружаем картинку в контейнер аватара
        B4.Ajax.request({
            url: B4.Url.action('GetFileUrl', 'RealityObjectImage'),
            method: 'Get',
            params: { id: realityObjectId }
        }).next(function (response) {
            var resp = Ext.JSON.decode(response.responseText);

            var bgrAvatar = me.getMainView().down('buttongroup[name="bgrAvatar"]');

            if (resp.success) {
                var image = me.getMainView().down('image[name="Avatar"]');
                image.setSrc('data:image/jpeg;base64,' + resp.src);
                me.imageId = resp.imageId;

                btnDelete.show();
                btnAdd.hide();
            } else {
                btnAdd.show();
                btnDelete.hide();
            }

            bgrAvatar.show();
        });
    },

    closeWindow: function (btn) {
        btn.up('addavatarimagewin').close();
    },

    onAddPhoto: function () {
        Ext.create('B4.view.realityobj.AddAvatarImage').show();
    },

    onRemovePhoto: function () {
        var me = this,
            btnAdd = me.getMainView().down('button[name="btnAddPhoto"]'),
            btnDelete = me.getMainView().down('button[name="btnDeletePhoto"]');

        Ext.Msg.confirm('Удаление!', 'Вы действительно хотите удалить данное изображение дома?', function (result) {
            if (result == 'yes') {
                me.mask('Удаление изображения...', me.getMainComponent());

                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'RealityObjectImage'),
                    method: 'Get',
                    params: {
                        records: Ext.encode([me.imageId])
                    },
                    timeout: 10000
                }).next(function (response) {
                    var resp = Ext.JSON.decode(response.responseText);

                    if (resp.success) {
                        var image = me.getMainView().down('image[name="Avatar"]');
                        image.setSrc(null);

                        btnAdd.show();
                        btnDelete.hide();

                        B4.QuickMsg.msg('Успешно', 'Изображение успешно удалено', 'success');
                    }
                    me.unmask();
                }).error(function () {
                    B4.QuickMsg.msg('Ошибка', 'При удалении изображения объекта произошла ошибка', 'error');
                    me.unmask();
                });
            }
        }, me);
    },

    onHasPrivatizedFlatsChange: function (cb, nv) {
        var me = this,
            privDate = me.getMainView().down('[name="PrivatizationDateFirstApartment"]'),
            record = me.getMainView().getForm().getRecord();

        if (nv === false) {
            Ext.Msg.confirm('Внимание', 'Дата приватизации будет очищена. Продолжить?', function (btn) {
                if (btn == 'yes') {
                    privDate.setValue(null);
                    record.set("PrivatizationDateFirstApartment");
                    privDate.disable();
                } else {
                    cb.setValue(true);
                }
            });
        } else {
            if (!cb.isDisabled() || cb.resetDisable) {
                privDate.enable();
            }
        }
    },

    saveBuildingFeatures: function (roId, buildFeatureIds) {
        var field = this.getFeaturesField();
        field.saving = true;
        B4.Ajax.request({
            url: B4.Url.action('SaveFeatures', 'RealityObjectBuildingFeature'),
            method: 'POST',
            params: {
                objectId: roId,
                featureIds: buildFeatureIds
            }
        }).next(function () {
            field.saving = false;
        }).error(function () {
            field.saving = false;
        });
    },

    setBuildingFeatures: function (roId) {
        var me = this,
            field = me.getFeaturesField();
        if (!field.saving) {
            B4.Ajax.request(B4.Url.action('List', 'RealityObjectBuildingFeature', {
                objectId: roId
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                if (obj) {
                    var rawValue = Ext.Array.map(obj.data, function(item) { return item.Name; }).join(", ");
                    field.setRawValue(rawValue);
                }
            }).error(function () {
            });
        }
    },

    hasChanges: function () {
        return this.getMainComponent().getForm().isDirty();
    },

    getCurrent: function () {
        var me = this;
        return me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },

    onIsNotInvolvedCrChange: function() {
        B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
    },

    fillButtonAction: function (button) {
        var me = this,
            name = button.action.replace('Fill', ''),
            field = button.up().down('[name=' + name + ']');

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetFieldValue', 'RealityObjectFields'),
            method: 'POST',
            params: {
                Id: me.getContextValue(me.getMainComponent(), 'realityObjectId'),
                FieldName: name
            }
        }).next(function (response) {
            me.unmask();
            var responseData = Ext.decode(response.responseText);
            if (responseData) {
                field.setValue(responseData.data);
            }
        }).error(function () {
            me.unmask();
        });
    },

    hideEmptyFieldset: function () {
        var checkFieldset = function(fieldset) {
            var show = false;
            Ext.each(fieldset.query('field'), function(field) {
                show = show || !field.hidden;
            });
            fieldset.setVisible(show);
        }

        Ext.each(this.getMainComponent().query('fieldset'),
            function (fieldset) {
                checkFieldset(fieldset);
            });
    }
});
