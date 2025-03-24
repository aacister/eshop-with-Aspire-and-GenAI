# Eshop with Aspire and Generative AI for chat and semantic search

* eshop app with Catalog and Basket microservices developed utilizing .NET Aspire, .NET Core, C#, razor pages UI, Keycloak auth and Ollama LLM with Microsoft Extensions-AI interface layer.
* Aspire Hosting and client Nuget packages
* Service discovery, observability, and Docker container deployments with Aspire structure on local run, utilizing Aspire dashboard.
* Basket microservice with Redis cache.  Catalog microservices with EF and Postgres storage.
* RabbitMQ async messaging used to keep Basket microservice's Redis cache in sync on price update.
* Endpoints secured with KeyCloak Authentication.
* Ollama LLM with Microsoft-Extensions.AI utilized to create support page with chat, and search page with semantic search.
* Semantic search utilizes in-memory vector db.
* Utilizes Ollama models 'llama3.2' for chat and 'all-minilm' for semantic search.

'
