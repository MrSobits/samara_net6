Ext.define('B4.controller.Competition', {
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.GkhGridEditForm',
       'B4.aspects.StateGridWindowColumn',
       'B4.controller.competition.Navi',
       'B4.aspects.ButtonDataExport',
       'B4.aspects.StateContextMenu',
       'B4.aspects.permission.GkhPermissionAspect',
       'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['Competition'],
    stores: ['Competition'],
    views: [
        'competition.Grid',
        'competition.AddWindow'
    ],

    mainView: 'competition.Grid',
    mainViewSelector: 'competitiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'competitiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.Competition.Create', applyTo: 'b4addbutton', selector: 'competitiongrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.Competition.Delete' }],
            name: 'deleteCompetitionStatePerm'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'competitionButtonExportAspect',
            gridSelector: 'competitiongrid',
            buttonSelector: 'competitiongrid #btnCompetitionExport',
            controllerName: 'Competition',
            actionName: 'Export'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'competitionStateTransferAspect',
            gridSelector: 'competitiongrid',
            menuSelector: 'competitiongridStateMenu',
            stateType: 'cr_competition'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'competitionaspect',
            gridSelector: 'competitiongrid',
            editFormSelector: '#competitionaddwin',
            storeName: 'Competition',
            modelName: 'Competition',
            editWindowView: 'competition.AddWindow',
            controllerEditName: 'B4.controller.competition.Navi',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;
                
                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('competitionedit/{0}', id));
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
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteCompetitionStatePerm').loadPermissions(record)
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

    index: function () {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('competitiongrid');
            me.getStore('Competition').on('beforeload', me.onBeforeLoad, me);
        }

        me.bindContext(view);
        me.application.deployView(view);
        
        me.getStore('Competition').load();
    },
    
    onBeforeLoad: function (store, operation) {
        
    }
});