namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class OverhaulProposalServiceInterceptor : EmptyDomainInterceptor<OverhaulProposal>
    {
        public override IDataResult BeforeCreateAction(IDomainService<OverhaulProposal> service, OverhaulProposal entity)
        {
            //проверяем, есть ли предложение с таким домом и такой программой
            try
            {
                if (service.GetAll().Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.ObjectCr.Id == entity.ObjectCr.Id))
                {
                    return Failure("Предложение КР с таким жилым домом и программой уже существует!");
                }
            }
            catch (Exception e)
            {
                return Failure("Ошибка создания объекта");
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<OverhaulProposal> service, OverhaulProposal entity)
        {
            var proposalWorkService = Container.Resolve<IDomainService<OverhaulProposalWork>>();
            if (!entity.State.StartState)
            {
                return Failure("Удаление при данном статусе запрещено");
            }
            try
            {
                var proposalWorkList = proposalWorkService.GetAll().Where(x => x.OverhaulProposal.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in proposalWorkList)
                {
                    proposalWorkService.Delete(value);
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка удаления работ " + e.Message);
            }
            finally
            {
                Container.Release(proposalWorkService);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<OverhaulProposal> service, OverhaulProposal entity)
        {
            //проверяем, есть ли предложение с таким домом и такой программой
            var worksDomain = this.Container.Resolve<IDomainService<OverhaulProposalWork>>();
            try
            {
                var opWorks = worksDomain.GetAll()
                    .Where(x => x.OverhaulProposal.Id == entity.Id)
                    .Select(x => new
                    {                       
                        x.Work.Description
                    }).ToList();              
                string str = string.Join(",", opWorks.Select(x => x.Description));
                
                entity.Description = str;
                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка создания объекта");
            }
            finally
            {
                Container.Release(worksDomain);
            }

        }
       
    }
}