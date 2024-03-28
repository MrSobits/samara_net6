var Crypto = {
    constants: {
        findType: {
            CAPICOM_CERTIFICATE_FIND_SHA1_HASH: 0,
            CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME: 1,
            CAPICOM_CERTIFICATE_FIND_ISSUER_NAME: 2,
            CAPICOM_CERTIFICATE_FIND_ROOT_NAME: 3,
            CAPICOM_CERTIFICATE_FIND_TEMPLATE_NAME: 4,
            CAPICOM_CERTIFICATE_FIND_EXTENSION: 5,
            CAPICOM_CERTIFICATE_FIND_EXTENDED_PROPERTY: 6,
            CAPICOM_CERTIFICATE_FIND_APPLICATION_POLICY: 7,
            CAPICOM_CERTIFICATE_FIND_CERTIFICATE_POLICY: 8,
            CAPICOM_CERTIFICATE_FIND_TIME_VALID: 9,
            CAPICOM_CERTIFICATE_FIND_TIME_NOT_YET_VALID: 10,
            CAPICOM_CERTIFICATE_FIND_TIME_EXPIRED: 11,
            CAPICOM_CERTIFICATE_FIND_KEY_USAGE: 12
        },
        encodingType: {
            CAPICOM_ENCODE_BASE64: 0,
            CAPICOM_ENCODE_BINARY: 1
        },
        storeLocation: {
            CAPICOM_MEMORY_STORE: 0,
            CAPICOM_LOCAL_MACHINE_STORE: 1,
            CAPICOM_CURRENT_USER_STORE: 2,
            CAPICOM_ACTIVE_DIRECTORY_USER_STORE: 3,
            CAPICOM_SMART_CARD_USER_STORE: 4
        },
        storeName: {
            CAPICOM_MY_STORE: "My",
            CAPICOM_CA_STORE: "Ca",
            CAPICOM_ROOT_STORE: "Root",
            CAPICOM_OTHER_STORE: "AddressBook"
        },
        cryptoPro: {
            CADES_BES: 0x01,
            CADESCOM_CADES_DEFAULT: 0x00,
            CADESCOM_CADES_X_LONG_TYPE_1: 0x5D,
            CADESCOM_STRING_TO_UCS2LE: 0x00,
            CADESCOM_BASE64_TO_BINARY: 0x01
        },
        verifyFlag: {
            CAPICOM_VERIFY_SIGNATURE_ONLY: 0,
            CAPICOM_VERIFY_SIGNATURE_AND_CERTIFICATE: 1
        },
        keyUsage: {
            CAPICOM_DIGITAL_SIGNATURE_KEY_USAGE: 128,
            CAPICOM_NON_REPUDIATION_KEY_USAGE: 64,
            CAPICOM_KEY_ENCIPHERMENT_KEY_USAGE: 32,
            CAPICOM_DATA_ENCIPHERMENT_KEY_USAGE: 16,
            CAPICOM_KEY_AGREEMENT_KEY_USAGE: 8,
            CAPICOM_KEY_CERT_SIGN_KEY_USAGE: 4,
            CAPICOM_OFFLINE_CRL_SIGN_KEY_USAGE: 2,
            CAPICOM_CRL_SIGN_KEY_USAGE: 2,
            CAPICOM_ENCIPHER_ONLY_KEY_USAGE: 1,
            CAPICOM_DECIPHER_ONLY_KEY_USAGE: 32768
        },
        certificateInclude: {
            CAPICOM_CERTIFICATE_INCLUDE_CHAIN_EXCEPT_ROOT: 0,
            CAPICOM_CERTIFICATE_INCLUDE_WHOLE_CHAIN: 1,
            CAPICOM_CERTIFICATE_INCLUDE_END_ENTITY_ONLY: 2
        },
        error: {
            Signer: 'Ошибка создания объекта для подписи!',
            SignedData: 'Ошибка создания структуры подписи!',
            XmlSigner: 'Ошибка создания для подписи XML!',
            XmlSign: 'Ошибка подписи XML сообщения!'
        },
        digestMethod: {
            Gost3411: 'http://www.w3.org/2001/04/xmldsig-more#gostr3411'
        },
        signatureMethod: {
            Gost3411: 'http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411'
        }
    },

    isCryptoEnabled: false,

    cryptoEnabled: function () {
        return this.isCryptoEnabled;
    },

    isIE: function () {
        return navigator.appName == 'Microsoft Internet Explorer';
    },

    getStoreString: function () {
        return 'CAPICOM.Store';
    },

    getSignerString: function () {
        if (!this.isIE()) {
            return 'CAdESCOM.CPSigner';
        }
        return 'CAPICOM.Signer';
    },

    getXmlSignerString: function () {
        return 'CAdESCOM.SignedXML';
    },

    getSignedDataString: function () {
        if (!this.isIE()) {
            return 'CAdESCOM.CadesSignedData';
        }
        return 'CAPICOM.SignedData';
    },

    alert: function (message) {
        if (typeof Ext != "undefined") {
            Ext.Msg.alert('Ошибка', message);
        }
        else {
            alert(message);
        }
    },

    /**
    * Проверяет наличие установленного плагина Крипто Про
    * @method checkForCryptoPlugin
    */
    checkForCryptoPlugin: function () {
        if (this.isCryptoEnabled) {
            return true;
        }
        debugger;
        switch (navigator.appName) {
            
            case 'Microsoft Internet Explorer':
                try {
                    var obj = new ActiveXObject("CAdESCOM.CPSigner");
                    this.isCryptoEnabled = true;
                } catch (err) {
                    this.isCryptoEnabled = false;
                }
                break;
            default:
                debugger;
                var mimetype = navigator.mimeTypes["application/x-cades"];
                if (mimetype) {
                    var plugin = mimetype.enabledPlugin;
                    var objectString = "<object id=\"cadesplugin\" type=\"application/x-cades\" style=\"width:0;height:0;visibility:hidden;\"></object>";
                    if (plugin) {
                        if (typeof jQuery != "undefined")
                            jQuery(objectString).appendTo(document.body);
                        else if (typeof Ext != "undefined")
                            Ext.append(Ext.getBody(), objectString);
                        else {
                            var body = document.body;
                            var cadesElement = document.createElement("object");
                            cadesElement.id = "cadesplugin";
                            cadesElement.type = "application/x-cades";
                            cadesElement.style.display = "none";

                            body.appendChild(cadesElement);
                        }

                        this.isCryptoEnabled = true;
                    }
                }
                break;
        }
        return this.isCryptoEnabled;
    },

    /**
    * Создает экземпляр нужного криптографического типа
    * @method createCryptoObject
    * @param {type} string Наименование требуемого типа
    * return Object
    */
    createCryptoObject: function (type) {
        if (!this.cryptoEnabled()) {
            if (!this.checkForCryptoPlugin()) {
                this.alert("Необходимо установить КриптоПро browser plugin!");
                throw "No crypto plugin error";
            }
        }

        switch (navigator.appName) {
            case 'Microsoft Internet Explorer':
                return new ActiveXObject(type);
            default:
                var cadesobject = document.getElementById("cadesplugin");
                return cadesobject.CreateObject(type);
        }
    },

    /**
    * Получает хранилище сертификатов текущего пользователя
    * @method getCurrentUserMyStore
    * return CAPICOM.Store
    */
    getCurrentUserMyStore: function () {
        var store = this.createCryptoObject('CAPICOM.Store');
        if (store) {
            store.Open(this.constants.storeLocation.CAPICOM_CURRENT_USER_STORE, this.constants.storeName.CAPICOM_MY_STORE);
            return store;
        }

        throw "Не удалось получить доступ к хранилищу сертификатов.";
    },

    /**
    * Возвращает массив сертификатов
    * @method getCertificates
    * @param {type} CAPICOM.Store Открытое хранилище сертификатов
    * @param {type} Boolean Ignore IE default select certificate window
    * return Array
    */
    getCertificates: function (store, ignoreIeAutoSelect) {
        if (this.isIE() && !ignoreIeAutoSelect) {
            try {
                var certificates = store.Certificates.Select();
            }
            catch (e) {
                throw "Ошибка получения сертификата: " + e.message;
            }
            if (certificates && certificates.Count > 0)
                return [{
                    thumbprint: certificates.Item(1).Thumbprint,
                    serial: certificates.Item(1).SerialNumber,
                    subjectName: certificates.Item(1).SubjectName,
                    issuerName: certificates.Item(1).IssuerName
                }];
            return [];
        }

        var certificates = [];
        for (var i = 1; i <= store.Certificates.Count; ++i) {
            var certificate = store.Certificates.Item(i);
            certificates.push({
                thumbprint: certificate.Thumbprint,
                serial: certificate.SerialNumber,
                subjectName: certificate.SubjectName,
                issuerName: certificate.IssuerName
            });
        }

        return certificates;
    },

    /**
    * Возвращает массив сертификатов. Сертификаты годны на текущий момент и годятся для подписи
    * @method getCertificates
    * @param {type} CAPICOM.Store Открытое хранилище сертификатов
    * return Array
    */
    getSignatureCertificatesTimeValid: function (store) {
        var certificates = store.Certificates
            .Find(this.constants.findType.CAPICOM_CERTIFICATE_FIND_TIME_VALID)
            .Find(this.constants.findType.CAPICOM_CERTIFICATE_FIND_KEY_USAGE, this.constants.keyUsage.CAPICOM_DIGITAL_SIGNATURE_KEY_USAGE);

        if (this.isIE()) {
            try {
                var certificate = certificates.Select();
            }
            catch (e) {
                throw "Ошибка получения сертификата: " + e.message;
            }
            if (certificate && certificate.Count > 0)
                return [{
                    thumbprint: certificate.Item(1).Thumbprint,
                    serial: certificate.Item(1).SerialNumber,
                    subjectName: certificate.Item(1).SubjectName,
                    issuerName: certificate.Item(1).IssuerName
                }];

            throw 'В хранилище сертификатов нет подходящих сертификатов.';
        }

        var result = [];
        for (var i = 1; i <= certificates.Count; ++i) {
            var certificate = certificates.Item(i);
            result.push({
                thumbprint: certificate.Thumbprint,
                serial: certificate.SerialNumber,
                subjectName: certificate.SubjectName,
                issuerName: certificate.IssuerName
            });
        }

        if (result.length == 0) {
            throw 'В хранилище сертификатов нет подходящих сертификатов.';
        }

        return result;
    },

    /**
    * Возвращает сертификат формата X509
    * @method getCertificate
    * @param {type} CAPICOM.Store Открытое хранилище сертификатов
    * @param {type} int Тип поиска
    * @param {type} string/int Значение для поиска
    * return Object
    */
    getCertificate: function (store, findType, findValue) {
        var certificates = store.Certificates.Find(findType, findValue, false);
        if (certificates && certificates.Count > 0) {
            return certificates.Item(1);
        }

        return null;
    },

    /**
    * Осуществляет подпись данных
    * @method sign
    * @param {data} string Данные для подписи в формате base64
    * @param {certificate} Сертификат для подписи
    * return string
    */
    sign: function (data, certificate) {
        try {
            var signer = this.createCryptoObject(this.getSignerString());
        }
        catch (e) {
            throw 'Ошибка получения объекта для подписания: ' + e.message;
        }

        if (signer) {
            signer.Certificate = certificate;
            signer.Options = this.constants.certificateInclude.CAPICOM_CERTIFICATE_INCLUDE_END_ENTITY_ONLY;
        }
        else {
            throw 'Не удалось получить объект для подписания';
        }

        var signedData = this.createCryptoObject(this.getSignedDataString());

        if (!this.isIE()) {
            signedData.ContentEncoding = this.constants.cryptoPro.CADESCOM_BASE64_TO_BINARY;
        }

        signedData.Content = data;

        try {
            var signature = this.isIE() ?
                signedData.Sign(signer, true /*detached*/, this.constants.encodingType.CAPICOM_ENCODE_BASE64)
                : signedData.SignCades(signer, this.constants.cryptoPro.CADES_BES, true /*detached*/, this.constants.encodingType.CAPICOM_ENCODE_BASE64);
            return signature;
        }
        catch (e) {
            throw 'Ошибка подписания документа: ' + e.message;
        }
    },

    /**
    * Осуществляет соподпись данных
    * @method coSign
    * @param {signed} string Подписанные данные
    * @param {data} string Данные для подписи в формате base64
    * @param {certificate} Сертификат для подписи
    * return string
    */
    coSign: function (signed, data, certificate) {
        try {
            var signer = this.createCryptoObject(this.getSignerString());
        }
        catch (e) {
            throw 'Ошибка получения объекта для подписания: ' + e.message;
        }

        var signedData = this.createCryptoObject(this.getSignedDataString());

        if (!this.isIE()) {
            signedData.ContentEncoding = this.constants.cryptoPro.CADESCOM_BASE64_TO_BINARY;
        }
        signedData.Content = data;

        try {
            /*
            * На этом этапе в signedData.Content будут исходные данные
            */

            if (this.isIE()) {
                signedData.Verify(signed, true /*detached*/, this.constants.verifyFlag.CAPICOM_VERIFY_SIGNATURE_ONLY);
            }
            else {
                signedData.VerifyCades(signed, this.constants.cryptoPro.CADES_BES, true /*detached*/);
            }
        }
        catch (e) {
            throw "Ошибка при соподписи: " + e.message;
        }

        signer.Certificate = certificate;

        if (!this.isIE()) {
            signedData.ContentEncoding = this.constants.cryptoPro.CADESCOM_BASE64_TO_BINARY;
        }

        try {
            var signature = this.isIE() ?
                signedData.CoSign(signer, this.constants.encodingType.CAPICOM_ENCODE_BASE64)
                : signedData.CoSignCades(signer, this.constants.cryptoPro.CADES_BES, this.constants.encodingType.CAPICOM_ENCODE_BASE64);
            return signature;
        }
        catch (e) {
            throw "Ошибка соподписи: " + e.message;
        }
    },

    /**
    * Осуществляет проверку подписанных данных
    * @method verify
    * @param {signed} string Подписанные данные в формате base64
    * @param {detached} bool Подпись отделаенная или нет
    * @param {content} string Исходные данные для проверки отделенной подписи
    * return bool
    */
    verify: function (signed, detached, content) {
        var signedData = this.createCryptoObject(this.getSignedDataString());
        if (detached) {
            if (!this.isIE()) {
                signedData.ContentEncoding = this.constants.cryptoPro.CADESCOM_BASE64_TO_BINARY;
            }
            signedData.Content = content;
        }

        if (this.isIE()) {
            try {
                signedData.Verify(signed, detached, this.constants.verifyFlag.CAPICOM_VERIFY_SIGNATURE_ONLY);
                return true;
            }
            catch (e) {
                throw 'Ошибка проверки подписи: ' + e.message;
            }
        }
        else {
            try {
                signedData.VerifyCades(signed, this.constants.cryptoPro.CADES_BES, detached);
                return true;
            }
            catch (e) {
                throw 'Ошибка проверки подписи: ' + e.message;
            }
        }
    },

    /**
    * Подпись Xml содержимого
    * @method signXml
    * @param {certificate} Certificate сертификат подписи
    * @param {dataBase64} string Данные Xml, перекодированные в base64
    * return string
    * warning Not tested
    */
    signXml: function (certificate, dataBase64) {
        debugger;
        if (!this.checkForCryptoPlugin()) {
            this.alert("Shit");
            return false;
        }

        var signer;
        try {
            signer = this.createCryptoObject(this.getSignerString());
        } catch (e) {
            this.alert(this.constants.error.Signer + '<br>' + e);
            throw e;
        }
        signer.Certificate = certificate;

        var xmlSigner;
        try {
            xmlSigner = this.createCryptoObject(this.getXmlSignerString());
        } catch (e) {
            this.alert(this.constants.error.XmlSigner + '<br>' + e);
            throw e;
        }

        xmlSigner.Content = dataBase64;
        xmlSigner.DigestMethod = this.getXmlDigestMethod(); // implement
        xmlSigner.SignatureMethod = this.getXmlSignatureMethod(); // implement
        xmlSigner.SignatureType = this.getXmlSignatureType(); // implement

        try {
            return xmlSigner.Sign(signer);
        } catch (e) {
            this.alert(this.constants.error.XmlSign + '<br>' + e);
            throw e;
        }
    },

    // FOR testing
    run: function () {
        // var storeObj = this.createCryptoObject(this.getStoreString());
        var myStore = this.getCurrentUserMyStore();

        //var certs = this.getCertificates(myStore); 
        var certs = this.getSignatureCertificatesTimeValid(myStore);
        // var signature = this.sign('Messgae.', certs[0]);

        var content = 'Message.';
        var cert = this.getCertificate(myStore, this.constants.findType.CAPICOM_CERTIFICATE_FIND_SHA1_HASH, certs[0].thumbprint);
        var signature = this.sign(content, cert);
        var signatureDuplicate = this.sign(content, cert);
        if (signature == signatureDuplicate) {
            alert('signature and signatureDuplicate are equal!');
        } else {
            alert('signature and signatureDuplicate are NOT equal!!');
        }

        var certsOnClient = this.verify(signature, true, content);
        if (certsOnClient != null) {
            var cert1 = certsOnClient.Item(1);
            alert('success verify. ' + cert1.SubjectName);
        }

        var signature2 = this.coSign(signature, content, cert);
        var certs2OnClient = this.verify(signature2, true, content);
    }
}