Ext.define('B4.controller.DocumentsRealityObj', {
    extend: 'B4.base.Controller',

    requires:
        [
            'B4.Ajax',
            'B4.Url',
            'B4.aspects.GkhEditPanel',
            'B4.aspects.GridEditWindow',
            'B4.aspects.permission.documentsrealityobj.State',
            'B4.aspects.FieldRequirementAspect'
        ],

    models: [
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
            xtype: 'requirementaspect',
            requirements: [
                { name: 'Gkh.DisclosureInfo.RealityObject.Documents.Fields.Year', applyTo: '[name=Year]', selector: 'realityobjprotocoleditwindow' },
            ]
        },
        {
            xtype: 'documentsrealityobjstateperm',
            name: 'documentsRealityObjPermissionAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'documentsRealityObjEditPanelAspect',
            modelName: 'DocumentsRealityObj',
            editPanelSelector: '#documentsRealityObjEditPanel',
            copyWindowSelector: '#documentsRealityObjCopyWindow',
            copyWindowView: 'documents.RealityObjCopyWindow',        
            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #copyDocsRealityObjButton'] = { 'click': { fn: me.openCopyWindow, scope: me } };
                actions[me.editPanelSelector + ' [name=HasGeneralMeetingOfOwners]'] = { 'change': { fn: me.onChangeGeneralMeetingOfOwners, scope: me } };
                actions[me.copyWindowSelector + ' b4closebutton'] = { 'click': { fn: me.closeCopyWindow, scope: me } };
                actions[me.copyWindowSelector + ' b4savebutton'] = { 'click': { fn: me.copyDocs, scope: me } };
            },

            getCopyWindow: function () {
                var me = this;
                var copyWindow = Ext.ComponentQuery.query(me.copyWindowSelector)[0];

                if (!copyWindow) {
                    copyWindow = me.controller.getView(me.copyWindowView).create();
                }

                return copyWindow;
            },

            openCopyWindow: function () {
                var asp = this,
                    copyWindow = asp.getCopyWindow();

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

                var copyWindow = asp.getCopyWindow();

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

            onChangeGeneralMeetingOfOwners: function(fld, newValue) {
                var panel = fld.up('#documentsRealityObjEditPanel'),
                    docsGrid = panel.down('#realityObjProtocolGrid');

                if (!fld.isHidden()) {
                    if (newValue === 10)
                    {
                        docsGrid.show();
                    } else
                    {
                        docsGrid.hide();
                    }
                }
            },
            listeners: {
                beforesetpaneldata: function (asp, rec, panel) {
                    var docsGrid = panel.down('#realityObjProtocolGrid'),
                        docsNotation = panel.down('#cmpGridNotation'),
                        docsGeneralMeetingNotation = panel.down('#cmpGeneralMeetingNotation'),
                        generalMeetingCbx = panel.down('[name=HasGeneralMeetingOfOwners]'),
                        docNumColumn = docsGrid.down('[dataIndex=DocNum]'),
                        docDateColumn = docsGrid.down('[dataIndex=DocDate]');                   

                    var yearFromParams = asp.controller.getYear();


                    if (docsGrid && docsNotation) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetTypeManagingByDisinfo', 'DisclosureInfo', {
                            disclosureInfoId: asp.controller.params.disclosureInfoId
                        })).next(function (response) {
                            asp.controller.unmask();
                            
                            var typeManagement = Ext.JSON.decode(response.responseText);
                            if (yearFromParams < 2015 && typeManagement != '20' && typeManagement != '40') {
                                asp.controller.withoutGridDocs = true;
                                docsNotation.hide();
                                docsGrid.hide();
                            } else {
                                asp.controller.withoutGridDocs = false;
                                docsNotation.show();
                                docsGrid.show();
                            }

                            if (yearFromParams >= 2015) {
                                docNumColumn.show();
                                docDateColumn.show();
                                docsGeneralMeetingNotation.show();
                                generalMeetingCbx.show();

                                if (rec.get('HasGeneralMeetingOfOwners') !== 10) {
                                   docsGrid.hide();
                                }
                            } else {
                                docNumColumn.hide();
                                docDateColumn.hide();
                                docsGeneralMeetingNotation.hide();
                                generalMeetingCbx.hide();
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
                    
                    var model = asp.controller.getModel('DisclosureInfo');

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
                    var model = asp.controller.getModel('DisclosureInfoRealityObj'),
                        window = asp.getForm(),
                        docNumFld = window.down('[name=DocNum]'),
                        docDateFld = window.down('[name=DocDate]'),
                        year = asp.controller.getYear();

                    if (year >= 2015) {
                        docNumFld.setVisible(true);
                        docDateFld.setVisible(true);
                        docNumFld.allowBlank = false;
                        docDateFld.allowBlank = false;
                    } else {
                        docNumFld.setVisible(false);
                        docDateFld.setVisible(false);
                        docNumFld.allowBlank = true;
                        docDateFld.allowBlank = true;
                    }

                    docNumFld.validate();
                    docDateFld.validate();

                    model.load(asp.controller.params.disclosureInfoRealityObjId, {
                        success: function (record) {
                            rec.set("RealityObject", record.get('RealityObject').Id);
                        },
                        scope: this
                    });
                }
            }
        }  
    ],

    init: function () {
        var me = this;

        me.callParent(arguments);      
        me.getStore('documents.RealityObjProtocol').on('beforeload', me.onBeforeLoad, me);
    },

    onLaunch: function () {
        var me = this;

        me.getStore('documents.RealityObjProtocol').load();
        
        if (me.params) {
            me.getAspect('documentsRealityObjEditPanelAspect').setDataDocuments(me.params.disclosureInfoRealityObjId);
            me.params.getId = function () { return me.params.disclosureInfoId; };
            me.getAspect('documentsRealityObjPermissionAspect').setPermissionsByRecord(me.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params) {
            operation.params.disclosureInfoRealityObjId = me.params.disclosureInfoRealityObjId;
        }
    },

    getYear: function() {
        return parseInt(this.params.year.substring(0, 4));
    }
});
