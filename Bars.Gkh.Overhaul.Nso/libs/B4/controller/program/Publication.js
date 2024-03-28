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
                        }, scope: me
                    }
                };
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
                {
                    name: 'Ovrhl.PublicationProgs.PublishDate.View', applyTo: '[name=PublishDate]', selector: 'publicationproggrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ],
            name: 'publishedProgramStatePerm'
        }
    ],

    init: function() {
        this.control({
            'publicationproggrid b4updatebutton': { click: { fn: this.onUpdate, scope: this } },
            'publicationproggrid button[actionName=sign]': { click: { fn: this.signBtnClick, scope: this } },
            'publicationproggrid button[name="btnDeletePublishedProgram"]': { click: { fn: this.deleteBtnClick, scope: this } },
            'pdfWindow': {
                createsignature: {
                    fn: this.onCreateSignature,
                    scope: this
                }
            }
        });

        this.callParent(arguments);
    },

    index: function() {
        var me = this,
            model,
            view = me.getMainPanel(),
            publishDateField;
        
        if (!view) {
            view = Ext.widget('publicationproggrid');
            
            me.bindContext(view);
            me.application.deployView(view);
            
            view.getStore().load();

            // При отработки даного контроллера требуеются некотоыре параметры
            // их мы сохраняем в панель
            view.params = {};
            view.params.dataToSign = null;
            view.params.xmlId = null;
            view.params.pdfId = null;
            
            B4.Ajax.request({
                url: B4.Url.action('GetPublishedProgram', 'PublishedProgram'),
                method: 'GET',
                timeout: 9999999
            }).next(function (resp) {
                var data = Ext.decode(resp.responseText);
                view.params.programId = data.Id;

                me.getAspect('publicationStateButtonAspect').setStateData(data.Id, data.State);
                
                model = me.getModel('program.PublishedProgram');
                data.Id ? model.load(data.Id, {
                    success: function (rec) {
                        me.getAspect('publishedProgramStatePerm').setPermissionsByRecord(rec);
                        publishDateField = view.down('[name=PublishDate]');
                        publishDateField.setValue(rec.get('PublishDate'));
                    }
                }) : me.getAspect('publishedProgramStatePerm').setPermissionsByRecord(new model({ Id: 0 }));

            }).error(function (e) {
                Ext.Msg.alert('Ошибка!', (e.message || e));
            });
        }
    },
    
    onUpdate: function () {
        this.getMainPanel().getStore().load();
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
        var me = this;
        
        B4.Ajax.request({
            url: B4.Url.action('GetValidationForSignEcp', 'PublishedProgram'),
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
            response,
            pdfUrl;
        
        me.mask('Подготовка данных...', grid);
        
        B4.Ajax.request({
            url: B4.Url.action('GetDataToSignEcp', 'PublishedProgram'),
            timeout: 9999999
        }).next(function (resp) {
            response = Ext.decode(resp.responseText);
            me.unmask();
            
            me.setDataToSign(response.dataToSign);
            me.setXmlId(response.xmlId);
            me.setPdfId(response.pdfId);

            me.unmask();

            pdfUrl = B4.Url.action(Ext.String.format("/{0}/{1}?pdfId={2}", 'PublishedProgram', 'GetPdf', response.pdfId));
            me.showFrame(pdfUrl);
        }).error(function (e) {
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
                        timeout: 1000
                    }).next(function () {
                        B4.QuickMsg.msg('Статус удаления', 'Опубликованная программа успешно удалена', 'success');
                        
                        grid.getStore().load();
                    }).error(function (e) {
                        Ext.Msg.alert('Ошибка!', (e.message || e));
                    });
                }
            }
        }, me);
    }
});