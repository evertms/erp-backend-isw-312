# Examen Parcial — [Tu nombre]

## Sección 1 — Identificación

- **Nombre completo:** Evert Moreno Serrate
- **Pareja asignada para el sábado:** Josue Matias Molina Palacios
- **Repositorio de Inventario:** [Inventory](github.com/evertms/erp-backend-isw-312/tree/main/Modules/Inventory)
- **Repositorio de Ventas:** [Sales](github.com/evertms/erp-backend-isw-312/tree/main/Modules/Sales)
- **Contrato API acordado en grupo:** [Contrato de Ventas](github.com/evertms/erp-backend-isw-312/tree/main/Modules/Sales/contrato-api.yaml)
- **URL del Swagger autogenerado** (cuando levantás el backend localmente): http://localhost:5239/index.html

## Sección 2 — Decisiones técnicas con snippets

### 2.1 Árbol de carpetas del backend de Ventas

Pegá la estructura de carpetas de tu proyecto de Ventas. Ejemplo:

```
Sales/
├── Sales.Domain/
│   ├── Entities/
│   ├── Repositories/
│   └── Enums/
├── Sales.Application/
│   ├── DTOs/
│   ├── Services/
│   └── Features/
│       └── {MyFeature}/
├── Sales.Infrastructure/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── Repositories/
│   ├── ServiceCollectionExtensions.cs
│   ├── SalesDbContext.cs
│   └── SalesSeeder.cs
├── Sales.API/
│   ├── Endpoints/
│   ├── Extensions/
│   └── Program.cs
├── EXAMEN.md
└── sales-contract-v1.yml
```

**Explicá en 2-3 líneas por qué la organizaste así.**  
Pensando en cumplir con la división por módulos de un monolito modular para lograr migrar más fácilmente a microservicios y, a su vez, cada módulo por capas siguiendo clean architecture. Siento que me ayuda a que el núcleo del negocio, las reglas y el dominio sean totalmente independientes de detalles de implementación externa como bases de datos o sistemas/servicios externos, como el módulo de inventario, que a mi módulo de ventas no le debe importar qué hace y los puede cambiar sin apenas tocar lógica interna.  

### 2.2 Flujo de "registrar una venta"

Pegá los snippets del código que se ejecuta cuando un usuario confirma una venta, en orden:

1. El endpoint que recibe el request (Controller):
```csharp
group.MapPost("/{ticketCen}/payment", async (string companyCen, string ticketCen, PayTicketContractRequest request, ISender sender) =>
{
    try
    {
        var result = await sender.Send(new PayTicketCommand(companyCen, ticketCen, request));
        return Results.Ok(result);
    }
    catch (StockInsufficiencyException ex)
    {
        return Results.Conflict(new ProcessRestaurantOrderPaymentResultDto(
            false, null, null, null, 0, 0, 0, ex.Message, ex.Insufficiencies));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ex.Message });
    }
})
.Produces<PayTicketContractResponse>(StatusCodes.Status200OK)
.Produces<ProcessRestaurantOrderPaymentResultDto>(StatusCodes.Status409Conflict)
.WithName("PayTicket")
.WithSummary("Procesa el pago de un ticket");
```  

2. La capa intermedia que procesa la lógica (Service / Use Case / Handler).
```csharp
public async Task<PayTicketContractResponse> Handle(PayTicketCommand command, CancellationToken cancellationToken)
{
    var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
    if (ticket == null || ticket.CompanyCen != command.CompanyCen)
        throw new ArgumentException("Ticket no encontrado.");

    if (ticket.Status == TicketStatus.Paid)
        throw new InvalidOperationException("El ticket ya ha sido pagado.");

    var warehouseCen = configuration["InventorySettings:DefaultWarehouseCen"] 
        ?? throw new InvalidOperationException("La bodega por defecto no está configurada en las variables de entorno.");

    // 1. Validar Stock (Fase 1 del 2PC)
    var validationRequest = new StockValidationContractRequest(
        warehouseCen,
        "SalesModule",
        ticket.Cen,
        ticket.Lines.Select(l => new StockValidationItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
    );

    var validationResponse = await inventoryService.ValidateStockAsync(command.CompanyCen, validationRequest, cancellationToken);
    
    if (!validationResponse.IsValid)
    {
        throw new StockInsufficiencyException(validationResponse.Requirements
            .Where(r => r.MissingQuantity > 0)
            .Select(r => new StockInsufficiencyResponseDto(null, r.ProductCen, r.ProductName, r.WarehouseCen, (int)r.RequestedQuantity, (int)r.AvailableQuantity, (int)r.MissingQuantity))
            .ToList());
    }

    // 2. Procesar Pago Localmente
    if (!Enum.TryParse<PaymentMethod>(command.Request.PaymentMethodCode, true, out var method))
    {
        throw new ArgumentException($"El método de pago '{command.Request.PaymentMethodCode}' no es válido.");
    }

    ticket.Pay(method, ticket.Total);

    await ticketRepository.UpdateAsync(ticket, cancellationToken);
    await unitOfWork.SaveChangesAsync(cancellationToken);

    // 3. Consumir Stock (Fase 2 del 2PC)
    var consumeRequest = new StockConsumeContractRequest(
        warehouseCen,
        "SalesModule",
        ticket.Cen,
        $"Venta ticket {ticket.Cen}",
        ticket.Lines.Select(l => new StockConsumeItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
    );

    var consumeResponse = await inventoryService.ConsumeStockAsync(command.CompanyCen, consumeRequest, cancellationToken);

    var recentPayment = ticket.Payments.LastOrDefault();

    return new PayTicketContractResponse(
        recentPayment?.Cen ?? throw new InvalidOperationException("No se generó el registro de pago en el dominio."),
        ticket.Cen,
        ticket.Status.ToString(),
        (double)ticket.Subtotal,
        (double)ticket.TaxAmount,
        (double)ticket.Total,
        consumeResponse.DocumentCen
    );
}
```

