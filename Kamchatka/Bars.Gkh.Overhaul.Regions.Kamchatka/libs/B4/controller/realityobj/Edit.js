Ext.define('B4.controller.realityobj.Edit', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.realityobj.RealityObjEditPanelAspect',
        'B4.aspects.StatusButton',
        'B4.enums.ConditionHouse',
        'B4.aspects.permission.realityobj.RealityObjectFields'
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

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjEditPanel'
        },
        {
            ref: 'conditionHouseChangeValBtn',
            selector: 'realityobjEditPanel changevalbtn[propertyName=ConditionHouse]'
        }
    ],

    aspects: [
         {
             xtype: 'realityobjinfoperm',
             name: 'realityObjInfoPerm'
         },
         {
             /*
             Вешаем аспект смены статуса в карточке редактирования
             */
             xtype: 'statusbuttonaspect',
             name: 'realityobjStateButtonAspect',
             stateButtonSelector: 'realityobjEditPanel #btnState',
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
             reportClassName: 'TP Report.TechPassport',
             otherActions: function (actions) {
                 var me = this;
                 actions[me.editPanelSelector + ' #btnMap'] = { 'click': { fn: me.onClickbtnMap, scope: me } };
                 actions[me.editPanelSelector + ' #btnExportTp'] = { 'click': { fn: me.onClickbtnExportTp, scope: me } };
                 actions[me.editPanelSelector + ' #btnExportCh'] = { 'click': { fn: me.onClickbtnExportCh, scope: me } };
                 actions[me.editPanelSelector + ' checkbox[name=IsCulturalHeritage]'] = { 'change': { fn: me.onIsCulturalHeritageChange, scope: me } };
             },

             onClickbtnExportTp: function () {

                 var asp = this;

                 asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                 B4.Ajax.request(B4.Url.action('GetReportId', 'TechPassport', {
                     className: asp.reportClassName
                 })).next(function (response) {
                     asp.controller.unmask();

                     var obj = Ext.JSON.decode(response.responseText);

                     var params = Ext.Object.toQueryString({
                         house: asp.controller.getContextValue(asp.controller.getMainComponent(), 'realityObjectId')
                     });

                     var url = Ext.urlAppend('/TechPassport/GetPrintFormResult/?id=' + obj.ReportId + '&' + params, '_dc=' + (new Date().getTime()));
                     //window.open(B4.Url.action(url));

                     Ext.DomHelper.append(document.body, {
                         tag: 'iframe',
                         id: 'downloadIframe',
                         frameBorder: 0,
                         width: 0,
                         height: 0,
                         css: 'display:none;visibility:hidden;height:0px;',
                         src: B4.Url.action(url)
                     });

                 }).error(function () {
                     asp.controller.unmask();
                     Ext.Msg.alert('Ошибка', 'Не удалось распечатать форму');
                 });
             },
             
             onClickbtnExportCh: function () {

                 var asp = this;

                 asp.controller.mask('Загрузка', asp.controller.getMainComponent());

                 try {
                     var params = Ext.Object.toQueryString({
                         house: asp.controller.getContextValue(asp.controller.getMainComponent(), 'realityObjectId')
                     });

                     var url = Ext.urlAppend('/RealtyObjectData/GetPrintFormResult/?id=0&' + params, '_dc=' + (new Date().getTime()));
                     //window.open(B4.Url.action(url));

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

                 /*
                 B4.Ajax.request(B4.Url.action('GetReportId', 'RealtyObjectData', {
                     className: asp.reportClassName
                 })).next(function (response) {
                     asp.controller.unmask();

                     var obj = Ext.JSON.decode(response.responseText);

                 }).error(function () {
                     asp.controller.unmask();
                     Ext.Msg.alert('Ошибка', 'Не удалось распечатать форму');
                 });*/
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
                 aftersetpaneldata: function (asp, rec, panel) {
                     var privDate = panel.down('[name="PrivatizationDateFirstApartment"]'),
                         hasPrivatizedFlats = panel.down('checkbox[name = "HasPrivatizedFlats"]'),
                         relEstTypeField = panel.down('[name=RealEstateType]'),
                         dfDateDemolutionRealityObject = panel.down('#dfDateDemolutionRealityObject');

                     rec.get('HasPrivatizedFlats') && (!hasPrivatizedFlats.isDisabled() || hasPrivatizedFlats.resetDisable) ? privDate.enable() : privDate.disable();
                     
                     asp.controller.getRealityObjectImage(rec.get('Id'));
                     asp.controller.getAspect('realityobjStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                     asp.controller.getConditionHouseChangeValBtn().setEntityId(rec.get('Id'));

                     if (relEstTypeField) {
                         if (rec.get('IsAutoRealEstType') === true) {
                             relEstTypeField.setReadOnly(true);
                             relEstTypeField.updateDisplayedText(rec.get('AutoRealEstType'));
                         } else {
                             relEstTypeField.setReadOnly(false);
                         }
                     }

                     var stateButtonAspect = this.controller.getAspect('realityobjStateButtonAspect');

                     if (stateButtonAspect) {
                         stateButtonAspect.setStateData(rec.get('Id'), rec.get('State'));
                     }

                     if (rec.get('ConditionHouse') !== B4.enums.ConditionHouse.Razed) {
                         dfDateDemolutionRealityObject.setDisabled(true);
                     }
                 }
             }
         }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getAspect('realityobjEditPanelAspect').setData(id);
        me.getAspect('realityObjInfoPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    init: function () {
        var me = this,
            actions = {};

        actions['realityobjEditPanel button[name="btnAddPhoto"]'] = { click: { fn: this.onAddPhoto, scope: this } };
        actions['realityobjEditPanel button[name="btnDeletePhoto"]'] = { click: { fn: this.onRemovePhoto, scope: this } };
        actions['realityobjEditPanel #hasPrivatizedFlats'] = { change: this.onHasPrivatizedFlatsChange, scope: this };
        actions['addavatarimagewin b4savebutton'] = { click: { fn: this.onSaveImage, scope: me } };
        actions['addavatarimagewin b4closebutton'] = { click: { fn: this.closeWindow, scope: me } };
        actions['realityobjEditPanel'] = { render: { fn: me.onEditPanelRender, scope: me } };

        this.control(actions);

        this.callParent(arguments);
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

        var maxFileSize = 5 * 1024 * 1024; // Ограничение в 5 Mb
        if (!Ext.isEmpty(maxFileSize) && filefield.isFileLoad() && filefield.getSize() > maxFileSize) {
            Ext.Msg.alert('Ошибка', 'Размер файла не должен превышать 5 MB');
            return;
        }

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
                image.setSrc('data: image / jpeg; base64,' + resp.src);
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
    }
});