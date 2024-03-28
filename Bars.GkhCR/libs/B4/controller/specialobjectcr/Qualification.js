Ext.define('B4.controller.specialobjectcr.Qualification', {
    /*
    * Контроллер раздела квалификационный отбор
    */
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.specialobjectcr.Qualification',
        'B4.Ajax',
        'B4.Url',
        
        'B4.enums.TypeAcceptQualification'
    ],

    models:[
        'Builder',
        'specialobjectcr.Qualification',
        'specialobjectcr.qualification.VoiceMember'
    ],
    
    stores: [
        'specialobjectcr.Qualification',
        'specialobjectcr.qualification.TypeWorkCr',
        'specialobjectcr.qualification.VoiceMember'
    ],
    
    views: [
        'specialobjectcr.qualification.EditWindow',
        'specialobjectcr.qualification.Panel',
        'specialobjectcr.qualification.VoiceMemberGrid',
        'specialobjectcr.qualification.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    editWindowSelector: 'specialobjectcrqualeditwindow',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    mainView: 'specialobjectcr.qualification.Panel',
    mainViewSelector: 'specialobjectcrqualificationpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'specialobjectcrqualificationpanel'
        },
        {
            ref: 'qualGrid',
            selector: 'specialobjectcrqualgrid'
        },
        {
            ref: 'editWindow',
            selector: 'specialobjectcrqualeditwindow'
        }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования квалификационного отбора
            */
            xtype: 'grideditctxwindowaspect',
            name: 'defectListGridWindowAspect',
            gridSelector: 'specialobjectcrqualgrid',
            editFormSelector: 'specialobjectcrqualeditwindow',
            modelName: 'specialobjectcr.Qualification',
            editWindowView: 'specialobjectcr.qualification.EditWindow',
            //перекрываем стандартный метод аспекта что бы не закрывать форму при сохранение а при добавление еще и сделать доступным грид для работы
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            otherActions: function (actions) {
                var me = this;

                actions['specialobjectcrqualificationpanel specialobjectcrqualgrid'] = {
                    'beforerender': { fn: me.onBeforeRenderQualification, scope: this }
                };
                actions[me.gridSelector + ' #qualToBuilderColumn'] = {
                    'click': { fn: me.onClickActionColumn, scope: this }
                };
                actions[me.editFormSelector + ' b4closebutton'] = {
                    'click': { fn: me.updateGrid, scope: this }
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
                B4.Ajax.request(B4.Url.action('GetActiveColumns', 'SpecialQualification', {
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
            gridSelector: 'specialobjectcrqualvoicemembergrid',
            storeName: 'specialobjectcr.qualification.VoiceMember',
            modelName: 'specialobjectcr.qualification.VoiceMember',
            saveButtonSelector: 'specialobjectcrqualvoicemembergrid [actionName=voiceQualificationMemberSaveButton]',
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
                    B4.Ajax.request(B4.Url.action('SaveVoiceMembers', 'SpecialVoiceMember', {
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
            xtype: 'qualificationspecialobjectcrperm',
            name: 'qualificationObjectCrPerm',
            gridSelector: 'specialobjectcrqualgrid',
            editFormSelector: 'specialobjectcrqualeditwindow',
            modelName: 'specialobjectcr.Qualification',
            editWindowView: 'specialobjectcr.qualification.EditWindow'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrqualificationpanel'),
            store,
            storeTypeWorkCr;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        store = view.down('specialobjectcrqualgrid').getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);

        storeTypeWorkCr = view.down('specialobjectcrqualtypeworkgrid').getStore();
        storeTypeWorkCr.clearFilter(true);
        storeTypeWorkCr.filter([
            { property: 'objectCrId', value: id }
        ]);
    },

    setCurrentId: function(id) {
        var me = this,
            editWindow = me.getEditWindow(),
            gridVoice = editWindow.down('specialobjectcrqualvoicemembergrid'),
            storeVoice = gridVoice.getStore();

        storeVoice.removeAll();

        gridVoice.setDisabled(!id);

        var store = this.getStore('specialobjectcr.qualification.VoiceMember');
        store.removeAll();
        editWindow.down('specialobjectcrqualvoicemembergrid').setDisabled(!id);
        if (id) {
            storeVoice.clearFilter(true);
            storeVoice.filter([
                { property: 'qualificationId', value: id },
                { property: 'objectCrId', value: me.getContextValue(me.getMainComponent(), 'objectcrId') }
            ]);
        }
    }
});