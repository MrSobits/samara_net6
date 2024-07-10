Ext.define('B4.aspects.GkhGjiNestedDigitalSignatureGridAspect', {
    extend: 'B4.base.Aspect',
    alias: 'widget.gkhgjinesteddigitalsignaturegridaspect',

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
        var file = record.get('File' || 'FileDoc');
        var signed = false;
        if (this.signedFileField != null) {
            if (record.get(this.signedFileField) != null) {
                signed = true;
            }
        }
        if (t != null) {
            if (signed || file == null) {
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
            pdfUrl = B4.Url.action(Ext.String.format("/{0}/{1}?xmlId={2}", me.controllerName, me.pdfAction, id));

        if (grid.mask) {
            grid.mask('Подготовка данных...', Ext.getBody());
        }
        me.showFrame(pdfUrl);
        grid.unmask();

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

    signHash: function (data, cert) {
        return new Promise(function (resolve) {
            cadesplugin.async_spawn(function* () {
                let oSigner = yield cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");

                oSigner.propset_Certificate(cert);
                oSigner.propset_Options(cadesplugin.CAPICOM_CERTIFICATE_INCLUDE_WHOLE_CHAIN);

                let hashObject = yield cadesplugin.CreateObjectAsync("CAdESCOM.HashedData");

                let certPublicKey = yield cert.PublicKey();
                let certAlgorithm = yield certPublicKey.Algorithm;
                let algorithmValue = yield certAlgorithm.Value;

                if (algorithmValue === "1.2.643.7.1.1.1.1") {
                    yield hashObject.propset_Algorithm(cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256);
                } else if (algorithmValue === "1.2.643.7.1.1.1.2") {
                    yield hashObject.propset_Algorithm(cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_512);
                } else if (algorithmValue === "1.2.643.2.2.19") {
                    yield hashObject.propset_Algorithm(cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411);
                } else {
                    alert("Невозможно подписать документ этим сертификатом");
                    return;
                }
                //в объект описания hash вставляем уже готовый hash с сервера
                yield hashObject.SetHashValue(data);

                let oSignedData = yield cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");
                oSignedData.propset_ContentEncoding(cadesplugin.CADESCOM_BASE64_TO_BINARY);
                //результат подписания в base64
                let signatureHex = yield oSignedData.SignHash(hashObject, oSigner, 1);

                resolve(signatureHex);
            });
        })
    },

    getCertData: function (cert) {
        return new Promise(function (resolve, reject) {
            cadesplugin.async_spawn(
                regeneratorRuntime.mark(function _callee() {
                    var certData, certificate, SubjectName, SerialNumber, ValidFromDate, ValidToDate;
                    return regeneratorRuntime.wrap(function _callee$(_context) {
                        while (1) {
                            switch (_context.prev = _context.next) {
                                case 0:
                                    certificate = cert;
                                    _context.next = 1;
                                    break;
                                case 1:
                                    _context.prev = 1;
                                    _context.t1 = String;
                                    _context.next = 2;
                                    return certificate.SubjectName;

                                case 2:
                                    _context.t2 = _context.sent;
                                    SubjectName = new _context.t1(_context.t2);
                                    _context.next = 4;
                                    break;

                                case 3:
                                    _context.prev = 3;
                                    _context.t3 = _context["catch"](1);
                                    alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(_context.t3));

                                case 4:
                                    _context.prev = 4;
                                    _context.t4 = String;
                                    _context.next = 5;
                                    return certificate.SerialNumber;

                                case 5:
                                    _context.t5 = _context.sent;
                                    SerialNumber = new _context.t4(_context.t5);
                                    _context.next = 7;
                                    break;

                                case 6:
                                    _context.prev = 6;
                                    _context.t6 = _context["catch"](4);
                                    alert("Ошибка при получении свойства SerialNumber: " + cadesplugin.getLastError(_context.t6));

                                case 7:
                                    _context.prev = 7;
                                    _context.t7 = Date;
                                    _context.next = 8;
                                    return certificate.ValidFromDate;

                                case 8:
                                    _context.t8 = _context.sent;
                                    ValidFromDate = new _context.t7(_context.t8);
                                    _context.next = 10;
                                    break;

                                case 9:
                                    _context.prev = 9;
                                    _context.t9 = _context["catch"](7);
                                    alert("Ошибка при получении свойства ValidFromDate: " + cadesplugin.getLastError(_context.t9));

                                case 10:
                                    _context.prev = 10;
                                    _context.t10 = Date;
                                    _context.next = 11;
                                    return certificate.ValidToDate;

                                case 11:
                                    _context.t11 = _context.sent;
                                    ValidToDate = new _context.t10(_context.t11);
                                    _context.next = 13;
                                    break;

                                case 12:
                                    _context.prev = 12;
                                    _context.t12 = _context["catch"](10);
                                    alert("Ошибка при получении свойства ValidToDate: " + cadesplugin.getLastError(_context.t12));

                                case 13:
                                    certData = {
                                        cert: certificate,
                                        subjectName: SubjectName,
                                        serialNumber: SerialNumber,
                                        validFromDate: ValidFromDate,
                                        validToDate: ValidToDate
                                    };
                                    _context.next = 14;

                                case 14:
                                    resolve(certData);

                                case 15:
                                case "end":
                                    return _context.stop();
                            }
                        }
                    }, _callee, null, [[1, 3], [4, 6], [7, 9], [10, 12]]);
                }), resolve, reject);
        });
    },

    onCreateSignature: function (win) {
        var me = this,
            id = this.getId(),
            grid = me.getGrid(),
            certField = win.down('combo'),
            val = certField.getValue();

        if (!val) {
            Ext.Msg.alert('Внимание!', 'Не выбран сертификат!');
            return false;
        }

        me.getGrid().mask('Подписывание', me.getGrid());

        var certData = me.getCertData(val);

        certData.then(
            function (result) {
                var subjectName = result.subjectName,
                    serialNumber = result.serialNumber,
                    validFromDate = result.validFromDate,
                    validToDate = result.validToDate;

                B4.Ajax.request({
                    url: B4.Url.action("GetPdfHash", me.controllerName),
                    params: {
                        Id: id,
                        SubjectName: subjectName,
                        SerialNumber: serialNumber,
                        ValidFromDate: validFromDate,
                        ValidToDate: validToDate
                    },
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

                    var signature = me.signHash(me.getDataToSign(), val);

                    signature.then(
                        function (result) {
                            B4.Ajax.request({
                                url: B4.Url.action("PostSignature", me.controllerName),
                                params: {
                                    Id: id,
                                    Signature: result
                                },
                                timeout: 9999999
                            }).next(function (resp) {
                                B4.QuickMsg.msg('Сообщение', 'Документ успешно подписан', 'success');
                                me.setSignatureCreated(true);
                                me.getGrid().unmask();
                                me.getGrid().getStore().load();
                                Ext.ComponentQuery.query('pdfWindow')[0].close();
                            }).error(function (e) {
                                B4.QuickMsg.msg('Сообщение', 'Ошибка при подписании: ' + (e.message || e), 'error');
                                me.getGrid().unmask();
                                Ext.ComponentQuery.query('pdfWindow')[0].close();
                            })
                        }
                    );

                }).error(function (e) {
                    B4.QuickMsg.msg('Ошибка!', 'Не удалось получить данные для подписания!', 'error');
                    grid.unmask();
                });
            }
        );
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