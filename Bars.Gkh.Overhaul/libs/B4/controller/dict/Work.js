Ext.define('B4.controller.dict.Work', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context:'B4.mixins.Context'
    },

    models: ['dict.Work', 'dict.WorkTypeFinSource', 'dict.AdditWork'],
    stores: ['dict.Work', 'dict.WorkTypeFinSource', 'dict.AdditWork'],
    views: [
        'dict.work.TypeFinSourceGrid',
        'dict.work.EditWindow',
        'dict.work.Grid',
        'dict.additwork.Grid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Dictionaries.Work.Create', applyTo: 'b4addbutton', selector: '#workGrid' },
                { name: 'Gkh.Dictionaries.Work.Edit', applyTo: 'b4savebutton', selector: '#workEditWindow' },
                {
                    name: 'Gkh.Dictionaries.Work.Delete', applyTo: 'b4deletecolumn', selector: '#workGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'workGridWindowAspect',
            gridSelector: 'workGrid',
            editFormSelector: '#workEditWindow',
            storeName: 'dict.Work',
            modelName: 'dict.Work',
            editWindowView: 'dict.work.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var frm = asp.getForm();
                    asp.controller.setCurrentId(record.getId());
                }
            },
            saveRecordHasNotUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                me.controller.saveWorkWithFinSources(me, frm, rec);
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'additWorkGridAspect',
            storeName: 'dict.AdditWork',
            modelName: 'dict.AdditWork',
            gridSelector: 'additWorkGrid',
            saveButtonSelector: '#workEditWindow #additWorkSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    Ext.each(store.data.items, function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('Work', asp.controller.workId);
                        }
                    });

                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        this.getStore('dict.WorkTypeFinSource').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.AdditWork').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    refs: [
        {
            ref: 'EditWindow',
            selector: '#workEditWindow'
        },
        {
            ref: 'mainView',
            selector: 'workGrid'
        }
    ],

    mainView: 'dict.work.Grid',
    mainViewSelector: 'workGrid',

    index: function () {
        var view = this.getMainView() || Ext.widget('workGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Work').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params['workId'] = this.workId;
    },

    setCurrentId: function (id) {
        this.workId = id;

        var store = this.getStore('dict.WorkTypeFinSource');
        store.removeAll();
        var store2 = this.getStore('dict.AdditWork');
        store2.removeAll();

        //this.getEditWindow().down('#worktypefinsourcegrid').setDisabled(!id);

        if (id) {
            store.load();
            store2.load();
        }
    },

    saveWorkWithFinSources: function (asp, frm, rec) {
        var recId = rec.getId(),
            fSourcesCb = Ext.ComponentQuery.query('finsourcechecker checkbox'),
            finSources = [];
            //finSourcesFromForm = frm.getValues()['FinSource'],
            //finSources = finSourcesFromForm ? Ext.Array.filter(finSourcesFromForm, function (item) { return !Ext.isEmpty(item); }) : [];

        Ext.each(fSourcesCb, function(item) {
            if (item.getValue()) {
                finSources.push(item.recValue);
            }
        });
        
        B4.Ajax.request({
            url: B4.Url.action(recId ? 'Update' : 'Create', 'OvrhlWork'),
            params: {
                records: Ext.encode([rec.data]),
                FinSources: Ext.encode(finSources)
            }
        }).next(function (responseObj) {
            if (!recId) { //Режим создания, Id должен быть возвращен
                var response = Ext.JSON.decode(responseObj.responseText);
                rec.setId(response.data);
            }

            B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
            asp.unmask();
            return true;
        }).error(function () {
            B4.QuickMsg.msg('Сохранение записи!', 'Произошла ошибка', 'error');
            asp.unmask();
            return true;
        });
    }
});