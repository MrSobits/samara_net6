Ext.define('B4.controller.ResolutionRospotrebnadzor', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.ResolutionRospotrebnadzor',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'ResolutionRospotrebnadzor',
        'resolutionrospotrebnadzor.Annex',
        'resolutionrospotrebnadzor.Dispute',
        'resolutionrospotrebnadzor.PayFine',
        'resolutionrospotrebnadzor.Definition',
        'resolutionrospotrebnadzor.ArticleLaw',
        'resolutionrospotrebnadzor.Violation'
    ],

    stores: [
        'ResolutionRospotrebnadzor',
        'resolutionrospotrebnadzor.Annex',
        'resolutionrospotrebnadzor.Definition',
        'resolutionrospotrebnadzor.Dispute',
        'resolutionrospotrebnadzor.PayFine',
        'resolutionrospotrebnadzor.ArticleLaw',
        'resolutionrospotrebnadzor.Violation',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'dict.Inspector',
        'dict.SanctionGji',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected'
    ],

    views: [
        'resolutionrospotrebnadzor.EditPanel',
        'resolutionrospotrebnadzor.AnnexEditWindow',
        'resolutionrospotrebnadzor.AnnexGrid',
        'resolutionrospotrebnadzor.DefinitionEditWindow',
        'resolutionrospotrebnadzor.DefinitionGrid',
        'resolutionrospotrebnadzor.PayFineGrid',
        'resolutionrospotrebnadzor.DisputeEditWindow',
        'resolutionrospotrebnadzor.DisputeGrid',
        'resolutionrospotrebnadzor.ViolationGrid',
        'resolutionrospotrebnadzor.ArticleLawGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    refs: [
        {
            ref: 'ArticleLawGrid',
            selector: '#resolutionRospotrebnadzorArticleLawGrid'
        }
    ],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'resolutionrospotrebnadzor.EditPanel',
    mainViewSelector: '#resolutionRospotrebnadzorEditPanel',

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#resolutionRospotrebnadzorAnnexGrid',
            controllerName: 'ResolutionRospotrebnadzorAnnex',
            name: 'resolutionRospotrebnadzorAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * Апект для основной панели постановления
            */
            xtype: 'gjidocumentaspect',
            name: 'resolutionRospotrebnadzorEditPanelAspect',
            editPanelSelector: '#resolutionRospotrebnadzorEditPanel',
            modelName: 'ResolutionRospotrebnadzor',

            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = this,
                    documentReason = panel.down('#tfDocumentReason');

                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle('Постановление Роспотребнадзора ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Постановление Роспотребнадзора');

                //ставим активной вкладку "реквизиты"
                me.getPanel().down('.tabpanel').setActiveTab(0);

                //Делаем запросы на получение документа основания
                if (!documentReason.getValue()) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ResolutionRospotrebnadzor', {
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        me.controller.unmask();
                        var obj = Ext.JSON.decode(response.responseText);

                        documentReason.setValue(obj.data);
                    }).error(function () {
                        me.controller.unmask();
                    });
                }

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('resolutionRospotrebnadzorStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                me.disableButtons(false);
            },

            onChangeTypeExecutant: function(field, value, oldValue) {

                var data = field.getRecord(value);

                var contragentField = field.up(this.editPanelSelector).down('#sfContragent');

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }

                if (data) {
                    if (this.controller.params) {
                        this.controller.params.typeExecutant = data.Code;
                    }
                    this.setTypeExecutantPermission(data);
                }
            },

            onBeforeLoadContragent: function(field, options, store) {
                var executantField = this.controller.getMainView().down('#cbExecutant');

                var typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

                return true;
            },

            onBeforeLoadOfficial: function(field, options, store) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;

                return true;
            },

            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'resolutionRospotrebnadzorStateButtonAspect',
            stateButtonSelector: '#resolutionRospotrebnadzorEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('resolutionRospotrebnadzorEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            xtype: 'resolutionrospperm',
            editFormAspectName: 'resolutionRospotrebnadzorEditPanelAspect'
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'resolutionRospotrebnadzorAnnexAspect',
            gridSelector: '#resolutionRospotrebnadzorAnnexGrid',
            editFormSelector: '#resolutionRospotrebnadzorAnnexEditWindow',
            storeName: 'resolutionrospotrebnadzor.Annex',
            modelName: 'resolutionrospotrebnadzor.Annex',
            editWindowView: 'resolutionrospotrebnadzor.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'resolutionRospotrebnadzorDefinitionAspect',
            gridSelector: '#resolutionRospotrebnadzorDefinitionGrid',
            editFormSelector: '#resolutionRospotrebnadzorDefinitionEditWindow',
            storeName: 'resolutionrospotrebnadzor.Definition',
            modelName: 'resolutionrospotrebnadzor.Definition',
            editWindowView: 'resolutionrospotrebnadzor.DefinitionEditWindow',
            onSaveSuccess: function(asp, record) {
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function(asp, record, form) {
                    asp.setDefinitionId(record.getId());
                }
            }
        },
        {
            /* Аспект взаимодействия Таблицы оплаты штрафов с формой редактирования */
            xtype: 'gkhinlinegridaspect',
            name: 'resolutionRospotrebnadzorPayFineAspect',
            storeName: 'resolutionrospotrebnadzor.PayFine',
            modelName: 'resolutionrospotrebnadzor.PayFine',
            gridSelector: '#resolutionRospotrebnadzorPayFineGrid',
            saveButtonSelector: '#resolutionRospotrebnadzorPayFineGrid button[name=saveButton]',
            listeners: {
                beforesave: function(asp, store) {
                    store.each(function(record, index) {
                        if (!record.get('Id')) {
                            record.set('Resolution', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /* Аспект взаимодействия Таблицы оспариваний с формой редактирования */
            xtype: 'grideditwindowaspect',
            name: 'resolutionRospotrebnadzorDisputeAspect',
            gridSelector: '#resolutionRospotrebnadzorDisputeGrid',
            editFormSelector: '#resolutionRospotrebnadzorDisputeEditWindow',
            storeName: 'resolutionrospotrebnadzor.Dispute',
            modelName: 'resolutionrospotrebnadzor.Dispute',
            editWindowView: 'resolutionrospotrebnadzor.DisputeEditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #cbDisputeAppeal'] = { 'change': { fn: this.onChangeDisputeAppeal, scope: this } };
            },
            onChangeDisputeAppeal: function(field, newValue, oldValue) {
                var form = this.getForm();
                if (newValue === 40) {
                    form.down('#cbDisputeCourt').setDisabled(false);
                    form.down('#cbDisputeInstance').setDisabled(false);
                } else {
                    form.down('#cbDisputeCourt').setDisabled(true);
                    form.down('#cbDisputeInstance').setDisabled(true);
                }
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /* Аспект взаимодействия таблицы Статьи закона */
            xtype: 'gkhinlinegridaspect',
            name: 'resolutionRospotrebnadzorArticleLawGridAspect',
            storeName: 'resolutionrospotrebnadzor.ArticleLaw',
            modelName: 'resolutionrospotrebnadzor.ArticleLaw',
            gridSelector: '#resolutionRospotrebnadzorArticleLawGrid',
            saveButtonSelector: '#resolutionRospotrebnadzorArticleLawGrid button[name=saveButton]',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (record, index) {
                        if (!record.get('Resolution')) {
                            record.set('Resolution', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'resolutionRospotrebnadzorArticleLawEditWindowAspect',
            buttonSelector: '#resolutionRospotrebnadzorArticleLawGrid b4addbutton',
            multiSelectWindowSelector: '#resolutionRospotrebnadzorArticleLawMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи закона для отбора',
            titleGridSelected: 'Выбранные статьи закона',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        articleLawGrid = asp.controller.getArticleLawGrid();

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    B4.Ajax.request(B4.Url.action('AddArticleLawList', 'ResolutionRospotrebnadzor', {
                        articleLawIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        articleLawGrid.getStore().load();
                        return true;
                    });
                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'resolutionRospotrebnadzorViolationAspect',
            storeName: 'resolutionrospotrebnadzor.Violation',
            modelName: 'resolutionrospotrebnadzor.Violation',
            gridSelector: '#resolutionRospotrebnadzorViolationGrid',
            saveButtonSelector: '#resolutionRospotrebnadzorViolationGrid button[name=saveButton]',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (record, index) {
                        if (!record.get('Resolution')) {
                            record.set('Resolution', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
    ],
    init: function () {
        var me = this;

        me.getStore('resolutionrospotrebnadzor.PayFine').on('beforeload', me.onBeforeLoad, me);
        me.getStore('resolutionrospotrebnadzor.Dispute').on('beforeload', me.onBeforeLoad, me);
        me.getStore('resolutionrospotrebnadzor.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('resolutionrospotrebnadzor.Definition').on('beforeload', me.onBeforeLoad, me);
        me.getStore('resolutionrospotrebnadzor.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('resolutionrospotrebnadzor.Violation').on('beforeload', me.onBeforeLoad, me);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('resolutionRospotrebnadzorEditPanelAspect').setData(me.params.documentId);

            // Обновляем стор оплат штрафов
            me.getStore('resolutionrospotrebnadzor.PayFine').load();

            // Обновляем стор приложений
            me.getStore('resolutionrospotrebnadzor.Annex').load();

            // Обновляем стор оспариваний
            me.getStore('resolutionrospotrebnadzor.Dispute').load();

            // Обновляем стор определений
            me.getStore('resolutionrospotrebnadzor.Definition').load();

            // Обновляем стор статей
            me.getStore('resolutionrospotrebnadzor.ArticleLaw').load();

            // Обновляем стор нарушений
            me.getStore('resolutionrospotrebnadzor.Violation').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});