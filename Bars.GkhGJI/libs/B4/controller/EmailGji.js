Ext.define('B4.controller.EmailGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',     
        'B4.aspects.GkhBlobText',
        'B4.Ajax',
        'B4.view.email.PdfWindow',
        'B4.view.email.PreviewAttachmentGrid',
        'B4.Url'
    ],
    models: [
        'email.EmailGji',
        'email.EmailGjiAttachment'
    ],
    stores: [
        'email.EmailGji',
        'email.ListAttachments',
        'email.EmailGjiAttachment'
    ],
    views: [
        'email.Grid',
        'email.EditWindow',
        'email.AttachmentGrid'
    ],
    emailId: null,
    appPref: '',
    appSuf: '',
    appNumber: null,
    statPref: '',
    statSuf: '',
    statNumber: null,
    messageType:0,
    mainView: 'email.Grid',
    mainViewSelector: 'emailgjigrid',
    mixins: {
        context: 'B4.mixins.Context'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'emailgjigrid'
        }
    ],
    aspects: [
        {
            xtype: 'gkhblobtextaspect',
            name: 'emailGjiContentAspect',
            fieldSelector: '[name=Content]',
            editPanelAspectName: 'emailGjiGridWindowAspect',
            controllerName: 'EmailGji',
            valueFieldName: 'Content',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'emailGjiGridWindowAspect',
            gridSelector: 'emailgjigrid',
            editFormSelector: '#emailGjiEditWindow',
            modelName: 'email.EmailGji',
            storeName: 'email.EmailGji',
            editWindowView: 'email.EditWindow',            
         
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {               
                actions[this.gridSelector + ' #btnProcess'] = { 'click': { fn: this.ProcessEmails, scope: this } };
                actions['emailpdfWindow #btnSkip'] = { 'click': { fn: this.SkipEmails, scope: this } };
                actions['emailpdfWindow #btnNext'] = { 'click': { fn: this.SkipEmails, scope: this } };
                actions['emailpdfWindow #btnReg'] = { 'click': { fn: this.RegisterEmail, scope: this } };
                actions['emailpdfWindow #btnDecline'] = { 'click': { fn: this.DeclineEmail, scope: this } };
                actions['emailpdfWindow radiogroup[name=MessageType]'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions['previewemailgjiattachmentgrid'] = { 'itemclick': { fn: this.changeImage, scope: this } };
            },
            onChangeType: function (field, newValue) {
                
                var me = this;
                var w = field.up('emailpdfWindow'); 
                var nfAppealNumber = w.down('#nfAppealNumber'),
                    tfPrefix = w.down('#tfPrefix'),
                    tfSuffix = w.down('#tfSuffix');
                if (newValue) {
                    if (newValue.MessageType == 0) {
                        nfAppealNumber.setValue(me.controller.appNumber);
                        tfPrefix.setValue(me.controller.appPref);
                        tfSuffix.setValue(me.controller.appSuf);
                    }
                    else {
                        nfAppealNumber.setValue(me.controller.statNumber);
                        tfPrefix.setValue(me.controller.statPref);
                        tfSuffix.setValue(me.controller.statSuf);
                    }
                }
            },
            RegisterEmail: function (btn) {
                var me = this;
                me.mask('Отправка запроса', B4.getBody());
                var w = btn.up('emailpdfWindow');        
                var nfAppealNumber = w.down('#nfAppealNumber'),
                    tfPrefix = w.down('#tfPrefix'),
                    tfSuffix = w.down('#tfSuffix'),
                    filterValue = w.down('radiogroup[name=MessageType]').getValue();
                if (filterValue) {
                    me.controller.messageType = filterValue.MessageType;
                    if (filterValue.MessageType == 0) {
                        me.controller.appNumber = nfAppealNumber.getValue() + 1;
                        me.controller.appPref = tfPrefix.getValue();
                        me.controller.appSuf = tfSuffix.getValue();
                    }
                    else {
                        me.controller.statNumber = nfAppealNumber.getValue() + 1;
                        me.controller.statPref = tfPrefix.getValue();
                        me.controller.statSuf = tfSuffix.getValue();
                    }
                }
                B4.Ajax.request({
                    url: B4.Url.action('RegisterEmail', 'EmailGji'),
                    timeout: 9999999,
                    params: {
                        documentId: me.controller.emailId,
                        msgType: me.controller.messageType,
                        appNumber: nfAppealNumber.getValue(),
                        appPref: tfPrefix.getValue(),
                        appSuf: tfSuffix.getValue()
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    me.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }


                }).error(function (err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });
                w.header.setTitle('Зарегистрирован');
                w.header.titleCmp.addCls('headerregistred');
                var nextBtn = w.down('#btnNext');
                var declineBtn = w.down('#btnDecline');
                nextBtn.setDisabled(false);
             // присваиваем новый номер
                
                btn.setDisabled(true);
                declineBtn.setDisabled(true);
                B4.QuickMsg.msg('Email', 'Обращение успешно зарегистрировано', 'success');
                this.SkipEmails(btn);
            },
            DeclineEmail: function (btn) {
                var me = this;
                var w = btn.up('emailpdfWindow');
                var typeDeclineFiled = w.down('#ecEmailDenailReason');
                var taDeclineReason = w.down('#taDeclineReason');
                B4.Ajax.request({
                    url: B4.Url.action('DeclineEmail', 'EmailGji'),
                    params: {
                        documentId: me.controller.emailId,
                        typeDecline: typeDeclineFiled.getValue(),
                        declineReason: taDeclineReason.getValue()
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }

            
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });

                w.header.setTitle('Отклонен');
                w.header.titleCmp.addCls('headerdeclined');
                var nextBtn = w.down('#btnNext');
                var btnReg = w.down('#btnReg');
                nextBtn.setDisabled(false);
                var nfAppealNumber = w.down('#nfAppealNumber'),
                    tfPrefix = w.down('#tfPrefix'),
                    tfSuffix = w.down('#tfSuffix'),
                    filterValue = w.down('radiogroup[name=MessageType]').getValue();
                if (filterValue) {
                    me.controller.messageType = filterValue.MessageType;
                    if (filterValue.MessageType == 0) {
                        me.controller.appNumber = nfAppealNumber.getValue();
                        me.controller.appPref = tfPrefix.getValue();
                        me.controller.appSuf = tfSuffix.getValue();
                    }
                    else {
                        me.controller.statNumber = nfAppealNumber.getValue();
                        me.controller.statPref = tfPrefix.getValue();
                        me.controller.statSuf = tfSuffix.getValue();
                    }
                }
                btnReg.setDisabled(true);
                btn.setDisabled(true);
                nextBtn.setDisabled(false);

            },

            changeImage: function (grid, record) {
                
                var me = this;
                if (grid) {
                    var win = grid.up('emailpdfWindow');
                    var cmp = win.down('#fileCmp');
                    var mainPDFUrl = B4.Url.action('/FilePreview/PreviewFile?id=' + record.getId());
                    cmp.autoEl.src = mainPDFUrl;
                    var the_iframe = cmp.getEl().dom;
                    the_iframe.src = mainPDFUrl;                    
                }
            },

            ProcessEmails: function (btn) {
                var me = this;    
                me.controller.emailId = 0;
                B4.Ajax.request({
                    url: B4.Url.action('GetNextQuestion', 'EmailGji'),
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }
                    if (tryDecoded.data.FileId) {
                        var mainPDFUrl = B4.Url.action('/FilePreview/PreviewFile?id=' + tryDecoded.data.FileId);
                        me.controller.emailId = tryDecoded.data.EmailId;
                        var pdfWindow = Ext.create('widget.emailpdfWindow', {
                            layout: 'fit',
                            closeAction: 'destroy',
                            title: 'Регистрация e-почты. ' + tryDecoded.data.SenderInfo,
                            items: [
                                {
                                    xtype: 'component',
                                    itemId: 'fileCmp',
                                    autoEl: {
                                        tag: 'iframe',
                                        itemId: 'fileFrame',
                                        style: 'height: 100%; width: 100%; border: none',
                                        src: mainPDFUrl
                                    }
                                }                 
                            ],
                            constrain: true
                        });
                        var nextBtn = pdfWindow.down('#btnNext');
                        nextBtn.setDisabled(true);
                        var filesgrid = pdfWindow.down('previewemailgjiattachmentgrid'),
                            filesStore = filesgrid.getStore();
                        filesStore.on('beforeload', function (store, operation) {
                            operation.params.documentId = me.controller.emailId;
                        });
                        filesStore.load();
                        pdfWindow.show();
                    }
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },

            SkipEmails: function (btn) {
                
                var w = btn.up('emailpdfWindow');
                w.close();          
                var me = this;
                B4.Ajax.request({
                    url: B4.Url.action('SkipAndGetNextQuestion', 'EmailGji'),
                    params: {
                        documentId: me.controller.emailId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }
                    if (tryDecoded.data.FileId) {
                        var mainPDFUrl = B4.Url.action('/FilePreview/PreviewFile?id=' + tryDecoded.data.FileId);
                        me.controller.emailId = tryDecoded.data.EmailId;
                        var pdfWindow = Ext.create('widget.emailpdfWindow', {
                            layout: 'fit',
                            title: 'Регистрация e-почты. ' + tryDecoded.data.SenderInfo,
                            items: [
                                {
                                    xtype: 'component',
                                    itemId: 'fileCmp',
                                    autoEl: {
                                        tag: 'iframe',
                                        itemId: 'fileFrame',
                                        style: 'height: 100%; width: 100%; border: none',
                                        src: mainPDFUrl
                                    }
                                }
                            ],
                            constrain: true
                        });
                        var nextBtn = pdfWindow.down('#btnNext');
                        nextBtn.setDisabled(true);
                        var filesgrid = pdfWindow.down('previewemailgjiattachmentgrid'),
                            filesStore = filesgrid.getStore();
                        filesStore.on('beforeload', function (store, operation) {
                            operation.params.documentId = me.controller.emailId;
                        });
                        filesStore.load();

                        var nfAppealNumber = pdfWindow.down('#nfAppealNumber'),
                            tfPrefix = pdfWindow.down('#tfPrefix'),
                            tfSuffix = pdfWindow.down('#tfSuffix'),
                            filterValue = pdfWindow.down('radiogroup[name=MessageType]').getValue();
                        if (filterValue) {
                            if (filterValue.MessageType == 0) {
                                nfAppealNumber.setValue(me.controller.appNumber);
                                tfPrefix.setValue(me.controller.appPref);
                                tfSuffix.setValue(me.controller.appSuf);
                            }
                            else {
                                nfAppealNumber.setValue(me.controller.statNumber);
                                tfPrefix.setValue(me.controller.statPref);
                                tfSuffix.setValue(me.controller.statSuf);
                            }
                        }


                        pdfWindow.show();
                    }
                }).error(function (err) {
                    //asp.unmask();
                    
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },

            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    
                    if (rec.getId()) {
                        var grid = form.down('emailgjiattachmentgrid'),
                            store = grid.getStore();
                        store.filter('GjiEmail', rec.getId());
                    }
                    else {
                        var grid = form.down('emailgjiattachmentgrid'),
                            store = grid.getStore();
                            store.clearData();
                    }
                    
                    this.controller.getAspect('emailGjiContentAspect').doInjection();
                }
            }
        },
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('emailgjigrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('email.EmailGji').load();
    }
});