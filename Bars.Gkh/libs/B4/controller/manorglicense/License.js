Ext.define('B4.controller.manorglicense.License', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.controller.manorglicense.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['manorglicense.License'],
    stores: ['manorglicense.License'],
    views: [
        'manorglicense.LicenseGrid',
        'manorglicense.GisExportWindow'
    ],

    mainView: 'manorglicense.LicenseGrid',
    mainViewSelector: 'manorglicensegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'manorglicensegrid'
        }
    ],

    aspects: [
        /* Пока для лицензий нетребуется создание из реестра , но если изменится процесс расскоментируйте
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.ManOrgLicense.License.Create', applyTo: 'b4addbutton', selector: 'manorglicensegrid' }
            ]
        },*/
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.ManOrgLicense.License.Delete' }],
            name: 'deleteLicenseStatePerm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'licenseStateTransferAspect',
            gridSelector: 'manorglicensegrid',
            menuSelector: 'manorglicensegridStateMenu',
            stateType: 'gkh_manorg_license'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'manOrgLicenseGridWindowAspect',
            gridSelector: 'manorglicensegrid',
            storeName: 'manorglicense.License',
            modelName: 'manorglicense.License',
            controllerEditName: 'B4.controller.manorglicense.Navi',
            deleteWithRelatedEntities: true,

            otherActions: function (actions) {

                actions[this.gridSelector + ' #btnExportTR'] = { 'click': { fn: this.onClickbtnExportTR, scope: this } };
                actions['gisexportdetailswindow #tbSave'] = { 'click': { fn: this.runExport, scope: this } };

            },
            onClickbtnExportTR: function () {
                var me = this;
                var asp = this,
                    params,
                    url;
                debugger;
                gisinfo = Ext.create('B4.view.manorglicense.GisExportWindow');
                gisinfo.show();
            },

            runExport: function (btn) {
                var formWindow = btn.up('gisexportdetailswindow');
                var dateFrom = formWindow.down('#dfDateFrom').getValue();
                var dateTo = formWindow.down('#dfDateTo').getValue();
                var asp = this,
                    params,
                    url;
                debugger;
                formWindow.close();

                asp.controller.mask('Загрузка', asp.controller.getMainComponent());

                try {
                    params = Ext.Object.toQueryString({
                        dateFrom: dateFrom,
                        dateTo: dateTo
                    });

                    url = Ext.urlAppend('/ManOrgLicense/GetPrintFormResult/?id=0&' + params, '_dc=' + (new Date().getTime()));

                    Ext.DomHelper.append(document.body, {
                        tag: 'iframe',
                        id: 'downloadIframe',
                        frameBorder: 0,
                        width: 0,
                        height: 0,
                        css: 'display:none;visibility:hidden;height:0px;',
                        src: B4.Url.action(url)
                    });
                    //B4.Ajax.request(B4.Url.action('GetPrintFormResult', 'ManOrgLicense', {
                    //    dateFrom: dateFrom,
                    //    dateTo: dateTo
                    //})).next(function (response) {
                    //    asp.controller.unmask();
                    //    return true;
                    //}).error(function () {
                    //    asp.controller.unmask();
                    //});
                } catch (e) {
                    Ext.Msg.alert('Ошибка', 'Не удалось распечатать форму');
                } finally {
                    asp.controller.unmask();
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
                    model,
                    url;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {

                        if (record.data.Request) {
                            url = Ext.String.format('manorglicense/{0}/{1}/editlicense', 'request', record.data.Request);
                        } else {
                            url = Ext.String.format('manorglicense/{0}/{1}/editlicense', 'license', id);
                        }

                        me.controller.application.redirectTo(url);
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
                    me.setFormData(new model({ Id: 0 }));
                }
            }
            ,
            
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteLicenseStatePerm').loadPermissions(record)
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

    init: function() {
     this.getStore('manorglicense.License').on('beforeload', this.onBeforeLoadLicense, this);
        this.callParent(arguments);
    },

 onBeforeLoadLicense: function(store, operation) {  
      var filterPanel = this.getMainView();
        if (filterPanel) {
            operation.params.dateFromStart = filterPanel.down('#dfDateFromStart').getValue();
            operation.params.dateFromEnd = filterPanel.down('#dfDateFromEnd').getValue();
            operation.params.endDateStart = filterPanel.down('#dfEndDateStart').getValue();
            operation.params.endDateEnd = filterPanel.down('#dfEndDateEnd').getValue();                      
        }
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('manorglicensegrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});