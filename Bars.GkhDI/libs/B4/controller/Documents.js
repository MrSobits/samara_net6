Ext.define('B4.controller.Documents', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.documents.State'
    ],

    models: ['Documents'],

    views: [
        'documents.EditPanel',
        'documents.CopyWindow'
    ],

    mainView: 'documents.EditPanel',
    mainViewSelector: '#documentsEditPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'documentsstateperm',
            name: 'documentsPermissionAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'documentsEditPanelAspect',
            modelName: 'Documents',
            editPanelSelector: '#documentsEditPanel',
            copyWindowSelector: '#documentsCopyWindow',
            copyWindowView: 'documents.CopyWindow',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #copyDocsButton'] = { 'click': { fn: this.openCopyWindow, scope: this} };
                actions[this.copyWindowSelector + ' b4closebutton'] = { 'click': { fn: this.closeCopyWindow, scope: this} };
                actions[this.copyWindowSelector + ' b4savebutton'] = { 'click': { fn: this.copyDocs, scope: this } };
                actions[this.editPanelSelector + ' b4filefield'] = { 'fileclear': { fn: this.onClearFileTrigger, scope: this } };
            },

            listeners: {
                beforesave: function (asp, rec) {
                        rec.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                },
                beforesetpaneldata: function (asp, rec, panel) {

                    var typeManagement = asp.controller.params.TypeManagement;
                    var fsProjectContract = panel.down('#fsProjectContract');
                    if (typeManagement == 40) {
                        fsProjectContract.hide();
                    } else {
                        fsProjectContract.show();
                    }
                },
                
                savesuccess: function(asp, rec) {
                    asp.setPanelData(rec);
                }
            },

            //Перед методом setdata получаем id Documents по disclosureInfoId
            setDataDocuments: function (disclosureInfoId) {

                var asp = this;

                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetIdByDisnfoId', 'Documents', {
                    disclosureInfoId: disclosureInfoId
                })).next(function (response) {
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    //Запоминаеем название периода, ук для формы копирования
                    var disclosureInfo = obj.disclosureInfo;
                    if (!Ext.isEmpty(disclosureInfo)) {
                        asp.controller.params.ContragentName = disclosureInfo.ContragentName;
                        asp.controller.params.PeriodDiName = disclosureInfo.PeriodDiName;
                        asp.controller.params.PeriodDi = disclosureInfo.PeriodDi;
                        asp.controller.params.TypeManagement = disclosureInfo.TypeManagement;
                    }

                    asp.controller.params.Id = obj.Id;
                    asp.setData(asp.controller.params.Id);
                    asp.controller.unmask();
                }).error(function () {
                    asp.setData(0);
                    asp.controller.unmask();
                });
            },

            getCopyWindow: function () {

                var copyWindow = Ext.ComponentQuery.query(this.copyWindowSelector)[0];

                if (!copyWindow) {
                    copyWindow = this.controller.getView(this.copyWindowView).create(); 
                }

                return copyWindow;
            },

            openCopyWindow: function () {

                var copyWindow = this.getCopyWindow();

                copyWindow.down('#tfManagingOrg').setValue(this.controller.params.ContragentName);
                copyWindow.down('#tfPeriodDiCurrent').setValue(this.controller.params.PeriodDiName);

                copyWindow.show();
            },

            closeCopyWindow: function () {

                var copyWindow = this.getCopyWindow();
                copyWindow.close();
            },

            onClearFileTrigger: function (fld) {
                // при сохранении панели вызывается метод updateRecord
                // он восстанавливает Id файла и тем самым файл не удаляется с сервера
                // здесь мы сбрасываем начальное состояние fileField'a
                fld.resetOriginalValue();
            },

            copyDocs: function () {
                var asp = this;

                var copyWindow = this.getCopyWindow();

                var choosenPeriodDi = copyWindow.down('#sfPeriodDiFrom').getValue();
                var cbProjectContract = copyWindow.down('#cbProjectContract').getValue();
                var cbCommunalService = copyWindow.down('#cbCommunalService').getValue();
                var cbApartmentService = copyWindow.down('#cbApartmentService').getValue();

                if (asp.controller.params.PeriodDi.Id == choosenPeriodDi) {
                    Ext.Msg.alert('Копирование документов', 'Период копирования должен быть отличен от текущего периода!');
                    return;
                }

                if (!cbProjectContract && !cbCommunalService && !cbApartmentService) {
                    Ext.Msg.alert('Копирование документов', 'Не выбрано ни одного типа файла для копирования!');
                    return;
                }
                if (copyWindow.getForm().isValid()) {
                    Ext.Msg.confirm('Копирование документов', 'Скопировать документы из другого отчетного периода?', function (result) {
                        if (result == 'yes') {
                            asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                            B4.Ajax.request(B4.Url.action('CopyDocs', 'Documents', {
                                disclosureInfoId: asp.controller.params.disclosureInfoId,
                                choosenPeriodDiId: choosenPeriodDi,
                                projectContract: cbProjectContract,
                                communalService: cbCommunalService,
                                apartmentService: cbApartmentService

                            })).next(function () {
                                asp.controller.unmask();
                                asp.setDataDocuments(asp.controller.params.disclosureInfoId);
                                Ext.Msg.alert('Копирование документов', 'Документы успешно скопированы');
                            }).error(function () {
                                Ext.Msg.alert('Копирование документов', 'Не удалось скопировать документы!');
                                asp.controller.unmask();
                            });
                        }
                    });
                }
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            var me = this;
            me.getAspect('documentsEditPanelAspect').setDataDocuments(me.params.disclosureInfoId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('documentsPermissionAspect').setPermissionsByRecord(me.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    },

    getModel: function (model) {
        return this.application.getModel(model);
    } 

});