3. La parte que llama al Inventario del compañero (HttpClient o equivalente).
```csharp
public async Task<StockValidationContractResponse> ValidateStockAsync(string companyCen, StockValidationContractRequest request, CancellationToken cancellationToken = default)
{
    var response = await httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/validate", request, cancellationToken);
    response.EnsureSuccessStatusCode();
    
    var result = await response.Content.ReadFromJsonAsync<StockValidationContractResponse>(_jsonOptions, cancellationToken);
    return result ?? throw new InvalidOperationException("Failed to deserialize stock validation response.");
}
```

4. La parte que persiste la venta en tu BD.
```csharp
// PayTicketHandler.cs, método Handle
ticket.Pay(method, ticket.Total);

await ticketRepository.UpdateAsync(ticket, cancellationToken);
await unitOfWork.SaveChangesAsync(cancellationToken);
```

**Explicá en 3-5 líneas por qué dividiste así las responsabilidades.**  
Dividí las responsabilidades aplicando un poco la parte de principios de Domain-Driven Design. El método `ticket.Pay()` encapsula la lógica de negocio, que son validaciones de flujo y cambio de estado a pagado. O sea, literalmente es la entidad del proyecto de Dominio, evitando que el modelo sea anémico, es decir, que no solo tenga atributos sino también métodos que validen flujos o si la entidad es válida en sí. Luego, la capa de aplicación (el handler) delega la persistencia en base de datos al repository y al UnitOfWork, con esto logro aislar la lógica central de mi app de la tecnología de base de datos.

### 2.3 Llamada al Inventario del compañero

Pegá el código exacto donde tu Ventas llama al API del Inventario del compañero.
```csharp
public async Task<StockValidationContractResponse> ValidateStockAsync(string companyCen, StockValidationContractRequest request, CancellationToken cancellationToken = default)
{
    var response = await httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/validate", request, cancellationToken);
    response.EnsureSuccessStatusCode();
    
    var result = await response.Content.ReadFromJsonAsync<StockValidationContractResponse>(_jsonOptions, cancellationToken);
    return result ?? throw new InvalidOperationException("Failed to deserialize stock validation response.");
}
```

Respondé brevemente:  
- ¿Qué pasa si el compañero responde con código 200 OK?
Si me devuelve un 200, el método `EnsureSuccessStatusCode()` deja pasar la ejecución. Después, el JSON se convierte al objeto de respuesta y mi handler recibe el resultado para chequear si `IsValid` es true o false y decidir si sigue con el pago o no.

- ¿Qué pasa si responde con 404 o 500?  
Como tengo configurada una política de reintentos con Polly, si responde con 500 (error transitorio) o 404, el sistema no falla al toque. Va a intentar 2 veces más con una espera exponencial (2 y 4 segundos). Si después de eso sigue fallando, recién ahí lanza la excepción que corta el proceso de venta.

- ¿Qué pasa si el compañero está caído (timeout)?  
Igual que con los errores 500, Polly detecta que es un error de comunicación y aplica la política de 2 reintentos. Si el compañero sigue caído después de los intentos, el `httpClient` lanza la excepción final por timeout y la venta se cancela para no vender sin stock real.

Pegá:
- La línea relevante de tu `.env.example` o `appsettings.json`.  
`"PARTNER_INVENTORY_URL": "http://localhost:5239"` (appsettings.json)

- El código que lee esa configuración y la usa para construir la llamada HTTP.  
```csharp
// ServiceCollectionExtensions.cs
var inventoryUrl = configuration["PARTNER_INVENTORY_URL"] ?? "http://localhost:5239";
        
services.AddHttpClient<IInventoryIntegrationService, InventoryIntegrationService>(client =>
{
    client.BaseAddress = new Uri(inventoryUrl);
})
.AddPolicyHandler(GetRetryPolicy());
```

