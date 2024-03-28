Ext.define('B4.controller.SMEVNDFL', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'gisGkh.SignWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    smevNDFL: null,
  
    models: [
        'smev.SMEVNDFL',
        'smev.SMEVNDFLFile',
        'smev.SMEVNDFLAnswer'
    ],
    stores: [
        'smev.SMEVNDFL',
        'smev.SMEVNDFLFile',
        'smev.SMEVNDFLAnswer'
    ],
    views: [
        'smevndfl.Grid',
        'gisGkh.SignWindow',
        'smevndfl.EditWindow',
        'smevndfl.FileInfoGrid',
        'smevndfl.AnswerGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevndflGridAspect',
            gridSelector: 'smevndflgrid',
            editFormSelector: '#smevndflEditWindow',
            storeName: 'smev.SMEVNDFL',
            modelName: 'smev.SMEVNDFL',
            editWindowView: 'smevndfl.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevndflEditWindow #sendGetrequestButton'] = { 'click': { fn: this.getResp, scope: this } };
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevNDFL = record.getId();
                    var grid = form.down('smevndflfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevNDFL', record.getId());

                    var answergrid = form.down('smevndflanswergrid'),
                    answerstore = answergrid.getStore();
                    answerstore.filter('smevNDFL', record.getId());
                }
            },
            getResp: function (record) {
                var me = this;
                var taskId = smevNDFL;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVNDFLExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevndflfileinfogrid'),
                        store = grid.getStore();
                        store.filter('smevNDFL', smevNDFL);

                        var answergrid = form.down('smevndflanswergrid'),
                        answstore = answergrid.getStore();
                        answstore.filter('smevNDFL', smevNDFL);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smev.SMEVNDFL').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevndfl.Grid',
    mainViewSelector: 'smevndflgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevndflgrid'
        },
        {
            ref: 'smevndflFileInfoGrid',
            selector: 'smevndflfileinfogrid'
        }
        ,
        {
            ref: 'smevndflAnswerGrid',
            selector: 'smevndflanswergrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevndflgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } },
            'gisGkhSignWindow': { createsignature: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    onCreateSignature: function (win) {
        debugger;
        var me = this;
        certCombo = win.down('#dfCert');
        record = win.rec;
        if (!certCombo.value) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            return false;
        }
        me.mask('Подписание и отправка запроса в СМЭВ', this.getMainComponent());
        me.signRequest(record.getId(), [], win)
        //me.unmask();
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0) {
          //  Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
         //   return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');
  //      debugger;
        signwindow = Ext.create('B4.view.gisGkh.SignWindow');
        signwindow.rec = rec;
        signwindow.signer = new XadesSigner();
        signwindow.signer.getCertificates(2, 'My', me.fillCertificatesCombo, function (e) {
            Ext.Msg.alert('Ошибка получения списка сертификатов!', 'Не удалось получить сертификаты' + '<br>' + (e.message || e));
        }, me);
        signwindow.show();

        //B4.Ajax.request({
        //    url: B4.Url.action('Execute', 'SMEVNDFLExecute'),
        //    params: {
        //        taskId: rec.getId()
        //    },
        //    timeout: 9999999
        //}).next(function (response) {
        //    me.unmask();
        //    me.getStore('smev.SMEVNDFL').load();
        //    return true;
        //}).error(function () {
        //    me.unmask();
        //    me.getStore('smev.SMEVNDFL').load();
        //    return false;
        //});
    },

    getCertificates: function (win) {
        var finder = this.findCert();
        finder.then(function (certNames) {
            return certNames;
        }).then(function (val) {
            var field = win.down('#dfCert');
            var newStore = Ext.create('Ext.data.Store', {
                fields: ['cert', 'subjectName', 'thumbprint'],
                data: val
            });
            field.bindStore(newStore);
            field.getStore().load();
            return val;
        });
    },

    fillCertificatesCombo: function (certificates) {
        var me = this,
            certCombo = signwindow.down('#dfCert'),
            certComboStore = certCombo.getStore();

        certCombo.clearValue();
        certComboStore.removeAll();
        Ext.each(certificates, function (cert) {
            var certificateRec = Ext.create('B4.model.gisGkh.Certificate', {
                SubjectName: cert.subjectName,
                Certificate: cert,
                Thumbprint: cert.thumbprint
            });

            certComboStore.add(certificateRec);
        });
    },

    sendRequest: function () {
        var me = this;
            debugger;
        var reqId = signwindow.rec.getId();
        signwindow.close();
              B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVNDFLExecute'),
            params: {
                taskId: reqId
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVNDFL').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVNDFL').load();
            return false;
        });
    },

    signRequest: function (reqId, reqIdsToSign, win) {
    //    debugger;
        var me = this,
            signingResult = {
                success: true,
                message: '',
                reqId: reqId
            };

        me.fireEvent('packageSigning', {
            id: reqId
        });

        var certField = win.down('#dfCert'),
            val = certField.getValue(),
            cert,
            certBase64,
            sign;
        //if (!certField.value) {
        //    Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
        //    return;
        //}
        B4.Ajax.request({
            url: B4.Url.action('GetXml', 'SMEVNDFLExecute'),
            params: {
                reqId: reqId
            },
            timeout: 9999999
        }).next(function (response) {
            var notSignedData = Ext.decode(response.responseText).data;
            debugger;
            //if (!notSignedData || notSignedData.length === 0) {
            //    Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            //}
            if (!notSignedData || notSignedData.length === 0) {
                signingResult.success = false;
                signingResult.message = 'Не получены неподписанные данные с сервера';
               
            }
            debugger;
            var signedXml = me.createSign(val.subjectName, notSignedData, function(xml) {
            }, function (e) {
                    debugger;
                signingResult.success = false;
                signingResult.message = 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e);
                //Ext.Msg.alert('Ошибка!', 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e));
            }, me);

            debugger;
            //win.signer.setCertificate(val);
            //win.signer.signXml(notSignedData, function (xml) {
            //    debugger;
            //}, function (e) {
            //        debugger;
            //    signingResult.success = false;
            //    signingResult.message = 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e);
            //    //Ext.Msg.alert('Ошибка!', 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e));
            //}, me);
        })
            .error(function (e) {
                debugger;
                signingResult.success = false;
                signingResult.message = 'Не получены неподписанные данные с сервера' + '<br>' + (e.message || e);
                me.signNextRequest(win, reqIdsToSign, signingResult);
                //Ext.Msg.alert('Ошибка', resp.message);
                //me.unmask();
            });
    },

    checkCertificate: function (cert) {
        return B4.Ajax.request({
            url: B4.Url.action('/certificate/validate'),
            params: {
                certificate: cert
            }
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevndflGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevndflgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVNDFL').load();
    },
    CAPICOM_CURRENT_USER_STORE : 2,
    CAPICOM_MY_STORE : "My",
    CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED : 2,
    CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME : 1,
    CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED : 0,
    CADESCOM_XML_SIGNATURE_TYPE_ENVELOPING: 1,
    CADESCOM_XML_SIGNATURE_TYPE_TEMPLATE : 2,
    XmlDsigGost3411Url2012256 : "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256",
    XmlDsigGost3410Url2012256 : "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256",

    createSign: function (sCertName, notSigData) {
        signContent = notSigData;
   var me = this;
            cadesplugin.async_spawn(function* (args) {
            if ("" === sCertName) {
                alert("Введите имя сертификата (CN).");
                return;
            }

            // Ищем сертификат для подписи
            var oStore = yield cadesplugin.CreateObjectAsync("CAdESCOM.Store");
            yield oStore.Open(me.CAPICOM_CURRENT_USER_STORE, me.CAPICOM_MY_STORE,
                me.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
                var oStoreCerts = yield oStore.Certificates;
                var certCount = yield oStoreCerts.Count;
                var oCertificate = null;
                for (i = 1; i <= certCount; i++)
                {
                    var carts = yield oStoreCerts.Item(i);
                    var cert = yield carts.SubjectName;
                    if (sCertName === cert) {
                        oCertificate = yield oStoreCerts.Item(i);
                    }
                }
                
            //var oCertificates = yield oStoreCerts.Find(
            //    me.CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME, sCertName);
            //var certsCount = yield oCertificates.Count;
                if (!oCertificate) {
                Ext.Msg.alert("Certificate not found:", sCertName);
                return;
            }
                yield oStore.Close();

                var sContent =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                    "<env:Envelope xmlns:env=\"http://schemas.xmlsoap.org/soap/envelope/\">\n" +
                    "<tns:NDFL2Request xmlns:tns=\"urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1\" xml:id=\"PERSONAL_SIGNATURE\" ОтчетГод=\"2020\" ТипЗапросП=\"2\" ИдЗапрос=\"41fead94-3fbc-11eb-94ed-e4c1c3bd3c9b\">\n" +
                    "<tns:СвФЛ ДатаРожд=\"2020-10-01\" СНИЛС=\"2123123212312312\" НомЗаявФЛ=\"фывфыв\" ДатаЗаявФЛ=\"2020-12-12\">\n" +
                    "<tns:ФИОФЛ FirstName=\"фывфыв\" Patronymic=\"фывфыв\" FamilyName=\"ывфыв\" />\n" +
                    "<tns:УдЛичн DocumentCode=\"01\" SeriesNumber=\"123321231\" />\n" +
                    "</tns:СвФЛ>\n" +
                    "</tns:NDFL2Request >\n" +
                    "  <Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">\n" +
                    "  <SignedInfo>\n" +
                    "      <CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>\n" +
                    "      <SignatureMethod Algorithm=\"urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256\"/>\n" +
                    "      <Reference URI=\"#PERSONAL_SIGNATURE\">\n" +
                    "      <Transforms>\n" +
                    "          <Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\"/>\n" +
                    "      </Transforms>\n" +
                    "      <DigestMethod Algorithm=\"urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256\"/>\n" +
                    "      <DigestValue/>\n" +
                    "      </Reference>\n" +
                    "  </SignedInfo>\n" +
                    "  <SignatureValue/>\n" +
                    "  <KeyInfo/>\n" +
                    "  </Signature>\n" +
                    "</env:Envelope>";


            // Создаем объект CAdESCOM.CPSigner
            var oSigner = yield cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");
            yield oSigner.propset_Certificate(oCertificate);
            yield oSigner.propset_CheckCertificate(true);
                debugger;
            // Создаем объект CAdESCOM.SignedXML
            var oSignedXML = yield cadesplugin.CreateObjectAsync("CAdESCOM.SignedXML");
                yield oSignedXML.propset_Content(sContent);

            // Указываем тип подписи - в данном случае вложенная
           // yield oSignedXML.propset_SignatureType(me.CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED);
                //по шаблону
            yield oSignedXML.propset_SignatureType(me.CADESCOM_XML_SIGNATURE_TYPE_TEMPLATE);

            // Указываем алгоритм подписи
            //yield oSignedXML.propset_SignatureMethod(me.XmlDsigGost3410Url2012256);

            //// Указываем алгоритм хэширования
            //yield oSignedXML.propset_DigestMethod(me.XmlDsigGost3411Url2012256);
                debugger;
            var sSignedMessage = "";
            try {
                sSignedMessage = yield oSignedXML.Sign(oSigner);
            } catch (err) {
                debugger;
                Ext.Msg.alert("Failed to create signature. Error: ", err);
                return;
            }

                Ext.Msg.alert("",sSignedMessage);

            // Создаем объект CAdESCOM.SignedXML
            var oSignedXML2 = yield cadesplugin.CreateObjectAsync("CAdESCOM.SignedXML");

            //try {
            //    yield oSignedXML2.Verify(sSignedMessage);
            //    Ext.Msg.alert("Signature verified");
            //} catch (err) {
            //    Ext.Msg.alert("Failed to verify signature. Error: " + cadesplugin.getLastError(err));
            //    return;
            //}
                debugger;
                return oSignedXML2;
        });
    },

    getCertificateBySubjectName: function (certSubjectName) {
        var me = this;
        debugger;
        try {
            //var plug = signwindow.cadesplugin;
            //var stroeSert = signwindow.cadesplugin.CreateObjectAsync("CAdESCOM.Store");
            //debugger;
            //var yeldstore = cadesplugin.CreateObjectAsync("CAdESCOM.Store");
        }
        catch(e)
        {
            debugger;
        }
        debugger;
        var oStore = cadesplugin.CreateObject("CAdESCOM.Store");
        debugger;
         oStore.Open(me.CAPICOM_CURRENT_USER_STORE, me.CAPICOM_MY_STORE,
            me.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

        var oCertificates = oStore.Certificates.Find(
            me.CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME, certSubjectName);
        if (oCertificates.Count == 0) {
            alert("Certificate not found: " + certSubjectName);
            return;
        }
        var oCertificate = oCertificates.Item(1);
        oStore.Close();
        return oCertificate;
    },

    signCreate: function (oCertificate, dataToSign) {
        var me = this;
        // Создаем объект CAdESCOM.CPSigner
        var oSigner = cadesplugin.CreateObject("CAdESCOM.CPSigner");
        oSigner.Certificate = oCertificate;

        // Создаем объект CAdESCOM.SignedXML
        var oSignedXML = cadesplugin.CreateObject("CAdESCOM.SignedXML");
        oSignedXML.Content = dataToSign;

        // Указываем тип подписи - в данном случае вложенная
        oSignedXML.SignatureType = CADESCOM_XML_SIGNATURE_TYPE_ENVELOPED;

        // Указываем алгоритм подписи
        oSignedXML.SignatureMethod = XmlDsigGost3410Url;

        // Указываем алгоритм хэширования
        oSignedXML.DigestMethod = XmlDsigGost3411Url;

        var sSignedMessage = "";
        try {
            sSignedMessage = oSignedXML.Sign(oSigner);
        } catch (err) {
            alert("Failed to create signature. Error: " + cadesplugin.getLastError(err));
            return;
        }

        return sSignedMessage;
    },

    Verify: function (sSignedMessage) {
        // Создаем объект CAdESCOM.SignedXML
        var oSignedXML = cadesplugin.CreateObject("CAdESCOM.SignedXML");

        try {
            oSignedXML.Verify(sSignedMessage);
        } catch (err) {
            alert("Failed to verify signature. Error: " + cadesplugin.getLastError(err));
            return false;
        }

        return true;
    },




});