Ext.define('B4.controller.manorglicense.EditLicense', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextButton',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    stores: [
        'manorglicense.License',
        'manorglicense.LicenseDoc',
        'manorglicense.LicenseExtension'
    ],

    models: [
        'Contragent',
        'ManagingOrganization',
        'manorglicense.License',
        'manorglicense.LicenseDoc',
        'manorglicense.LicenseExtension'
    ],

    views: [
        'manorglicense.EditLicensePanel',
        'manorglicense.LicenseDocGrid',
        'manorglicense.LicenseExtensionGrid',
        'manorglicense.LicenseDocEditWindow',
        'manorglicense.LicenseExtensionEditWindow'
    ],

    mainView: 'manorglicense.EditLicensePanel',
    mainViewSelector: 'manOrgLicenseEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'manOrgLicenseEditPanel'
        },
        {
            ref: 'docGrid',
            selector: 'manorglicensegrid'
        },
        {
            ref: 'docEditWindow',
            selector: 'manorglicensedoceditwindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'licensePrintAspect',
            buttonSelector: 'manOrgLicenseEditPanel #btnPrint',
            codeForm: 'ManOrgLicense',
            getUserParams: function () {
                var me = this,
                    param = { LicenseId: me.controller.getContextValue('licenseId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'backforwardaspect',
            panelSelector: 'personEditPanel',
            backForwardController: 'manorglicense.License'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'manOrgLicenseStatePermAspect',
            permissions: [
                { name: 'Gkh.ManOrgLicense.License.Edit', applyTo: 'b4savebutton', selector: 'manOrgLicenseEditPanel' },
                { name: 'Gkh.ManOrgLicense.License.Delete', applyTo: 'button[action=Delete]', selector: 'manOrgLicenseEditPanel' },
                { name: 'Gkh.ManOrgLicense.License.Field.LicNum_Edit', applyTo: '[name=LicNum]', selector: 'manOrgLicenseEditPanel' }
            ]
        },
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'licenseStateButtonAspect',
            stateButtonSelector: 'manOrgLicenseEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('manOrgLicenseEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'manOrgLicenseEditPanelAspect',
            editPanelSelector: 'manOrgLicenseEditPanel',
            modelName: 'manorglicense.License',
            otherActions: function (actions) {
                var me = this;
                
                actions['manOrgLicenseRequestEditPanel b4selectfield[name=Contragent]'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
            },
            onBeforeLoadContragent: function (field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.typeJurOrg = 10; // нужны тольк оуправляющие организации

                return true;
            },
            listeners: {
                savesuccess: function(asp, rec) {
                    asp.setData(rec.getId());
                },
                aftersetpaneldata: function(asp, rec, panel) {
                    var me = this,
                        docGrid = panel.down('manorglicensedocgrid'),
                        docStore = docGrid.getStore();
                        
                        extGrid = panel.down('manorglicenseextensiongrid'),
                        extStore = extGrid.getStore();

                    me.controller.getAspect('licenseStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    me.controller.getAspect('manOrgLicenseStatePermAspect').setPermissionsByRecord({ getId: function () { return rec.get('Id'); } });

                    docStore.clearFilter(true);
                    docStore.filter('licenseId', rec.getId());

                    extStore.clearFilter(true);
                    extStore.filter('licenseId', rec.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licenseDocGridWindowAspect',
            gridSelector: 'manorglicensedocgrid',
            editFormSelector: 'manorglicensedoceditwindow',
            modelName: 'manorglicense.LicenseDoc',
            editWindowView: 'manorglicense.LicenseDocEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.ManOrgLicense = me.controller.getContextValue(me.controller.getMainView(), 'licenseId');
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licenseExtensionGridWindowAspect',
            gridSelector: 'manorglicenseextensiongrid',
            editFormSelector: 'manorglicenseextensioneditwindow',
            modelName: 'manorglicense.LicenseExtension',
            editWindowView: 'manorglicense.LicenseExtensionEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.ManOrgLicense = me.controller.getContextValue(me.controller.getMainView(), 'licenseId');
                    }
                }
            }
        }
    ],

    index: function (type, id) {
        var me = this,
            view = me.getMainView();
            
        if (!view) {
            view = Ext.widget('manOrgLicenseEditPanel');

            view.params = {};

            B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgLicense', {
                type: type,
                id: id
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);

                view.params.type = type;
                view.params.licenseId = obj.licenseId;
                view.params.requestId = obj.requestId;
                
                if (view.params.licenseId) {
                    me.bindContext(view);
                    me.setContextValue(view, 'type', view.params.type);
                    me.setContextValue(view, 'id', id);
                    me.setContextValue(view, 'requestId', view.params.requestId);
                    me.setContextValue(view, 'licenseId', view.params.licenseId);
                    me.application.deployView(view, 'license_info');

                    me.getAspect('licensePrintAspect').loadReportStore();
                    me.getAspect('manOrgLicenseEditPanelAspect').setData(view.params.licenseId);
                } 
                
                return true;
            }).error(function () {
            });
        }
        else {
            if (view.params) {
                me.bindContext(view);
                me.setContextValue(view, 'type', type);
                me.setContextValue(view, 'id', id);
                me.setContextValue(view, 'requestId', view.params.requestId);
                me.setContextValue(view, 'licenseId', view.params.licenseId);
                me.application.deployView(view, 'license_info');
                
                me.getAspect('licensePrintAspect').loadReportStore();
                me.getAspect('manOrgLicenseEditPanelAspect').setData(view.params.licenseId);
            }
        }
    },

    init: function() {
        var me = this,
            actions = {};

        actions['manOrgLicenseEditPanel combobox[name=TypeTermination]'] = {
            'change': {
                fn: me.onChangeTypeTermination,
                scope: me
            }
        };
        
        actions['manOrgLicenseEditPanel button[action=Delete]'] = {
            'click': {
                fn: me.onDeleteLicense,
                scope: me
            }
        };
        
        actions['manOrgLicenseEditPanel button[action=goToManOrgContracts]'] = {
            'click': {
                fn: me.goToManOrgContracts,
                scope: me
            }
        };

        actions['manOrgLicenseEditPanel button[action=ERULRequest]'] =
            { click: me.eRULRequest, scope: me };

        actions['manOrgLicenseEditPanel button[action=ERULUpdateRequest]'] =
            { click: me.eRULUpdateRequest, scope: me }

        me.control(actions);

        me.callParent(arguments);
    },

    eRULRequest: function (btn) {
        var me = this,
            panel = btn.up('manOrgLicenseEditPanel'),
            record = panel.getForm().getRecord();

        Ext.Msg.confirm('Запрос в ЕРУЛ', 'Подтвердите запрос номера лицензии в ЕРУЛ', function (result) {
            if (result == 'yes') {
                me.mask('Отправка запроса', B4.getBody());
                B4.Ajax.request({
                    url: B4.Url.action('SendErulRequest', 'SMEVERULExecute'),
                    method: 'POST',
                    timeout: 100 * 60 * 60 * 3,
                    params: {
                        docId: panel.params.licenseId
                    }
                }).next(function () {
                    B4.QuickMsg.msg('СМЭВ', 'Запрос на получение номера лицензии отправлен', 'success');
                    me.unmask();
                }, me)
                    .error(function (result) {
                        if (result.responseData || result.message) {
                            Ext.Msg.alert('Ошибка отправки запроса!', Ext.isString(result.responseData) ? result.responseData : result.message);
                        }
                        me.unmask();
                    }, me);

            }
        }, this);
    },

    eRULUpdateRequest: function (btn) {
        var me = this,
            panel = btn.up('manOrgLicenseEditPanel'),
            record = panel.getForm().getRecord();

        Ext.Msg.confirm('Запрос в ЕРУЛ', 'Подтвердите отправку данных в ЕРУЛ', function (result) {
            if (result == 'yes') {
                me.mask('Отправка запроса', B4.getBody());
                B4.Ajax.request({
                    url: B4.Url.action('SendErulUpdateRequest', 'SMEVERULExecute'),
                    method: 'POST',
                    timeout: 100 * 60 * 60 * 3,
                    params: {
                        docId: panel.params.licenseId
                    }
                }).next(function () {
                    B4.QuickMsg.msg('СМЭВ', 'Запрос на получение номера лицензии отправлен', 'success');
                    me.unmask();
                }, me)
                    .error(function (result) {
                        if (result.responseData || result.message) {
                            Ext.Msg.alert('Ошибка отправки запроса!', Ext.isString(result.responseData) ? result.responseData : result.message);
                        }
                        me.unmask();
                    }, me);

            }
        }, this);
    },

    onDeleteLicense: function(btn) {
        var me = this,
            panel = btn.up('manOrgLicenseEditPanel'),
            record = panel.getForm().getRecord();
        
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
            if (result == 'yes') {
                me.mask('Удаление', B4.getBody());
                record.destroy()
                    .next(function () {
                        
                        B4.QuickMsg.msg('Удаление', 'Лицензия удалена успешно', 'success');
                        
                        if (panel.params.requestId) {
                            Ext.History.add(Ext.String.format("manorglicense/{0}/{1}/deletelicense", panel.params.type, panel.params.requestId));
                        }
                        
                        me.unmask();
                    }, me)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        me.unmask();
                    }, me);

            }
        }, this);
    },

    onChangeTypeTermination: function (cmp, nv) {
        var panel = cmp.up('manOrgLicenseEditPanel'),
            fldDateTermination = panel.down('[name=DateTermination]');

        if (nv > 0) {
            fldDateTermination.allowBlank = false;
        } else {
            fldDateTermination.allowBlank = true;
        }
        
        panel.getForm().isValid();
    },

    goToManOrgContracts: function(btn) {
        var me = this,
            params = Ext.create('B4.model.ManagingOrganization'),
            view = me.getMainView(),
            contragentId = view.down('[name=Contragent]').getValue();

        if (!contragentId) {
            Ext.Msg.alert('Внимание!', 'Не выбрана управляющая организация!');
            return;
        }


        B4.Ajax.request(B4.Url.action('GetManOrgByContagentId', 'ManagingOrganization', {
            contragentId: contragentId
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);

            if (!obj) {
                Ext.Msg.alert('Внимание!', 'Не удалось определить управляющую организацию!');
                return;
            }

            Ext.History.add('managingorganizationedit/' + obj.data.Id + '/realityObject');

        }).error(function () {
        });
    }
});