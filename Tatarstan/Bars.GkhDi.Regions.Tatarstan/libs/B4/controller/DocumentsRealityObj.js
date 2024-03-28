Ext.define('B4.controller.DocumentsRealityObj', {
    extend: 'B4.base.Controller',

    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.documentsrealityobj.State',
        'B4.aspects.FieldRequirementAspect',
    ],

    models:[
        'DocumentsRealityObj',
        'documents.RealityObjProtocol'
    ],
    
    stores: [
        'documents.RealityObjProtocol'
    ],
    
    views: [
        'documents.RealityObjEditPanel',
        'documents.RealityObjProtocolEditWindow',
        'documents.RealityObjCopyWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'documents.RealityObjEditPanel',
    mainViewSelector: '#documentsRealityObjEditPanel',

    aspects: [
        {
            xtype: 'documentsrealityobjstateperm',
            name: 'documentsRealityObjPermissionAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'Gkh.DisclosureInfo.RealityObject.Documents.Fields.Year', applyTo: '[name=Year]', selector: 'realityobjprotocoleditwindow' },
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'documentsRealityObjEditPanelAspect',
            modelName: 'DocumentsRealityObj',
            editPanelSelector: '#documentsRealityObjEditPanel',
            copyWindowSelector: '#documentsRealityObjCopyWindow',
            copyWindowView: 'documents.RealityObjCopyWindow',
            
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #copyDocsRealityObjButton'] = { 'click': { fn: this.openCopyWindow, scope: this } };
                actions[this.copyWindowSelector + ' b4closebutton'] = { 'click': { fn: this.closeCopyWindow, scope: this } };
                actions[this.copyWindowSelector + ' b4savebutton'] = { 'click': { fn: this.copyDocs, scope: this } };
            },

            getCopyWindow: function () {

                var copyWindow = Ext.ComponentQuery.query(this.copyWindowSelector)[0];

                if (!copyWindow) {
                    copyWindow = this.controller.getView(this.copyWindowView).create(); 
                }

                return copyWindow;
            },

            openCopyWindow: function () {

                var copyWindow = this.getCopyWindow(),
                asp = this;

                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetCopyInfo', 'DisclosureInfoRealityObj', {
                    disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId,
                    disclosureInfoId: asp.controller.params.disclosureInfoId
                })).next(function (response) {
                    asp.controller.unmask();
                    var obj = Ext.JSON.decode(response.responseText);
                    copyWindow.down('#tfManagingOrg').setValue(obj.ManagingOrgName);
                    copyWindow.down('#tfPeriodDiCurrent').setValue(obj.PeriodName);
                    copyWindow.down('#tfRealityObj').setValue(obj.Address);

                    asp.controller.params.PeriodDiId = obj.PeriodDiId;
                }).error(function () {
                    asp.controller.unmask();
                });

                copyWindow.show();
            },

            closeCopyWindow: function () {

                var copyWindow = this.getCopyWindow();
                copyWindow.close();
            },

            copyDocs: function () {
                var asp = this;

                var copyWindow = this.getCopyWindow();

                var choosenPeriodDi = copyWindow.down('#sfPeriodDiFrom').getValue();
                var cbAct = copyWindow.down('#cbAct').getValue();
                var cbListWork = copyWindow.down('#cbListWork').getValue();
                var cbReport = copyWindow.down('#cbReport').getValue();

                if (asp.controller.params.PeriodDiId == choosenPeriodDi) {
                    Ext.Msg.alert('Копирование документов', 'Период копирования должен быть отличен от текущего периода!');
                    return;
                }

                if (!cbAct && !cbListWork && !cbReport) {
                    Ext.Msg.alert('Копирование документов', 'Не выбрано ни одного типа файла для копирования!');
                    return;
                }
                if (copyWindow.getForm().isValid()) {
                    Ext.Msg.confirm('Копирование документов', 'Скопировать документы из другого отчетного периода?', function (result) {
                        if (result == 'yes') {
                            asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                            B4.Ajax.request(B4.Url.action('CopyDocs', 'DocumentsRealityObj', {
                                disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId,
                                choosenPeriodDiId: choosenPeriodDi,
                                act: cbAct,
                                listWork: cbListWork,
                                report: cbReport
                            })).next(function () {
                                asp.controller.unmask();
                                asp.setDataDocuments(asp.controller.params.disclosureInfoRealityObjId);
                                Ext.Msg.alert('Копирование документов', 'Документы успешно скопированы');
                            }).error(function () {
                                asp.controller.unmask();
                                Ext.Msg.alert('Копирование документов', 'Не удалось скопировать документы!');
                            });
                        }
                    });
                }
            },

            //перекрываем стандартную реализацию что бы сообщение из savesuccess выходило после сообщения успешного сохранения
            onPreSaveSuccess: function (asp, record) {
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                asp.fireEvent('savesuccess', asp, record);
            },

            listeners: {
                beforesetpaneldata: function (asp, rec, panel) {
                    var docsGrid = panel.down('#realityObjProtocolGrid');
                    var docsNotation = panel.down('#cmpGridNotation');
                    
                    if (docsGrid && docsNotation) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetTypeManagingByDisinfo', 'DisclosureInfo', {
                            disclosureInfoId: asp.controller.params.disclosureInfoId
                        })).next(function (response) {
                            asp.controller.unmask();
                            
                            var typeManagement = parseInt(Ext.JSON.decode(response.responseText));
                            if (Ext.Array.indexOf([10, 20, 40], typeManagement) == -1) {
                                asp.controller.withoutGridDocs = true;
                                docsGrid.hide();
                                docsNotation.hide();
                            } else {
                                asp.controller.withoutGridDocs = false;
                                docsGrid.show();
                                docsNotation.show();

                            }
                                
                            panel.doLayout();

                        }).error(function () {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка', 'Не удалось получить тип управления');
                        });
                    }
                },
                beforesave: function (asp, rec) {
                    if (Ext.isEmpty(rec.get("DisclosureInfoRealityObj"))) {
                        rec.set("DisclosureInfoRealityObj", asp.controller.params.disclosureInfoRealityObjId);
                    }
                },
                savesuccess: function (asp, rec) {
                    
                    asp.setPanelData(rec);

                    var panel = asp.getPanel();
                    var store = panel.down('#realityObjProtocolGrid').getStore();
                    var current = false;
                    var last = false;
                    
                    var model = this.controller.getModel('DisclosureInfo');

                    model.load(asp.controller.params.disclosureInfoId, {
                        success: function (rec) {
                            var year = new Date(rec.get('PeriodDi').DateStart).getFullYear();

                            store.each(function (record) {
                                if (!record.phantom) {
                                    if (record.get('Year') == year && record.get('File')) {
                                        current = true;
                                    }

                                    if (record.get('Year') == (year - 1) && record.get('File')) {
                                        last = true;
                                    }
                                }
                            }, this);

                            if ((!current || !last) && !asp.controller.withoutGridDocs) {
                                Ext.Msg.alert('Внимание', 'Загружены не все документы');
                            }
                        },
                        scope: this
                    });
                }
            },

            //Перед методом setdata получаем id DocumentsRealityObj по disclosureInfoRealityObjId
            setDataDocuments: function (disclosureInfoRealityObjId) {

                var asp = this;

                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetIdByDisnfoId', 'DocumentsRealityObj', {
                    disclosureInfoRealityObjId: disclosureInfoRealityObjId
                })).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    //Запоминаеем название периода, ук для формы копирования
                    var disclosureInfoRealityObj = obj.disclosureInfoRealityObj;
                    if (!Ext.isEmpty(disclosureInfoRealityObj)) {
                        asp.controller.params.PeriodDi = disclosureInfoRealityObj.PeriodDi;
                    }

                    asp.controller.params.Id = obj.Id;
                    asp.setData(asp.controller.params.Id);
                }).error(function () {
                    asp.controller.unmask();
                    asp.setData(0);
                });
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'documentsRealityObjProtocolGridWindowAspect',
            gridSelector: '#realityObjProtocolGrid',
            editFormSelector: '#realityObjProtocolEditWindow',
            storeName: 'documents.RealityObjProtocol',
            modelName: 'documents.RealityObjProtocol',
            editWindowView: 'documents.RealityObjProtocolEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec) {

                    //получаем record по id => по record модель => по модели упр орг
                    var model = asp.controller.getModel('DisclosureInfoRealityObj');
                    model.load(asp.controller.params.disclosureInfoRealityObjId, {
                        success: function (record) {
                            rec.set("RealityObject", record.get('RealityObject').Id);
                            rec.set("DisclosureInfoRealityObj", record.getId());
                        },
                        scope: this
                    });
                },
                beforesave: function (asp, rec) {
                    var periodYear = new Date(asp.controller.params.PeriodDi.DateEnd).getFullYear();
                    if (rec.data.Year != null && (rec.data.Year < periodYear - 1 || rec.data.Year > periodYear)) {
                        Ext.Msg.alert('Ошибка сохранения', 'Следующие поля содержат ошибки:<br><b>Год:</b> значение не соответствует текущему или предшествующему году раскрытия информации.');
                        return false;
                    }
                }
            }
        }  
    ],

    init: function () {
        this.callParent(arguments);
        
        this.getStore('documents.RealityObjProtocol').on('beforeload', this.onBeforeLoad, this);
    },

    onLaunch: function () {
        this.getStore('documents.RealityObjProtocol').load();
        
        if (this.params) {
            var me = this,
                dateStart = me.params.periodDiDateStart,
                dateEnd = me.params.periodDiDateEnd;

            me.getAspect('documentsRealityObjEditPanelAspect').setDataDocuments(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('documentsRealityObjPermissionAspect').setPermissionsByRecord(me.params);
            // если у нас год больше или равен 2015, тогда меняем текст
            var isYearGte2015 = false;
            try {
                var yearFromParams = parseInt(me.params.year.substring(0, 4));
                if (yearFromParams >= 2015) {
                    isYearGte2015 = true;
                }
            } catch (e) {
            }
            if (isYearGte2015) {
                var gridNotationComponent = me.getMainView().down('#cmpGridNotation');
                if (gridNotationComponent) {
                    gridNotationComponent.update('<div>Необходимо загрузить следующие документы: <br/>' +
                        'Протоколы общих собраний за текущий год и за год, предшествующий текущему году</div>');
                }
            }
            if (dateStart && dateEnd) {
                var generalMeetingNotationComponent = me.getMainView().down('#cmpGeneralMeetingNotation');
                if (generalMeetingNotationComponent) {
                    generalMeetingNotationComponent.update(
                        '<div>Проводились ли общие собрания собственников помещений в многоквартирном доме с участием управляющей организации c ' +
                        dateStart + 'г.  по ' + dateEnd + 'г.?</div>');
                }
            }
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
    }
});
