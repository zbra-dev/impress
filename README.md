# Impress
ZBRA Impress implementation for .NET Standard 2.0 compatible with .NET Core. 

ZBRA Impress is a commons library containing very useful classes like `Hash`, `Maybe` , `Interval` or `Fraction` together in an assortment of useful extension methods.

## Toolboxes

* Impress - Contains the core basics that may be useful on any and every project
* Impress.DomainEvents - Contains a simple [Domain Event](https://martinfowler.com/eaaDev/DomainEvent.html) framework based on the Observer/Listener pattern
* Impress.Futures - Contains the concept of CompletableFuture to help make sense of parallel executions as an alternative to async/await. Depends on Impress core.
* Impress.Globalization - Contains a globalization API to correctly  internationalize and localize your application. Useful even if your application has a target single language.
* Impress.Logging - Contains a logging isolation API that makes easier log exceptions.
* Impress.Mail - Contains a framework for sending email. It can send mail directly, using queues and/or using storable queues allowing sending mail to be part of unit of work transaction. Depends on Impress.Logging.
* Impress.Validation - Contains a validation framework together with a annotations based validator. Creates messages compatible with the Globalization toolbox.


## Instalation

Toolboxes are available at nuget.org

* Impress - https://www.nuget.org/packages/Impress.Core
* Impress.DomainEvents - https://www.nuget.org/packages/Impress.DomainEvents
* Impress.Futures - https://www.nuget.org/packages/Impress.Futures
* Impress.Globalization - https://www.nuget.org/packages/Impress.Globalization
* Impress.Logging -  https://www.nuget.org/packages/Impress.Logging
* Impress.Mail - https://www.nuget.org/packages/Impress.Mail
* Impress.Validation - https://www.nuget.org/packages/Impress.Validation


