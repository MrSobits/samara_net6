Ext.define('B4.controller.program.Publication', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateContextButton',
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport',
        'B4.view.PdfWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'program.PublishedProgram'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'program.PublicationGrid',
        'program.PublishProgramDateEditWindow'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'publicationproggrid' }
    ],
    
    aspects: [
        {
            xtype: 'statecontextbuttonaspect',
            name: 'publicationStateButtonAspect',
            stateButtonSelector: 'publicationproggrid #btnState',
            customWindowSelector: 'publishprogramdateeditwin',
            customWindowView: 'program.PublishProgramDateEditWindow',
            customWindowApplyButtonSelector: 'b4savebutton',
            otherActions: function (actions) {
                var me = this;

                actions['publishprogramdateeditwin b4closebutton'] = {
                    'click': {
                        fn: function (btn) {
                            btn.up('publishprogramdateeditwin').close();
                        }, scope: me } };
            },
            listeners: {
                transfersuccess: function (me, entityId, newState) {
                    var grid = me.controller.getMainPanel(),
                        publishDateField = grid.down('[name=PublishDate]');
                    
                    me.setStateData(entityId, newState);
                    
                    var model = me.controller.getModel('program.PublishedProgram');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            publishDateField.setValue(rec.get('PublishDate'));
                            me.controller.getAspect('publishedProgramStatePerm').setPermissionsByRecord(rec);
                        }
                    }) : me.controller.getAspect('publishedProgramStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'publicationproggrid',
            buttonSelector: 'publicationproggrid #btnExport',
            controllerName: 'PublishedProgramRecord',
            actionName: 'Export'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'Ovrhl.PublicationProgsDelete.Delete', applyTo: 'button[actionName=delete]', selector: 'publicationproggrid' },
                { name: 'Ovrhl.PublicationProgs.PublishDate.View', applyTo: '[name=PublishDate]', selector: 'publicationproggrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Ovrhl.PublicationProgs.Summary.View', applyTo: 'tbtext[name=Summary]', selector: 'publicationproggrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                }
            ],
            name: 'publishedProgramStatePerm'
        }
    ],

    init: function() {
        this.control({
            'publicationproggrid': { render: { fn: this.onGridRender, scope: this } },
            'publicationproggrid b4updatebutton': { click: { fn: this.onUpdate, scope: this } },
            'publicationproggrid button[actionName=sign]': { click: { fn: this.signBtnClick, scope: this } },
            'publicationproggrid button[actionName=delete]': { click: { fn: this.deleteBtnClick, scope: this } },
            'publicationproggrid combobox[name="Municipality"]': {
                select: { fn: this.onSelectMunicipality, scope: this },
                render: { fn: this.renderMuField, scope: this }
            },
            'pdfWindow': {
                createsignature: {
                    fn: this.onCreateSignature,
                    scope: this
                }
            }
        });

        this.callParent(arguments);
    },

    index: function(muId) {
        var me = this,
            view = me.getMainPanel();
        
        if (!view) {
            view = Ext.widget('publicationproggrid');
            
            me.bindContext(view);
            me.application.deployView(view);

            // При отработки даного контроллера требуются некоторые параметры
            // их мы сохраняем в панель
            view.params = {};
            view.params.dataToSign = null;
            view.params.xmlId = null;
            view.params.pdfId = null;
            view.params.muId = muId;
        }
    },
    
    deleteBtnClick: function () {
        var me = this,
            grid = me.getMainPanel();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                if (grid.params.programId) {
                    B4.Ajax.request({
                        url: B4.Url.action('DeletePublishedProgram', 'PublishedProgram', { programId: grid.params.programId }),
                        method: 'GET',
                        timeout: 10000
                    }).next(function() {
                        grid.getStore().load();
                    }).error(function(e) {
                        Ext.Msg.alert('Ошибка!', (e.message || e));
                    });
                }
            }
        }, me);
    },

    onGridRender: function(grid) {
        grid.getStore().on('beforeload', this.onGridStoreBeforeLoad, this);
    },
    
    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me, { single: true });
        store.load();
    },

    onLoadMunicipality: function (store, records) {
        var me = this,
            cmb,
            panel = me.getMainPanel(),
            muId = panel.params.muId,
            record;

        if (records[0]) {

            cmb = panel.down('combobox[name="Municipality"]');

            if (muId) {
                record = store.findRecord('Id', muId, 0, false, true, true);
            }

            cmb.setValue(record ? record : records[0]);
            me.onSelectMunicipality(cmb, [record ? record : records[0]]);
        }
    },
    
    onSelectMunicipality: function (f, records) {
        var me = this,
            view = me.getMainPanel(),
            value = records[0].getId();

        f.store.clearFilter();

        me.getPublishProgram(value);
        view.getStore().load();       
    },
    
    onGridStoreBeforeLoad: function(store) {
        var me = this,
            moId = me.getMainPanel().down('combobox[name="Municipality"]').getValue();

        Ext.apply(store.getProxy().extraParams, { mo_id: moId });
    },
    
    getPublishProgram: function (muId) {
        var me = this,
            grid = me.getMainPanel(),
            publishDateField = grid.down('[name=PublishDate]'),
            summaryField = grid.down('[name=Summary]');

        debugger;
        var tfMainProg = grid.down('#tfMainProg');
        var tfSubProg = grid.down('#tfSubProg');
        var tfTotalCount = grid.down('#tfHousesCount');
        var tfAllTotalCount = grid.down('#tfAllHousesCount');
        var tfAllMainProg = grid.down('#tfAllMainProg');
        var tfAllSubProg = grid.down('#tfAllSubProg');
        var tfLift = grid.down('#tfLift');
        var tfAllLift = grid.down('#tfAllLift');
        var tfWaterO = grid.down('#tfWaterO');
        var tfAllWaterO = grid.down('#tfAllWaterO');
        var tfWaterS = grid.down('#tfWaterS');
        var tfAllWaterS = grid.down('#tfAllWaterS');
        var tfRoof = grid.down('#tfRoof');
        var tfAllRoof = grid.down('#tfAllRoof');
        var tfGas = grid.down('#tfGas');
        var tfAllGas = grid.down('#tfAllGas');
        var tfPodv = grid.down('#tfPodv');
        var tfAllPodv = grid.down('#tfAllPodv');
        var tfWarm = grid.down('#tfWarm');
        var tfAllWarm = grid.down('#tfAllWarm');
        var tfFundam = grid.down('#tfFundam');
        var tfAllFundam = grid.down('#tfAllFundam');
        var tfFas = grid.down('#tfFas');
        var tfAllFas = grid.down('#tfAllFas');
        var tfElect = grid.down('#tfElect');
        var tfAllElect = grid.down('#tfAllElect');
        B4.Ajax.request(B4.Url.action('GetDataForInfoPanel', 'PublishedProgram', { muId: muId }
        )).next(function (response) {
            var data = Ext.decode(response.responseText);
            tfMainProg.setValue(data.data.mainCount);
            tfSubProg.setValue(data.data.subCount);
            tfTotalCount.setValue(data.data.totalCount);
            tfAllMainProg.setValue(data.data.allMainCount);
            tfAllSubProg.setValue(data.data.allSubCount);
            tfLift.setValue(data.data.comObjLift);
            tfAllLift.setValue(data.data.allComObjLift);
            tfWaterO.setValue(data.data.comObjWaterO);
            tfAllWaterO.setValue(data.data.allComObjWaterO);
            tfWaterS.setValue(data.data.comObjWaterS);
            tfAllWaterS.setValue(data.data.allComObjWaterS);
            tfRoof.setValue(data.data.comObjRoof);
            tfAllRoof.setValue(data.data.allComObjRoof);
            tfGas.setValue(data.data.comObjGas);
            tfAllGas.setValue(data.data.allComObjGas);
            tfPodv.setValue(data.data.comObjPodv);
            tfAllPodv.setValue(data.data.allComObjPodv);
            tfWarm.setValue(data.data.comObjWarm);
            tfAllWarm.setValue(data.data.allComObjWarm);
            tfFundam.setValue(data.data.comObjFundam);
            tfAllFundam.setValue(data.data.allComObjFundam);
            tfFas.setValue(data.data.comObjFas);
            tfAllFas.setValue(data.data.allComObjFas);
            tfElect.setValue(data.data.comObjElect);
            tfAllElect.setValue(data.data.allComObjElect);
            tfAllTotalCount.setValue(data.data.allTotalCount);
            return true;
        }).error(function () {
            debugger;
            });

        B4.Ajax.request({
            url: B4.Url.action('GetPublishedProgram', 'PublishedProgram', { muId: muId }),
            method: 'GET',
            timeout: 9999999
        }).next(function (resp) {
            var data = Ext.decode(resp.responseText);
            grid.params.programId = data.Id;
            me.getAspect('publicationStateButtonAspect').setStateData(data.Id, data.State);

            var model = me.getModel('program.PublishedProgram');
            data.Id ? model.load(data.Id, {
                success: function (rec) {
                    me.getAspect('publishedProgramStatePerm').setPermissionsByRecord(rec);

                    publishDateField.setValue(rec.get('PublishDate'));

                    var summary = {};
                    summary.total = Ext.isEmpty(rec.get('TotalRoCount')) ? '' : rec.get('TotalRoCount');
                    summary.included = Ext.isEmpty(rec.get('IncludedRoCount')) ? '' : rec.get('IncludedRoCount');
                    summary.excluded = Ext.isEmpty(rec.get('ExcludedRoCount')) ? '' : rec.get('ExcludedRoCount');

                    summaryField.update(summary);
                }
            }) : me.getAspect('publishedProgramStatePerm').setPermissionsByRecord(new model({ Id: 0 }));

        }).error(function (e) {
            grid.params.programId = null;
            Ext.Msg.alert('Ошибка!', (e.message || e));
            });
        
    },

    onUpdate: function () {
        this.getMainPanel().getStore().load();
        this.updateRoSummary();
    },

    updateRoSummary: function () {
        var me = this,
            grid = me.getMainPanel(),
            municipalityField = grid.down('combobox[name="Municipality"]'),
            summaryField = grid.down('[name=Summary]');

        B4.Ajax.request({
            url: B4.Url.action('GetPublishedProgram', 'PublishedProgram', { muId: municipalityField.getValue() }),
            method: 'GET',
            timeout: 1 * 60 * 1000
        }).next(function(resp) {
            var data = Ext.decode(resp.responseText);

            var summary = {};
            summary.total = Ext.isEmpty(data.TotalRoCount) ? '' : data.TotalRoCount;
            summary.included = Ext.isEmpty(data.IncludedRoCount) ? '' : data.IncludedRoCount;
            summary.excluded = Ext.isEmpty(data.ExcludedRoCount) ? '' : data.ExcludedRoCount;

            summaryField.update(summary);
        });
    },
    
    signBtnClick: function () {
        this.validationBeforeSign();
    },
    
    setDataToSign: function (data) {
        this.getMainPanel().params.dataToSign = data;
    },
    
    setXmlId: function (data) {
        this.getMainPanel().params.xmlId = data;
    },
    
    setPdfId: function (data) {
        this.getMainPanel().params.pdfId = data;
    },
    
    getDataToSign: function () {
        return this.getMainPanel().params.dataToSign;
    },
    
    getXmlId: function () {
        return this.getMainPanel().params.xmlId;
    },
    
    getPdfId: function () {
        return this.getMainPanel().params.pdfId;
    },
    
    validationBeforeSign: function () {
        var me = this,
            muId = me.getMainPanel().down('combobox[name=Municipality]').getValue();
        
        B4.Ajax.request({
            url: B4.Url.action('GetValidationForSignEcp', 'PublishedProgram', { muId: muId }),
            method: 'GET',
            timeout: 9999999
        }).next(function (resp) {
            var message = Ext.decode(resp.responseText);

            Ext.Msg.confirm('Внимание', message, function (result) {
                if (result == 'yes') {
                    me.prepareData();
                }
            });

        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    prepareData: function () {
        var me = this,
            grid = me.getMainPanel(),
            pdfUrl,
            muId = grid.down('combobox[name=Municipality]').getValue();
        
        me.mask('Подготовка данных...', grid);
        
        B4.Ajax.request({
            url: B4.Url.action('GetDataToSignEcp', 'PublishedProgram', { muId: muId }),
            method: 'GET',
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            
            me.setDataToSign(response.dataToSign);
            me.setXmlId(response.xmlId);
            me.setPdfId(response.pdfId);

            pdfUrl = B4.Url.action(Ext.String.format("/{0}/{1}?pdfId={2}", 'PublishedProgram', 'GetPdf', response.pdfId));
            me.showFrame(pdfUrl);
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Не удалось получить данные для подписания: ' + (e.message || e));
        });
    },
    
    showFrame: function (url) {
        var pdfWindow = Ext.create('widget.pdfWindow', {
            layout: 'fit',
            items: [
                {
                    xtype: 'component',
                    autoEl: {
                        tag: 'iframe',
                        style: 'height: 100%; width: 100%; border: none',
                        src: url
                    }
                }
            ],
            constrain: true
        });

        this.getMainPanel().up().getActiveTab().add(pdfWindow);

        pdfWindow.show();
    },
    
    onCreateSignature: function (win) {
        var me = this,
            certField = win.down('combo'),
            val = certField.getValue(),
            cert,
            certBase64,
            sign;

        if (!val) {
            Ext.Msg.alert('Внимание!', 'Не выбран сертификат!');
            return false;
        }

        try {
            cert = Crypto.getCertificate(Crypto.getCurrentUserMyStore(), Crypto.constants.findType.CAPICOM_CERTIFICATE_FIND_SHA1_HASH, val);

            certBase64 = cert.Export(Crypto.constants.encodingType.CAPICOM_ENCODE_BASE64);

            sign = Crypto.sign(me.getDataToSign(), cert);
                        
            win.mask('Подписывание', win);
            
            B4.Ajax.request({
                url: B4.Url.action('SaveSignedResult', 'PublishedProgram'),
                params: {
                    xmlId: me.getXmlId(),
                    pdfId: me.getPdfId(),
                    sign: sign,
                    certificate: certBase64
                }
            }).next(function () {
                B4.QuickMsg.msg('Сообщение', 'Опубликованная программа успешно подписана', 'success');
                win.unmask();
            }).error(function (e) {
                Ext.Msg.alert('Ошибка!', 'Ошибка при подписании: ' + (e.message || e));
                win.unmask();
            });
        } catch (e) {
            Ext.Msg.alert('Ошибка!', (e.message || e));
            win.unmask();
        }
    }
});