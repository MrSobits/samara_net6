Ext.define('B4.controller.dict.OwnerProtocolType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.OwnerProtocolType',
        'dict.OwnerProtocolTypeDecision'
    ],
    stores: ['dict.OwnerProtocolType',
        'dict.OwnerProtocolTypeDecision'
    ],
    views: [
        'dict.ownerprotocoltype.EditWindow',
        'dict.ownerprotocoltype.Grid',
        'dict.ownerprotocoltype.OwnerProtocolTypeDecisionGrid',
    ],
    mainView: 'dict.ownerprotocoltype.Grid',
    mainViewSelector: 'ownerProtocolTypeGrid',
    ownerProtocolTypeEditWindowSelector: '#ownerProtocolTypeEditWindow',
    refs: [
        {
            ref: 'mainView',
            selector: 'ownerProtocolTypeGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'ownerProtocolTypeDecisionGridAspect',
            storeName: 'dict.OwnerProtocolTypeDecision',
            modelName: 'dict.OwnerProtocolTypeDecision',
            gridSelector: 'ownerProtocolTypeDecisionGrid',
            saveButtonSelector: '#ownerProtocolTypeDecisionGrid #saveItemsDataButton',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.data.OwnerProtocolType = this.controller.docId;
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
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'ownerProtocolTypeGridAspect',
            gridSelector: 'ownerProtocolTypeGrid',
            editFormSelector: '#ownerProtocolTypeEditWindow',
            storeName: 'dict.OwnerProtocolType',
            modelName: 'dict.OwnerProtocolType',
            editWindowView: 'dict.ownerprotocoltype.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setDocId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setDocId(record.getId());
                }
            }
        }
    ],

    init: function () {
        this.getStore('dict.OwnerProtocolType').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.OwnerProtocolTypeDecision').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('ownerProtocolTypeGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.OwnerProtocolType').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.docId = this.docId;
    },

    setDocId: function (id) {
        this.docId = id;

        var ownerProtocolTypeDecision = this.getStore('dict.OwnerProtocolTypeDecision');
        ownerProtocolTypeDecision.removeAll();

        var editWindow = Ext.ComponentQuery.query(this.ownerProtocolTypeEditWindowSelector)[0];

        if (id > 0) {
            editWindow.down('#ownerProtocolTypeDecisionGrid').setDisabled(false);
            ownerProtocolTypeDecision.load();
        } else {
            editWindow.down('#ownerProtocolTypeDecisionGrid').setDisabled(true);
        }
    }
});