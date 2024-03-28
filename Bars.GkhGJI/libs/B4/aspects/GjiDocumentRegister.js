/*
Данный аспект предназначен для описание взаимодействия компонентов грида Документов ГЖИ
и открытия карточки Проверки по inspection_id
*/


Ext.define('B4.aspects.GjiDocumentRegister', {
    extend: 'B4.aspects.GridEditForm',

    requires:
        [
            'B4.DisposalTextValues',
            'B4.enums.TypeBase'
        ],

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
                    typeDocument: typeDocument,
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
                portal.loadController(me.controllerEditName, inspect, portal.containerSelector, me.controller.hideMask);
            }
        }
    },

    getControllerName: function (typeBase) {
        var controllerName = '';
        switch (typeBase) {
            case B4.enums.TypeBase.Inspection: controllerName = 'B4.controller.baseinscheck.Navigation'; break;

            case B4.enums.TypeBase.CitizenStatement: controllerName = 'B4.controller.basestatement.Navigation'; break;

            case B4.enums.TypeBase.PlanJuridicalPerson: controllerName = 'B4.controller.basejurperson.Navigation'; break;

            case B4.enums.TypeBase.DisposalHead: controllerName = 'B4.controller.basedisphead.Navigation'; break;

            case B4.enums.TypeBase.GjiWarning: controllerName = 'B4.controller.warninginspection.Navigation'; break;

            case B4.enums.TypeBase.ProsecutorsClaim: controllerName = 'B4.controller.baseprosclaim.Navigation'; break;

            case B4.enums.TypeBase.ProsecutorsResolution: controllerName = 'B4.controller.resolpros.Navigation'; break;

            case B4.enums.TypeBase.ActivityTsj: controllerName = 'B4.controller.baseactivitytsj.Navigation'; break;

            case B4.enums.TypeBase.HeatingSeason: controllerName = 'B4.controller.baseheatseason.Navigation'; break;

            case B4.enums.TypeBase.ProtocolMvd: controllerName = 'B4.controller.protocolmvd.Navigation'; break;

            case B4.enums.TypeBase.PlanAction: controllerName = 'B4.controller.baseplanaction.Navigation'; break;

            case B4.enums.TypeBase.ProtocolMhc: controllerName = 'B4.controller.protocolmhc.Navigation'; break;

            case B4.enums.TypeBase.LicenseApplicants: controllerName = 'B4.controller.baselicenseapplicants.Navigation'; break;

            case B4.enums.TypeBase.ProtocolGji: controllerName = 'B4.controller.tatarstanprotocolgji.Navigation'; break;

            case B4.enums.TypeBase.InspectionActionIsolated: controllerName = 'B4.controller.inspectionactionisolated.Navigation'; break;

            case B4.enums.TypeBase.ActionIsolated: controllerName = 'B4.controller.actionisolated.Navigation'; break;

            case B4.enums.TypeBase.PreventiveAction: controllerName = 'B4.controller.preventiveaction.Navigation'; break;

            case B4.enums.TypeBase.MotivatedPresentationAppealCits: controllerName = 'B4.controller.appealcits.Navigation'; break;

            case B4.enums.TypeBase.InspectionPreventiveAction: controllerName = 'B4.controller.inspectionpreventiveaction.Navigation'; break;

            case B4.enums.TypeBase.Default: controllerName = 'B4.controller.basedefault.Navigation'; break;
        }

        return controllerName;
    },

    getDefaultParams: function (typeDocument) {
        var controllerName = '', docName = '';
        switch (typeDocument) {
            //Распоряжение
            case 10:
                controllerName = 'B4.controller.Disposal';
                docName = B4.DisposalTextValues.getSubjectiveCase();
                break;

            //Предостережение
            case 11:
                controllerName = 'B4.controller.WarningDoc';
                docName = 'Предостережение';
                break;

            //Акт проверки
            case 20:
                controllerName = 'B4.controller.ActCheck';
                docName = 'Акт проверки';
                break;

            //Акт устранения нарушений
            case 30:
                controllerName = 'B4.controller.ActRemoval';
                docName = 'Акт проверки';
                break;

            //Акт обследования
            case 40:
                controllerName = 'B4.controller.ActSurvey';
                docName = 'Акт обследования';
                break;

            //Предписание
            case 50:
                controllerName = 'B4.controller.Prescription';
                docName = 'Предписание';
                break;

            //Протокол
            case 60:
                controllerName = 'B4.controller.ProtocolGji';
                docName = 'Протокол';
                break;

            //Постановление
            case 70:
                controllerName = 'B4.controller.Resolution';
                docName = 'Постановление';
                break;

            //Постановление прокуратуры
            case 80:
                controllerName = 'B4.controller.ResolutionProsecutor';
                docName = 'Постановление прокуратуры';
                break;

            //Представление
            case 90:
                controllerName = 'B4.controller.Presentation';
                docName = 'Представление';
                break;

            //Постановление Роспотребнадзора
            case 150:
                controllerName = 'B4.controller.ResolutionRospotrebnadzor';
                docName = 'Постановление Роспотребнадзора';
                break;

            //Постановление по протокол ст.20.6.1 КоАП РФ
            case 170:
                controllerName = 'B4.controller.tatarstanresolutiongji.Edit';
                docName = 'Постановление ГЖИ';
                break;

            //Задание КНМ
            case 180:
                controllerName = 'B4.controller.actionisolated.taskaction.TaskAction';
                docName = 'Задание КНМ';
                break;

            //Профилактическое мероприятие
            case 190:
                controllerName = 'B4.controller.preventiveaction.Edit';
                docName = 'Профилактическое мероприятие';
                break;

            //Акт по КНМ без взаимодействия с контролируемыми лицами
            case 200:
                controllerName = 'B4.controller.actionisolated.actaction.ActActionIsolated';
                docName = 'Акт по КНМ без взаимодействия с контролируемыми лицами';
                break;

            // Лист визита
            case 210:
                controllerName = 'B4.controller.preventiveaction.Visit';
                docName = 'Лист визита';
                break;

            //Задание профилактического мероприятия
            case 220:
                controllerName = 'B4.controller.preventiveaction.task.Task';
                docName = 'Задание профилактического мероприятия';
                break;

            //Мотивированное представление
            case 230:
                controllerName = 'B4.controller.actionisolated.motivatedpresentation.MotivatedPresentation';
                docName = 'Мотивированное представление';
                break;

            //Решение
            case 240:
                controllerName = 'B4.controller.Disposal';
                docName = 'Решение';
                break;

            //Мотивированные представления по обращениям граждан
            case 250:
                controllerName = 'B4.controller.appealcits.MotivatedPresentation';
                docName = 'Мотивированное представление по обращению граждан';
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