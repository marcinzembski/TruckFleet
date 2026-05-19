# TruckFleet

REST API do zarządzania flotą pojazdów, zbudowane na .NET 10, FastEndpoints i Clean Architecture.

## Stack

- **.NET 10** + **FastEndpoints 8.1** - routing i walidacja
- **EF Core** - InMemory (docelowo PostgreSQL)
- **Docker** + **docker-compose** - domyślny punkt startowy rozwiązania

## Architektura

Rozwiązanie jest podzielone na 4 warstwy zgodnie z Clean Architecture:

"""
FleetApi.Domain          ← encje, value objects, wyjątki domenowe
FleetApi.Application     ← serwisy, interfejsy repozytoriów, DTO
FleetApi.Infrastructure  ← EF Core, repozytoria
FleetApi.Api             ← endpointy, walidatory, middleware
"""

Każdy endpoint ma własny folder z request, validatorem i endpointem w jednym miejscu:

"""
Endpoints/Trucks/
  CreateTruck/   → CreateTruckEndpoint, CreateTruckValidator, CreateTruckRequest
  GetTruck/      → GetTruckEndpoint
  ListTrucks/    → ListTrucksEndpoint, ListTrucksValidator, ListTrucksRequest
  UpdateTruck/   → UpdateTruckEndpoint, UpdateTruckValidator, UpdateTruckRequest
  UpdateTruckStatus/ → UpdateTruckStatusEndpoint, UpdateTruckStatusValidator, UpdateTruckStatusRequest
  DeleteTruck/   → DeleteTruckEndpoint
"""

## Dlaczego nie MediatR?

MediatR rozwiązuje konkretny problem: odsprzęganie wywołującego od handlera gdy są oni w różnych miejscach systemu. W tym projekcie:

- FastEndpoints i tak wymaga endpointów jako klas - nie ma nic do odsprzęgania
- Mapping 1:1:1 endpoint → command → handler dodaje pliki bez żadnej wartości
- Pipeline behaviors MediatR nie są używane (logowanie, autoryzacja, caching)
- "TruckService" jako prosta klasa jest czytelniejszy, łatwiejszy do testowania i debugowania

MediatR warto rozważyć gdy pojawi się potrzeba pipeline behaviors lub zdarzeń domenowych między modułami.

## Dlaczego nie pełne CQRS?

CQRS to wzorzec (nie architektura), który mówi: rozdziel operacje zapisu od odczytu. W tym projekcie to rozróżnienie istnieje:

- Operacje zapisu: "CreateAsync", "UpdateAsync", "ChangeStatusAsync", "DeleteAsync"
- Operacje odczytu: "GetByIdAsync", "ListAsync"

Nie zdecydowałem się na osobne modele odczytu i zapisu (osobne DbContexty, read modele, projekcje), bo przy tej skali ERP nie przynosi to wartości, a dodaje złożoność bez mierzalnego zysku. Jeśli pojawi się potrzeba (np. raportowanie na dużym wolumenie), read model można dodać bez zmiany istniejącej struktury.

## Możliwości rozbudowy

### Nowy moduł ERP

Fundament jest gotowy - nowy moduł to:

1. "Domain/{Moduł}/" - encja dziedzicząca z "AggregateRoot<TId>", value objects
2. "Application/{Moduł}/" - "I{Moduł}Repository", "{Moduł}Service", "{Moduł}Filter"
3. "Infrastructure/Persistence/" - konfiguracja EF Core (auto-discovered), repozytorium, "DbSet<T>" w "AppDbContext"
4. "Api/Endpoints/{Moduł}/" - endpointy i walidatory (auto-discovered przez FastEndpoints)

"GlobalExceptionHandler", "ProblemDetails", base classy domenowe i wzorzec DI działają od razu dla każdego nowego modułu.

### Outbox Pattern

Infrastruktura domenowa zawiera "IDomainEvent" i "AggregateRoot<TId>.DomainEvents" — fundament gotowy, ale żadne zdarzenia nie są jeszcze podnoszone ani publikowane.

Dodanie Outbox wymaga:
- Tabeli "OutboxMessages" w bazie
- Interceptora EF Core zapisującego zdarzenia przy "SaveChangesAsync"
- Background service publikującego zdarzenia (np. przez MassTransit lub własny worker)

### PostgreSQL

Wystarczy zamienić "UseInMemoryDatabase" na "UseNpgsql" w "Infrastructure/DependencyInjection.cs" i dodać migracje EF Core. Cała reszta infrastruktury pozostaje bez zmian.
