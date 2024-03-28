Ext.define('B4.aspects.DebtorClaimWorkAspect', {
    extend: 'B4.aspects.GkhGridEditForm',

    alias: 'widget.debtorclaimworkaspect',

    requires: [
        'B4.enums.DebtorType',
        'B4.enums.ClaimWorkDocumentType'
    ],

    getDebtorType: null,
    entityName: null,

    init: function (controller) {
        var me = this,
            actions = {};
        this.callParent(arguments);

        actions[me.gridSelector + ' button[actionName=updateState]'] = { 'click': { fn: me.updateState, scope: me } };
        actions[me.gridSelector + ' button[actionName=CreateDoc] menuitem'] = { 'click': { fn: me.createDoc, scope: me } };
        actions[me.gridSelector + ' button[actionName=CreateDoc]'] = { 'menushow': { fn: me.showMenu, scope: me } };

        controller.control(actions);
    },
    updateGrid: function () {
        debugger;
        this.getGrid().getStore().load();
    },
    editRecord: function (record) {
        var me = this,
            id = record ? record.data.Id : null,
            model = me.controller.getModel(me.modelName);
        
        if (id) {
            if (me.controllerEditName) {
                me.controller.application.redirectTo(Ext.String.format('claimwork/{0}/{1}', me.entityName, id));
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
    updateState: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelectionModel().getSelection(),
            ids = [];
        selection.forEach(function (entry) {
            ids.push(entry.getId());
        });

        var count = ids.length > 0 ? ids.length : store.totalCount;

        Ext.Msg.confirm('Внимание!', 'Выполнить обновление по ' + count + ' выбранным лицевым счетам?', function (confirmationResult) {
            if (confirmationResult === 'yes') {
                me.controller.mask('Обновление');
                B4.Ajax.request({
                    url: B4.Url.action('UpdateStates', me.entityName, {
                        ids: ids.join(',')
                    }),
                    timeout: 100 * 60 * 60
                }).next(function (response) {
                    me.controller.unmask();
                    var json = Ext.JSON.decode(response.responseText);

                    if (json && json.success) {
                        var message = 'Задача поставлена в очередь на обработку. Статус ее выполнения можно отследить в разделе задачи';
                        me.updateGrid();
                        Ext.Msg.alert('Внимание!', message);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Ошибка обновления статусов');
                    }
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', e.message);
                });
            }
        }, me);
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
            me.controller.mask('Формирование');
            B4.Ajax.request({
                url: B4.Url.action('MassCreateDocs', me.entityName, {
                    typeDocument: btn.typeDoc,
                    ids: ids.join(',')
                }),
                timeout: 3600000
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
    },
    showMenu: function (btn) {
        var me = this,
            notifFormationKind,
            pretensionFormationKind,
            items = btn.menuItems;

        switch (me.getDebtorType()) {
            case B4.enums.DebtorType.Individual:
                notifFormationKind = Gkh.config.DebtorClaimWork.Individual.DebtNotification.NotifFormationKind !== B4.enums.DocumentFormationKind.Form;
                pretensionFormationKind = Gkh.config.DebtorClaimWork.Individual.Pretension.PretensionFormationKind !== B4.enums.DocumentFormationKind.Form;
                break;
            case B4.enums.DebtorType.Legal:
                notifFormationKind = Gkh.config.DebtorClaimWork.Legal.DebtNotification.NotifFormationKind !== B4.enums.DocumentFormationKind.Form;
                pretensionFormationKind = Gkh.config.DebtorClaimWork.Legal.Pretension.PretensionFormationKind !== B4.enums.DocumentFormationKind.Form;
                break;
        }

        btn.menu.removeAll();

        if (btn.actionName === 'CreateDoc')
        {
            Ext.Array.filter(items, function (item) {
                if (item.Type === B4.enums.ClaimWorkDocumentType.Notification && notifFormationKind) { return false; }
                if (item.Type === B4.enums.ClaimWorkDocumentType.Pretension && pretensionFormationKind) { return false; }

                btn.menu.add({
                    xtype: 'menuitem',
                    text: item.Name,
                    textAlign: 'left',
                    typeDoc: item.Type
                });

                return true;
            });
        }
    },
    getDocsTypeToCreate: function () {
        var me = this,
            createDocField = me.getGrid().down('[actionName=CreateDoc]');

        B4.Ajax.request({
            url: B4.Url.action('GetDocsTypeToCreate', me.entityName),
            timeout: 60*60*3
        }).next(function (response) {
            var docs = Ext.JSON.decode(response.responseText);
            createDocField.menuItems = docs;

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