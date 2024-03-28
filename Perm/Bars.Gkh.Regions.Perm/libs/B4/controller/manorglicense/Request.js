Ext.define('B4.controller.manorglicense.Request', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.controller.manorglicense.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.enums.LicenseRequestType'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['manorglicense.Request'],
    stores: ['manorglicense.Request'],
    views: [
        'manorglicense.Grid',
        'manorglicense.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'manorglicense.Grid',
    mainViewSelector: 'manorglicenserequestgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'manorglicenserequestgrid'
        },
        {
            ref: 'addView',
            selector: 'manorglicenseaddwindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.ManOrgLicense.Request.Create', applyTo: 'b4addbutton', selector: 'manorglicenserequestgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.ManOrgLicense.Request.Delete' }],
            name: 'deleteRequestStatePerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'licenseRequestStateTransferAspect',
            gridSelector: 'manorglicenserequestgrid',
            menuSelector: 'rmanorglicenserequestgridStateMenu',
            stateType: 'gkh_manorg_license_request'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'manOrgLicenseRequestGridWindowAspect',
            gridSelector: 'manorglicenserequestgrid',
            editFormSelector: 'manorglicenseaddwindow',
            storeName: 'manorglicense.Request',
            modelName: 'manorglicense.Request',
            editWindowView: 'manorglicense.AddWindow',
            controllerEditName: 'B4.controller.manorglicense.Navi',
            deleteWithRelatedEntities: true,

            otherActions: function (actions) {
                actions['manorglicenseaddwindow b4combobox[name=Type]'] = { 'change': { fn: this.onChangeType, scope: this } };
            },

            onChangeType: function (cmb, newValue) {
                var form = cmb.up('manorglicenseaddwindow'),
                    contragent = form.down('b4selectfield[name=Contragent]'),
                    manOrgLicense = form.down('b4selectfield[name=ManOrgLicense]');

                switch (newValue) {
                    case B4.enums.LicenseRequestType.GrantingLicense:
                    case B4.enums.LicenseRequestType.ExtractFromRegisterLicense:
                        contragent.setVisible(true);
                        manOrgLicense.setVisible(false);
                        contragent.allowBlank = false;
                        manOrgLicense.allowBlank = true;
                        break;

                    case B4.enums.LicenseRequestType.RenewalLicense:
                        contragent.setVisible(true);
                        contragent.allowBlank = false;
                        manOrgLicense.setVisible(true);
                        manOrgLicense.allowBlank = false;
                        break;

                    case B4.enums.LicenseRequestType.IssuingDuplicateLicense:
                    case B4.enums.LicenseRequestType.TerminationActivities:
                    case B4.enums.LicenseRequestType.ProvisionCopiesLicense:
                        contragent.setVisible(false);
                        contragent.allowBlank = true;
                        manOrgLicense.setVisible(true);
                        manOrgLicense.allowBlank = false;
                        break;
                }
            },
            
            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                    case 'edit':
                        this.editRecord(record);
                        break;
                    case 'delete':
                        this.deleteRecord(record);
                        break;
                    }
                }
            },

            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('manorglicense/{0}/{1}/editrequest', 'request', id));
                    }
                    else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                }
                else {
                    me.setFormData(new model({ Id: 0, Type: B4.enums.LicenseRequestType.GrantingLicense }));
                }
            },
            
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteRequestStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
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

                        }, this);
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('manorglicenserequestgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});