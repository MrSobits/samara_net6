Ext.define('B4.controller.dict.SMEVComplaintsDecision', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    decId: null,
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['complaints.Decision',
        'complaints.DecisionLS'],
    stores: ['complaints.Decision',
        'complaints.DecisionLS'],

    views: ['complaints.DecisionDictGrid',
        'complaints.DecisionLSGrid',
        'complaints.DictEditWindow'],

    mainView: 'complaints.DecisionDictGrid',
    mainViewSelector: 'complaintsdecdictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'complaintsdecdictgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsdecdictGridAspect',
            gridSelector: 'complaintsdecdictgrid',
            editFormSelector: '#complaintsdecEditWindow',
            storeName: 'complaints.Decision',
            modelName: 'complaints.Decision',
            editWindowView: 'complaints.DictEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    if (record.getId()) {
                        asp.controller.decId = record.getId();
                        var grid = form.down('complaintsdeclsgrid'),
                            store = grid.getStore();
                        store.filter('decId', record.getId());
                        grid.setDisabled(false);
                    }
                    else {
                        var grid = form.down('complaintsdeclsgrid'),
                            store = grid.getStore();
                        store.clearData();
                        grid.setDisabled(true);
                    }

                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'complainsdictDecisionLSGridAspect',
            storeName: 'complaints.DecisionLS',
            modelName: 'complaints.DecisionLS',
            gridSelector: 'complaintsdeclsgrid',
            saveButtonSelector: 'complaintsdeclsgrid #saveItemsDataButton',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.data.SMEVComplaintsDecision = this.controller.decId;
                }
            },
            save: function () {
                if (this.controller.saving) {
                    this.controller.saving = false;
                    return;
                }

                this.controller.saving = true;
                var me = this;
                var store = this.getStore();

                var modifiedRecs = store.getModifiedRecords();
                var removedRecs = store.getRemovedRecords();
                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (this.fireEvent('beforesave', this, store) !== false) {
                        me.mask('Сохранение', this.getGrid());
                        store.sync({
                            callback: function () {
                                me.unmask();
                                store.load();
                            },
                            // выводим сообщение при ошибке сохранения
                            failure: function (result) {
                                me.unmask();
                                if (result && result.exceptions[0] && result.exceptions[0].response) {
                                    Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                                }
                            }
                        });
                    }
                }
            }
        }
        //{
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'complaintsdecdictGridAspect',
        //    storeName: 'complaints.Decision',
        //    modelName: 'complaints.Decision',
        //    gridSelector: 'complaintsdecdictgrid'
        //}
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('complaintsdecdictgrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('complaints.Decision').load();
    }
});