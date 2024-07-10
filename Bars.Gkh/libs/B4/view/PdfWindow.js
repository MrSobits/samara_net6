Ext.define('B4.view.PdfWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.pdfWindow',

    requires: [
        'B4.mixins.MaskBody'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    closeAction: 'close',
    modal: true,
    signAspect: null,

    layout: {
        type: 'fit',
        align: 'stretch'
    },

    width: 900,
    height: 550,

    title: 'Подписываемый документ',

    maximizable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Подписать',
                            iconCls: 'icon-accept',
                            handler: function (b) {
                                var win = b.up('pdfWindow');
                                win.fireEvent('createsignature', win);
                            }
                        },
                        {
                            xtype: 'combo',
                            flex: 1,
                            //editable: false,
                            itemId: 'dfCert',
                            valueField: 'cert',
                            displayField: 'subjectName'
                        }
                    ]
                }
            ]
        });
        me.getCertificates();
        me.callParent(arguments);
    },

    // прототип ES6
    //findCert: function () {
    //    return new Promise(function (resolve, reject) {
    //        cadesplugin.async_spawn(function* () {
    //            var MyStoreExists = true;
    //            try {
    //                var oStore = yield cadesplugin.CreateObjectAsync("CAdESCOM.Store");
    //                if (!oStore) {
    //                    alert("Create store failed");
    //                    return;
    //                }

    //                yield oStore.Open(cadesplugin.CAPICOM_CURRENT_USER_STORE,
    //                    cadesplugin.CAPICOM_MY_STORE,
    //                    cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
    //            }
    //            catch (ex) {
    //                MyStoreExists = false;
    //            }
    //            var certCnt;
    //            var certs;
    //            if (MyStoreExists) {
    //                try {
    //                    certs = yield oStore.Certificates;
    //                    certCnt = yield certs.Count;
    //                }
    //                catch (ex) {
    //                    alert("Ошибка при получении Certificates или Count: " + cadesplugin.getLastError(ex));
    //                    return;
    //                }
    //                var certNames = [];
    //                for (var i = 1; i <= certCnt; i++) {
    //                    var cert;
    //                    try {
    //                        cert = yield certs.Item(i);
    //                    }
    //                    catch (ex) {
    //                        alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(ex));
    //                        return;
    //                    }

    //                    try {
    //                        var SubjectName = new String(yield cert.SubjectName);
    //                    }
    //                    catch (ex) {
    //                        alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(ex));
    //                    }
    //                    try {
    //                        var Thumbprint = new String(yield cert.Thumbprint);
    //                    }
    //                    catch (ex) {
    //                        alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(ex));
    //                    }
    //                    certNames.push({ cert: cert, subjectName: SubjectName, thumbprint: Thumbprint });

    //                }
    //                yield oStore.Close();
    //            }

    //            //В версии плагина 2.0.13292+ есть возможность получить сертификаты из 
    //            //закрытых ключей и не установленных в хранилище
    //            try {
    //                yield oStore.Open(cadesplugin.CADESCOM_CONTAINER_STORE);
    //                certs = yield oStore.Certificates;
    //                certCnt = yield certs.Count;
    //                for (var i = 1; i <= certCnt; i++) {
    //                    var cert = yield certs.Item(i);
    //                    //Проверяем не добавляли ли мы такой сертификат уже?
    //                    var found = false;
    //                    for (var j = 0; j < certNames.length; j++) {
    //                        if ((yield certNames[j].Thumbprint) === (yield cert.Thumbprint)) {
    //                            found = true;
    //                            break;
    //                        }
    //                    }
    //                    if (found)
    //                        continue;

    //                    try {
    //                        var SubjectName = new String(yield cert.SubjectName);
    //                    }
    //                    catch (ex) {
    //                        alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(ex));
    //                    }
    //                    try {
    //                        var Thumbprint = new String(yield cert.Thumbprint);
    //                    }
    //                    catch (ex) {
    //                        alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(ex));
    //                    }
    //                    certNames.push({ cert: cert, subjectName: SubjectName, thumbprint: Thumbprint });
    //                }
    //                yield oStore.Close();

    //            }
    //            catch (ex) {
    //            }
    //            resolve(certNames);

    //        }, resolve, reject);
    //    });
    //},

    findCert: function () {
        return new Promise(function (resolve, reject) {
            cadesplugin.async_spawn(
                /*#__PURE__*/
                regeneratorRuntime.mark(function _callee() {
                    var MyStoreExists, oStore, certCnt, certs, certNames, i, cert, SubjectName, Thumbprint, found, j;
                    return regeneratorRuntime.wrap(function _callee$(_context) {
                        while (1) {
                            switch (_context.prev = _context.next) {
                                case 0:
                                    MyStoreExists = true;
                                    _context.prev = 1;
                                    _context.next = 4;
                                    return cadesplugin.CreateObjectAsync("CAdESCOM.Store");

                                case 4:
                                    oStore = _context.sent;

                                    if (oStore) {
                                        _context.next = 8;
                                        break;
                                    }

                                    alert("Create store failed");
                                    return _context.abrupt("return");

                                case 8:
                                    _context.next = 10;
                                    return oStore.Open(cadesplugin.CAPICOM_CURRENT_USER_STORE, cadesplugin.CAPICOM_MY_STORE, cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

                                case 10:
                                    _context.next = 15;
                                    break;

                                case 12:
                                    _context.prev = 12;
                                    _context.t0 = _context["catch"](1);
                                    MyStoreExists = false;

                                case 15:
                                    if (!MyStoreExists) {
                                        _context.next = 70;
                                        break;
                                    }

                                    _context.prev = 16;
                                    _context.next = 19;
                                    return oStore.Certificates;

                                case 19:
                                    certs = _context.sent;
                                    _context.next = 22;
                                    return certs.Count;

                                case 22:
                                    certCnt = _context.sent;
                                    _context.next = 29;
                                    break;

                                case 25:
                                    _context.prev = 25;
                                    _context.t1 = _context["catch"](16);
                                    alert("Ошибка при получении Certificates или Count: " + cadesplugin.getLastError(_context.t1));
                                    return _context.abrupt("return");

                                case 29:
                                    certNames = [];
                                    i = 1;

                                case 31:
                                    if (!(i <= certCnt)) {
                                        _context.next = 68;
                                        break;
                                    }

                                    _context.prev = 32;
                                    _context.next = 35;
                                    return certs.Item(i);

                                case 35:
                                    cert = _context.sent;
                                    _context.next = 42;
                                    break;

                                case 38:
                                    _context.prev = 38;
                                    _context.t2 = _context["catch"](32);
                                    alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(_context.t2));
                                    return _context.abrupt("return");

                                case 42:
                                    _context.prev = 42;
                                    _context.t3 = String;
                                    _context.next = 46;
                                    return cert.SubjectName;

                                case 46:
                                    _context.t4 = _context.sent;
                                    SubjectName = new _context.t3(_context.t4);
                                    _context.next = 53;
                                    break;

                                case 50:
                                    _context.prev = 50;
                                    _context.t5 = _context["catch"](42);
                                    alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(_context.t5));

                                case 53:
                                    _context.prev = 53;
                                    _context.t6 = String;
                                    _context.next = 57;
                                    return cert.Thumbprint;

                                case 57:
                                    _context.t7 = _context.sent;
                                    Thumbprint = new _context.t6(_context.t7);
                                    _context.next = 64;
                                    break;

                                case 61:
                                    _context.prev = 61;
                                    _context.t8 = _context["catch"](53);
                                    alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(_context.t8));

                                case 64:
                                    certNames.push({
                                        cert: cert,
                                        subjectName: SubjectName,
                                        thumbprint: Thumbprint
                                    });

                                case 65:
                                    i++;
                                    _context.next = 31;
                                    break;

                                case 68:
                                    _context.next = 70;
                                    return oStore.Close();

                                case 70:
                                    _context.prev = 70;
                                    _context.next = 73;
                                    return oStore.Open(cadesplugin.CADESCOM_CONTAINER_STORE);

                                case 73:
                                    _context.next = 75;
                                    return oStore.Certificates;

                                case 75:
                                    certs = _context.sent;
                                    _context.next = 78;
                                    return certs.Count;

                                case 78:
                                    certCnt = _context.sent;
                                    i = 1;

                                case 80:
                                    if (!(i <= certCnt)) {
                                        _context.next = 127;
                                        break;
                                    }

                                    _context.next = 83;
                                    return certs.Item(i);

                                case 83:
                                    cert = _context.sent;
                                    //Проверяем не добавляли ли мы такой сертификат уже?
                                    found = false;
                                    j = 0;

                                case 86:
                                    if (!(j < certNames.length)) {
                                        _context.next = 99;
                                        break;
                                    }

                                    _context.next = 89;
                                    return certNames[j].Thumbprint;

                                case 89:
                                    _context.t9 = _context.sent;
                                    _context.next = 92;
                                    return cert.Thumbprint;

                                case 92:
                                    _context.t10 = _context.sent;

                                    if (!(_context.t9 === _context.t10)) {
                                        _context.next = 96;
                                        break;
                                    }

                                    found = true;
                                    return _context.abrupt("break", 99);

                                case 96:
                                    j++;
                                    _context.next = 86;
                                    break;

                                case 99:
                                    if (!found) {
                                        _context.next = 101;
                                        break;
                                    }

                                    return _context.abrupt("continue", 124);

                                case 101:
                                    _context.prev = 101;
                                    _context.t11 = String;
                                    _context.next = 105;
                                    return cert.SubjectName;

                                case 105:
                                    _context.t12 = _context.sent;
                                    SubjectName = new _context.t11(_context.t12);
                                    _context.next = 112;
                                    break;

                                case 109:
                                    _context.prev = 109;
                                    _context.t13 = _context["catch"](101);
                                    alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(_context.t13));

                                case 112:
                                    _context.prev = 112;
                                    _context.t14 = String;
                                    _context.next = 116;
                                    return cert.Thumbprint;

                                case 116:
                                    _context.t15 = _context.sent;
                                    Thumbprint = new _context.t14(_context.t15);
                                    _context.next = 123;
                                    break;

                                case 120:
                                    _context.prev = 120;
                                    _context.t16 = _context["catch"](112);
                                    alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(_context.t16));

                                case 123:
                                    certNames.push({
                                        cert: cert,
                                        subjectName: SubjectName,
                                        thumbprint: Thumbprint
                                    });

                                case 124:
                                    i++;
                                    _context.next = 80;
                                    break;

                                case 127:
                                    _context.next = 129;
                                    return oStore.Close();

                                case 129:
                                    _context.next = 133;
                                    break;

                                case 131:
                                    _context.prev = 131;
                                    _context.t17 = _context["catch"](70);

                                case 133:
                                    resolve(certNames);

                                case 134:
                                case "end":
                                    return _context.stop();
                            }
                        }
                    }, _callee, null, [[1, 12], [16, 25], [32, 38], [42, 50], [53, 61], [70, 131], [101, 109], [112, 120]]);
                }), resolve, reject);
        });
    },

    getCertificates: function () {
        var me = this;
        var canAsync = !!cadesplugin.CreateObjectAsync;
        if (canAsync) {
            var finder = me.findCert();
            finder.then(function (certNames) {
                return certNames;
            }).then(function (val) {
                var field = me.down('#dfCert');
                var newStore = Ext.create('Ext.data.Store', {
                    fields: ['cert', 'subjectName', 'thumbprint'],
                    data: val
                });
                field.bindStore(newStore);
                field.getStore().load();
                return val;
            });
        }
        else {
            var MyStoreExists = true;
            try {
                var oStore = cadesplugin.CreateObject("CAdESCOM.Store");
                oStore.Open();
            }
            catch (ex) {
                MyStoreExists = false;
            }
            var certCnt;
            if (MyStoreExists) {
                certCnt = oStore.Certificates.Count;
                var certNames = [];
                for (var i = 1; i <= certCnt; i++) {
                    var cert;
                    try {
                        cert = oStore.Certificates.Item(i);
                    }
                    catch (ex) {
                        alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(ex));
                        return;
                    }
                    try {
                        //var certObj = new CertificateObj(cert, true);
                        var SubjectName = cert.SubjectName;
                    }
                    catch (ex) {
                        alert("Ошибка при получении свойства SubjectName: " + cadesplugin.getLastError(ex));
                    }
                    try {
                        var Thumbprint = cert.Thumbprint;
                    }
                    catch (ex) {
                        alert("Ошибка при получении свойства Thumbprint: " + cadesplugin.getLastError(ex));
                    }
                    certNames.push({ cert: cert, subjectName: SubjectName, thumbprint: Thumbprint });
                }
                oStore.Close();
                
                var field = Ext.getCmp('#dfCert')
                //var field = me.down('#dfCert');
                var newStore = Ext.create('Ext.data.Store', {
                    fields: ['cert', 'subjectName', 'thumbprint'],
                    data: certNames
                });
                field.bindStore(newStore);
                field.getStore().load();
            }
            return;
        }
    }
});