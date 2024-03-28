Ext.define('B4.aspects.GkhGjiDigitalSignatureGridAspect', {
    extend: 'B4.base.Aspect',
    alias: 'widget.gkhgjidigitalsignaturegridaspect',

    requires: [
        'B4.view.PdfWindow'
    ],

    gridSelector: null,

    controllerName: null,

    idProperty: null,

    signedFileField: null,

    dataAction: 'getdata',

    pdfAction: 'getxml',

    signatureCreated: false,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.gridSelector] = {
            'itemclick': {
                fn: this.itemClick,
                scope: this
            }
        };

        actions[this.gridSelector + ' b4signbutton'] = {
            'click': {
                fn: this.signBtnClick,
                scope: this
            }
        };

        actions['pdfWindow'] = {
            //createsignature: {
            //    fn: this.onCreateSignature,
            //    scope: this
            //},
            close: {
                fn: this.onPdfWindowClose,
                scope: this
            }
        };
        controller.control(actions);
    },

    getGrid: function () {
        if (this.gridSelector) {
            return this.componentQuery(this.gridSelector);
        }
        return null;
    },

    signBtnClick: function () {
        this.prepareData();
    },

    itemClick: function (row, record) {
        var t = this.getGrid();
        var signed = false;
        if (this.signedFileField != null)
        {
            if (record.get(this.signedFileField) != null)
            {
                signed = true;
            }
        }
        if (t != null) {
            if (signed) {
                try {
                    t.down('b4signbutton').disable();
                } catch (e) {
                    var f = e;
                }
            }
            else {
                try {
                    t.down('b4signbutton').enable();
                } catch (e) {
                    var f = e;
                }
            }
        }
        this.setId(record.get(this.idProperty || 'Id'));
    },

    getId: function () {
        return this.controller.entityId;
    },

    setId: function (id) {
        this.controller.entityId = id;
    },

    getDataToSign: function () {
        return this.controller.dataToSign;
    },

    setDataToSign: function (data) {
        this.controller.dataToSign = data;
    },

    setDataIds: function (data) {
        this.controller.xmlId = data;
    },

    getDataIds: function () {
        return this.controller.xmlId;
    },

    setSignatureCreated: function (val) {
        this.controller.signatureCreated = val;
    },

    getSignatureCreated: function () {
        return this.controller.signatureCreated === true;
    },

    prepareData: function () {
        var id = this.getId();
        var me = this,
            grid = me.getGrid(),
            dataUrl = B4.Url.action(Ext.String.format("/{0}/{1}?Id={2}", this.controllerName, this.dataAction, id));

        if (grid.mask) {
            grid.mask('Подготовка данных...', Ext.getBody());
        }
        B4.Ajax.request({
            url: dataUrl,
            params: { Id: id },
            timeout: 9999999
        }).next(function (resp) {
            var txt = resp.responseText,
                decoded = Ext.decode(txt);
            if (!decoded.success) {
                B4.QuickMsg.msg('Ошибка!', decoded.message, 'error');
                grid.unmask();
                return;
            }
            me.setDataToSign(decoded.data.dataToSign);
            me.setDataIds(decoded.data.xmlId);

            grid.unmask();

            var pdfUrl = B4.Url.action(Ext.String.format("/{0}/{1}?xmlId={2}", me.controllerName, me.pdfAction, decoded.data.xmlId));
            me.showFrame(pdfUrl);
        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка!', 'Не удалось получить данные для подписания!', 'error');
            grid.unmask();
        });
    },

    showFrame: function (url) {
        var pdfWindow = Ext.create('widget.pdfWindow', {
            layout: 'fit',
            signAspect: this,
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
        var panel = this.getGrid().up();
        if (panel.activeTab != null) {
            panel.getActiveTab().add(pdfWindow);
        }
        else {
            panel.add(pdfWindow);
        }
        pdfWindow.show();
        pdfWindow.on('createsignature', function () {
            this.signAspect.onCreateSignature(this);
        });
    },

    //// прототип ES6
    //SignCreate: function(cert, dataToSign, oldScope) {
    //    return new Promise(function (resolve, reject) {
    //        cadesplugin.async_spawn(function* (args) {
    //            try {
    //                var oCertificate = cert;
    //                certBase64: undefined;
    //                oCertificate.Export(Crypto.constants.encodingType.CAPICOM_ENCODE_BASE64)
    //                    .then((res) => {
    //                        certBase64 = res;
    //                    });
    //                try {
    //                    var oSigner = yield cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");
    //                } catch (err) {
    //                    throw "Failed to create CAdESCOM.CPSigner: " + err.number;
    //                }
    //                var oSigningTimeAttr = yield cadesplugin.CreateObjectAsync("CADESCOM.CPAttribute");
    //                var CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME = 0;
    //                yield oSigningTimeAttr.propset_Name(CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME);
    //                var oTimeNow = new Date();
    //                yield oSigningTimeAttr.propset_Value(oTimeNow);
    //                var attr = yield oSigner.AuthenticatedAttributes2;
    //                yield attr.Add(oSigningTimeAttr);

    //                var oDocumentNameAttr = yield cadesplugin.CreateObjectAsync("CADESCOM.CPAttribute");
    //                var CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME = 1;
    //                yield oDocumentNameAttr.propset_Name(CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME);
    //                yield oDocumentNameAttr.propset_Value("Document Name");
    //                yield attr.Add(oDocumentNameAttr);

    //                if (oSigner) {
    //                    yield oSigner.propset_Certificate(cert);
    //                }
    //                else {
    //                    throw "Failed to create CAdESCOM.CPSigner";
    //                }
    //                var oSignedData = yield cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");
    //                var CADES_BES = 1;

    //                if (dataToSign) {
    //                    // Данные на подпись ввели
    //                    yield oSignedData.propset_ContentEncoding(1); //CADESCOM_BASE64_TO_BINARY
    //                    yield oSignedData.propset_Content(dataToSign);
    //                    yield oSigner.propset_Options(1); //CAPICOM_CERTIFICATE_INCLUDE_WHOLE_CHAIN
    //                    try {
    //                        //var StartTime = Date.now();
    //                        sSignedMessage = yield oSignedData.SignCades(oSigner, CADES_BES);
    //                        args[2]({ certBase64, sSignedMessage, oldScope });
    //                        //var EndTime = Date.now();
    //                        //document.getElementsByName('TimeTitle')[0].innerHTML = "Время выполнения: " + (EndTime - StartTime) + " мс";
    //                    }
    //                    catch (err) {
    //                        throw "Не удалось создать подпись из-за ошибки: " + err.message;
    //                    }
    //                }
    //            }
    //            catch (err) {
    //                args[3](err);
    //            }
    //        }, cert, dataToSign, resolve, reject);
    //    });
    //},

    SignCreate: function (cert, dataToSign, oldScope) {
        return new Promise(function (resolve, reject) {
            debugger;
            cadesplugin.async_spawn(
                /*#__PURE__*/
                regeneratorRuntime.mark(function _callee(args) {
                    var oCertificate, oSigner, oSigningTimeAttr, CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME, oTimeNow, attr, oDocumentNameAttr, CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME, oSignedData, CADES_BES;
                    return regeneratorRuntime.wrap(function _callee$(_context) {
                        while (1) {
                            switch (_context.prev = _context.next) {
                                case 0:
                                    _context.prev = 0;
                                    oCertificate = cert;

                                    certBase64: undefined;

                                    oCertificate.Export(Crypto.constants.encodingType.CAPICOM_ENCODE_BASE64).then(function (res) {
                                        certBase64 = res;
                                    });
                                    _context.prev = 4;
                                    _context.next = 7;
                                    return cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");

                                case 7:
                                    oSigner = _context.sent;
                                    _context.next = 13;
                                    break;

                                case 10:
                                    _context.prev = 10;
                                    _context.t0 = _context["catch"](4);
                                    throw "Failed to create CAdESCOM.CPSigner: " + _context.t0.number;

                                case 13:
                                    _context.next = 15;
                                    return cadesplugin.CreateObjectAsync("CADESCOM.CPAttribute");

                                case 15:
                                    oSigningTimeAttr = _context.sent;
                                    CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME = 0;
                                    _context.next = 19;
                                    return oSigningTimeAttr.propset_Name(CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME);

                                case 19:
                                    oTimeNow = new Date();
                                    _context.next = 22;
                                    return oSigningTimeAttr.propset_Value(oTimeNow);

                                case 22:
                                    _context.next = 24;
                                    return oSigner.AuthenticatedAttributes2;

                                case 24:
                                    attr = _context.sent;
                                    _context.next = 27;
                                    return attr.Add(oSigningTimeAttr);

                                case 27:
                                    _context.next = 29;
                                    return cadesplugin.CreateObjectAsync("CADESCOM.CPAttribute");

                                case 29:
                                    oDocumentNameAttr = _context.sent;
                                    CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME = 1;
                                    _context.next = 33;
                                    return oDocumentNameAttr.propset_Name(CADESCOM_AUTHENTICATED_ATTRIBUTE_DOCUMENT_NAME);

                                case 33:
                                    _context.next = 35;
                                    return oDocumentNameAttr.propset_Value("Document Name");

                                case 35:
                                    _context.next = 37;
                                    return attr.Add(oDocumentNameAttr);

                                case 37:
                                    if (!oSigner) {
                                        _context.next = 42;
                                        break;
                                    }

                                    _context.next = 40;
                                    return oSigner.propset_Certificate(cert);

                                case 40:
                                    _context.next = 43;
                                    break;

                                case 42:
                                    throw "Failed to create CAdESCOM.CPSigner";

                                case 43:
                                    _context.next = 45;
                                    return cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");

                                case 45:
                                    oSignedData = _context.sent;
                                    CADES_BES = 1;

                                    if (!dataToSign) {
                                        _context.next = 64;
                                        break;
                                    }

                                    _context.next = 50;
                                    return oSignedData.propset_ContentEncoding(1);

                                case 50:
                                    _context.next = 52;
                                    return oSignedData.propset_Content(dataToSign);

                                case 52:
                                    _context.next = 54;
                                    return oSigner.propset_Options(1);

                                case 54:
                                    _context.prev = 54;
                                    _context.next = 57;
                                    return oSignedData.SignCades(oSigner, CADES_BES);

                                case 57:
                                    sSignedMessage = _context.sent;
                                    args[2]({
                                        certBase64: certBase64,
                                        sSignedMessage: sSignedMessage,
                                        oldScope: oldScope
                                    }); //var EndTime = Date.now();
                                    //document.getElementsByName('TimeTitle')[0].innerHTML = "Время выполнения: " + (EndTime - StartTime) + " мс";

                                    _context.next = 64;
                                    break;

                                case 61:
                                    _context.prev = 61;
                                    _context.t1 = _context["catch"](54);
                                    throw "Не удалось создать подпись из-за ошибки: " + _context.t1.message;

                                case 64:
                                    _context.next = 69;
                                    break;

                                case 66:
                                    _context.prev = 66;
                                    _context.t2 = _context["catch"](0);
                                    args[3](_context.t2);

                                case 69:
                                case "end":
                                    return _context.stop();
                            }
                        }
                    }, _callee, null, [[0, 66], [4, 10], [54, 61]]);
                }), cert, dataToSign, resolve, reject);
        });
    },

    onCreateSignature: function (win) {
        var me = this;
        id = this.getId();

        var certField = win.down('combo'),
            val = certField.getValue();

        if (!val) {
            Ext.Msg.alert('Внимание!', 'Не выбран сертификат!');
            return false;
        }

        try {
            var cert = val;
            var oldScope = me;
            var thenable = me.SignCreate(cert, me.getDataToSign(), oldScope);
            sign: undefined;

            thenable.then(
                function (result) {
                    var me = result.oldScope;
                    me.checkCertificate(certBase64).next(function () {
                        var sign = result.sSignedMessage;
                        var certBase64 = result.certBase64;
                        var url = B4.Url.action(Ext.String.format('/{0}/sign', me.controllerName));

                        var xmlId = me.getDataIds();

                        me.getGrid().mask('Подписывание', me.getGrid());
                        B4.Ajax.request({
                            url: url,
                            params: {
                                Id: id,
                                xmlId: xmlId,
                                sign: sign,
                                certificate: certBase64
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('Сообщение', 'Документ успешно подписан', 'success');
                            me.setSignatureCreated(true);
                            me.getGrid().unmask();
                            me.getGrid().getStore().load();
                            Ext.ComponentQuery.query('pdfWindow')[0].close();
                        }).error(function (e) {
                            B4.QuickMsg.msg('Сообщение', 'Ошибка при подписании: ' + (e.message || e), 'error');
                            me.getGrid().unmask();
                            Ext.ComponentQuery.query('pdfWindow')[0].close();
                        });
                    }).error(function (resp) {
                        B4.QuickMsg.msg('Ошибка', 'Ошибка проверки корретности сертификата:' + (resp.message || resp), 'error');
                        Ext.ComponentQuery.query('pdfWindow')[0].close();
                    });
                },
                function (result) {
                    B4.QuickMsg.msg('Сообщение', result, 'error');
                });
        } catch (e) {
            B4.QuickMsg.msg('Ошибка', e, 'error');
            Ext.ComponentQuery.query('pdfWindow')[0].close();
        }
    },

    onPdfWindowClose: function () {
        this.deleteTmpDocs();
    },

    deleteTmpDocs: function () {
        var me = this,
            data = me.getDataIds();

        if (me.getSignatureCreated() === false && data) {
            B4.Ajax.request({
                url: B4.Url.action(Ext.String.format('/{0}/deletedocs', this.controllerName)),
                params: {
                    xmlId: data.xmlId,
                    pdfId: data.pdfId
                }
            });
            me.setDataIds(null);
        }
    },

    checkCertificate: function (cert) {
        return B4.Ajax.request({
            url: B4.Url.action('/certificate/validate'),
            params: {
                certificate: cert
            }
        });
    }
});