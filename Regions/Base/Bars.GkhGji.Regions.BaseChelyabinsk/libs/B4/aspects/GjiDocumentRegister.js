/*
Данный аспект предназначен для описание взаимодействия компонентов грида Документов ГЖИ
и открытия карточки Проверки по inspection_id
*/

Ext.define('B4.aspects.GjiDocumentRegister', {
    extend: 'B4.aspects.GridEditForm',

    requires: ['B4.DisposalTextValues',
               'B4.enums.TypeDocumentGji'],

    alias: 'widget.gjidocumentregisteraspect',

    /*
    Перекрываем метод для того чтобы по нажатию на карандаш открыть карточку Проверки и передать туда параметры
    */
    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null;

        if (id) {
            me.controllerEditName = me.getControllerName(record.get('TypeBase'));
            var inspect = me.getInspectionRecord(record);

            // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
            var typeDocument = record.get('TypeDocumentGji');
            if (typeDocument) {
                var defaultParams = me.getDefaultParams(typeDocument);
                inspect.defaultController = defaultParams.controllerName;
                inspect.defaultParams = {
                                            inspectionId: record.get('InspectionId'),
                                            documentId: record.getId(),
                                            title: defaultParams.docName
                };
            }
            
            if (me.controllerEditName) {
                var portal = me.controller.getController('PortalController');
                
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.controller.hideMask) {
                    me.controller.hideMask = function () { me.controller.unmask(); };
                }
                
                me.controller.mask('Загрузка', me.controller.getMainComponent());

                //у данного документа нет основания и на форме редактирования не нужно древовидное меню и формирования документов
                //if (typeDocument == B4.enums.TypeDocumentGji.Protocol197) { //Протокол по ст.19.7 КоАП РФ
                //    portal.loadController(inspect.defaultController, record, portal.containerSelector, me.controller.hideMask);
                //} else {
                    portal.loadController(me.controllerEditName, inspect, portal.containerSelector, me.controller.hideMask);
               // }
            }
        }
    },

    getControllerName: function (typeBase) {
        var controllerName = '';
        switch (typeBase) {
            //Инспекционная проверка                 
            case 10: controllerName = 'B4.controller.baseinscheck.Navigation'; break;

                //Обращение граждан                  
            case 20: controllerName = 'B4.controller.basestatement.Navigation'; break;

                //Плановая проверка юр лиц                
            case 30: controllerName = 'B4.controller.basejurperson.Navigation'; break;

            //Плановая проверка омсу              
            case 31: controllerName = 'B4.controller.baseomsu.Navigation'; break;

                //Распоряжение руководства               
            case 40: controllerName = 'B4.controller.basedisphead.Navigation'; break;

                //Требование прокуратуры                 
            case 50: controllerName = 'B4.controller.baseprosclaim.Navigation'; break;

                //Постановление прокуратуры                  
            case 60: controllerName = 'B4.controller.resolpros.Navigation'; break;

                //Проверка деятельности ТСЖ                   
            case 70: controllerName = 'B4.controller.baseactivitytsj.Navigation'; break;

                //Отопительный сезон                    
            case 80: controllerName = 'B4.controller.baseheatseason.Navigation'; break;
                
                //Протокол МВД
            case 100: controllerName = 'B4.controller.protocolmvd.Navigation'; break;

                //План мероприятий                   
            case 110: controllerName = 'B4.controller.baseplanaction.Navigation'; break;

                //План мероприятий                   
            case 120: controllerName = 'B4.controller.protocolmhc.Navigation'; break;

            //Протокол 19.7                 
            case 140: controllerName = 'B4.controller.protocol197.Navigation'; break;

            //Проверка соискателей лицензии
            case 130: controllerName = 'B4.controller.baselicenseapplicants.Navigation'; break;

            //Проверка лицензиата
            case 135: controllerName = 'B4.controller.baselicensereissuance.Navigation'; break;

                //Без основания                     
            case 150: controllerName = 'B4.controller.basedefault.Navigation'; break;
        }

        return controllerName;
    },
    
    getDefaultParams: function (typeDocument) {
        var controllerName = '', docName = '';
        switch (typeDocument) {
        //Распоряжение               
        case 10:
            {
                controllerName = 'B4.controller.Disposal';
                docName = B4.DisposalTextValues.getSubjectiveCase();
            }
                break;

            case 15:
                {
                    controllerName = 'B4.controller.Decision';
                    docName = 'Решение';
                }
                break;

                //Акт проверки                 
            case 20:
                {
                    controllerName = 'B4.controller.ActCheck';
                    docName = 'Акт проверки';
                }
                break;

                //Акт устранения нарушений                
            case 30:
                {
                    controllerName = 'B4.controller.ActRemoval';
                    docName = 'Акт проверки';
                }
                break;

                //Акт обследования               
            case 40:
                {
                    controllerName = 'B4.controller.ActSurvey';
                    docName = 'Акт обследования';
                }
                break;

                //Предписание                 
            case 50:
                {
                    controllerName = 'B4.controller.Prescription';
                    docName = 'Предписание';
                }
                break;

                //Протокол                  
            case 60:
                {
                    controllerName = 'B4.controller.ProtocolGji';
                    docName = 'Протокол';
                }
                break;

                //Постановление                   
            case 70:
                {
                    controllerName = 'B4.controller.Resolution';
                    docName = 'Постановление';
                }
                break;

                //Постановление прокуратуры                    
            case 80:
                {
                    controllerName = 'B4.controller.ResolutionProsecutor';
                    docName = 'Постановление прокуратуры';
                }
                break;

                //Представление                     
            case 90:
                {
                    controllerName = 'B4.controller.Presentation';
                    docName = 'Представление';
                }
                break;

                //Протокол по ст.19.7 КоАП РФ                     
            case 140:
                {
                    controllerName = 'B4.controller.protocol197.Edit';
                    docName = 'Протокол по ст.19.7 КоАП РФ';
                }
                break;

            case 220:
                {
                    controllerName = 'B4.controller.PreventiveVisit';
                    docName = 'Акт профилактического визита';
                }
                break;
        }

        return { controllerName: controllerName, docName: docName };
    },

    getInspectionRecord: function (document) {
        if (document.get('InspectionType') == 60) {
            //Если Проверка по Постановлению прокуратуры то возвращаем запись документа
            return document;
        } else {
            //Иначе получаем модель Инспекции
            var model = this.controller.getModel('InspectionGji');
            return new model({ Id: document.get('InspectionId') });
        }
    }
});