Ext.define('B4.controller.dict.MandatoryReqs', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',        
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.SelectWindow',
        'B4.store.dict.ControlListTypicalQuestion',
        'B4.model.dict.ControlListTypicalQuestion',
        'B4.view.dict.mandatoryreqs.QuestionNpdGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.MandatoryReqs',
        'dict.MandatoryReqsNormativeDoc',
        'dict.ControlListTypicalQuestion'
    ],

    stores: [
        'dict.MandatoryReqs',
        'dict.MandatoryReqsNormativeDoc',
        'dict.ControlListTypicalQuestion'
    ],

    views: [
        'dict.mandatoryreqs.Grid',
        'dict.mandatoryreqs.EditWindow',
        'dict.mandatoryreqs.NormativeDocGrid',
        'dict.mandatoryreqs.QuestionNpdGrid'
    ],

    mainView: 'dict.mandatoryreqs.Grid',
    mainViewSelector: 'mandatoryreqsgrid',

    aspects: [

        //аспект окна редактирования обязательных требований

        {
            xtype: 'grideditwindowaspect',
            name: 'mandatoryReqsGridAspect',
            gridSelector: 'mandatoryreqsgrid',
            editFormSelector: 'mandatoryreqseditwindow',
            storeName: 'dict.MandatoryReqs',
            modelName: 'dict.MandatoryReqs',
            editWindowView: 'dict.mandatoryreqs.EditWindow', 

            listeners: {
                aftersetformdata: function (asp, record) {
                    this.getDataToView(record.getId());
                }
            },

            getDataToView: function (id) {
                var me = this,
                    form = me.getForm(me.editFormSelector),
                    normativeDocGrid = form.down('mandatoryreqsnormativedocgrid'),
                    typicaQuestionsGrid = form.down('mandatoryreqsquestionnpdgrid'),
                    normativeDocStore = normativeDocGrid.getStore(),
                    typicalQuestionsStore = typicaQuestionsGrid.getStore();
                
                me.controller.setContextValue(me.controller.getMainView(), 'mandatoryReqId', id);

                typicalQuestionsStore.removeAll();
                normativeDocStore.removeAll();

                normativeDocGrid.setDisabled(id === 0);
                typicaQuestionsGrid.setDisabled(id === 0);

                if (id > 0) {
                    normativeDocStore.on('beforeload', me.onBeforeLoad, me);
                    typicalQuestionsStore.on('beforeload', me.onBeforeLoad, me);

                    normativeDocStore.load();
                    typicalQuestionsStore.load();
                }
            },

            onBeforeLoad: function (store, operation) {
                operation.params.mandatoryReqId = this.controller.getContextValue(this.controller.getMainView(), 'mandatoryReqId');
            },
        },

        //аспект грида редактирования нормативных документов

        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'mandatoryreqsnormativedocgrid',
            storeName: 'dict.MandatoryReqsNormativeDoc',
            modelName: 'dict.MandatoryReqsNormativeDoc',
            saveButtonSelector: '[name=normativeDocSaveButton]',

            save: function () {
                var me = this,
                    store = me.getStore(),
                    modifRecords = store.getModifiedRecords(),
                    removedRecords = store.getRemovedRecords(),
                    addIds = [],
                    updateData = [],
                    deleteIds = [];

                if (modifRecords.length > 0 || removedRecords.length > 0) {
                    if (me.fireEvent('beforesave', me, store) !== false) {
                        me.controller.mask('Сохранение', me.controller.getMainView());

                        modifRecords.forEach(function (rec) {
                            if (rec.data.Npa) {
                                if (rec.data.Id) {
                                    updateData.push(rec.data);
                                    return true;
                                }
                                addIds.push(rec.data.Npa.Id);
                            }
                        });

                        removedRecords.forEach(function (rec) {
                            deleteIds.push(rec.data.Id);
                        });

                        B4.Ajax.request({
                            url: B4.Url.action('AddUpdateDeleteNpa', 'MandatoryReqsNormativeDoc'),
                            timeout: 9999999,
                            params: {
                                addIds: Ext.encode(addIds),
                                updateData: Ext.encode(updateData),
                                deleteIds: Ext.encode(deleteIds),
                                mandatoryReqId: me.controller.getContextValue(me.controller.getMainView(), 'mandatoryReqId')
                            }
                        }).next(function (e) {
                            store.load();
                            me.controller.unmask();
                            if (e.message) {
                                Ext.Msg.alert('Сохранение', 'Выполнено с ошибками:<br>' + e.message);
                            }
                        }).error(function (e) {
                            me.controller.unmask();
                            Ext.Msg.alert('Ошибка', 'Ошибки при сохранении нормативно-правовых документов:<br>' + e.message);
                        });
                    }
                }
            }
        },

        //аспект грида редактирования типовых вопросов
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'mandatoryreqsquestionnpdgrid',
            storeName: 'dict.ControlListTypicalQuestion',
            modelName: 'dict.ControlListTypicalQuestion',
            saveButtonSelector: '[name=questionsSaveButton]',

            save: function() {
                var me = this,
                    store = me.getStore(),
                    modifRecords = store.getModifiedRecords(),
                    removedRecords = store.getRemovedRecords(),
                    addIds = [],
                    deleteIds = [];

                if (modifRecords.length > 0 || removedRecords.length > 0) {
                    if (me.fireEvent('beforesave', me, store) !== false) {
                        me.controller.mask('Сохранение', me.controller.getMainView());

                        modifRecords.forEach(function (rec) {
                            if (rec.data.Question) {
                                addIds.push(rec.data.Question.Id);
                            }
                        });

                        removedRecords.forEach(function (rec) {
                            deleteIds.push(rec.data.Id);
                        });

                        B4.Ajax.request({
                            url: B4.Url.action('UpdateControlListTypicalQuestion', 'ControlListTypicalQuestion'),
                            timeout: 9999999,
                            params: {
                                addIds: Ext.encode(addIds),
                                deleteIds: Ext.encode(deleteIds),
                                mandatoryReqId: me.controller.getContextValue(me.controller.getMainView(), 'mandatoryReqId')
                            }

                        }).next(function (response) {
                            var result = Ext.JSON.decode(response.responseText);
                            me.controller.unmask();
                            if (result.success) {
                                store.load();
                                return;
                            }
                            Ext.Msg.alert('Ошибка', 'Ошибка при сохранении вопросов проверочного листа');
                        }).error(function () {
                            me.controller.unmask();
                            Ext.Msg.alert('Ошибка', 'Ошибка при сохранении вопросов проверочного листа');
                        });
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'mandatoryreqsgrid [name=sendToTorButton]': { 'click': { fn: me.onClickSendToTor }},
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget(this.mainViewSelector);
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.MandatoryReqs').load();
    },

    onClickSendToTor: function () {
        var me = this,
            view = me.getMainView(),
            store = view.getStore(),
            Ids = [];

        Ext.each(store.data.items, function (rec) {
            Ids.push(rec.data.Id);
        });

        me.mask('Отправление в ТОР КНД', me.getMainComponent());

        B4.Ajax.request({
            url: B4.Url.action('SendMandatoryReqsToTor', 'TorIntegration'),
            timeout: 9999999,
            params: {
                ids: Ext.encode(Ids),
            }
        }).next(function (response) {
            me.unmask();
            Ext.Msg.alert('Отправка в ТОР КНД', response.message || 'Выполнено успешно');
            return true;
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
        });
    },
});