Explicá en 1 línea cómo cambiarías esa URL si el sábado tu pareja levanta su backend en otra IP.  
Fácil, directamente cambio el atributo "PARTNER_INVENTORY_URL" en appsettings.json. Si falla, el sistema se conecta automáticamente a mi backend si está corriendo en el localhost:5239

## Sección 3 — Sobre el trabajo en grupo del contrato API

- **3.1** ¿Hubo desacuerdos al definir el contrato? ¿Cuáles?  
La verdad que no, mi compañero que tenía todo más avanzado nos lo mostró y nadie se quejó. Además que cumple con todas las observaciones y ADR vistos en clase, no hubo desacuerdos.

- **3.2** ¿Cómo se resolvieron?  
Como dije, no hubo ningún desacuerdo. 

- **3.3** ¿Qué propusiste vos específicamente que quedó en el contrato final?  
Honestamente nada jeje.

## Sección 4 — Teoría aplicada

Respondé cada pregunta en 1-2 párrafos. Está permitido usar IA para mejorar redacción, pero las respuestas deben hacer referencia explícita a tu propio código o decisiones.

**4.1** Tu compañero te avisa que va a cambiar el campo `cantidad` por `qty` en su respuesta del endpoint de stock. Tu sistema ya consume ese endpoint. Explicá qué riesgos genera ese cambio y qué prácticas conocés para evitar que un cambio así rompa los sistemas que dependen de su API.  

El riesgo obvio sería que es un *breaking change* y rompería directamente mi deserialización. Si mi contrato en el código espera `cantidad` y llega `qty`, mi deserializador JSON va a mapear `cantidad` a nulo o cero, y mi venta va a fallar asumiendo que no hay stock (o directamente va a tirar una excepción). Para evitar esto, lo ideal sería usar versionado de API (tipo `/api/v2/inventory/...`) para que mi sistema siga consumiendo la versión 1 intacta, o que mi compañero mantenga compatibilidad hacia atrás devolviendo ambos campos (`cantidad` y `qty` juntos) hasta que yo tenga tiempo de actualizar mi código.

**4.2** Tu sistema de Ventas hace una petición al Inventario para descontar stock. La red se cae justo después de que Inventario procesó el descuento pero antes de que la respuesta llegue a Ventas. ¿Qué problema se genera? ¿Cómo lo manejarías?  

Se genera una inconsistencia de datos grave. El Inventario descuenta el producto de forma exitosa, pero como mi módulo de Ventas no recibe la confirmación (por timeout de red), hace un rollback y el ticket queda sin pagar. Nos quedamos con stock restado sin haber registrado el pago de la venta real. Para manejarlo, el endpoint de Inventario debe ser **idempotente** (aprovechando que ya le mando el `ticket.Cen` como identificador de transacción) para que si mis reintentos de Polly vuelven a dispararse, no me descuente el stock dos veces. Además, si Ventas finalmente cancela la operación de su lado, debería mandarle un request de "compensación" (rollback) a Inventario o, idealmente, pasar a una arquitectura de comunicación asíncrona con colas (como RabbitMQ) para asegurar la consistencia eventual en vez de usar una llamada HTTP tan acoplada.

**4.3** Si el Inventario del compañero está caído, ¿debería tu Ventas permitir seguir registrando ventas? Justificá considerando ventajas y desventajas de cada postura. ¿Qué hace TU sistema hoy en ese caso?  

Es el clásico dilema del Teorema CAP (disponibilidad vs consistencia). Si permito ventas, priorizo la disponibilidad: no freno el negocio, pero corro el riesgo de vender productos sin stock y que me vengan a reclamar después. Si lo bloqueo, aseguro 100% la consistencia, pero la venta se detiene al toque. Hoy, mi sistema corta el proceso de venta. Como detallé en la sección 2.3, prioricé la consistencia; si después de los reintentos de Polly el Inventario sigue sin responder, mi `HttpClient` tira timeout, el handler cancela el pago, lanza excepción y no permite que se cobre nada "en el aire".

**4.4** Explicá por qué tener la URL del compañero hardcodeada como `http://localhost:5000` es un problema. ¿Cuál es la solución correcta y cómo la implementaste vos?  

Es un problema grave porque arruina la portabilidad de la aplicación. Si la hardcodeo, el backend solo funciona en mi máquina local. Si lo quiero subir a Docker, a un servidor real, o si el sábado cambia la IP de la compu de mi compañero en la red de la clase, tendría que tocar código, recompilar todo el proyecto y volver a arrancar. La solución correcta sería inyectar esa URL desde el entorno. Yo lo implementé leyendo la variable `PARTNER_INVENTORY_URL` directamente desde el `appsettings.json` o en mi `ServiceCollectionExtensions.cs`, usándola como BaseAddress a la hora de configurar el `HttpClient`.
