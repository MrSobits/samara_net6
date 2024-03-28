using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Qa.Utils;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{

    [Binding]
    public class StructuralElementSteps : BindingBase
    {

        [Given(@"пользователь добавляет Тип группы ООИ с Наименованием ""(.*)"" и Кодом ""(.*)""")]
        public void ДопустимПользовательДобавляетТипГруппыООИСНаименованиемИКодом(string name, string code)
        {
            GroupTypeHelper.Current = new GroupType();
            GroupTypeHelper.Current.Name = name;
            GroupTypeHelper.Current.Code = code;
            Container.Resolve<IDomainService<GroupType>>().SaveOrUpdate(GroupTypeHelper.Current);
        }

        [Given(@"пользователь добавляет объект общего имущества с Наименованием ""(.*)"" и Кодом ""(.*)""")]
        public void ДопустимПользовательДобавляетОбъектОбщегоИмуществаСНаименованиемИКодом(string name, string code)
        {
            CommonEstateObjectHelper.Current = new CommonEstateObject();
            CommonEstateObjectHelper.Current.Name = name;
            CommonEstateObjectHelper.Current.Code = code;
            CommonEstateObjectHelper.Current.ShortName = name;
            CommonEstateObjectHelper.Current.GroupType = GroupTypeHelper.Current;
            Container.Resolve<IDomainService<CommonEstateObject>>().SaveOrUpdate(CommonEstateObjectHelper.Current);
        }


        [Given(@"пользователь добавляет группу конструктивных элементов ""(.*)""")]
        public void ДопустимПользовательДобавляетГруппуКонструктивныхЭлементов(string name)
        {
            StructuralElementGroupHelper.Current = new StructuralElementGroup();
            StructuralElementGroupHelper.Current.Formula = "";
            StructuralElementGroupHelper.Current.FormulaName = "";
            StructuralElementGroupHelper.Current.CommonEstateObject = CommonEstateObjectHelper.Current;
            StructuralElementGroupHelper.Current.Name = name;
            Container.Resolve<IDomainService<StructuralElementGroup>>().SaveOrUpdate(StructuralElementGroupHelper.Current);
        }

        [Given(@"пользователь добавляет конструктивный элемент ""(.*)""")]
        public void ДопустимПользовательДобавляетКонструктивныйЭлемент(string name)
        {
            StructuralElementHelper.Current = new StructuralElement();
            StructuralElementHelper.Current.Name = name;
            StructuralElementHelper.Current.Code = "12334";
            StructuralElementHelper.Current.UnitMeasure = UnitMeasureHelper.Current;
            StructuralElementHelper.Current.Group = StructuralElementGroupHelper.Current;
            Container.Resolve<IDomainService<StructuralElement>>().SaveOrUpdate(StructuralElementHelper.Current);
        }
    }

}
