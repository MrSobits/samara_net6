"use strict";

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var __awaiter = undefined && undefined.__awaiter || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) {
            try {
                step(generator.next(value));
            } catch (e) {
                reject(e);
            }
        }
        function rejected(value) {
            try {
                step(generator["throw"](value));
            } catch (e) {
                reject(e);
            }
        }
        function step(result) {
            result.done ? resolve(result.value) : new P(function (resolve) {
                resolve(result.value);
            }).then(fulfilled, rejected);
        }
        step((generator = generator.apply(thisArg, _arguments)).next());
    });
};

var XadesSigner = function () {
    function XadesSigner() {
        _classCallCheck(this, XadesSigner);

        this.indexAlgorithmMap = {};
        this.provider = js_xmldsig.cryptography.getProvider(js_xmldsig.cryptography.cryptopro.Id);
        this.indexAlgorithmMap["1.2.643.2.2.19"] = { signature: "http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411", digest: "http://www.w3.org/2001/04/xmldsig-more#gostr3411" };
        this.indexAlgorithmMap["1.2.643.7.1.1.1.1"] = { signature: "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256", digest: "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256" };
        this.indexAlgorithmMap["1.2.643.7.1.1.1.2"] = { signature: "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512", digest: "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512" };
    }

    _createClass(XadesSigner, [{
        key: "checkPlugin",
        value: function checkPlugin(callback) {
            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
                var loaded, version, pluginVersion, cspVersion;
                return regeneratorRuntime.wrap(function _callee$(_context) {
                    while (1) {
                        switch (_context.prev = _context.next) {
                            case 0:
                                _context.next = 2;
                                return this.provider.isLoaded();

                            case 2:
                                loaded = _context.sent;

                                if (loaded) {
                                    _context.next = 6;
                                    break;
                                }

                                callback(false);
                                return _context.abrupt("return");

                            case 6:
                                _context.next = 8;
                                return this.provider.getVersion();

                            case 8:
                                version = _context.sent;
                                _context.next = 11;
                                return version.PluginVersion;

                            case 11:
                                pluginVersion = _context.sent;
                                _context.next = 14;
                                return version.CSPVersion();

                            case 14:
                                cspVersion = _context.sent;
                                _context.t0 = callback;
                                _context.next = 18;
                                return pluginVersion.MajorVersion;

                            case 18:
                                _context.t1 = _context.sent;
                                _context.next = 21;
                                return pluginVersion.MinorVersion;

                            case 21:
                                _context.t2 = _context.sent;
                                _context.next = 24;
                                return pluginVersion.BuildVersion;

                            case 24:
                                _context.t3 = _context.sent;
                                _context.t4 = {
                                    major: _context.t1,
                                    minor: _context.t2,
                                    build: _context.t3
                                };
                                _context.next = 28;
                                return cspVersion.MajorVersion;

                            case 28:
                                _context.t5 = _context.sent;
                                _context.next = 31;
                                return cspVersion.MinorVersion;

                            case 31:
                                _context.t6 = _context.sent;
                                _context.next = 34;
                                return cspVersion.BuildVersion;

                            case 34:
                                _context.t7 = _context.sent;
                                _context.t8 = {
                                    major: _context.t5,
                                    minor: _context.t6,
                                    build: _context.t7
                                };
                                (0, _context.t0)(true, _context.t4, _context.t8);

                            case 37:
                            case "end":
                                return _context.stop();
                        }
                    }
                }, _callee, this);
            }));
        }
    }, {
        key: "getCertificates",
        value: function getCertificates(storeLocation, storeName, success, error) {
            var scope = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : window;

            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
                var store, certificates, certDtos, i, dto, cert, publicKey, algorithm;
                return regeneratorRuntime.wrap(function _callee2$(_context2) {
                    while (1) {
                        switch (_context2.prev = _context2.next) {
                            case 0:
                                _context2.prev = 0;
                                _context2.next = 3;
                                return this.provider.getStore();

                            case 3:
                                store = _context2.sent;
                                certDtos = [];
                                _context2.next = 7;
                                return store.Open(storeLocation, storeName, js_xmldsig.capicom.CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);

                            case 7:
                                _context2.next = 9;
                                return store.Certificates;

                            case 9:
                                certificates = _context2.sent;
                                i = 1;

                            case 11:
                                _context2.t0 = i;
                                _context2.next = 14;
                                return certificates.Count;

                            case 14:
                                _context2.t1 = _context2.sent;

                                if (!(_context2.t0 <= _context2.t1)) {
                                    _context2.next = 52;
                                    break;
                                }

                                dto = { thumbprint: null, simpleName: null, issuerName: null, subjectName: null, validFrom: null, validTo: null, _cert: null, algorithmValue: null };
                                _context2.next = 19;
                                return certificates.Item(i);

                            case 19:
                                cert = _context2.sent;

                                dto._cert = cert;
                                _context2.next = 23;
                                return cert.Thumbprint;

                            case 23:
                                dto.thumbprint = _context2.sent;
                                _context2.next = 26;
                                return cert.IssuerName;

                            case 26:
                                dto.issuerName = _context2.sent;
                                _context2.next = 29;
                                return cert.SubjectName;

                            case 29:
                                dto.subjectName = _context2.sent;
                                _context2.next = 32;
                                return cert.ValidFromDate;

                            case 32:
                                dto.validFrom = _context2.sent;
                                _context2.next = 35;
                                return cert.ValidToDate;

                            case 35:
                                dto.validTo = _context2.sent;
                                _context2.next = 38;
                                return cert.PublicKey();

                            case 38:
                                publicKey = _context2.sent;
                                _context2.next = 41;
                                return publicKey.Algorithm;

                            case 41:
                                algorithm = _context2.sent;
                                _context2.next = 44;
                                return algorithm.Value;

                            case 44:
                                dto.algorithmValue = _context2.sent;
                                _context2.next = 47;
                                return cert.GetInfo(js_xmldsig.capicom.CAPICOM_CERT_INFO_TYPE.CAPICOM_CERT_INFO_SUBJECT_SIMPLE_NAME);

                            case 47:
                                dto.simpleName = _context2.sent;

                                certDtos.push(dto);

                            case 49:
                                i++;
                                _context2.next = 11;
                                break;

                            case 52:
                                _context2.next = 58;
                                break;

                            case 54:
                                _context2.prev = 54;
                                _context2.t2 = _context2["catch"](0);

                                error.apply(scope, [_context2.t2]);
                                return _context2.abrupt("return");

                            case 58:
                                success.apply(scope, [certDtos]);

                            case 59:
                            case "end":
                                return _context2.stop();
                        }
                    }
                }, _callee2, this, [[0, 54]]);
            }));
        }
    }, {
        key: "setCertificate",
        value: function setCertificate(certDto) {
            this.cert = certDto._cert;
            this.algorithmValue = certDto.algorithmValue;
        }
    }, {
        key: "signXml",
        value: function signXml(xml, success, error) {
            var scope = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : window;
            debugger;
            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
                var doc, signatureMethod, digestMethod, signedXml, ref1, sig, res;
                debugger;
                return regeneratorRuntime.wrap(function _callee3$(_context3) {
                    while (1) {
                        switch (_context3.prev = _context3.next) {
                            case 0:
                                _context3.prev = 0;
                                doc = js_xmldsig.utils.parseXml(xml), signatureMethod = "", digestMethod = "";

                                if (this.indexAlgorithmMap[this.algorithmValue]) {
                                    _context3.next = 4;
                                    break;
                                }

                                throw 'Реализуемый алгоритм не подходит для подписания документа.';

                            case 4:
                                if (doc) {
                                    _context3.next = 6;
                                    break;
                                }

                                throw 'XML пуста';

                            case 6:
                                signatureMethod = this.indexAlgorithmMap[this.algorithmValue].signature;
                                digestMethod = this.indexAlgorithmMap[this.algorithmValue].digest;
                                signedXml = new js_xmldsig.signature.SignedXml(doc, this.cert);
                                debugger;
                                signedXml.signature.signedInfo.canonicalizationMethod = 'http://www.w3.org/2001/10/xml-exc-c14n#';
                                signedXml.signature.signedInfo.signatureMethod = signatureMethod;
                                ref1 = signedXml.signature.signedInfo.addReference(new js_xmldsig.signature.base.Reference(new js_xmldsig.signature.base.Uri(doc.documentElement)));

                                ref1.digestMethod = digestMethod;
                                ref1.addTransform(new js_xmldsig.signature.base.Transform('http://www.w3.org/2000/09/xmldsig#enveloped-signature'));
                                ref1.addTransform(new js_xmldsig.signature.base.Transform('http://www.w3.org/2001/10/xml-exc-c14n#'));
                                signedXml.signature.keyInfo.addContent(new js_xmldsig.signature.base.keyinfo.X509Certificate());
                                js_xmldsig.xades.Apply(signedXml, digestMethod);
                                _context3.next = 19;
                                return signedXml.createSignature();

                            case 19:
                                sig = _context3.sent;
                                debugger;
                                _context3.next = 22;
                                return signedXml.createSignedXml();

                            case 22:
                                res = _context3.sent;
                                _context3.next = 29;
                                break;

                            case 25:
                                _context3.prev = 25;
                                _context3.t0 = _context3["catch"](0);

                                error.apply(scope, [_context3.t0]);
                                return _context3.abrupt("return");

                            case 29:
                                success.apply(scope, [js_xmldsig.utils.stringifyNode(res), js_xmldsig.utils.stringifyNode(sig)]);

                            case 30:
                            case "end":
                                return _context3.stop();
                        }
                    }
                }, _callee3, this, [[0, 25]]);
            }));
        }
    }]);

    return XadesSigner;
}();

window.XadesSigner = XadesSigner;