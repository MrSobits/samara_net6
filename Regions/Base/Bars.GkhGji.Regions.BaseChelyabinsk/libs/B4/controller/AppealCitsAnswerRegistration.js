Ext.define('B4.controller.AppealCitsAnswerRegistration', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhBlobText',
        'B4.Ajax',
        'B4.view.appealcits.appealcitsanswerregistration.PdfWindow',
        'B4.Url'
    ],
    models: [
        'appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration'
    ],
    stores: [
        'appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration'
    ],
    views: [
        'appealcits.appealcitsanswerregistration.Grid',
        'appealcits.appealcitsanswerregistration.EditWindow',
        'appealcits.appealcitsanswerregistration.PdfWindow'
    ],
    answerId: null,
    pref: '',
    suf: '',
    number: null,
    mainView: 'appealcits.appealcitsanswerregistration.Grid',
    mainViewSelector: 'ansreggrid',
    mixins: {
        context: 'B4.mixins.Context'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'ansreggrid'
        }
    ],
    aspects: [
        {
            xtype: 'gkhblobtextaspect',
            name: 'descriptionBlobTextAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'appCitAnsRegGridWindowAspect',
            controllerName: 'AppealCitsAnswerRegistration',
            getAction: 'GetDescription',
            saveAction: 'SaveDescription',
            valueFieldName: 'Description',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'description2BlobTextAspect',
            fieldSelector: '[name=Description2]',
            editPanelAspectName: 'appCitAnsRegGridWindowAspect',
            controllerName: 'AppealCitsAnswerRegistration',
            getAction: 'GetDescription',
            saveAction: 'SaveDescription',
            valueFieldName: 'Description2',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appCitAnsRegGridWindowAspect',
            gridSelector: 'ansreggrid',
            editFormSelector: '#ansRegEditWindow',
            modelName: 'appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration',
            storeName: 'appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration',
            editWindowView: 'appealcits.appealcitsanswerregistration.EditWindow',

            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions[this.gridSelector + ' #btnProcess'] = { 'click': { fn: this.ProcessAnswers, scope: this } };
                actions['ansRegPdfWindow #btnSkip'] = { 'click': { fn: this.SkipEmails, scope: this } };
                actions['ansRegPdfWindow #btnNext'] = { 'click': { fn: this.SkipEmails, scope: this } };
                actions['ansRegPdfWindow #btnReg'] = { 'click': { fn: this.RegisterAnswer, scope: this } };
                actions['ansRegPdfWindow #btnRegAndSend'] = { 'click': { fn: this.RegisterAndSendAnswer, scope: this } };
            },
            RegisterAnswer: function (btn) {
                var me = this;
                me.mask('Отправка запроса', B4.getBody());
                var w = btn.up('ansRegPdfWindow');
                var nfAnswerNumber = w.down('#nfAnswerNumber'),
                    tfPrefix = w.down('#tfPrefix'),
                    tfSuffix = w.down('#tfSuffix');

                me.controller.number = nfAnswerNumber.getValue() + 1;
                me.controller.pref = tfPrefix.getValue();
                me.controller.suf = tfSuffix.getValue();

                B4.Ajax.request({
                    url: B4.Url.action('RegisterAnswer', 'AppealCitsAnswerRegistration'),
                    timeout: 9999999,
                    params: {
                        answerId: me.controller.answerId,
                        number: nfAnswerNumber.getValue(),
                        pref: tfPrefix.getValue(),
                        suf: tfSuffix.getValue()
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
                var skipBtn = w.down('#btnSkip');
                var regAndSendBtn = w.down('#btnRegAndSend');

                nextBtn.setDisabled(false);
                btn.setDisabled(true);
                skipBtn.setDisabled(true);
                regAndSendBtn.setDisabled(true);
                B4.QuickMsg.msg('Email', 'Ответ успешно зарегистрирован', 'success');
                this.SkipEmails(btn);
            },
            RegisterAndSendAnswer: function (btn) {
                var me = this;
                me.mask('Отправка запроса', B4.getBody());
                var w = btn.up('ansRegPdfWindow');
                var nfAnswerNumber = w.down('#nfAnswerNumber'),
                    tfPrefix = w.down('#tfPrefix'),
                    tfSuffix = w.down('#tfSuffix');

                me.controller.number = nfAnswerNumber.getValue() + 1;
                me.controller.pref = tfPrefix.getValue();
                me.controller.suf = tfSuffix.getValue();

                B4.Ajax.request({
                    url: B4.Url.action('RegisterAndSendAnswer', 'AppealCitsAnswerRegistration'),
                    timeout: 9999999,
                    params: {
                        answerId: me.controller.answerId,
                        number: nfAnswerNumber.getValue(),
                        pref: tfPrefix.getValue(),
                        suf: tfSuffix.getValue()
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
                w.header.setTitle('Зарегистрирован и отправлен');
                w.header.titleCmp.addCls('headerregistred');
                var nextBtn = w.down('#btnNext');
                var skipBtn = w.down('#btnSkip');
                var regAndSendBtn = w.down('#btnRegAndSend');

                nextBtn.setDisabled(false);
                btn.setDisabled(true);
                skipBtn.setDisabled(true);
                regAndSendBtn.setDisabled(true);
                B4.QuickMsg.msg('Email', 'Ответ успешно зарегистрирован и отправлен', 'success');
                this.SkipEmails(btn);
            },

            ProcessAnswers: function (btn) {
                var me = this;
                me.controller.answerId = 0;
                B4.Ajax.request({
                    url: B4.Url.action('GetNextQuestion', 'AppealCitsAnswerRegistration'),
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
                        me.controller.answerId = tryDecoded.data.AnswerId;
                        var pdfWindow = Ext.create('widget.ansRegPdfWindow', {
                            layout: 'fit',
                            closeAction: 'destroy',
                            title: 'Регистрация ответа по обращению № ' + tryDecoded.data.AnswerInfo,
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
                        var nextBtn = pdfWindow.down('#btnNext'),
                            regAndSendBtn = pdfWindow.down('#btnRegAndSend'),
                            tfEmail = pdfWindow.down('#tfEmail');

                        regAndSendBtn.setDisabled(true);
                        nextBtn.setDisabled(true);

                        if (tryDecoded.data.Email != '') {
                            tfEmail.setValue(tryDecoded.data.Email);
                            regAndSendBtn.setDisabled(false);
                        }

                        pdfWindow.show();
                    }
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },

            SkipEmails: function (btn) {
                debugger;
                var w = btn.up('ansRegPdfWindow');
                w.close();
                var me = this;
                B4.Ajax.request({
                    url: B4.Url.action('SkipAndGetNextQuestion', 'AppealCitsAnswerRegistration'),
                    params: {
                        answerId: me.controller.answerId
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
                        me.controller.answerId = tryDecoded.data.AnswerId;
                        var pdfWindow = Ext.create('widget.ansRegPdfWindow', {
                            layout: 'fit',
                            title: 'Регистрация ответа по обращению № ' + tryDecoded.data.AnswerInfo,
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
                        var nextBtn = pdfWindow.down('#btnNext'),
                            regAndSendBtn = pdfWindow.down('#btnRegAndSend'),
                            tfEmail = pdfWindow.down('#tfEmail'),
                            nfAnswerNumber = pdfWindow.down('#nfAnswerNumber'),
                            tfPrefix = pdfWindow.down('#tfPrefix'),
                            tfSuffix = pdfWindow.down('#tfSuffix');

                        regAndSendBtn.setDisabled(true);
                        nextBtn.setDisabled(true);

                        if (tryDecoded.data.Email != '') {
                            tfEmail.setValue(tryDecoded.data.Email);
                            regAndSendBtn.setDisabled(false);
                        }

                        nfAnswerNumber.setValue(me.controller.number);
                        tfPrefix.setValue(me.controller.pref);
                        tfSuffix.setValue(me.controller.suf);

                        pdfWindow.show();
                    }
                }).error(function (err) {
                    //asp.unmask();
                    debugger;
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },

            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    this.controller.getAspect('descriptionBlobTextAspect').doInjection();
                    this.controller.getAspect('description2BlobTextAspect').doInjection();
                }
            }
        },
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('ansreggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration').load();
    }
});