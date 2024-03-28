Ext.define('B4.controller.objectcr.Qualification', {
    /*
    * Контроллер раздела квалификационный отбор
    */
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.objectcr.Qualification',
        'B4.Ajax',
        'B4.Url',
        
        'B4.enums.TypeAcceptQualification'
    ],

    models:[
        'Builder',
        'objectcr.Qualification',
        'objectcr.qualification.VoiceMember'
    ],
    
    stores: [
        'objectcr.Qualification',
        'objectcr.qualification.TypeWorkCr',
        'objectcr.qualification.VoiceMember'
    ],
    
    views: [
        'objectcr.qualification.EditWindow',
        'objectcr.qualification.Panel',
        'objectcr.qualification.VoiceMemberGrid',
        'objectcr.qualification.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context',
      //  controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    editWindowSelector: 'qualificationeditwindow',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    mainView: 'objectcr.qualification.Panel',
    mainViewSelector: 'qualificationpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'qualificationpanel'
        },
        {
            ref: 'qualGrid',
            selector: 'qualgrid'
        },
        {
            ref: 'editWindow',
            selector: 'qualificationeditwindow'
        }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования квалификационного отбора
            */
            xtype: 'grideditctxwindowaspect',
            name: 'defectListGridWindowAspect',
            gridSelector: 'qualgrid',
            editFormSelector: 'qualificationeditwindow',
            modelName: 'objectcr.Qualification',
            editWindowView: 'objectcr.qualification.EditWindow',
            //перекрываем стандартный метод аспекта что бы не закрывать форму при сохранение а при добавление еще и сделать доступным грид для работы
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            otherActions: function(actions) {
                actions['qualificationpanel qualgrid'] = {
                    'beforerender': { fn: this.onBeforeRenderQualification, scope: this }
                };
                actions['qualgrid #qualToBuilderColumn'] = {
                    'click': { fn: this.onClickActionColumn, scope: this }
                };
                actions['qualificationeditwindow b4closebutton'] = {
                    'click': { fn: this.updateGrid, scope: this }
                };
            },
            onClickActionColumn: function(gridView, rowIndex, colIndex, el, e, rec) {
                var me = this,
                    portal,
                    builder,
                    model;
                if (rec != undefined) {
                    var buildId = rec.get('Builder').Id;

                    if (buildId) {
                        model = me.controller.getModel('Builder');
                        model.load(buildId, {
                            success: function(record) {
                                builder = record;
                            },
                            scope: this
                        });

                        var params = new model({ Id: buildId, ContragentName: rec.get('BuilderName') });
                        params.childController = 'B4.controller.builder.Edit';
                        params.record = builder;
                        portal = me.controller.getController('PortalController');

                        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                        if (!me.controller.hideMask) {
                            me.controller.hideMask = function() { me.controller.unmask(); };
                        }

                        me.controller.mask('Загрузка', me.controller.getMainComponent());
                        portal.loadController('B4.controller.builder.Navigation', params, portal.containerSelector, me.controller.hideMask);
                    }
                }
            },
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.getId()) {
                        record.set('ObjectCr', me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId'));
                    }
                },
                //после вставки данных запоминаем параметры и в зависимости от того редактирование или создание делаем доступным грид
                aftersetformdata: function(asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            },
            onBeforeRenderQualification: function(grid) {
                var me = this,
                     column;

                me.controller.mask();
                B4.Ajax.request(B4.Url.action('GetActiveColumns', 'Qualification', {
                    objectCrId: me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId')
                })).next(function(response) {
                    var objs = Ext.JSON.decode(response.responseText);
                    Ext.Array.each(objs, function(rec) {
                        column = Ext.create('Ext.grid.column.Column', {
                            text: 'Принято ' + rec,
                            flex: 1,
                            dataIndex: 'Dict',
                            sortable: false,
                            renderer: function(val) {
                                if (val) {
                                    var res = val[rec];
                                    return B4.enums.TypeAcceptQualification.displayRenderer(res ? res : 10);
                                }
                                return B4.enums.TypeAcceptQualification.displayRenderer(10);
                            }
                        });

                        grid.headerCt.add(grid.columns.length, column);
                        grid.getView().refresh();
                    });
                    column = Ext.create('B4.ux.grid.column.Delete', { scope: grid });
                    grid.headerCt.add(grid.columns.length, column);
                    grid.getView().refresh();
                    me.controller.unmask();
                }, me).error(function () {
                    me.controller.unmask();
                }, me);
            }
        },
        {
            /*
            * Аспект инлайн редактирования таблицы голосов квалиф отбора
            */
            xtype: 'gkhinlinegridaspect',
            name: 'voiceMemberGkhInlineGridAspect',
            gridSelector: 'qualvoicemembergrid',
            storeName: 'objectcr.qualification.VoiceMember',
            modelName: 'objectcr.qualification.VoiceMember',
            saveButtonSelector: 'qualvoicemembergrid [actionName=voiceQualificationMemberSaveButton]',
            save: function () {
                var me = this,
                    editWindow = me.controller.getEditWindow(),
                    record = editWindow.getForm().getRecord(),
                    grid = me.getGrid(),
                    store = grid.getStore(),
                    modifyRecords = store.getModifiedRecords(),
                    removedRecords = store.getRemovedRecords(),
                    data = [],
                    removedIds = [];

                Ext.each(modifyRecords, function (rec) {
                    data.push(rec.data);
                });

                Ext.each(removedRecords, function (rec) {
                    removedIds.push(rec.get('Id'));
                });

                if (data.length > 0 || removedIds.length > 0) {
                    me.mask('Сохранение', me.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('SaveVoiceMembers', 'VoiceMember', {
                        records: Ext.encode(data),
                        removed: Ext.encode(removedIds),
                        qualificationId: record.get('Id'),
                    })).next(function () {
                        me.unmask();
                        store.load();
                        Ext.Msg.alert('Сохранение', 'Успешно сохранено');
                        return true;
                    }).error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка', e.message);
                    });
                } 
            }
        },
        {
            /*
            * Аспект c правами для квалификационного отбора
            */
            xtype: 'qualificationobjectcrperm',
            name: 'qualificationObjectCrPerm',
            gridSelector: 'qualgrid',
            editFormSelector: 'qualificationeditwindow',
            modelName: 'objectcr.Qualification',
            editWindowView: 'objectcr.qualification.EditWindow'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('qualificationpanel'),
            store,
            storeTypeWorkCr;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.down('qualgrid').getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);

        storeTypeWorkCr = view.down('qualtypeworkgrid').getStore();
        storeTypeWorkCr.clearFilter(true);
        storeTypeWorkCr.filter([
            { property: 'objectCrId', value: id }
        ]);
    },

    setCurrentId: function(id) {
        var me = this,
            editWindow = me.getEditWindow(),
            gridVoice = editWindow.down('qualvoicemembergrid'),
            storeVoice = gridVoice.getStore();

        storeVoice.removeAll();

        gridVoice.setDisabled(!id);

        var store = this.getStore('objectcr.qualification.VoiceMember');
        store.removeAll();
        editWindow.down('qualvoicemembergrid').setDisabled(!id);
        if (id) {
            storeVoice.clearFilter(true);
            storeVoice.filter([
                { property: 'qualificationId', value: id },
                { property: 'objectCrId', value: me.getContextValue(me.getMainComponent(), 'objectcrId') }
            ]);
        }
    }
});