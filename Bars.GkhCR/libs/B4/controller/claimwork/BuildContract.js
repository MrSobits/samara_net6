Ext.define('B4.controller.claimwork.BuildContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.claimwork.buildcontract.Grid',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.controller.claimwork.BuildContractNavi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.enums.ClaimWorkTypeBase'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.BuildContract',
        'ObjectCr',
        'objectcr.BuildContract'
    ],
    
    stores: [
         'claimwork.BuildContract'
    ],
    
    views: [
        'claimwork.buildcontract.Grid'
    ],
    
    refs: [
       {
           ref: 'mainView',
           selector: 'buildcontractclaimworkgrid'
       }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.BuildContract.Update', applyTo: 'button[actionName=updState]', selector: 'buildcontractclaimworkgrid'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'builderctrClaimWorkButtonExportAspect',
            gridSelector: 'buildcontractclaimworkgrid',
            buttonSelector: 'buildcontractclaimworkgrid #btnExport',
            controllerName: 'BuildContractClaimWork',
            actionName: 'Export'
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'buildcontractclaimworkgrid',
            storeName: 'claimwork.BuildContract',
            modelName: 'claimwork.BuildContract',
            controllerEditName: 'B4.controller.claimwork.BuildContractNavi',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            otherActions: function (actions) {
                var me = this;
                actions['buildcontractclaimworkgrid button[actionName=updState]'] = { 'click': { fn: me.updState, scope: me } };
                actions['buildcontractclaimworkgrid button[actionName=CreateDoc] menuitem'] = { 'click': { fn: me.createDoc, scope: me } };
                
            },
            editRecord: function(record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('claimworkbc/BuildContractClaimWork/{0}', id));
                    } else {
                        model.load(id, {
                            success: function(rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            updState: function () {
                var me = this,
                    json;
                
                me.controller.mask("Обновление");
                B4.Ajax.request({
                    url: B4.Url.action('UpdateStates', 'BuildContractClaimWork'),
                    timeout: 999999
                }).next(function (response) {
                    me.controller.unmask();
                    json = Ext.JSON.decode(response.responseText);
                    if (json.success !== true) {
                        B4.QuickMsg.msg('Внимание!', json.message, 'error');
                    } else {
                        B4.QuickMsg.msg('Успешно!', "Статусы обновлены", 'success');
                        me.updateGrid();
                    }
                }).error(function () {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', 'Ошибка обновления статусов');
                });
            },
            createDoc: function (btn) {
                var me = this,
                    grid = btn.up('grid'),
                    selection = grid.getSelectionModel().getSelection(),
                    ids = [];

                selection.forEach(function (entry) {
                    ids.push(entry.getId());
                });
                if (ids.length > 0) {
                    me.controller.mask("Формирование");
                    B4.Ajax.request({
                        url: B4.Url.action('MassCreateDocs', 'BuildContractClaimWork', {
                            typeDocument: btn.typeDoc,
                            ids: ids.join(',')
                        }),
                        timeout: 999999
                    }).next(function (response) {
                        me.controller.unmask();
                        var json = Ext.JSON.decode(response.responseText);
                        if (json.success !== true) {
                            B4.QuickMsg.msg('Внимание!', json.message, 'error');
                        } else {
                            B4.QuickMsg.msg('Успешно!', json.message, 'success');
                            me.updateGrid();
                        }
                    }).error(function () {
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка!', 'Ошибка формирования документов');
                    });
                } else {
                    Ext.Msg.alert('Внимание!', 'Выберите записи');
                }
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'buildContractPrintAspect',
            buttonSelector: 'buildcontractclaimworkgrid gkhbuttonprint',
            codeForm: 'NotificationClw,Pretension,LawSuit',
            massPrint: true,
            getClaimWorkType: function () {
                return B4.enums.ClaimWorkTypeBase.BuildContract;
            }
        }
    ],
    
    init: function () {
        var me = this;

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('buildcontractclaimworkgrid'),
            createDocField = view.down('[actionName=CreateDoc]');
        
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();

        me.getAspect('buildContractPrintAspect').loadReportStore();
        
        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var settlementCol = view.down('[dataIndex=Settlement]'),
                json = Ext.JSON.decode(response.responseText);

            if (settlementCol) {
                if (json.ShowStlBuildContractGrid) {
                    settlementCol.show();
                } else {
                    settlementCol.hide();
                }
            }

        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'Ошибка получения параметров приложения');
        });


        B4.Ajax.request({
            url: B4.Url.action('GetDocsTypeToCreate', 'BuildContractClaimWork')
        }).next(function (response) {
            var docs = Ext.JSON.decode(response.responseText);

            createDocField.menu.removeAll();

            Ext.each(docs, function (doc) {
                createDocField.menu.add({
                    xtype: 'menuitem',
                    text: doc.Name,
                    textAlign: 'left',
                    typeDoc: doc.Type
                });
            });

        }).error(function () {

        });
    }
});