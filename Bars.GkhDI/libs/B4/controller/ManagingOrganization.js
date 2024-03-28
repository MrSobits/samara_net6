// Внимание у данного контроллера есть клон в проекте GkhDi
Ext.define('B4.controller.ManagingOrganization', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ManOrg'
    ],
    controllers: ['B4.controller.manorg.Navigation'],
    models: ['ManagingOrganization'],
    stores: ['ManagingOrganization'],
    views: [
        'manorg.AddWindow',
        'manorg.Grid'
    ],

    aspects: [
        {
            xtype: 'manorgperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'manorgGridWindowAspect',
            gridSelector: 'manorgGrid',
            editFormSelector: '#manorgAddWindow',
            storeName: 'ManagingOrganization',
            modelName: 'ManagingOrganization',
            editWindowView: 'manorg.AddWindow',
            controllerEditName: 'B4.controller.manorg.Navigation',
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowNotValid'] = { 'change': { fn: this.cbShowNotValidChange, scope: this } };
                actions[this.editFormSelector + ' #cbOfficialSite731'] = { 'change': { fn: this.cbOfficialSite731Change, scope: this } };
                actions[this.editFormSelector + ' #sflContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
            },

            cbShowNotValidChange: function () {
                this.controller.getStore(this.storeName).load();
            },
            //deleteRecord: function (record) {
            //    var me = this;
            //    var existDi = false;

            //    me.controller.mask('Копирование', me.controller.getMainComponent());
            //    Ext.Ajax.request({
            //        url: B4.Url.action('/DisclosureInfo/GetDisclosureOfManOrg'),
            //        params: {
            //            manorgId: record.getId()
            //        },
            //        timeout: 1200000,
            //        async: false,
            //        success: function (response) {
            //            me.controller.unmask();
            //            var obj = Ext.decode(response.responseText);
            //            if (obj) {
            //                existDi = obj.exist;
            //            }
            //            return true;
            //        },
            //        failure: function (response) {
            //            var obj = Ext.decode(response.responseText);
            //            me.controller.unmask();
            //            Ext.Msg.alert('Ошибка', obj.message || 'Не удалось скопировать сведения об услугах');
            //        }
            //    });

            //    var messageDi = existDi ? 'По данной организации имеются сведения в разделе "Раскрытие информации по 731 ПП РФ"' : '';

            //    Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?' + messageDi, function (result) {
            //        if (result == 'yes') {
            //            var model = this.getModel(record);

            //            var rec = new model({ Id: record.getId() });
            //            me.controller.mask('Удаление', B4.getBody());
            //            rec.destroy()
            //                .next(function (result) {
            //                    this.fireEvent('deletesuccess', this);
            //                    me.updateGrid();
            //                    me.controller.unmask();
            //                }, this)
            //                .error(function (result) {
            //                    Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            //                    me.controller.unmask();
            //                }, this);
            //        }
            //    }, me);
            //},

            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                        case 'map':
                            this.onClickActionMap(record);
                            break;
                    }
                }
            },

            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('managingorganizationedit/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            cbOfficialSite731Change: function (cb, newValue) {
                var form = this.getForm();
                if (form) {
                    form.down('#tfOfficialSite').setDisabled(!newValue);
                }
            },
            onBeforeLoadContragent: function (field, options) {
                if (options.params == null) {
                    options.params = {};
                }

                options.params.showAll = true;
                options.params.operatorHasContragent = true;
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'managingOrganizationButtonExportAspect',
            gridSelector: 'manorgGrid',
            buttonSelector: 'manorgGrid #btnExport',
            controllerName: 'ManagingOrganization',
            actionName: 'Export'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    mainView: 'manorg.Grid',
    mainViewSelector: 'manorgGrid',

    refs: [{
        ref: 'mainView',
        selector: 'manorgGrid'
    }],

    init: function () {
        this.getStore('ManagingOrganization').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('manorgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ManagingOrganization').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.showAll = false;
        operation.params.operatorHasContragent = true;

        var mainView = this.getMainView();
        if (mainView) {
            operation.params.showNotValid = mainView.down('#cbShowNotValid').checked;
        }
    }
});