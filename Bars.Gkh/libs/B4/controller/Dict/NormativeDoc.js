Ext.define('B4.controller.dict.NormativeDoc', {
    extend: 'B4.base.Controller',
    saving: false,
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'dict.NormativeDoc',
        'dict.NormativeDocItem'
    ],
    stores: [
        'dict.NormativeDoc',
        'dict.NormativeDocItem'
    ],
    views: [
        'dict.normativedoc.Grid',
        'dict.normativedoc.EditWindow',
        'dict.normativedoc.NormativeDocItemGrid'
    ],

    mainView: 'dict.normativedoc.Grid',
    mainViewSelector: 'normativeDocGrid',
    normativeDocEditWindowSelector: '#normativeDocEditWindow',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'normativeDocGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.NormativeDoc.Field.FullName', applyTo: 'gridcolumn[dataIndex=FullName]', selector: 'normativeDocGrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.NormativeDoc.Field.FullName', applyTo: 'textfield[name=FullName]', selector: 'normativeDocEditWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.allowBlank = false;
                                component.show();
                            } else {
                                component.allowBlank = true;
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.NormativeDoc.Field.Validity', applyTo: 'fieldset[name=Validity]', selector: 'normativeDocEditWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'normativeDocItemGridAspect',
            storeName: 'dict.NormativeDocItem',
            modelName: 'dict.NormativeDocItem',
            gridSelector: 'normativeDocItemGrid',
            saveButtonSelector: '#normativeDocItemGrid #saveItemsDataButton',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.data.NormativeDoc = this.controller.docId;
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
            name: 'normativeDocWindowAspect',
            gridSelector: 'normativeDocGrid',
            editFormSelector: '#normativeDocEditWindow',
            storeName: 'dict.NormativeDoc',
            modelName: 'dict.NormativeDoc',
            editWindowView: 'dict.normativedoc.EditWindow',
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
        this.getStore('dict.NormativeDoc').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.NormativeDocItem').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('normativeDocGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.NormativeDoc').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.docId = this.docId;
    },

    setDocId: function (id) {
        this.docId = id;

        var normativeDocItems = this.getStore('dict.NormativeDocItem');
        normativeDocItems.removeAll();

        var editWindow = Ext.ComponentQuery.query(this.normativeDocEditWindowSelector)[0];

        if (id > 0) {
            editWindow.down('#normativeDocItemGrid').setDisabled(false);
            normativeDocItems.load();
        } else {
            editWindow.down('#normativeDocItemGrid').setDisabled(true);
        }
    }
});