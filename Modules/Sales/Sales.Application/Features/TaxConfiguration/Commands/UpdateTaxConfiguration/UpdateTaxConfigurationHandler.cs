using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;
using Sales.Domain.Entities;

namespace Sales.Application.Features.TaxConfiguration.Commands.UpdateTaxConfiguration;

public record UpdateTaxConfigurationCommand(
    string CompanyCen,
    UpdateTaxConfigurationContractRequest Request
) : IRequest<TaxConfigurationContractResponse>;

public class UpdateTaxConfigurationHandler(
    ITaxConfigurationRepository taxRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaxConfigurationCommand, TaxConfigurationContractResponse>
{
    public async Task<TaxConfigurationContractResponse> Handle(UpdateTaxConfigurationCommand command, CancellationToken cancellationToken)
    {
        var config = await taxRepository.GetByCompanyCenAsync(command.CompanyCen, cancellationToken);
        
        if (config == null)
        {
            config = Sales.Domain.Entities.TaxConfiguration.Create(command.CompanyCen, (decimal)command.Request.GlobalTaxPercentage);
            await taxRepository.AddAsync(config, cancellationToken);
        }
        else
        {
            config.Update((decimal)command.Request.GlobalTaxPercentage);
            await taxRepository.UpdateAsync(config, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TaxConfigurationContractResponse(
            config.CompanyCen,
            (double)config.GlobalTaxRate
        );
    }
}
