Ext.define('B4.controller.activitytsj.Statute', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActivityTsjStatute',
        'B4.aspects.StateContextMenu'
    ],

    models: ['activitytsj.Statute'],
    
    stores: [
        'activitytsj.Statute',
        'activitytsj.Article'
    ],
    
    views: [
        'activitytsj.StatuteGrid',
        'activitytsj.StatuteEditWindow'
    ],

    aspects: [
        {
            xtype: 'activitytsjstatutestateperm',
            editFormAspectName: 'activityTsjStatuteGridEditWindow',
            setPermissionEvent: 'aftersetformdata',
            name: 'activityTsjStatuteStatePerm'
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'activityTsjStatuteStateTransferAspect',
            gridSelector: '#activityTsjStatuteGrid',
            stateType: 'gji_activity_tsj_statute',
            menuSelector: 'activityTsjStatuteGridStateMenu'
        },
        {
            /* Вешаем аспект смены статуса в карточке редактирования */
            xtype: 'statebuttonaspect',
            name: 'activityTsjStatuteStateButtonAspect',
            stateButtonSelector: '#activityTsjStatuteEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('activityTsjStatuteGridEditWindow');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('activitytsj.Statute');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('activityTsjStatuteStatePerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('activityTsjStatuteStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'activityTsjStatuteGridEditWindow',
            storeName: 'activitytsj.Statute',
            modelName: 'activitytsj.Statute',
            gridSelector: '#activityTsjStatuteGrid',
            editFormSelector: '#activityTsjStatuteEditWindow',
            editWindowView: 'activitytsj.StatuteEditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.get('Id'));
            },
            listeners: {
                getdata: function(asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('ActivityTsj', this.controller.params.get('Id'));
                    }
                },
                aftersetformdata: function(asp, record, form) {
                    asp.controller.setCurrentId(record.get('Id'));
                    //проставляем статус
                    asp.controller.getAspect('activityTsjStatuteStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'activityTsjArticleGridAspect',
            storeName: 'activitytsj.Article',
            modelName: 'activitytsj.Article',
            gridSelector: '#activityTsjArticleGrid',
            saveButtonSelector: '#activityTsjArticleGrid #buttonSave',
            //Переопределяем метод сохранения
            save: function() {
                var me = this,
                    data,
                    i,
                    store = me.controller.getStore(me.storeName);
                if (store) {
                    var modifiedRecords = store.getModifiedRecords();
                    if (modifiedRecords.length > 0) {
                        //отправляем измененные записи на сервер для обработки
                        var modifiedRecordsData = [];
                        for (i = 0; i < modifiedRecords.length; i++) {
                            data = modifiedRecords[i].getData();
                            if (data.ArticleTsjId == 0) {
                                data.ArticleTsjId = data.Id;
                                data.Id = 0;
                            }
                            modifiedRecordsData.push(
                                {
                                    Id: data.Id || 0,
                                    ArticleTsjId: data.ArticleTsjId || 0,
                                    IsNone: data.IsNone,
                                    TypeState: data.TypeState,
                                    Paragraph: data.Paragraph,
                                    Name: data.Name,
                                    Code: data.Code,
                                    Group: data.Group
                                });
                        }
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('SaveParams', 'ActivityTsjArticle'),
                            method: 'POST',
                            params: {
                                statuteId: me.controller.statuteId,
                                modifiedRecordsJson: Ext.JSON.encode(modifiedRecordsData)
                            }
                        }).
                        next(function() {
                            me.controller.unmask();
                            store.load();
                            Ext.Msg.alert('Сохранение!', 'Сохранение результатов прошло успешно');
                            return true;
                        }, me).error(function () {
                            this.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Не удалось сохранить');
                        }, me);
                    }
                }
            }
        }
    ],

    mainView: 'activitytsj.StatuteGrid',
    mainViewSelector: '#activityTsjStatuteGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    activityTsjStatuteEditWindowSelector: '#activityTsjStatuteEditWindow',

    init: function() {
        this.getStore('activitytsj.Statute').on('beforeload', this.onBeforeLoad, this);
        this.getStore('activitytsj.Article').on('beforeload', this.onBeforeLoadStatute, this);

        this.callParent(arguments);
    },

    onBeforeLoadStatute: function(store, operation) {
        operation.params.statuteId = this.statuteId;

    },

    onLaunch: function() {
        this.getStore('activitytsj.Statute').load();
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.activityTSJ = this.params.get('Id');
        }
    },

    setCurrentId: function(id) {
        this.statuteId = id;

        var editWindow = Ext.ComponentQuery.query(this.activityTsjStatuteEditWindowSelector)[0];
        editWindow.down('.tabpanel').setActiveTab(0);

        var storeArticle = this.getStore('activitytsj.Article');
        storeArticle.removeAll();

        if (id > 0) {
            editWindow.down('#activityTsjArticleGrid').setDisabled(false);
            storeArticle.load();
        } else {
            editWindow.down('#activityTsjArticleGrid').setDisabled(true);
        }
    }
});