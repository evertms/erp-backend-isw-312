namespace Shared.Contracts.Sales;

public record KdsTeamContractResponse(
    string TeamCen,
    string Name,
    List<string> CategoryCens
);
