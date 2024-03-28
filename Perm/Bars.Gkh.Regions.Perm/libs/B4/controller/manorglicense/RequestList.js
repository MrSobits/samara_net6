Ext.define('B4.controller.manorglicense.RequestList', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    views: [
    ],

    mainView: 'B4.view.manorglicense.RequestListPanel',
    mainViewSelector: 'manorglicenserequestlistpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'manorglicenserequestlistpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.ManOrgLicense.Request.Delete' }],
            name: 'deleteRequestStatePerm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'manOrgLicenseRequestGridWindowAspect',
            gridSelector: 'manorglicenserequestlistgrid',
            storeName: 'manorglicense.Request',
            modelName: 'manorglicense.Request',
            controllerEditName: 'B4.controller.manorglicense.Navi',
            deleteWithRelatedEntities: true,
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
            editRecord: function(record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('manorglicense/{0}/{1}/editrequest', 'request', id));
                    }
                }
            },
            deleteRecord: function(record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteRequestStatePerm').loadPermissions(record)
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
                                        me.mask('Удаление', me.controller.getMainComponent());
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
        var me = this,
            actions = {};

        actions['manorglicenserequestlistpanel manorglicenserequestlistgrid'] = { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (type, id) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('manorglicenserequestlistpanel');

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
                    view.down('manorglicenserequestlistgrid').getStore().load();
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
                view.down('manorglicenserequestlistgrid').getStore().load();
            }
        }

        
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            licenseId = this.getContextValue(me.getMainView(), 'licenseId');

        operation.params.licenseId = licenseId;
    }
});